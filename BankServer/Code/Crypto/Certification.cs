using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pluralsight.Crypto;
using System.Security.Cryptography.X509Certificates;

namespace Code
{
    public static class ExtensionCertification
    {
        public static byte[] ToBytes(this X509Certificate2 cert, string pwd = "",bool isPFX=false)
        {
            if(isPFX)
                return cert.Export(X509ContentType.Pfx, pwd);
            return cert.Export(X509ContentType.SerializedCert, pwd);
        }

        public static X509Certificate2 ToX509(this byte[] data, string pwd = "")
        {
            X509Certificate2 cert = new X509Certificate2();
            cert.Import(data, pwd, X509KeyStorageFlags.DefaultKeySet);
            return cert;
        }

        public static string ToString2(this X509Certificate2 cert)
        {
            return string.Format(
                "Version: {0}\r\nIssuer: {1}\r\nfrom: {2}\r\nto: {3}\r\nSerialNumber: {4}\r\nSignatureAlgorithm: {5}\r\nThumbprint: {6}\r\nPublicKeys:\r\n{7}\r\nPrivateKeys:\r\n{8}",

                cert.Version,
                cert.Issuer,
                cert.NotBefore,
                cert.NotAfter,
                cert.SerialNumber,
                cert.SignatureAlgorithm.FriendlyName,
                cert.Thumbprint,
                cert.PublicKey.Key.ToXmlString(false),
                cert.HasPrivateKey ? cert.PrivateKey.ToXmlString(true) : "null"
                );
        }
    }

    public class CertificationFactory
    {
        public static X509Certificate2 GenerateCertificate()
        {
            using (CryptContext ctx = new CryptContext())
            {
                ctx.Open();

                X509Certificate2 cert = ctx.CreateSelfSignedCertificate(
                    new SelfSignedCertProperties
                    {
                        IsPrivateKeyExportable = true,
                        KeyBitLength = 1024,
                        Name = new X500DistinguishedName("cn=localhost"),
                        
                        ValidFrom = DateTime.Now,
                        ValidTo = DateTime.Now.AddDays(1),
                    });

                return cert;
            }
        }

        public static bool Verify(X509Certificate2 cert, ref string res)
        {
            X509Chain chain = new X509Chain()
            {
                ChainPolicy = new X509ChainPolicy()
                {
                    VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority
                }
            };

            if (chain.Build(cert) == false)
            {
                foreach (X509ChainStatus chainStatus in chain.ChainStatus)
                    if (chainStatus.Status != X509ChainStatusFlags.UntrustedRoot)
                        res += string.Format(chainStatus.StatusInformation) + "\r\n";

                return false;
            }
            return true;
        }
    }
}