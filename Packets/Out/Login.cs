using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace osuserver2012.Packets.Out
{
    public class Login : PacketOut
    {
        public int Status { get; init; }
        public override ushort id => 5;

        protected override void WritePayload(Stream buffer)
        {
            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, true))
            {
                writer.Write(Status);
            }
        }
    }
}