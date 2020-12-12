using System.IO;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class SpectatorFrames : PacketOut
    {
        public byte[] Frames { get; set; }
        public override ushort id => 15;
        
        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write(Frames);
            }
        }
    }
}