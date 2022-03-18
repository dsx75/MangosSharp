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

using Mangos.Tools.Extractors;
using NLog;
using System;

namespace Mangos.Tools.Extractor;

public class Program
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        // TODO Read these from command line arguments
        var wowDirectory = "";
        var outputDirectory = "";

        wowDirectory = @"F:\WoW\Clients\0";
        outputDirectory = @"F:\WoW\Temp";

        // TODO Implement proper handling for Exceptions (file not found etc.)

        var extractor = ExtractorManager.GetExtractor(wowDirectory, outputDirectory);

        logger.Info("Extracting Chat Types");
        extractor.ExtractChatTypes();

        logger.Info("Extracting OpCodes");
        extractor.ExtractOpCodes();

        logger.Info("Extracting Spell Failed Reasons");
        extractor.ExtractSpellFailedReasons();

        logger.Info("Extracting Update Fields");
        extractor.ExtractUpdateFields();

        logger.Info("DONE");
    }
}
