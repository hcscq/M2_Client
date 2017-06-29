using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ServerPacketsEx
{
    /*G sence packet*/
    public sealed class NewMap : Packet
    {
        public override short Index
        {
            get
            {
               return ServerMsgIds.SM_NEWMAP;
            }
        }
        public byte[] MapFileName;
        public byte[] MapName;
        public int CurrX { get { return wParam; } }
        public int CurrY { get { return wTag; } }
        public ushort MiniMap, BigMap, Music;
        public LightSetting Lights;
        public bool Lightning, Fire;
        public byte MapDarkLight { get { return (byte)wSeries; } }
        protected override void ReadPacket(BinaryReader reader)
        {
            MapFileName = reader.ReadBytes(16);
            MapName = reader.ReadBytes(40);
            MiniMap = reader.ReadUInt16();
            BigMap = reader.ReadUInt16();
            Music = reader.ReadUInt16();
            Lights = (LightSetting)reader.ReadSByte();
            Lightning = reader.ReadBoolean();
            Fire = reader.ReadBoolean();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class UserName : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_USERNAME;
            }
        }
        public Color NameColour { get { return Color.FromArgb(wParam); } }
        public byte[] CharName;
        protected override void ReadPacket(BinaryReader reader)
        {
            CharName = reader.ReadBytes(64);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
            
        }
    }
    public sealed class UseItems : Packet
    {
        public override short Index
        {
            get { return ServerMsgIds.SM_SENDUSEITEMS; }
        }

        public Guid ObjectID;

        public UserItem[]  Equipment;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = new Guid(new string(reader.ReadChars(GUIDLEN)));

            Equipment = new UserItem[10];
            for (int i = 0; i < Equipment.Length; i++)
            {
                if (reader.ReadBoolean()) continue;
                Equipment[i] = new UserItem(reader);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(ObjectID.ToByteArray());

            writer.Write(Equipment != null);
            if (Equipment != null)
            {
                writer.Write(Equipment.Length);
                for (int i = 0; i < Equipment.Length; i++)
                {
                    writer.Write(Equipment[i] != null);
                    if (Equipment[i] == null) continue;

                    Equipment[i].Save(writer);
                }
            }
        }
    }
    public sealed class MapLogon : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_LOGON;
            }
        }
        public int nCurrX { get { return wParam; } }
        public int nCUrrY { get { return wTag; } }
        public MirDirection btDirection { get { return (MirDirection)wSeries; } }
        public byte btLight { get { return (byte)(wSeries >> 8); } }

        public byte btGender;
        public byte btWear;
        public byte btHair;
        public byte btWeapon;
        public Guid CharID;
        protected override void ReadPacket(BinaryReader reader)
        {
            CharID = new Guid(new string(reader.ReadChars(Packet.GUIDLEN)));
            btGender=reader.ReadByte();
            btWear=reader.ReadByte();
            btHair=reader.ReadByte();
            btWeapon= reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class CharStatusChanged : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_CHARSTATUSCHANGED;
            }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
           
        }

        protected override void WritePacket(BinaryWriter writer)
        {

        }
    }
    public sealed class Turn : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_TURN;
            }
        }
        public long Feature;
        public long Status;
        byte btHorse;
        short dwHairColor;
        short dwWearColor;
        byte[] CharName;
        protected override void ReadPacket(BinaryReader reader)
        {
            Feature= reader.ReadInt64();
            Status = reader.ReadInt64();
            if (reader.ReadBoolean())
            {
                btHorse = reader.ReadByte();
                dwHairColor = reader.ReadInt16();
                dwWearColor = reader.ReadInt16();
            }
            if (reader.ReadBoolean())
                CharName = reader.ReadBytes(CHARNAMELEN);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
