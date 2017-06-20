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
        //Title = p.Title,
        //        MiniMap = p.MiniMap,
        //        BigMap = p.BigMap,
        //        Lights = p.Lights,
        //        Lightning = p.Lightning,
        //        Fire = p.Fire,
        //        MapDarkLight = p.MapDarkLight,
        //        Music = p.Music
        protected override void ReadPacket(BinaryReader reader)
        {
            MapFileName = reader.ReadBytes(16);
            MapName = reader.ReadBytes(40);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
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

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
