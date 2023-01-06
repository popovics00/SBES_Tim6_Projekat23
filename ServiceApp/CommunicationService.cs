using Contracts;
using Contracts.Enums;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp
{
    public class ComunicationService : DbService, ICComunication
    {
        public bool SendMessage(ClientCmds cmdForClient, byte[] digSignature)
        {
            //DIGITALNI POTPIS PROVERA
            var primIdentityName = ServiceSecurityContext.Current.PrimaryIdentity.Name;
            //var primIdentityName = WindowsIdentity.GetCurrent().Name;
            primIdentityName=primIdentityName.Replace("_sign", "");
            string curentClientName = Formatter.ParseName(primIdentityName).Split(',')[0];
            X509Certificate2 certificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, curentClientName+"_sign");
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
                if (cmdForClient.ToString().ToLower().Equals("stop"))
                {
                    lock (DbService.senzoPritiskaDatabaseOpenLock)
                    {
                        DbService.senzorPritiskaDatabaseOpen = true;
                    }
                    Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada otkljucana");
                    return true;
                }
                else if (cmdForClient.ToString().ToLower().Equals("start"))
                {
                    lock (DbService.senzoPritiskaDatabaseOpenLock)
                    {
                        if (!DbService.senzorPritiskaDatabaseOpen)
                        {
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Pristup bazi je BLOKIRAN, pa ne mozete pristupiti");
                            return false;
                        }
                        else
                        {
                            DbService.senzorPritiskaDatabaseOpen = false;
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada zakljucan");
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (group == UserGroup.SenzorTemperature)
            {
                if (cmdForClient.ToString().ToLower().Equals("stop"))
                {
                    lock (DbService.senzorTemperatureDatabaseOpenLock)
                    {
                        DbService.senzorTemperatureDatabaseOpen = true;
                    }
                    Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada otkljucana");
                    return true;
                }
                else if (cmdForClient.ToString().ToLower().Equals("start"))
                {
                    lock (DbService.senzorTemperatureDatabaseOpenLock)
                    {
                        if (!DbService.senzorTemperatureDatabaseOpen)
                        {
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Pristup bazi je BLOKIRAN, pa ne mozete pristupiti");
                            return false;
                        }
                        else
                        {
                            DbService.senzorTemperatureDatabaseOpen = false;
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada zakljucan");
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (group == UserGroup.SenzorVlaznosti)
            {
                if (cmdForClient.ToString().ToLower().Equals("stop"))
                {
                    lock (DbService.senzorVlaznostiDatabaseOpenLock)
                    {
                        DbService.senzorVlaznostiDatabaseOpen = true;
                    }
                    Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada otkljucana");
                    return true;
                }
                else if (cmdForClient.ToString().ToLower().Equals("start"))
                {
                    lock (DbService.senzorVlaznostiDatabaseOpenLock)
                    {
                        if (!DbService.senzorVlaznostiDatabaseOpen)
                        {
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Pristup bazi je BLOKIRAN, pa ne mozete pristupiti");
                            return false;
                        }
                        else
                        {
                            DbService.senzorVlaznostiDatabaseOpen = false;
                            Console.WriteLine("DATABASE (SenzorPritiskaDB) | Baza je sada zakljucan");
                            return true;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else if (group == UserGroup.NULL)
                throw new Exception("ALERT | Klijent nije u grupi.");
            else
                return false;
        }

        public void TestCommunication()
        {
            Console.WriteLine("INFO | Test komunikacije uspesno ostvaren!");
        }
    }
}
