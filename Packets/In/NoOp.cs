using System.IO;

namespace osuserver2012.Packets.In
{
    public class NoOp : IPacketIn
    {
        public void ReadPacket(Stream stream) {}

        public void ProcessPacket(Context ctx) {}
    }
}