using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Mirage.Sharp.Asfw.IO;

public class Compression
{
    public static byte[] CompressBytes(byte[] value)
    {
        int length = value.Length;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Compress))
                gzipStream.Write(value, 0, length);
            return memoryStream.ToArray();
        }
    }

    public static byte[] DecompressBytes(byte[] value)
    {
        int int32 = BitConverter.ToInt32(value, value.Length - 4);
        byte[] buffer = new byte[int32];

        using (MemoryStream memoryStream = new MemoryStream(value))
        using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            int bytesRead;
            int offset = 0;

            // Read in a loop to ensure all data is decompressed.
            while ((bytesRead = gzipStream.Read(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += bytesRead;
            }

            if (offset != int32)
            {
                throw new InvalidOperationException("Decompressed data size does not match the expected size.");
            }
        }

        return buffer;
    }

    public static async Task<byte[]> DecompressBytesAsync(byte[] value)
    {
        int int32 = BitConverter.ToInt32(value, value.Length - 4);
        byte[] buffer = new byte[int32];
        using (MemoryStream ms = new MemoryStream(value))
        {
            using (GZipStream gs = new GZipStream((Stream) ms, CompressionMode.Decompress))
            {
                int num = await gs.ReadAsync(buffer, 0, int32);
            }
        }

        byte[] numArray = buffer;
        buffer = (byte[]) null;
        return numArray;
    }

    public static byte[] DecompressFile(string path)
    {
        byte[] buffer1 = File.ReadAllBytes(path);
        int int32 = BitConverter.ToInt32(buffer1, buffer1.Length - 4);
        byte[] buffer2 = new byte[int32];
        using (MemoryStream memoryStream = new MemoryStream(buffer1))
        {
            using (GZipStream gzipStream = new GZipStream((Stream) memoryStream, CompressionMode.Decompress))
                gzipStream.Read(buffer2, 0, int32);
        }

        return buffer2;
    }

    public static async Task<byte[]> DecompressFileAsync(string path)
    {
        byte[] buffer1 = File.ReadAllBytes(path);
        int int32 = BitConverter.ToInt32(buffer1, buffer1.Length - 4);
        byte[] buffer = new byte[int32];
        using (MemoryStream ms = new MemoryStream(buffer1))
        {
            using (GZipStream gs = new GZipStream((Stream) ms, CompressionMode.Decompress))
            {
                int num = await gs.ReadAsync(buffer, 0, int32);
            }
        }

        byte[] numArray = buffer;
        buffer = (byte[]) null;
        return numArray;
    }
}