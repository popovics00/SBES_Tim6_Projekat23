using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using Manager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using Contracts.Enums;

namespace ClientApp
{
	public class ClientToClient : ChannelFactory<ICComunication>, ICComunication, IDisposable
	{
		ICComunication factory;
        public readonly UserGroup group = UserGroup.NULL;

		public ClientToClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
		{
            WindowsIdentity myIdentity = WindowsIdentity.GetCurrent();
            string cltCertCN = Formatter.ParseName(myIdentity.Name);
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            group = CertManager.GetMyGroupFromCert(clientCert);
            this.Credentials.ClientCertificate.Certificate = clientCert;
            factory = this.CreateChannel();
		}

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (TestCommunication) | {0}", e.Message);
            }
        }

		public void Dispose()
		{
			if (factory != null)
			{
				factory = null;
			}

			this.Close();
		}

        public bool SendMessage(ClientCmds cmdForClient, byte[] signature)
        {
            try
            {
                return factory.SendMessage(cmdForClient, signature);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (SendMessage) | " + e.Message);
                return false;
            }
        }
    }
}
