using System;
using System.IO;
using MySql.Data.MySqlClient;
using osuserver2012.MySql;

namespace osuserver2012.Packets.In
{
    public class StatusUpdate : IPacketIn
    {
        public void ReadPacket(Stream stream) {}

        public void ProcessPacket(Context ctx)
        {
            var query = ctx.Database.Get("select * from (select *, ROW_NUMBER() OVER(PARTITION BY rankedscore) AS 'rank' from osu_users) t WHERE username = @username;", new[] {
                new MySqlParameter("@username", ctx.User.username),
            });

            ctx.PlayerInfo.Rank = int.Parse(query["rank"]);
            Console.WriteLine(query["rank"]);
            ctx.PlayerInfo.Accuracy = float.Parse(query["accuracy"]);
            ctx.PlayerInfo.RankedScore = int.Parse(query["rankedscore"]);
            ctx.PlayerInfo.Score = int.Parse(query["totalscore"]);
            ctx.PlayerInfo.PlayCount = int.Parse(query["playcount"]);
            
            ctx.User.QueuePacket(new Packets.Out.StatusUpdate() { Ctx = ctx, PlayerInfo = ctx.PlayerInfo });
        }
    }
}