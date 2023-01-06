using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;
using Contracts.Enums;
using System.Threading;

namespace ClientApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			string serverCertNC = "wcfservice";
			bool lockBaze;


            //SERTIFIKAT ZA POTPISIVANJE _SIGN
			string signatureCertificateCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
			X509Certificate2 signatureCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signatureCertificateCN+"_sign");

			byte[] signatureStopPoruke = DigitalSignature.Create(ClientCmds.stop.ToString(), Hash.SHA1, signatureCertificate);
			byte[] signatureStartPoruke = DigitalSignature.Create(ClientCmds.start.ToString(), Hash.SHA1, signatureCertificate);
			byte[] signaturePoruke;

			//SERTIFIKATI I PODESAVANJE SERVISA
			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,StoreLocation.LocalMachine, serverCertNC);
			EndpointAddress CommunicationEndpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:8086/CommunicationService"), new X509CertificateEndpointIdentity(srvCert));
			EndpointAddress DbEndpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:8096/DbService"), new X509CertificateEndpointIdentity(srvCert));

			ClientToClient proxyCommunication = new ClientToClient(binding, CommunicationEndpointAddress);
			ClientToDb proxyDB = new ClientToDb(binding, DbEndpointAddress);

            #region CommunicationWithClient
            //UTVRDJUJEMO GRUPU
            UserGroup group = proxyCommunication.group; // grupa korisnika koji je pokrenuo konzolu
            Console.WriteLine("\n\t\tVasa grupe je: " + group+"!\n\n");

			while(true)
			{
				proxyCommunication.TestCommunication();
                Console.WriteLine("--------------------------------------------------------------");
				Console.WriteLine("Stisni ENTER da startujes slanje poruke. IZLAZ - za izlaz");
				Console.WriteLine("--------------------------------------------------------------");
				if (Console.ReadLine().ToUpper().Equals("IZLAZ"))
					break;

				//LOGIKA PROGRAMA
				lockBaze = proxyCommunication.SendMessage(ClientCmds.start, signatureStartPoruke);
                Console.WriteLine("Slanje poruke...");
				if (lockBaze)
                {
                    Console.WriteLine("Spremni smo za slanje poruka.");
					
					string poruka = CreateMessageDb(group); // ovo milos menja i generise kasnije
					Console.WriteLine(poruka);

					signaturePoruke = DigitalSignature.Create(poruka, Hash.SHA1, signatureCertificate);
					if(group == UserGroup.SenzorPritiska)
					{
                        Console.WriteLine("Senzor pritiska odradjen");
						proxyDB.WriteToSenzorPritiskaDB(poruka, signaturePoruke);
						proxyCommunication.SendMessage(ClientCmds.stop, signatureStopPoruke);
					}
					else if(group == UserGroup.SenzorTemperature)
                    {
						Console.WriteLine("Senzor temperature odradjen");
                        proxyDB.WriteToSenzorTemperatureDB(poruka, signaturePoruke);
                        proxyCommunication.SendMessage(ClientCmds.stop, signatureStopPoruke);
					}
					else if(group == UserGroup.SenzorVlaznosti)
                    {
						Console.WriteLine("Senzor vlaznosti odradjen");
                        proxyDB.WriteToSenzorVlaznostiDB(poruka, signaturePoruke);
                        proxyCommunication.SendMessage(ClientCmds.stop, signatureStopPoruke);
                    }
                    else
                    {
                        Console.WriteLine("ERROR | Doslo je do greske ocitavanje vase grupe");
						break;
                    }
				}

			}
            #endregion
        }
        // Kreiranje poruke za upis u BP
        private static string CreateMessageDb(UserGroup group)
        {
            Random random = new Random();
            string myUsername = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            string value = string.Empty;

            switch (group)
            {
                case UserGroup.SenzorPritiska:
                    value = $"Value: {random.Next(-100, 100)} [Pa]";
                    break;
                case UserGroup.SenzorTemperature:
                    value = $"Value: {random.Next(-100, 100)} [C]";
                    break;
                case UserGroup.SenzorVlaznosti:
                    value = $"Value: {random.Next(-100, 100)} [%]";
                    break;
            }

            return $"Time: {DateTime.Now}, Username: {myUsername}, GroupName: {group.ToString()}, {value}";
        }
    }
}
