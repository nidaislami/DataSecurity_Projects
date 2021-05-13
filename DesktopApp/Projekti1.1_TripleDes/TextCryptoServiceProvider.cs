using System.Security.Cryptography;

namespace Projekti1._1_TripleDes
{
    internal class TextCryptoServiceProvider
    {
        public byte[] Key { get; internal set; }
        public object Mode { get; internal set; }
        public PaddingMode Padding { get; internal set; }
    }
}