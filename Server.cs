using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using osuserver2012.Packets;

using static osuserver2012.Enums.Gamemode;
using static osuserver2012.Enums.Status;

namespace osuserver2012.MySql
{
    public class Server
    {
        private Database _database;
        private TcpListener _listener;

        public static Dictionary<int, User> Users = new Dictionary<int, User>();

        public Server(string ip, int port, Database database)
        {
            _database = database;
            _listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public async Task Start()
        {
            _listener.Start();
            Console.WriteLine("server start!");
            
            for (;;)
            {
                var client = await _listener.AcceptTcpClientAsync();
                var user = new User(client, _database, this);

                Console.WriteLine("user connected!");
                // starting a new job for handling the client
                await Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        if (await user.Authenticate())
                        {
                            user.QueuePacket(new Packets.Out.Login() {Status = user.id});
                            lock (Users) { Users.Add(user.id, user); }
                            
                            var query = _database.Get("select * from (select *, ROW_NUMBER() OVER(PARTITION BY rankedscore) AS 'rank' from osu_users) t WHERE username = @username;", new[] {
                                new MySqlParameter("@username", user.username),
                            });
                            
                            user.ctx = new Context() {
                                User = user,
                                Server = this,
                                Database = _database, 
                                TokenSource = user.TokenSource,
                                Token = user.TokenSource.Token,
                                PlayerInfo = new PlayerInfo() { 
                                    Score = int.Parse(query["totalscore"]),
                                    RankedScore = int.Parse(query["rankedscore"]),
                                    Accuracy = float.Parse(query["accuracy"]), 
                                    PlayCount = int.Parse(query["playcount"]),
                                    Rank = int.Parse(query["rank"]),
                                    Action = "", 
                                    ActionMD5 = "", 
                                    Gamemode = Standard, 
                                    Mods = 0,
                                    MapID = 0,
                                    Status = Idle
                                }
                            };
                            
                            user.QueuePacket( new Packets.Out.StatusUpdate() { Ctx = user.ctx, PlayerInfo = user.ctx.PlayerInfo } );
                            user.QueuePacket( new Packets.Out.ChannelJoin() { ChannelName = "#osu" } );
                            
                            await user.Start();
                        }
                        else
                        {
                            user.QueuePacket(new Packets.Out.Login() {Status = -1});
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        lock (Users) { Users.Remove(user.id); }
                        Console.WriteLine("closed");
                        //client.Close();
                    }
                }, user.TokenSource.Token);
            }
        }

        public void BroadcastPacket(PacketOut packet)
        {
            lock (Users)
            {
                foreach (User u in Users.Values) 
                    try {
                        u.QueuePacket(packet);
                    } catch { }
            }
        }
        
        public void BroadcastPacketToOthers(PacketOut packet, User user) 
        {
            lock (Users)
            {
                foreach (User u in Users.Values)
                {
                    if (u.id != user.id)
                        u.QueuePacket(packet);
                }
            }
        }

        public static void QueuePacketSpecificUser(PacketOut packet, string user)
        {
            lock (Users)
            {
                foreach (User u in Users.Values)
                {
                    if (u.username == user)
                        u.QueuePacket(packet);
                }
            }
        }
    }
}