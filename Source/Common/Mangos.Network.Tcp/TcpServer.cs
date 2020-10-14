﻿//
// Copyright (C) 2013-2020 getMaNGOS <https:\\getmangos.eu>
//
// This program is free software. You can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation. either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY. Without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//

using Mangos.Loggers;
using Mangos.Network.Tcp.Extensions;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Channels;

namespace Mangos.Network.Tcp
{
    public class TcpServer
    {
        private readonly ILogger logger;
        private readonly ITcpClientFactory tcpClientFactory;
        private readonly CancellationTokenSource cancellationTokenSource;

        private Socket socket;

        public TcpServer(ILogger logger, ITcpClientFactory tcpClientFactory)
        {
            this.logger = logger;
            this.tcpClientFactory = tcpClientFactory;

            cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start(IPEndPoint endPoint, int backlog)
        {
            if(socket != null)
            {
                logger.Error("TcpServer already started");
                throw new Exception("TcpServer already started");
            }
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(endPoint);
                socket.Listen(backlog);
                StartAcceptLoop();
                logger.Debug("TcpServer has been started");
            }
            catch (Exception ex)
            {
                logger.Error("TcpServer start execption", ex);
                throw;
            }
        }

        private async void StartAcceptLoop()
        {
            try
            {
                logger.Debug("Start accepting connections");
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    OnAcceptAsync(await socket.AcceptAsync());
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during accepting conenction", ex);
            }
        }

        private async void OnAcceptAsync(Socket clientSocket)
        {
            try
            {
                var tcpClient = await tcpClientFactory.CreateTcpClientAsync(clientSocket);
                var recieveChannel = Channel.CreateUnbounded<byte>();
                var sendChannel = Channel.CreateUnbounded<byte>();

                RecieveAsync(clientSocket, recieveChannel.Writer);
                SendAsync(clientSocket, sendChannel.Reader);
                tcpClient.HandleAsync(recieveChannel.Reader, sendChannel.Writer, cancellationTokenSource.Token);

                logger.Debug("New Tcp conenction established");
            }
            catch (Exception ex)
            {
                logger.Error("Error during accepting conenction handler", ex);
            }
        }


        private async void RecieveAsync(Socket client, ChannelWriter<byte> writer)
        {
            try
            {
                var buffer = new byte[client.ReceiveBufferSize];
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    var bytesRead = await client.ReceiveAsync(buffer, SocketFlags.None);
                    if (bytesRead == 0)
                    {
                        client.Dispose();
                        return;
                    }
                    await writer.WriteAsync(buffer, bytesRead);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during recieving data from socket", ex);
            }
        }

        private async void SendAsync(Socket client, ChannelReader<byte> reader)
        {
            try
            {
                var buffer = new byte[client.SendBufferSize];
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    await reader.WaitToReadAsync();
                    int writeCount;
                    for (writeCount = 0;
                        writeCount < buffer.Length && reader.TryRead(out buffer[writeCount]);
                        writeCount++) ;
                    var arraySegment = new ArraySegment<byte>(buffer, 0, writeCount);
                    await client.SendAsync(arraySegment, SocketFlags.None);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error during sending data to socket", ex);
            }
        }
    }
}