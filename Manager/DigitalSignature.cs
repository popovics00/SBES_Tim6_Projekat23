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
            RSACryptoServiceProvider privateKey = certificate.PrivateKey as RSACryptoServiceProvider;

            if (privateKey == null)
                throw new Exception("Sertifikat nije pronadjen");

            byte[] messageByte = new UnicodeEncoding().GetBytes(message);
            byte[] hash = new SHA1Managed().ComputeHash(messageByte);

            byte[] digSignature = privateKey.SignHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()));

            return digSignature;
        }
        public static bool Verify(string message, Hash hashAlgorithm, byte[] digSignature, X509Certificate2 certificate)
        {
            RSACryptoServiceProvider privateKey = certificate.PublicKey.Key as RSACryptoServiceProvider;

            if (privateKey == null)
                throw new Exception("Sertifikat nije pronadjen");

            byte[] messageByte = new UnicodeEncoding().GetBytes(message);
            byte[] hash = new SHA1Managed().ComputeHash(messageByte);

            return privateKey.VerifyHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm.ToString()), digSignature);

        }
    }
}
