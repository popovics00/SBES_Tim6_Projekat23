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

			NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			//SERVERSKI SERTIFIKAT IZ TRUSTED PEOPLE I PRAVLJENJE PROXYJA
			string signatureCertificateCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
			X509Certificate2 signatureCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, signatureCertificateCN);
			
			X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople,StoreLocation.LocalMachine, serverCertNC);
			EndpointAddress CommunicationEndpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:8086/CommunicationService"), new X509CertificateEndpointIdentity(srvCert));
			EndpointAddress DbEndpointAddress = new EndpointAddress(new Uri("net.tcp://localhost:8096/DbService"), new X509CertificateEndpointIdentity(srvCert));

			ClientToClient proxyCommunication = new ClientToClient(binding, CommunicationEndpointAddress);

			byte[] signatureStop = DigitalSignature.Create(ClientCmds.stop.ToString(), Hash.SHA1, signatureCertificate);
			byte[] signatureStart = DigitalSignature.Create(ClientCmds.start.ToString(), Hash.SHA1, signatureCertificate);
			byte[] signMessage;

			//UTVRDJUJEMO GRUPU
			UserGroup group = proxyCommunication.group; //grupa korisnika koji je pokrenuo konzolu
            Console.WriteLine("\n\t\tVasa grupe je: " + group+"!\n\n");

			while(true)
			{
				proxyCommunication.TestCommunication();
				Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
				Thread.Sleep(10000);
				Console.ReadLine();
			}
		}
	}
}
