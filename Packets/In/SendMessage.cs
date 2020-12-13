using System.IO;

namespace osuserver2012.Packets.In
{
    public class SendMessage : IPacketIn
    {
        public Message Message = new Message();

        public void ReadPacket(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);

            reader.ReadBytes(10);
            Message.Contents = reader.ReadString();
            reader.ReadByte();
            Message.Target = reader.ReadString();
        }

        public void ProcessPacket(Context ctx)
        {
            Message.Sender = ctx.User.username;
            
            ctx.Server.BroadcastPacket(new Packets.Out.SendMessage { Message = Message });
        }
    }
}