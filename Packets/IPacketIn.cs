using System;
using System.IO;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;

namespace osuserver2012.Packets
{
    interface IPacketIn
    {
        public static async Task<IPacketIn> ReadHeaderAsync(Stream stream, Context ctx)
        {
            ushort id;
            bool compress;
            uint length;
            
            using var reader = new BinaryReader(stream, Encoding.Default, leaveOpen: true);

            id = reader.ReadUInt16();
            compress = reader.ReadBoolean();
            length = reader.ReadUInt32();

            // if (id > 100) => do something to block it cuz it isnt a packet
            
            var buffer = new byte[length];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            
            Console.WriteLine(id);

            IPacketIn packet = id switch
            {
                // implement packets..
                0 => new Packets.In.PlayerInfoUpdate(),
                1 => new Packets.In.SendMessage(),
                2 => new Packets.In.LogOut(),
                3 => new Packets.In.StatusUpdate(),
                4 => new Packets.In.NoOp(),
                63 => new Packets.In.ChannelJoin(),/*
                16 => new Packets.In.StartSpectating(),
                17 => new Packets.In.StopSpectating(),
                18 => new Packets.In.FrameBundle(),
                25 => new Packets.In.SendMessagePM(),
                29 => new Packets.In.LeaveLobby(),
                30 => new Packets.In.JoinLobby(),
                
                85 => new Packets.In.ReceiveUserStats(),
                */
                _ => new Packets.In.NoOp()
            };
            
            return packet;
        }

        void ReadPacket();
        void ProcessPacket(Context ctx, Stream stream);
    }
}