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
using System.Runtime.InteropServices;

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

    [StructLayout(LayoutKind.Sequential)]
    private struct TypeEntry
    {
        public int Name;
        public int Offset;
        public int Size;
        public int Type;
        public int Flags;
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
            PrintHeader(w);
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
            PrintHeader(w);
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
            PrintHeader(w);
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
        var TBC = 0;
        var alpha = 0;
        FileStream f = new(WowClient, FileMode.Open, FileAccess.Read, FileShare.Read, 10000000);
        BinaryReader r1 = new(f);
        StreamReader r2 = new(f);

        var outputFile = WowClientVersion.FileMajorPart + "." + WowClientVersion.FileMinorPart + "."
            + WowClientVersion.FileBuildPart + "." + WowClientVersion.FilePrivatePart + "_Global.UpdateFields.cs";
        outputFile = Path.Combine(OutputDirectory, outputFile);
        FileStream o = new(outputFile, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
        StreamWriter w = new(o);
        var FIELD_NAME_OFFSET = Utils.SearchInFile(f, "CORPSE_FIELD_PAD");
        var OBJECT_FIELD_GUID = Utils.SearchInFile(f, "OBJECT_FIELD_GUID") + 0x400000;
        var FIELD_TYPE_OFFSET = Utils.SearchInFile(f, OBJECT_FIELD_GUID);
#if DEBUG
        logger.Debug("FIELD_NAME_OFFSET " + FIELD_NAME_OFFSET + " OBJECT_FIELD_GUID " + OBJECT_FIELD_GUID + " FIELD_TYPE_OFFSET " + FIELD_TYPE_OFFSET);
#endif
        if (FIELD_NAME_OFFSET == -1) // pre 1.5 vanilla support
        {
            FIELD_NAME_OFFSET = Utils.SearchInFile(f, "CORPSE_FIELD_FLAGS");
        }
        if (FIELD_NAME_OFFSET == -1) // alpha support
        {
            FIELD_NAME_OFFSET = Utils.SearchInFile(f, "CORPSE_FIELD_LEVEL");
            alpha = 1;
        }
        if (FIELD_TYPE_OFFSET == -1) // TBC support
        {
            OBJECT_FIELD_GUID = Utils.SearchInFile(f, "OBJECT_FIELD_GUID") + 0x1A00 + 0x400000;
            FIELD_TYPE_OFFSET = Utils.SearchInFile(f, OBJECT_FIELD_GUID);
            TBC = 1;
        }
        if (FIELD_NAME_OFFSET == -1 || FIELD_TYPE_OFFSET == -1)
        {
            logger.Error("Wrong offsets! " + FIELD_NAME_OFFSET + "  " + FIELD_TYPE_OFFSET);
        }
        else
        {
            List<string> Names = new();
            var Last = "";
            var Offset = FIELD_NAME_OFFSET;
            f.Seek(Offset, SeekOrigin.Begin);
            while (Last != "OBJECT_FIELD_GUID")
            {
                Last = Utils.ReadString(f);
                Names.Add(Last);
            }

            List<TypeEntry> Info = new();
            int Temp;
            var Buffer = new byte[4];
            Offset = 0;
            f.Seek(FIELD_TYPE_OFFSET, SeekOrigin.Begin);
            for (int i = 0, loopTo = Names.Count - 1; i <= loopTo; i++)
            {
                f.Seek(FIELD_TYPE_OFFSET + (i * 5 * 4) + Offset, SeekOrigin.Begin);
                f.Read(Buffer, 0, 4);
                Temp = BitConverter.ToInt32(Buffer, 0);
                if (Temp < 0xFFFF)
                {
                    i -= 1;
                    Offset += 4;
                    continue;
                }

                TypeEntry tmp = new()
                {
                    Name = Temp
                };
                f.Read(Buffer, 0, 4);
                Temp = BitConverter.ToInt32(Buffer, 0);
                tmp.Offset = Temp;
                f.Read(Buffer, 0, 4);
                Temp = BitConverter.ToInt32(Buffer, 0);
                tmp.Size = Temp;
                f.Read(Buffer, 0, 4);
                Temp = BitConverter.ToInt32(Buffer, 0);
                tmp.Type = Temp;
                f.Read(Buffer, 0, 4);
                Temp = BitConverter.ToInt32(Buffer, 0);
                tmp.Flags = Temp;
                Info.Add(tmp);
            }

            logger.Info(string.Format("{0} fields extracted.", Names.Count));
            PrintHeader(w);
            var LastFieldType = "";
            string sName;
            string sField;
            var BasedOn = 0;
            var BasedOnName = "";
            Dictionary<string, int> EndNum = new();
            for (int j = 0, loopTo1 = Info.Count - 1; j <= loopTo1; j++)
            {
                sName = Utils.ReadString(f, Info[j].Name - 0x400000);
                if (TBC == 1) // TBC support
                {
                    sName = Utils.ReadString(f, Info[j].Name - (0x1A00 + 0x400000));
                }
                if (!string.IsNullOrEmpty(sName))
                {
                    sField = Utils.ToField(sName.Substring(0, sName.IndexOf("_")));
                    if (sName == "OBJECT_FIELD_CREATED_BY" && alpha == 0)
                    {
                        sField = "GameObject";
                    }

                    if (sName is "UINT_FIELD_BASESTAT0" or // alpha support
                        "UINT_FIELD_BASESTAT1" or
                        "UINT_FIELD_BASESTAT2" or
                        "UINT_FIELD_BASESTAT3" or
                        "UINT_FIELD_BASESTAT4" or
                        "UINT_FIELD_BYTES_1")
                    {
                        sField = "Unit";
                    }

                    if ((LastFieldType ?? "") != (sField ?? ""))
                    {
                        if (!string.IsNullOrEmpty(LastFieldType))
                        {
                            EndNum.Add(LastFieldType, Info[j - 1].Offset + 1);
                            if (LastFieldType.ToLower() == "object")
                            {
                                w.WriteLine("    {0,-78}", LastFieldType.ToUpper() + "_END = " + ToHex(Info[j - 1].Offset + Info[j - 1].Size));
                            }
                            else
                            {
                                w.WriteLine("    {0,-78}// 0x{1:X3}", LastFieldType.ToUpper() + "_END = " + BasedOnName + " + " + ToHex(Info[j - 1].Offset + Info[j - 1].Size), BasedOn + Info[j - 1].Offset + Info[j - 1].Size);
                            }

                            w.WriteLine("}");
                        }

                        w.WriteLine("Public Enum E" + sField + "Fields");
                        w.WriteLine("{");
#if DEBUG
                        logger.Debug("sField: " + sField + "\nsName: " + sName);
#endif
                        if (TBC == 1) // TBC support
                        {
                            if (sField.ToLower() == "item")
                            {
                                BasedOn = EndNum["Container"];
                                BasedOnName = "EContainerFields.CONTAINER_END";
                            }
                            else if (sField.ToLower() == "player")
                            {
                                BasedOn = EndNum["Unit"];
                                BasedOnName = "EUnitFields.UNIT_END";
                            }
                            else if (sField.ToLower() != "object")
                            {
                                BasedOn = EndNum["Object"];
                                BasedOnName = "EObjectFields.OBJECT_END";
                            }
                        }

                        if (TBC == 0)
                        {
                            if (sField.ToLower() == "container")
                            {
                                BasedOn = EndNum["Item"];
                                BasedOnName = "EItemFields.ITEM_END";
                            }
                            else if (sField.ToLower() == "player")
                            {
                                BasedOn = EndNum["Unit"];
                                BasedOnName = "EUnitFields.UNIT_END";
                            }
                            else if (sField.ToLower() != "object")
                            {
                                BasedOn = EndNum["Object"];
                                BasedOnName = "EObjectFields.OBJECT_END";
                            }
                        }

                        LastFieldType = sField;
                    }

                    if (BasedOn > 0)
                    {
                        w.WriteLine("    {0,-78}// 0x{1:X3} - Size: {2} - Type: {3} - Flags: {4}", sName + " = " + BasedOnName + " + " + ToHex(Info[j].Offset) + ",", BasedOn + Info[j].Offset, Info[j].Size, Utils.ToType(Info[j].Type), Utils.ToFlags(Info[j].Flags));
                    }
                    else
                    {
                        w.WriteLine("    {0,-78}// 0x{1:X3} - Size: {2} - Type: {3} - Flags: {4}", sName + " = " + ToHex(Info[j].Offset) + ",", Info[j].Offset, Info[j].Size, Utils.ToType(Info[j].Type), Utils.ToFlags(Info[j].Flags));
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastFieldType))
            {
                w.WriteLine("    {0,-78}// 0x{1:X3}", LastFieldType.ToUpper() + "_END = " + BasedOnName + " + " + ToHex(Info[^1].Offset + Info[^1].Size), BasedOn + Info[^1].Offset + Info[^1].Size);
            }

            w.WriteLine("}");
            w.Flush();
        }

        o.Close();
        f.Close();
    }

}
