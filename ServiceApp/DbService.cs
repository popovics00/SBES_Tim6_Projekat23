using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Contracts.Enums;
using System.IO;

namespace ServiceApp
{
    public class DbService : IDatabase
    {
        private static string[] dataBasePaths = { "C:\\CETVRTA GODINA - VEZBE\\SBES\\SBES_Tim6_Projekat23\\Database\\" };

        public static bool senzorPritiskaDatabaseOpen = true;
        public static bool senzorTemperatureDatabaseOpen = true;
        public static bool senzorVlaznostiDatabaseOpen = true;
        public static object senzoPritiskaDatabaseOpenLock = new object();
        public static object senzorTemperatureDatabaseOpenLock = new object();
        public static object senzorVlaznostiDatabaseOpenLock = new object();

        private static object senzorPritiskaDatabaseLock = new object();
        private static object senzorTemperatureDatabaseLock = new object();
        private static object senzorVlaznostiDatabaseLock = new object();
        public void TestCommunication()
        {
            Console.WriteLine("INFO | Ovom porukom server potvrdjuje da je komunikacije sa bazom ostvarena!");
        }

        public void WriteToSenzorPritiskaDB(string report, byte[] digSignature)
        {
            //DIGITALNI POTPIS PROVERA
            var primIdentityName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            //var primIdentityName = WindowsIdentity.GetCurrent().Name;
            primIdentityName = primIdentityName.Replace("_sign", "");
            string curentClientName = Formatter.ParseName(primIdentityName).Split(',')[0];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, curentClientName + "_sign");
            if (!DigitalSignature.Verify(report, Hash.SHA1, digSignature, certificate))
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

            string activePath = dataBasePaths[0] + "senzorPritiska.xml";
            lock (senzorPritiskaDatabaseLock)
            {
                if (File.Exists(activePath))
                {
                    File.AppendAllText(activePath, report + Environment.NewLine);
                }
                else
                {
                    File.WriteAllText(activePath, report + Environment.NewLine);
                }
            }

            Console.WriteLine($"[{group.ToString()}]: {report}");
        }

        public void WriteToSenzorTemperatureDB(string report, byte[] digSignature)
        {
            //DIGITALNI POTPIS PROVERA
            var primIdentityName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            //var primIdentityName = WindowsIdentity.GetCurrent().Name;
            primIdentityName = primIdentityName.Replace("_sign", "");
            string curentClientName = Formatter.ParseName(primIdentityName).Split(',')[0];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, curentClientName + "_sign");
            if (!DigitalSignature.Verify(report, Hash.SHA1, digSignature, certificate))
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

            string activePath = dataBasePaths[0].ToString() + "senzorTemperature.xml";
            lock (senzorTemperatureDatabaseLock)
            {
                if (File.Exists(activePath))
                {
                    File.AppendAllText(activePath, report + Environment.NewLine);
                }
                else
                {
                    File.WriteAllText(activePath, report + Environment.NewLine);
                }
            }

            Console.WriteLine($"[{group.ToString()}]: {report}");
        }

        public void WriteToSenzorVlaznostiDB(string report, byte[] digSignature)
        {
            //DIGITALNI POTPIS PROVERA
            var primIdentityName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            //var primIdentityName = WindowsIdentity.GetCurrent().Name;
            primIdentityName = primIdentityName.Replace("_sign", "");
            string curentClientName = Formatter.ParseName(primIdentityName).Split(',')[0];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, curentClientName + "_sign");
            if (!DigitalSignature.Verify(report, Hash.SHA1, digSignature, certificate))
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


            string activePath = dataBasePaths[0].ToString() + "senzorVlaznosti.xml";
            lock (senzorVlaznostiDatabaseLock)
            {
                if (File.Exists(activePath))
                {
                    File.AppendAllText(activePath, report + Environment.NewLine);
                }
                else
                {
                    File.WriteAllText(activePath, report + Environment.NewLine);
                }
            }

            Console.WriteLine($"[{group.ToString()}]: {report}");
        }
    }
}
