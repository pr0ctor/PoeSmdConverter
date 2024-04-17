using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PoeSmdConverter.Model;

namespace PoeSmdConverter.Helpers
{
    internal static class Helpers
    {

        public static void ValidateCommandlineArgs(IConfiguration config, string[] args)
        {
            if (args.Length <= 1)
            {
                throw new ArgumentException("Execution requires two arguments: --smd <path_to_file.smd> --obj <path_to_output.obj>");
            }

            if (config["smd"] == null)
            {
                throw new ArgumentException("Path to the source .SMD file is missing.");
            }

            if (config["obj"] == null)
            {
                throw new ArgumentException("Path to the destination .OBJ file is missing.");
            }
        }

        public static bool CheckForBrotliCompression(byte[] firstThreeBytes)
        {
            if(firstThreeBytes.Length < 3 &&  firstThreeBytes.Length >= 4)
            {
                throw new Exception("Invalid number of bytes for brotli compression check");
            }

            return firstThreeBytes.SequenceEqual(MagicNumbers.BrotliCompressionIndicators);
        }

        public static byte[] AdvanceStreamReader(Stream stream, int numberOfBytes)
        {
            byte[] buffer = new byte[numberOfBytes];

            stream.Read(buffer, 0, numberOfBytes);

            return buffer;
        }

        public static string DecodeUTF16(byte[] b)
        {
            if (b.Length % 2 != 0)
            {
                throw new ArgumentException("Must have even length byte array");
            }

            var u16s = new ushort[1];
            var ret = new StringWriter();
            var b8buf = new byte[4];
            // takes two bytes (b[i] and b[i + 1]), combines them into a 16-bit unsigned integer, converts it into a character array, encodes the first character from the array into UTF-8 bytes, and then decodes those bytes back into a string for output.
            for (int i = 0; i < b.Length; i += 2)
            {
                // This line combines two bytes (b[i] and b[i + 1]) using bitwise shifting and addition to create a 16-bit unsigned integer (ushort) value. It uses unchecked to prevent overflow checking.
                u16s[0] = unchecked((ushort)(b[i] + (b[i + 1] << 8)));
                //  the above 16-bit unsigned integer value (u16s[0]) is passed to BitConverter.GetBytes() to convert it into a byte array. Then, System.Text.Encoding.Unicode.GetChars() is used to convert the byte array into a character array (char[]).
                char[] r = System.Text.Encoding.Unicode.GetChars(BitConverter.GetBytes(u16s[0]));
                // This line uses the Encoding.UTF8.GetBytes() method to encode the first character from the r character array into UTF-8 bytes. It stores the number of bytes written to the b8buf byte array in the variable n.
                int n = Encoding.UTF8.GetBytes(r, 0, 1, b8buf, 0);
                // the Encoding.UTF8.GetString() method is used to decode the UTF-8 bytes from b8buf starting from index 0 up to the value of n. The resulting string is then written to some output (ret).
                ret.Write(Encoding.UTF8.GetString(b8buf, 0, n));
            }

            return ret.ToString();
        }
    }
}
