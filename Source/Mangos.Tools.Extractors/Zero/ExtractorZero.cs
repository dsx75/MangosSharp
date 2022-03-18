//
// Copyright (C) 2013-2022 getMaNGOS <https://getmangos.eu>
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

using Mangos.Tools.Extractors.Common;
using NLog;
using System;
using System.Diagnostics;

namespace Mangos.Tools.Extractors.Zero;

/// <summary>
/// Implementation of Extractor for the Vanilla clients.
/// </summary>
internal class ExtractorZero : ExtractorBase
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ExtractorZero(string wowDirectory, string wowClient, FileVersionInfo wowClientVersion, string outputDirectory) :
        base(wowDirectory, wowClient, wowClientVersion, outputDirectory)
    {
    }

    public override void ExtractChatTypes()
    {
        FileStream f = new(WowClient, FileMode.Open, FileAccess.Read, FileShare.Read, 10000000);
        BinaryReader r1 = new(f);
        StreamReader r2 = new(f);

        var outputFile = Path.Combine(OutputDirectory, "Global.ChatTypes.cs");
        FileStream o = new(outputFile, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        StreamWriter w = new(o);
        var START = Utils.SearchInFile(f, "CHAT_MSG_RAID_WARNING");
        if (START == -1)
        {
            logger.Error("Wrong offsets!");
        }
        else
        {
            Stack<string> Names = new();
            var Last = "";
            var Offset = START;
            f.Seek(Offset, SeekOrigin.Begin);
            while (Last.Length == 0 || Last.Substring(0, 9) == "CHAT_MSG_")
            {
                Last = Utils.ReadString(f);
                if (Last.Length > 10 && Last.Substring(0, 9) == "CHAT_MSG_")
                {
                    Names.Push(Last);
                }
            }

            logger.Info(string.Format("{0} chat types extracted.", Names.Count));
            PrintHeader(w, null);
            w.WriteLine("Public Enum ChatMsg");
            w.WriteLine("{");
            var i = 0;
            while (Names.Count > 0)
            {
                w.WriteLine("    {0,-64}// 0x{1:X3}", Names.Pop() + " = " + ToHex(i) + ",", i);
                i += 1;
            }

            w.WriteLine("}");
            w.Flush();
        }

        o.Close();
        f.Close();
    }

    public override void ExtractDbcFiles()
    {
        throw new NotImplementedException();
    }

    public override void ExtractOpCodes()
    {
        FileStream f = new(WowClient, FileMode.Open, FileAccess.Read, FileShare.Read, 10000000);
        BinaryReader r1 = new(f);
        StreamReader r2 = new(f);

        var outputFile = Path.Combine(OutputDirectory, "Global.Opcodes.cs");
        FileStream o = new(outputFile, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        StreamWriter w = new(o);
        logger.Debug(Utils.ReadString(f, Utils.SearchInFile(f, "CMSG_REQUEST_PARTY_MEMBER_STATS")));
        var START = Utils.SearchInFile(f, "NUM_MSG_TYPES");
        if (START == -1)
        {
            logger.Error("Wrong offsets!");
        }
        else
        {
            Stack<string> Names = new();
            var Last = "";
            f.Seek(START, SeekOrigin.Begin);
            while (Last != "MSG_NULL_ACTION")
            {
                Last = Utils.ReadString(f);
                Names.Push(Last);
            }

            logger.Info(string.Format("{0} opcodes extracted.", Names.Count));
            PrintHeader(w, null);
            w.WriteLine("Public Enum OPCODES");
            w.WriteLine("{");
            var i = 0;
            while (Names.Count > 0)
            {
                w.WriteLine("    {0,-64}// 0x{1:X3}", Names.Pop() + "=" + i, i);
                i += 1;
            }

            w.WriteLine("}");
            w.Flush();
        }

        o.Close();
        f.Close();
    }

    public override void ExtractSpellFailedReasons()
    {
        FileStream f = new(WowClient, FileMode.Open, FileAccess.Read, FileShare.Read, 10000000);
        BinaryReader r1 = new(f);
        StreamReader r2 = new(f);

        var outputFile = Path.Combine(OutputDirectory, "Global.SpellFailedReasons.cs");
        FileStream o = new(outputFile, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        StreamWriter w = new(o);
        var REASON_NAME_OFFSET = Utils.SearchInFile(f, "SPELL_FAILED_UNKNOWN");
        if (REASON_NAME_OFFSET == -1)
        {
            logger.Error("Wrong offsets!");
        }
        else
        {
            Stack<string> Names = new();
            var Last = "";
            var Offset = REASON_NAME_OFFSET;
            f.Seek(Offset, SeekOrigin.Begin);
            while (Last.Length == 0 || Last.Substring(0, 13) == "SPELL_FAILED_")
            {
                Last = Utils.ReadString(f);
                if (Last.Length > 13 && Last.Substring(0, 13) == "SPELL_FAILED_")
                {
                    Names.Push(Last);
                }
            }

            logger.Info(string.Format("{0} spell failed reasons extracted.", Names.Count));
            PrintHeader(w, null);
            w.WriteLine("Public Enum SpellFailedReason");
            w.WriteLine("{");
            var i = 0;
            while (Names.Count > 0)
            {
                w.WriteLine("    {0,-64}// 0x{1:X3}", Names.Pop() + " = " + ToHex(i) + ",", i);
                i += 1;
            }

            w.WriteLine("    {0,-64}// 0x{1:X3}", "SPELL_NO_ERROR = " + ToHex(255), 255);
            w.WriteLine("}");
            w.Flush();
        }

        o.Close();
        f.Close();
    }

    public override void ExtractUpdateFields()
    {
        throw new NotImplementedException();
    }

    private static void PrintHeader(StreamWriter w, FileVersionInfo versInfo)
    {
        w.WriteLine("// Auto generated file");
        w.WriteLine("// {0}", DateTime.Now);

        if (versInfo != null)
        {
            w.WriteLine("// Patch: " + versInfo.FileMajorPart + "." + versInfo.FileMinorPart + "." + versInfo.FileBuildPart);
            w.WriteLine("// Build: " + versInfo.FilePrivatePart);
        }

        w.WriteLine();
    }

    /// <summary>
    /// Hexadecimal representation of an integer number
    /// </summary>
    private static string ToHex(int number)
    {
        var hex = "0x" + number.ToString("X");
        return hex;
    }

}
