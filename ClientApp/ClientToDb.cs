using Contracts;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    public class ClientToDb : ChannelFactory<IDatabase>, IDatabase, IDisposable
    {
        IDatabase database;

        public ClientToDb(NetTcpBinding binding, EndpointAddress endpointAddress) : base(binding, endpointAddress)
        {

            WindowsIdentity myIdentity = WindowsIdentity.GetCurrent();

            //ime klijenta
            string cltCertCN = Formatter.ParseName(myIdentity.Name);

            // Trust Chain validacija
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            X509Certificate2 clientCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            // Postavljanje klijentskog sertifikata
            this.Credentials.ClientCertificate.Certificate = clientCert;

            database = this.CreateChannel();

        }
        public void TestCommunication()
        {
            try
            {
                database.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (TestCommunication) | {0}", e.Message);
            }
        }

        public void WriteToSenzorPritiskaDB(string report, byte[] sign)
        {
            try
            {
                database.WriteToSenzorPritiskaDB(report, sign);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (TestCommunication) | {0}", e.Message);
            }
        }

        public void WriteToSenzorTemperatureDB(string report, byte[] sign)
        {
            try
            {
                database.WriteToSenzorTemperatureDB(report, sign);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (TestCommunication) | {0}", e.Message);
            }
        }

        public void WriteToSenzorVlaznostiDB(string report, byte[] sign)
        {
            try
            {
                database.WriteToSenzorVlaznostiDB(report, sign);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR (TestCommunication) | {0}", e.Message);
            }
        }
    }
}
