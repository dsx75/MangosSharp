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

namespace Mangos.Tools.Extractors.Two;

/// <summary>
/// Implementation of Extractor for the WotLK clients.
/// </summary>
internal class ExtractorTwo : ExtractorBase
{
    private readonly Logger logger = LogManager.GetCurrentClassLogger();

    public ExtractorTwo(string wowDirectory, string wowClient, FileVersionInfo wowClientVersion, string outputDirectory) :
        base(wowDirectory, wowClient, wowClientVersion, outputDirectory)
    {
    }

    public override void ExtractChatTypes()
    {
        throw new NotImplementedException();
    }

    public override void ExtractDbcFiles()
    {
        throw new NotImplementedException();
    }

    public override void ExtractMaps()
    {
        throw new NotImplementedException();
    }

    public override void ExtractOpCodes()
    {
        throw new NotImplementedException();
    }

    public override void ExtractSpellFailedReasons()
    {
        throw new NotImplementedException();
    }

    public override void ExtractUpdateFields()
    {
        throw new NotImplementedException();
    }
}
