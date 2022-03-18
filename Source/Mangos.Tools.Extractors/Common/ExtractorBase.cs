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

using System;
using System.Diagnostics;

namespace Mangos.Tools.Extractors.Common;

/// <summary>
/// The base class for all Extractor implementations.
/// </summary>
internal abstract class ExtractorBase : IExtractor
{
    protected readonly string _wowDirectory;
    protected readonly string _wowClient;
    protected readonly FileVersionInfo _wowClientVersion;
    protected readonly string _outputDirectory;

    internal ExtractorBase(string wowDirectory, string wowClient, FileVersionInfo wowClientVersion, string outputDirectory)
    {
        _wowDirectory = wowDirectory;
        _wowClient = wowClient;
        _wowClientVersion = wowClientVersion;
        _outputDirectory = outputDirectory;
    }

    public string WowDirectory => _wowDirectory;

    public string WowClient => _wowClient;

    public FileVersionInfo WowClientVersion => _wowClientVersion;

    public string OutputDirectory => _outputDirectory;

    public abstract void ExtractChatTypes();

    public abstract void ExtractDbcFiles();

    public abstract void ExtractOpCodes();

    public abstract void ExtractSpellFailureReasons();

    public abstract void ExtractUpdateFields();
}
