using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper
{
    private const int KeySize = 256;
    private const int BlockSize = 128;
    private const CipherMode Mode = CipherMode.CBC;
    private const PaddingMode Padding = PaddingMode.PKCS7;

    private const string Key = "ab2defghijkl4nopqrstuvw|yzabcdefghijklmnopqrstuvw3yzabcdef@hijklmnopqrstuvwx#zabcdefghijklmn%pqrstuvw*yzabcdefghijklmnopqr^tuv";

    public static string Encrypt(string plainText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        byte[] iv = new byte[BlockSize / 8]; // IV size is same as block size (128 bits for AES)

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.KeySize = KeySize;
            aesAlg.BlockSize = BlockSize;
            aesAlg.Mode = Mode;
            aesAlg.Padding = Padding;

            // Ensure key is correct size
            byte[] keyBytesCorrectSize = new byte[aesAlg.KeySize / 8];
            Buffer.BlockCopy(keyBytes, 0, keyBytesCorrectSize, 0, Math.Min(keyBytes.Length, keyBytesCorrectSize.Length));
            aesAlg.Key = keyBytesCorrectSize;

            aesAlg.IV = iv;

            // Create an encryptor to perform the stream transform
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream
                        swEncrypt.Write(plainText);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static string Decrypt(string cipherText)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
        byte[] iv = new byte[BlockSize / 8]; // IV size is same as block size (128 bits for AES)

        using Aes aesAlg = Aes.Create();
        aesAlg.KeySize = KeySize;
        aesAlg.BlockSize = BlockSize;
        aesAlg.Mode = Mode;
        aesAlg.Padding = Padding;

        // Ensure key is correct size
        byte[] keyBytesCorrectSize = new byte[aesAlg.KeySize / 8];
        Buffer.BlockCopy(keyBytes, 0, keyBytesCorrectSize, 0, Math.Min(keyBytes.Length, keyBytesCorrectSize.Length));
        aesAlg.Key = keyBytesCorrectSize;

        aesAlg.IV = iv;

        try
        {
            // Attempt to decode the cipherText parameter as a Base64 string
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            // Create a decryptor to perform the stream transform
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption
            using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream and place them in a string
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        catch (FormatException)
        {
            // If the cipherText parameter is not a valid Base64 string, return it as is
            return cipherText;
        }
    }
}
