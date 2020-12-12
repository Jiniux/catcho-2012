using System;
using System.IO;
using osuserver2012.Enums;

namespace osuserver2012.Packets.In
{
    public class PlayerInfoUpdate : IPacketIn
    {
        private Stream _stream;
        
        public Status Status { get; set; }
        public string Action { get; set; }
        public string ActionMD5 { get; set; }
        public short Mods { get; set; }
        public Gamemode Gamemode { get; set; }
        public int MapID { get; set; }

        public void ReadPacket()
        {
            using BinaryReader reader = new BinaryReader(_stream);
            
            Status = (Status)reader.ReadByte();
            reader.ReadByte();
            Action = reader.ReadString();
            reader.ReadByte();
            ActionMD5 = reader.ReadString();
            Mods = reader.ReadInt16();
            Gamemode = (Gamemode)reader.ReadByte();
            MapID = reader.ReadInt32();
        }

        public void ProcessPacket(Context ctx, Stream stream)
        {
            _stream = stream;
            
            ReadPacket();
            
            ctx.PlayerInfo.Status = Status;
            ctx.PlayerInfo.Action = Action;
            ctx.PlayerInfo.ActionMD5 = ActionMD5;
            ctx.PlayerInfo.Mods = Mods;
            ctx.PlayerInfo.Gamemode = Gamemode;
            ctx.PlayerInfo.MapID = MapID;
            
            ctx.Server.BroadcastPacket(new Packets.Out.StatusUpdate { Ctx = ctx, PlayerInfo = ctx.PlayerInfo });
        }
    }
}