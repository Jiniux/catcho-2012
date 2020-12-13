using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using static osuserver2012.Extensions;
using MySql.Data.MySqlClient;
using osuserver2012.Packets;

using static osuserver2012.Enums.Gamemode;
using static osuserver2012.Enums.Status;

namespace osuserver2012.MySql
{
    public class User
    {
        public string username;
        public int id;
        
        private TcpClient _socket;
        private Database _database;
        private NetworkStream _networkStream;
        private Server _server;
        internal Context ctx;

        private BlockingCollection<PacketOut> OutgoingPacketQueue = new BlockingCollection<PacketOut>();

        public CancellationTokenSource TokenSource = new CancellationTokenSource();

        public User(TcpClient socket, Database database, Server server)
        {
            _socket = socket;
            _database = database;
            _server = server;
            _networkStream = _socket.GetStream();
        }
        
        private async Task RunQueue(Context ctx) {
            for (;;)
            {
                try
                {
                    foreach (PacketOut packet in this.OutgoingPacketQueue.GetConsumingEnumerable())
                    {
                        packet.Send(_networkStream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ctx.TokenSource.Cancel();
                }
                await Task.Delay(10, ctx.TokenSource.Token);
            }
            
        }

        public void QueuePacket(PacketOut packet) {
            try {
                this.OutgoingPacketQueue.Add(packet);
            } catch (Exception ex) { Console.WriteLine(ex); } 
        }

        public async Task<bool> Authenticate()
        {
            var credentials = Task.Run(() =>
            {
                var username = LimitedReadLine(_networkStream, 64).Replace("\r", "");
                var password = LimitedReadLine(_networkStream, 64).Replace("\r", "");
                var info = LimitedReadLine(_networkStream, 512);

                return (username, password, info);
            });
            
            var (loginUsername, password, info) = credentials.Result;

            var query = _database.Get("SELECT username, userid FROM osu_users where username = @username and password = @password;",  new [] {
                new MySqlParameter("@username", loginUsername),
                new MySqlParameter("@password", password)
            });

            if (query != null)
            {
                username = loginUsername;
                id = int.Parse(query["userid"]);
                return true;
            }
            else
                return false;
        }

        private async Task ListenForPackets(Context ctx)
        {
            try
            {
                for (;;)
                {
                    if (_networkStream.CanRead && _networkStream.DataAvailable)
                        (await IPacketIn.ReadHeaderAsync(_networkStream)).ProcessPacket(ctx);
                    await Task.Delay(1, ctx.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task Pinger(Context ctx)
        {
            try
            { 
                for (;;)
                {
                    QueuePacket(new Packets.Out.Ping());
                    await Task.Delay(15000, ctx.Token);
                }
            }
            catch (Exception e) 
            {
                if (e is not OperationCanceledException)
                {
                    Console.WriteLine(e); 
                    ctx.TokenSource.Cancel();
                }
            }
        }

        public async Task Start()
        {
            await Task.WhenAny(new List<Task>() { Pinger(ctx), ListenForPackets(ctx), RunQueue(ctx) });
        }
    }
}