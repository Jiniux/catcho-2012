using System;
using System.IO;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class StatusUpdate : PacketOut
    {
        public PlayerInfo PlayerInfo { get; set; }
        public Context Ctx { get; set; }

        public override ushort id => 11;

        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write(Ctx.User.id);
                writer.Write((byte)PlayerInfo.Status);
                writer.Write((byte)11);
                writer.Write(PlayerInfo.Action);
                writer.Write((byte)11);
                writer.Write(PlayerInfo.ActionMD5);
                writer.Write(PlayerInfo.Mods);
                writer.Write((byte)PlayerInfo.Gamemode);
                writer.Write(PlayerInfo.MapID);
                writer.Write(PlayerInfo.RankedScore);
                writer.Write(PlayerInfo.Accuracy/100);
                writer.Write(PlayerInfo.PlayCount);
                writer.Write(PlayerInfo.Score);
                writer.Write(PlayerInfo.Rank);
            }
        }
    }
}