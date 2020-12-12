using System.IO;
using System.Text;

namespace osuserver2012
{
    public static class Extensions
    {
        public static string LimitedReadLine(Stream stream, uint limit)
        {
            var sb = new StringBuilder();

            int c;
            while (limit-- > 0 && (c = stream.ReadByte()) != '\n') { 
                sb.Append((char)c);
            }

            return sb.ToString();
        }

            /* JUST DO reader.readByte(); reader.ReadString();
            public static string ReadBString(this BinaryReader reader)
            {
                byte header = reader.ReadByte();
            
                if (header == 11)
                    return reader.ReadString();

                return null;
            }
            */

        public static void WriteBString(BinaryWriter writer, string s)
        {
            if (s.Length == 0)
            { 
                writer.Write((byte)0x00);
                return;
            }

            writer.Write((byte)11);
            writer.Write(s);
        }
    }
}