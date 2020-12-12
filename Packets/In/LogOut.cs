using System.IO;

namespace osuserver2012.Packets.In
{
    public class LogOut : IPacketIn
    {
        private Stream _stream;
        
        public void ReadPacket() { }

        public void ProcessPacket(Context ctx, Stream stream)
        {
            _stream = stream;
            
            ReadPacket();
            
            ctx.Server.BroadcastPacket(new Packets.Out.LogOut { ID = ctx.User.id });
        }
    }
}