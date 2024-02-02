using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Base.Services.Security
{
    public interface IEncryptionService
    {
        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");

        /// <summary>
        /// Crea un Hash de Md5
        /// </summary>
        /// <returns>Texto encriptado, se usa para el login en el sistema anterior - LEGACY</returns>
        string HashMd5(string text);

        string CreatePasswordHash(string password, string saltkey, string passwordFormat);
    }
}
