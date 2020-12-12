using System.IO;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class SendMessage : PacketOut
    {
        public Message Message { get; set; }
        public override ushort id => 7;
        
        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write((byte)11);
                writer.Write(Message.Sender);
                writer.Write((byte)11);
                writer.Write(Message.Contents);
                writer.Write((byte)11);
                writer.Write(Message.Target);
            }
        }
    }
}