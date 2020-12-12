using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using NUnit.Framework.Constraints;

namespace osuserver2012.Packets
{
    public class PacketOut
    {
        private byte[] payload;
        public virtual ushort id { get; init; } = 0;

        protected virtual void WritePayload(Stream buffer) {}

        public virtual void Send(NetworkStream ns)
        {
            using var buffer = new MemoryStream();

            using (var writer = new BinaryWriter(buffer, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(id);
                writer.Write((byte)0);
                writer.Write((int)0); // What is this int doing here???: becuz prepending wouldnt work, to get the length of the payload we need to do this and stuffoiufdsoni
                
                WritePayload(buffer);

                buffer.Position = 3;
                
                writer.Write((int)buffer.Length-7);
            }

            payload = buffer.ToArray();
            if (id < 100)
                ns.Write(payload, 0, payload.Length);
        }
    }
}