using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class DigitalSignature
    {
        public static byte[] Create(string message, Hash hashAlgorithm, X509Certificate2 certificate)
        {
            /// Looks for the certificate's private key to sign a message
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PrivateKey;

            if (csp == null)
            {
                throw new Exception("Valid certificate was not found.");
            }
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(message);
            byte[] hash = null;

            if (hashAlgorithm.Equals(Hash.SHA1))
            {
                SHA1Managed sha1 = new SHA1Managed();
                hash = sha1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(Hash.SHA256))
            {
                SHA256Managed sha256 = new SHA256Managed();
                hash = sha256.ComputeHash(data);
            }
            /// Use RSACryptoServiceProvider support to create a signature using a previously created hash value
            byte[] signature = csp.SignHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()));
            return signature;
        }
        public static bool Verify(string message, Hash hashAlgorithm, byte[] digSignature, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)certificate.PublicKey.Key;

            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(message);
            byte[] hash = null;

            if (hashAlgorithm.Equals(Hash.SHA1))
            {
                SHA1Managed sha1 = new SHA1Managed();
                hash = sha1.ComputeHash(data);
            }
            else if (hashAlgorithm.Equals(Hash.SHA256))
            {
                SHA256Managed sha256 = new SHA256Managed();
                hash = sha256.ComputeHash(data);
            }

            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()), digSignature);
        }
    }
}
