using System;
using System.Security.Cryptography;
using System.Text;

namespace API.BL.Operations
{
    /// <summary>
    /// Provides AES encryption and decryption methods.
    /// </summary>
    public class BLEncryption
    {
        private static AesCryptoServiceProvider _objAes = new AesCryptoServiceProvider();

        // Predefined 32-byte key and 16-byte IV as normal strings
        private static string AesKey = "Ruyek107701KeyurRuyek107701Keyur";  // 32 characters = 256-bit key
        private static string AesIv = "library107417Key";   // 16 characters = 128-bit IV

        /// <summary>
        /// Encrypts a plain text using AES algorithm.
        /// </summary>
        /// <param name="plainText">The text to be encrypted.</param>
        /// <returns>Encrypted text in Base64 format.</returns>
        /// <exception cref="ArgumentException">Thrown when key or IV length is incorrect.</exception>
        public static string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);

            // Convert the predefined strings to byte arrays
            byte[] keyBytes = Encoding.UTF8.GetBytes(AesKey); // Convert string to byte array
            byte[] ivBytes = Encoding.UTF8.GetBytes(AesIv);   // Convert string to byte array

            // Ensure the key and IV are the correct size
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes (256 bits).");
            if (ivBytes.Length != 16)
                throw new ArgumentException("IV must be 16 bytes (128 bits).");

            using (ICryptoTransform encript = _objAes.CreateEncryptor(keyBytes, ivBytes))
            {
                // Encrypt the bytes using AES
                byte[] encryptedBytes = encript.TransformFinalBlock(bytes, 0, bytes.Length);

                // Convert the encrypted bytes to a Base64-encoded string
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Decrypts a cipher text using AES algorithm.
        /// </summary>
        /// <param name="cipherText">The text to be decrypted.</param>
        /// <returns>Decrypted plain text.</returns>
        /// <exception cref="ArgumentException">Thrown when key or IV length is incorrect.</exception>
        public static string Decrypt(string cipherText)
        {
            byte[] bytes = Convert.FromBase64String(cipherText);

            // Convert the predefined strings to byte arrays
            byte[] keyBytes = Encoding.UTF8.GetBytes(AesKey); // Convert string to byte array
            byte[] ivBytes = Encoding.UTF8.GetBytes(AesIv);   // Convert string to byte array

            // Ensure the key and IV are the correct size
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes (256 bits).");
            if (ivBytes.Length != 16)
                throw new ArgumentException("IV must be 16 bytes (128 bits).");

            using (ICryptoTransform decript = _objAes.CreateDecryptor(keyBytes, ivBytes))
            {
                // Decrypt the bytes using AES
                byte[] decryptedBytes = decript.TransformFinalBlock(bytes, 0, bytes.Length);

                // Convert the decrypted bytes to a UTF-8 encoded string
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}