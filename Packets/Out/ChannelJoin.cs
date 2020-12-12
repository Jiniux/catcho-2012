using System.IO;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class ChannelJoin : PacketOut
    {
        public string ChannelName { get; set; }
        public override ushort id => 64;
        
        protected override void WritePayload(Stream buffer)
        {
            using var writer = new BinaryWriter(buffer, Encoding.UTF8, true);
            
            writer.Write((byte)11);
            writer.Write(ChannelName);
        }
    }
}