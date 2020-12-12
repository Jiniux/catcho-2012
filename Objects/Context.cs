using System.Threading;
using osuserver2012.MySql;

namespace osuserver2012
{
    public class Context
    {
        public User User { get; init; }
        public Server Server { get; init; }
        public Database Database { get; init; }
        public CancellationTokenSource TokenSource { get; init; }
        public CancellationToken Token { get; init; }
        public PlayerInfo PlayerInfo { get; set; }
    }
}