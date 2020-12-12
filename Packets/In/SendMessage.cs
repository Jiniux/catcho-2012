using System.IO;

namespace osuserver2012.Packets.In
{
    public class SendMessage : IPacketIn
    {
        private Stream _stream;
        
        public Message Message = new Message();

        public void ReadPacket()
        {
            BinaryReader reader = new BinaryReader(_stream);

            reader.ReadBytes(10);
            Message.Contents = reader.ReadString();
            reader.ReadByte();
            Message.Target = reader.ReadString();
        }

        public void ProcessPacket(Context ctx, Stream stream)
        {
            _stream = stream;
            
            ReadPacket();
            
            Message.Sender = ctx.User.username;
            
            ctx.Server.BroadcastPacket(new Packets.Out.SendMessage { Message = Message });
        }
    }
}