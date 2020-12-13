using System.IO;
using System.Net.Sockets;

namespace osuserver2012.Packets.In
{
    public class ChannelJoin : IPacketIn
    {
        public string ChannelName { get; set; }

        public void ReadPacket(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            reader.ReadByte();
            ChannelName = reader.ReadString();
        }

        public void ProcessPacket(Context ctx)
        {
            ctx.User.QueuePacket(new Packets.Out.ChannelJoin() { ChannelName = ChannelName });
        }
    }
}