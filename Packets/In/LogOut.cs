using System.IO;

namespace osuserver2012.Packets.In
{
    public class LogOut : IPacketIn
    {
        public void ReadPacket(Stream stream) { }

        public void ProcessPacket(Context ctx)
        {
            ctx.Server.BroadcastPacket(new Packets.Out.LogOut { ID = ctx.User.id });
        }
    }
}