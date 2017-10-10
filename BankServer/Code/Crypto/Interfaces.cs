using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Code
{
    public interface ICryptable
    {
        byte[] Encrypt(byte[] a);
        byte[] Decrypt(byte[] a);

        byte[] Sign(byte[] a);
        bool Varify(byte[] a, byte[] b);

        void Enable(bool t);
    }
}