using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashChecker
{
    public class HashCalculatord
    {
        public FileInfo File { get; }
        public byte[] MD5Hash { get; private set; } = Array.Empty<byte>();
        public byte[] SHA1Hash { get; private set; } = Array.Empty<byte>();
        public byte[] SHA256Hash { get; private set; } = Array.Empty<byte>();
        public byte[] SHA384Hash { get; private set; } = Array.Empty<byte>();
        public byte[] SHA512Hash { get; private set; } = Array.Empty<byte>();

        public HashCalculatord(FileInfo file)
        {
            File = file;

            MD5Hash = MD5.HashData(File.OpenRead());
            SHA1Hash = SHA1.HashData(File.OpenRead());
            SHA256Hash = SHA256.HashData(File.OpenRead());
            SHA384Hash = SHA384.HashData(File.OpenRead());
            SHA512Hash = SHA512.HashData(File.OpenRead());
        }

    }
}
