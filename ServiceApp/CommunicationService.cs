using Contracts;
using Contracts.Enums;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
            if (!DigitalSignature.Verify(cmdForClient.ToString(), Hash.SHA1, digSignature, certificate))
            {
                Console.WriteLine("ALERT | Digitalni potpis nije validan!");
                throw new Exception("Potpis nije validan!");
            }
            //UTVRDJIVANJE PRAVA KORISNIKA - AUTORIZACIJA
            UserGroup group = CertManager.GetMyGroupFromCert(certificate);
            if (RolesSettings.IsInRole(group.ToString(), "SendMessage"))
            {
                Console.WriteLine("INFO | Klijent {0} - {1} je autorizovan da zapocne komunikaciju.", curentClientName, group.ToString());
            }
            else
            {
                Console.WriteLine("INFO | Klijent nema permisiju za komunikaciju sa serverom.");
                throw new Exception("ERROR | Access denied!");
            }
            if (group == UserGroup.SenzorPritiska)
            {
                return true;
            }
            else if (group == UserGroup.SenzorTemperature)
            {
                return true;
            }
            else if (group == UserGroup.SenzorVlaznosti)
            {
                return true;
            }
            else if (group == UserGroup.NULL)
            {
                return true;
            }
            else
                return false;
        }

        public void TestCommunication()
        {
            throw new NotImplementedException();
        }
    }
}
