using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using Manager;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;

namespace ServiceApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
			//srvCertCN = "wcfservice";
			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			ServiceHost host = new ServiceHost(typeof(ComunicationService));
			string hostAddressCommunication = "net.tcp://localhost:8086/CommunicationService";
			host.AddServiceEndpoint(typeof(ICComunication), binding, hostAddressCommunication);
			string hostAddressDB = "net.tcp://localhost:8096/DbService";
			host.AddServiceEndpoint(typeof(IDatabase), binding, hostAddressDB);


			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.ChainTrust;
			host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            try
			{
				host.Open();
				Console.WriteLine("Servis je upravo pokrenut.\nStisni <enter> da bi zaustavio servis ...");
				Console.ReadLine();

                Console.WriteLine("\n\nServis se upravo gasi...");
                Thread.Sleep(1500);
            }
			catch (Exception e)
			{
				Console.WriteLine("[ERROR] {0}", e.Message);
				Console.WriteLine("[StackTrace] {0}", e.StackTrace);
			}
			finally
			{
				host.Close();
			}		
		}
	}
}
