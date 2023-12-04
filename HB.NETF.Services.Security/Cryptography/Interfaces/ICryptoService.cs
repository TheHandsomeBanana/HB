using HB.NETF.Services.Security.Cryptography.Keys;

namespace HB.NETF.Services.Security.Cryptography.Interfaces {
    public interface ICryptoService {

        byte[] Encrypt(byte[] data, IKey key);
        byte[] Decrypt(byte[] cipher, IKey key);

        IKey[] GenerateKeys(int keySize);
    }
}
