using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ServerPacketsEx
{
    /*G sence packet*/
    public sealed class ActionResult : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_ACTIONRESULT;
            }
        }
        public byte[] ResultMsg;

        protected override void ReadPacket(BinaryReader reader)
        {

            ResultMsg = reader.ReadBytes((int)reader.BaseStream.Length);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
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

        public UserItem[] Equipment;

        protected override void ReadPacket(BinaryReader reader)
        {
            ObjectID = new Guid((reader.ReadBytes(GUIDLEN)));

            Equipment = new UserItem[10];
            for (int i = 0; i < Equipment.Length; i++)
            {
                if (reader.ReadBoolean()) continue;// { reader.ReadBytes(204); continue; }
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
    public sealed class BagItems : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_BAGITEMS;
            }
        }
        public UserItem[] Inventory;
        public short Count { get { return wSeries; } }
        protected override void ReadPacket(BinaryReader reader)
        {
            Inventory = new UserItem[46];
            for (int i = 0; i < Count; i++)
            {

                Inventory[i] = new UserItem(reader);
            }
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class DropItem : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_DROPITEM;
            }
        }
        public Guid UniqueID;
        public bool Success { get { return nRecog > 0; } }

        protected override void ReadPacket(BinaryReader reader)
        {
            UniqueID = new Guid((reader.ReadBytes(GUIDLEN)));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class ItemShow : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_ITEMSHOW;
            }
        }
        //AddRefMsg(RM_ITEMSHOW, xpMapItem->wLooks,(int)xpMapItem,          nX,                         nY,                         xpMapItem->szName);
        //fnMakeDe(&DefMsg,      SM_ITEMSHOW,       lpProcessMsg->lParam1,(WORD)lpProcessMsg->lParam2, (WORD)lpProcessMsg->lParam3, lpProcessMsg->wParam);
        //fnMakeDefMe(lptdm, wIdent,     int nRecog,            WORD wParam,                 WORD wTag,                   WORD wSeries,int nlen=0)
        //public Guid ObjectID;
        public string Name = string.Empty;
        public Color NameColour;
        public Point Location { get { return new Point(wParam, wTag); } }
        public ushort Image { get { return (ushort)wSeries; } }
        public int ObjectID { get { return nRecog; } }
        //public ItemGrade grade;
        protected override void ReadPacket(BinaryReader reader)
        {
            Name = Packet.GetString(reader.ReadBytes(ITEMNAELEN));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class ItemHide : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_ITEMHIDE;
            }
        }
        public int ObjectID { get { return nRecog; } }
        public Point Location { get { return new Point(wParam, wTag); } }
        //AddRefMsg(    RM_ITEMHIDE, 0,         (int)pMapItem,  m_nCurrX,      m_nCurrY,        NULL);
        //AddRefMsg(WORD wIdent,    WORD wParam, DWORD lParam1, DWORD lParam2, DWORD lParam3, char *pszData)
        //fnMakeDefMe(lptdm, wIdent,     int nRecog,            WORD wParam,                 WORD wTag,                   WORD wSeries,int nlen=0)
        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
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
        public int nObjectId { get { return nRecog; } }
        protected override void ReadPacket(BinaryReader reader)
        {
            //CharID = new Guid(reader.ReadBytes(16));
            
            CharID = new Guid(reader.ReadBytes(Packet.GUIDLEN));
            //nObjectId = reader.ReadInt32();
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
    }//SM_OBJDISAPPEAR
    public sealed class HumDisappear : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_HUMDISAPPEAR;
            }
        }
        public int ObjectID { get { return nRecog; } }
        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class HumShow : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_HUMSHOW;
            }
        }
        public int ObjectID { get { return nRecog; } }
        public long Feature;
        public long Status;
        public byte btHorse;
        public short dwHairColor;
        public short dwWearColor;
        public string CharName;
        protected override void ReadPacket(BinaryReader reader)
        {
            Feature = reader.ReadInt64();
            Status = reader.ReadInt64();
            if (reader.ReadBoolean())
            {
                btHorse = reader.ReadByte();
                dwHairColor = reader.ReadInt16();
                dwWearColor = reader.ReadInt16();
            }
            if (reader.ReadBoolean())
                CharName =GetString(reader.ReadBytes(CHARNAMELEN));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
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
        public byte btHorse;
        public short dwHairColor;
        public short dwWearColor;
        public byte[] CharName;
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
    public sealed class Walk : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_WALK;
            }
        }
        public long Feature;
        public long Status;
        public byte btHorse;
        public short dwHairColor;
        public short dwWearColor;
        public byte[] CharName;
        protected override void ReadPacket(BinaryReader reader)
        {
            Feature = reader.ReadInt64();
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
    public sealed class Run : Packet
    {
        public override short Index
        {
            get
            {
                return (short)ServerPacketIds.SM_RUN;
            }
        }
        public long Feature;
        public long Status;
        public byte btHorse;
        public short dwHairColor;
        public short dwWearColor;
        public byte[] CharName;
        protected override void ReadPacket(BinaryReader reader)
        {
            Feature = reader.ReadInt64();
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
    public sealed class Ablity : Packet
    {
        public byte Level;

        public ushort HP;
        public ushort MP;
        public ushort MaxHP;
        public ushort MaxMP;
        public ushort Weight;
        public ushort MaxWeight;

        public uint Exp;
        public uint MaxExp;

        public byte WearWeight;
        public byte MaxWearWeight;
        public byte HandWeight;
        public byte MaxHandWeight;

        public ushort DC;
        public ushort MC;
        public ushort SC;
        public ushort AC;
        public ushort MAC;

        public short m_wWater;
        public short m_wFire;
        public short m_wWind;
        public short m_wLight;
        public short m_wEarth;
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_ABILITY;
            }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Level = reader.ReadByte();

            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            MaxHP = reader.ReadUInt16();
            MaxMP = reader.ReadUInt16();
            Weight = reader.ReadUInt16();
            MaxWeight = reader.ReadUInt16();

            Exp = reader.ReadUInt32();
            MaxExp = reader.ReadUInt32();

            WearWeight = reader.ReadByte();
            MaxWearWeight = reader.ReadByte();
            HandWeight = reader.ReadByte();

            DC = reader.ReadUInt16();
            MC = reader.ReadUInt16();
            SC = reader.ReadUInt16();
            AC = reader.ReadUInt16();
            MAC = reader.ReadUInt16();

            m_wWater = reader.ReadInt16();
            m_wFire = reader.ReadInt16();
            m_wWind = reader.ReadInt16();
            m_wLight = reader.ReadInt16();
            m_wEarth = reader.ReadInt16();

        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class Subability : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_SUBABILITY;
            }
        }
        public byte m_btAntiMagic { get { return (byte)nRecog; } }
        public byte m_btHitPoint { get { return (byte)wParam; } }
        public byte m_btSpeedPoint { get { return (byte)(wParam>>8); } }
        public byte m_btAntiPoison { get { return (byte)wTag; } }
        public byte m_btPoisonRecover { get { return (byte)(wTag>>8); } }
        public byte m_btHealthRecover { get { return (byte)wSeries; } }
        public byte m_btSpellRecover { get { return (byte)(wSeries>>8); } }
        protected override void ReadPacket(BinaryReader reader)
        {
            
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class DayChanging : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_DAYCHANGING;
            }
        }
        public byte m_btBright { get { return (byte)wParam; } }
        protected override void ReadPacket(BinaryReader reader)
        {

        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class SendMagic : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_SENDMYMAGIC;
            }
        }
        public short Count{get{ return wSeries; }}
        public List<ClientMagic> Magics = new List<ClientMagic>();
        protected override void ReadPacket(BinaryReader reader)
        {
            for (int i = 0; i < Count; i++)
                Magics.Add(new ClientMagic(reader));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class HearedMessage : Packet
    {
        public override short Index
        {
            get
            {
                return ServerMsgIds.SM_HEAR;
            }
        }
        public string Message;
        public MessageType Type { get { return (MessageType)wParam; } set { wParam = (short)value; } }
        protected override void ReadPacket(BinaryReader reader)
        {
            Message = GetString(reader.ReadBytes(MAXTEXTMSGLEN));
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class ChangeAMode : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.SM_ATTACKMODE; }
        }

        public AttackMode Mode { get { return (AttackMode)wSeries; }  }

        protected override void ReadPacket(BinaryReader reader)
        {
        }

        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }
    public sealed class AddItem : Packet
    {
        public override short Index
        {
            get { return (short)ServerPacketIds.SM_ADDITEM; }
        }
        public UserItem Item;
        protected override void ReadPacket(BinaryReader reader)
        {
            Item = new UserItem(reader);
        }
        protected override void WritePacket(BinaryWriter writer)
        {
        }
    }

}
