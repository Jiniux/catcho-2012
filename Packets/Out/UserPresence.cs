using System.IO;
using System.Text;
using osuserver2012.MySql;

namespace osuserver2012.Packets.Out
{
    public class UserPresence : PacketOut
    {
        public Context Context { get; set; }
        public override ushort id => 83;
        
        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write(Context.User.id);
                writer.Write(Context.User.username);
                writer.Write((byte)0); // What is this?
                writer.Write((byte)14); // these too???
                writer.Write((byte)14); // <-
                writer.Write((byte)11);
                writer.Write("Unknown"); // city
                writer.Write((byte)0); // what is this..
                writer.Write(1.0f); // longitude (world map)
                writer.Write(1.0f); // latitude
            }
        }
    }
}