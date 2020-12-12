using System.IO;

namespace osuserver2012.Packets.In
{
    public class NoOp : IPacketIn
    {
        private Stream _stream;
        
        public void ReadPacket() {}

        public void ProcessPacket(Context ctx, Stream stream) {}
    }
}