using System.IO;
using System.Net.Sockets;

namespace osuserver2012.Packets.In
{
    public class ChannelJoin : IPacketIn
    {
        private Stream _stream;

        public string ChannelName { get; set; }

        public void ReadPacket()
        {
            BinaryReader reader = new BinaryReader(_stream);

            reader.ReadByte();
            ChannelName = reader.ReadString();
        }

        public void ProcessPacket(Context ctx, Stream stream)
        {
            _stream = stream;
            
            ReadPacket();
            
            ctx.User.QueuePacket(new Packets.Out.ChannelJoin() { ChannelName = ChannelName });
        }
    }
}