using System.IO;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class SpecatorJoinedHost : PacketOut
    {
        public int ID { get; set; }
        public override ushort id => 13;
        
        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write(ID);
            }
        }
    }
}