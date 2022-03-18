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

using Foole.Mpq;
using NLog;
using System;

namespace Mangos.Tools.Extractors.Zero;

/// <summary>
/// DBC extractor by UniX
/// </summary>
internal class DbcExtractor
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    private readonly List<MpqArchive> MPQArchives = new();

    // TODO these seem to be some leftovers from an unfinished attempt to extract maps?
    //private readonly List<int> MapIDs = new();
    //private readonly List<string> MapNames = new();
    //private readonly Dictionary<int, int> MapAreas = new();
    //private readonly int MaxAreaID = -1;
    //private readonly Dictionary<int, int> MapLiqTypes = new();

    // input directory
    private readonly string _dataDirectory;

    // output directory
    private readonly string _dbcDirectory;

    // TODO is this needed?
    //private readonly string _mapsDirectory;

    internal DbcExtractor(string dataDirectory, string outputDirectory)
    {
        _dataDirectory = dataDirectory;
        _dbcDirectory = Path.Combine(outputDirectory, "dbc");
        //_mapsDirectory = Path.Combine(outputDirectory, "maps");
    }

    internal void Run()
    {
        logger.Info("DBC extractor by UniX");
        if (!Directory.Exists(_dataDirectory))
        {
            throw new DirectoryNotFoundException("Data directory not found: " + _dataDirectory);
        }

        List<string> MPQFilesToOpen = new() { "terrain.MPQ", "dbc.MPQ", "misc.MPQ", "patch.MPQ", "patch-2.MPQ" };
        foreach (var mpq in MPQFilesToOpen)
        {
            var file = Path.Combine(_dataDirectory, mpq);
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("File not found: " + mpq);
            }
        }

        foreach (var mpq in MPQFilesToOpen)
        {
            var file = Path.Combine(_dataDirectory, mpq);
            var stream = File.Open(Path.GetFullPath(file), FileMode.Open);
            MpqArchive newArchive = new(stream, true);
            MPQArchives.Add(newArchive);
            logger.Info("Loaded archive " + mpq);
        }

        try
        {
            Directory.CreateDirectory(_dbcDirectory);
            //Directory.CreateDirectory(_mapsDirectory);
            logger.Info("Output directory created.");
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Unable to create output directory " + _dbcDirectory);
            throw;
        }

        try
        {
            ExtractDBCs();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Unable to extract DBC Files.");
            throw;
        }

        // Try
        // ExtractMaps()
        // Catch ex As Exception
        // Console.ForegroundColor = ConsoleColor.Red
        // Console.WriteLine("Unable to extract Maps. Error: " & ex.Message)
        // GoTo ExitNow
        // End Try
    }

    private void ExtractDBCs()
    {
        logger.Info("Extracting DBC Files");
        var dbcFolder = Path.GetFullPath(_dbcDirectory);
        var numDBCs = 0;
        foreach (var mpqArchive in MPQArchives)
        {
            numDBCs += mpqArchive.Where(x => x.Filename is not null).Where(x => x.Filename.EndsWith(".dbc")).Count();
        }

        var i = 0;
        var numDiv30 = numDBCs / 30;
        foreach (var mpqArchive in MPQArchives)
        {
            foreach (var mpqFile in mpqArchive.Where(x => x.Filename is not null).Where(x => x.Filename.EndsWith(".dbc")))
            {
                using var mpqStream = mpqArchive.OpenFile(mpqFile);
                using var fileStream = File.Create(Path.Combine(dbcFolder, Path.GetFileName(mpqFile.Filename)));
                mpqStream.CopyTo(fileStream);

                /*
                i += 1;
                if (i % numDiv30 == 0)
                {
                    Console.Write(".");
                }
                */
            }
        }
    }

}
