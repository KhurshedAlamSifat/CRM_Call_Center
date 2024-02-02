using Base.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Base.Services.Security
{
    public class EncryptionService : IEncryptionService
    {

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
            {
                var toEncrypt = Encoding.Unicode.GetBytes(data);
                cs.Write(toEncrypt, 0, toEncrypt.Length);
                cs.FlushFinalBlock();
            }

            return ms.ToArray();
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using var ms = new MemoryStream(data);
            using var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.Unicode);
            return sr.ReadToEnd();
        }

        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat)
        {
            return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);
        }


        /// <summary>
        /// Encriptar texto
        /// </summary>
        /// <param name="plainText">Texto a encriptar</param>
        /// <param name="encryptionPrivateKey">Llave de encriptaci[on</param>
        /// <returns>Texto encriptado</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SecurityDefaults.EncryptionKey;

            using var provider = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
            return Convert.ToBase64String(encryptedBinary);
        }

        /// <summary>
        /// Descencripta texto
        /// </summary>
        /// <param name="cipherText">Texto a desencriptar</param>
        /// <param name="encryptionPrivateKey">Llave privada</param>
        /// <returns>Texto desencriptado</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = SecurityDefaults.EncryptionKey;

            using var provider = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16)),
                IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8))
            };

            var buffer = Convert.FromBase64String(cipherText);
            return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
        }
        

        /// <summary>
        /// Crea un Hash de Md5
        /// </summary>
        /// <returns>Texto encriptado, se usa para el login en el sistema anterior - LEGACY</returns>
        public virtual string HashMd5(string text)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            Byte[] hashedBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            StringBuilder sb = new StringBuilder();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(text));
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                //to make hex string use lower case instead of uppercase add parameter “X2″
                sb.Append(hashedBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
