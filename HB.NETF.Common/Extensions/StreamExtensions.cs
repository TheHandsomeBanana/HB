using System.IO;
using System.Threading.Tasks;

namespace HB.NETF.Common.Extensions {
    public static class StreamExtensions {
        public static byte[] Read(this Stream s) {
            return s.Read(s.Length);
        }

        public static byte[] Read(this Stream s, long length) {
            byte[] buffer = new byte[length];
            int numBytesToRead = buffer.Length;
            int numBytesRead = 0;

            while (numBytesToRead > 0) {
                // Read may return anything from 0 to numBytesToRead.
                int n = s.Read(buffer, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            return buffer;
        }

        public static void Write(this Stream s, byte[] buffer) {
            s.Write(buffer, 0, buffer.Length);
        }

        public static async Task<byte[]> ReadAsync(this Stream s) {
            return await s.ReadAsync(s.Length);
        }

        public static async Task<byte[]> ReadAsync(this Stream s, long length) {
            byte[] buffer = new byte[length];
            int numBytesToRead = buffer.Length;
            int numBytesRead = 0;

            while (numBytesToRead > 0) {
                // Read may return anything from 0 to numBytesToRead.
                int n = await s.ReadAsync(buffer, numBytesRead, numBytesToRead);

                // Break when the end of the file is reached.
                if (n == 0)
                    break;

                numBytesRead += n;
                numBytesToRead -= n;
            }

            return buffer;
        }

        public static async Task WriteAsync(this Stream s, byte[] buffer) {
            await s.WriteAsync(buffer, 0, buffer.Length);
        }

        public static void ResetPosition(this Stream s) {
            s.Position = 0;
        }
    }
}
