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

using NLog;
using System;
using System.Text;

namespace Mangos.Tools.Common;

/// <summary>
/// Helper methods used by Extractors.
/// </summary>
internal static class Utils
{
    private static readonly Logger logger = LogManager.GetCurrentClassLogger();

    internal static int SearchInFile(Stream f, string s, int o = 0)
    {
        f.Seek(0L, SeekOrigin.Begin);
        BinaryReader r = new(f);
        var b1 = r.ReadBytes((int)f.Length);
        var b2 = Encoding.ASCII.GetBytes(s);
        for (int i = o, loopTo = b1.Length - 1; i <= loopTo; i++)
        {
            for (int j = 0, loopTo1 = b2.Length - 1; j <= loopTo1; j++)
            {
                if (b1[i + j] != b2[j])
                {
                    break;
                }

                if (j == b2.Length - 1)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    internal static int SearchInFile(Stream f, int v)
    {
        f.Seek(0L, SeekOrigin.Begin);
        BinaryReader r = new(f);
        var b1 = r.ReadBytes((int)f.Length);
        var b2 = BitConverter.GetBytes(v);
        // Array.Reverse(b2)

        for (int i = 0, loopTo = b1.Length - 1; i <= loopTo; i++)
        {
            if (i + 3 >= b1.Length)
            {
                break;
            }

            if (b1[i] == b2[0] && b1[i + 1] == b2[1] && b1[i + 2] == b2[2] && b1[i + 3] == b2[3])
            {
                return i;
            }
        }

        return -1;
    }

    internal static string ReadString(FileStream f)
    {
        var r = "";
        byte t;

        // Read if there are zeros
        t = (byte)f.ReadByte();
        while (t == 0)
        {
            t = (byte)f.ReadByte();
        }

        // Read string
        while (t != 0)
        {
            r += Convert.ToString((char)t);
            t = (byte)f.ReadByte();
        }

        return r;
    }

    internal static string ReadString(FileStream f, long pos)
    {
        var r = "";
        byte t;
        if (pos == -1)
        {
            return "*Nothing*";
        }

        f.Seek(pos, SeekOrigin.Begin);
        try
        {
            // Read if there are zeros
            t = (byte)f.ReadByte();
            while (t == 0)
            {
                t = (byte)f.ReadByte();
            }

            // Read string
            while (t != 0)
            {
                r += Convert.ToString((char)t);
                t = (byte)f.ReadByte();
            }
        }
        catch (Exception e)
        {
            logger.Error(e, "ReadString has thrown an Exception! The string is {0}", e.Message);
        }

        return r;
    }

    internal static string ToField(string sField)
    {
        // Make the first letter in upper case and the rest in lower case
        var tmp = sField.Substring(0, 1).ToUpper() + sField[1..].ToLower();
        // Replace lowercase object with Object (used in f.ex Gameobject -> GameObject)
        if (tmp.IndexOf("object", StringComparison.OrdinalIgnoreCase) > 0)
        {
            tmp = tmp.Length > tmp.IndexOf("object", StringComparison.OrdinalIgnoreCase) + 6
                ? tmp.Substring(0, tmp.IndexOf("object")) + "Object" + tmp[(tmp.IndexOf("object") + 6)..]
                : tmp.Substring(0, tmp.IndexOf("object")) + "Object";
        }

        return tmp;
    }

    internal static string ToType(int iType)
    {
        // Get the typename
        switch (iType)
        {
            case 1:
                {
                    return "INT";
                }

            case 2:
                {
                    return "TWO_SHORT";
                }

            case 3:
                {
                    return "FLOAT";
                }

            case 4:
                {
                    return "GUID";
                }

            case 5:
                {
                    return "BYTES";
                }

            default:
                {
                    return "UNK (" + iType + ")";
                }
        }
    }

    internal static void AddFlag(ref string sFlags, string sFlag)
    {
        if (!string.IsNullOrEmpty(sFlags))
        {
            sFlags += " + ";
        }

        sFlags += sFlag;
    }

    internal static string ToFlags(int iFlags)
    {
        var tmp = "";
        if (iFlags == 0)
        {
            tmp = "NONE";
        }

        if (Convert.ToBoolean(iFlags & 1))
        {
            AddFlag(ref tmp, "PUBLIC");
        }

        if (Convert.ToBoolean(iFlags & 2))
        {
            AddFlag(ref tmp, "PRIVATE");
        }

        if (Convert.ToBoolean(iFlags & 4))
        {
            AddFlag(ref tmp, "OWNER_ONLY");
        }

        if (Convert.ToBoolean(iFlags & 8))
        {
            AddFlag(ref tmp, "UNK1");
        }

        if (Convert.ToBoolean(iFlags & 16))
        {
            AddFlag(ref tmp, "UNK2");
        }

        if (Convert.ToBoolean(iFlags & 32))
        {
            AddFlag(ref tmp, "UNK3");
        }

        if (Convert.ToBoolean(iFlags & 64))
        {
            AddFlag(ref tmp, "GROUP_ONLY");
        }

        if (Convert.ToBoolean(iFlags & 128))
        {
            AddFlag(ref tmp, "UNK5");
        }

        if (Convert.ToBoolean(iFlags & 256))
        {
            AddFlag(ref tmp, "DYNAMIC");
        }

        return tmp;
    }

}
