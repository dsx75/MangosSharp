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

using Mangos.Tools.Common;
using NLog;
using System;

namespace Mangos.Tools.Extractor;

public class Program
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        // TODO Read these from command line arguments
        var wowDir = "";
        var outputDir = "";

        //wowDir = @"F:\WoW\Clients\0";
        //outputDir = @"F:\WoW\Temp";

        // TODO Implement proper handling for Exceptions (file not found etc.)

        logger.Info("Extracting Chat Types");
        Extractors.ExtractChatTypes(wowDir, outputDir);

        logger.Info("Extracting OpCodes");
        Extractors.ExtractOpcodes(wowDir, outputDir);

        logger.Info("Extracting Spell Failed Reasons");
        Extractors.ExtractSpellFailedReason(wowDir, outputDir);

        logger.Info("Extracting Update Frields");
        Extractors.ExtractUpdateFields(wowDir, outputDir);

        logger.Info("DONE");
    }
}
