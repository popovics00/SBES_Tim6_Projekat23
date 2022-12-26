using Contracts;
using Contracts.Enums;
using Manager;
using System;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace ServiceApp
{
    public class ComunicationService : ICComunication
    {
        public bool SendMessage(ClientCmds cmdForClient, byte[] digSignature)
        {
            //DIGITALNI POTPIS PROVERA
            var primIdentityName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            string curentClientName = Formatter.ParseName(primIdentityName).Split(',')[0];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, curentClientName);
            if (!DigitalSignature.Verify(cmdForClient.ToString(), HashAlgorithm.SHA1, digSignature, certificate))
            {
                Console.WriteLine("KLIJENT | Digitalni potpis nije validan!");
                throw new Exception("Potpis nije validan!");
            }
            //
        }

        public void TestCommunication()
        {
            throw new NotImplementedException();
        }
    }
}
