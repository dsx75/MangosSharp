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

using Mangos.Tools.Extractors.One;
using Mangos.Tools.Extractors.Two;
using Mangos.Tools.Extractors.Zero;
using NLog;
using System;
using System.Diagnostics;

namespace Mangos.Tools.Extractors;
public static class ExtractorManager
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Gets the extractor for the specified WoW installation.
    /// </summary>
    /// <param name="wowDirectory">Directory where the WoW client is installed or empty string if the current directory should be used instead.</param>
    /// <param name="outputDirectory">Directory where all output files should be generated or empty string if the current directory should be used instead.</param>
    /// <returns>Extractor for extracting all the necessary data from the specified WoW client</returns>
    public static IExtractor GetExtractor(string wowDirectory, string outputDirectory)
    {
        var wowClient = Path.Combine(wowDirectory, "wow.exe");

        if (!File.Exists(wowClient))
        {
            throw new FileNotFoundException("No WoW client found at " + wowClient);
        }

        FileVersionInfo ver = FileVersionInfo.GetVersionInfo(wowClient);

        // TODO add all supported combinations

        // Vanilla clients
        if ((ver.FileMajorPart == 1) && (ver.FileMinorPart == 12) && (ver.FileBuildPart == 1) && (ver.FilePrivatePart == 5875))
        {
            // 1.12.1.5875
            return new ExtractorZero(wowDirectory, wowClient, ver, outputDirectory);
        }

        // TBC clients
        if ((ver.FileMajorPart == 2) && (ver.FileMinorPart == 4) && (ver.FileBuildPart == 3) && (ver.FilePrivatePart == 8606))
        {
            // 2.4.3.8606
            return new ExtractorOne(wowDirectory, wowClient, ver, outputDirectory);
        }

        // WotLK clients
        if ((ver.FileMajorPart == 3) && (ver.FileMinorPart == 3) && (ver.FileBuildPart == 5) && (ver.FilePrivatePart == 12340))
        {
            // 3.3.5.12340
            return new ExtractorTwo(wowDirectory, wowClient, ver, outputDirectory);
        }

        // Unsupported client
        throw new NotSupportedException("Unsupported WoW client " + wowClient + " " + ver.ToString());
    }

}
