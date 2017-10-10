using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Code
{
    public class RSA: ICryptable
    {
        public static void Test()
        {
            RSA rsa = new RSA();
            rsa.GenerateNewKeys().SetClientPublicKey(rsa.PublicKey);
            var data = "Welcome".StringToBytes();
            var encrypted = rsa.Encrypt(data);
            var signature = rsa.Sign(data);

            data = rsa.Decrypt(encrypted);
            System.Windows.Forms.MessageBox.Show(RSA.KeyToString(rsa.PublicKey).StringToBytes().Length + "");
            //data[3] += 1;
            //signature[2] += 1;
        }

        public RSAParameters PublicKey { get; private set; }
        private RSAParameters PrivateKey { get; set; }

        public RSAParameters ClientPublicKey { get; set; }

        public RSA() { }
        public RSA(RSA copy)
        {
            PrivateKey = copy.PrivateKey;
            PublicKey = copy.PublicKey;
        }

        public RSA GenerateNewKeys()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            PublicKey = rsa.ExportParameters(false);
            PrivateKey = rsa.ExportParameters(true);

            return this;
        }

        public RSA SetClientPublicKey(RSAParameters key)
        {
            ClientPublicKey = key;
            return this;
        }

        public byte[] Encrypt(byte[] a)
        {
            if (enable == false) return a;
            if (a.Length == 0)
                return a;
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(ClientPublicKey); //public key
                encryptedData = RSA.Encrypt(a, false);
            }
            return encryptedData;
        }
        public byte[] Decrypt(byte[] a)
        {
            if (enable == false) return a;

            if (a.Length == 0)
                return a;
            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(PrivateKey); //private key
                decryptedData = RSA.Decrypt(a, false);
            }
            return decryptedData;
        }

        public byte[] Sign(byte[] DataToSign)
        {
            byte[] signedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(PrivateKey); //private key
                signedData = RSA.SignData(DataToSign, CryptoConfig.MapNameToOID("SHA1"));
            }
            return signedData;
        }
        public bool Varify(byte[] DataToVerify, byte[] signature)
        {
            if (enable == false)
                return true;
            bool signedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.ImportParameters(ClientPublicKey); // public key only
                signedData = RSA.VerifyData(DataToVerify, CryptoConfig.MapNameToOID("SHA1"), signature);
            }
            return signedData;
        }



        public static string KeyToString(RSAParameters key)
        {
            var sw = new StringWriter();
            new XmlSerializer(typeof(RSAParameters)).Serialize(sw, key);
            return sw.ToString();
        }
        public static RSAParameters StringToKey(string str)
        {
            return (RSAParameters)new XmlSerializer(typeof(RSAParameters)).Deserialize(new StringReader(str));
        }

        bool enable = true;
        public void Enable(bool t)
        {
            enable = t;
        }



        ///////////////////////////////////////////// some information form stackoverflow.com :)
        //////
        //
        //    Encryption is where anyone may write a message, but only the private key holder may read it.
        //    Signing is where anyone may read a message, but only the private key holder may write it.
        //
        //    When you call Decrypt, the RSACryptoServiceProvider is looking for encryption, that is,
        //    public write private read. Thus it looks for the private key.
        //    You want to use the SignData and VerifyData functions to sign the payload so that people can't write it.
        //
        //////
        //////
        //
        //    to send a message, call gpg, encrypt with the public key of the recipient,
        //    and while you're at it sign with the private key of the sender.
        //    When receiving a message, check the signature against the purported sender's public key,
        //    and decrypt with the private key of the receiver.
        //    In other words, send with gpg --sign --encrypt -r NAME_OF_RECIPIENT and receive with gpg --verify followed by gpg --decrypt.
        //
        //////

        public static RSAParameters keyToRSAKey(System.Security.Cryptography.X509Certificates.PublicKey publicKey)
        {
            return ((RSACryptoServiceProvider)publicKey.Key).ExportParameters(false);
        }
    }
}