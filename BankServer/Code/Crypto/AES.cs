using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Code
{
    public class AES: ICryptable
    {
        RijndaelManaged myRijndael;

        public AES()
        {
            myRijndael = new RijndaelManaged();

            myRijndael.Key = Encoding.ASCII.GetBytes("this is password".makeThisStringXLetterLength(32));
            myRijndael.IV = Encoding.ASCII.GetBytes("none".makeThisStringXLetterLength(16));
            myRijndael.Padding = PaddingMode.Zeros;
            myRijndael.Mode = CipherMode.CBC;
        }

        public byte[] Encrypt(byte[] a)
        {
            if (enable == false) return a;

            if (a.Length == 0)
                return a;
            byte[] encrypted;
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = myRijndael.Key;
                rijAlg.IV = myRijndael.IV;
                rijAlg.Padding = myRijndael.Padding;
                rijAlg.Mode = myRijndael.Mode;

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV), CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(a, 0, a.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public byte[] Decrypt(byte[] a)
        {
            if (enable == false) return a;
            if (a.Length == 0)
                return a;

            List<byte> plain = new List<byte>();

            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = myRijndael.Key;
                rijAlg.IV = myRijndael.IV;
                rijAlg.Padding = myRijndael.Padding;
                rijAlg.Mode = myRijndael.Mode;

                using (MemoryStream msDecrypt = new MemoryStream(a))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV), CryptoStreamMode.Read))
                    {
                        byte[]b = new byte[4096];

                        for (int read ; (read = csDecrypt.Read(b, 0, b.Length)) > 0 ; )
                            for (int i = 0 ; i < read ; i++)
                                plain.Add(b[i]);
                    }
                }
            }
            return plain.ToArray();
        }

        public byte[] Sign(byte[] a) { return a; }
        public bool Varify(byte[] a, byte[] b) { return true; }

        bool enable = true;
        public void Enable(bool t)
        {
            enable = t;
        }

        public AES SetKey(byte[] key)
        {
            myRijndael.Key = key;
            return this;
        }

        public static string GenerateRandomKey()
        {
            RijndaelManaged v = new RijndaelManaged();
            v.GenerateKey();
            return v.Key.BytesToString();
        }

        public string GetKeyAsString()
        {
            return myRijndael.Key.BytesToString();
        }
    }
}