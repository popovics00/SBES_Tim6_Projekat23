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

namespace ServiceApp
{
    public class DbService : IDatabase
    {
        private static string[] dataBasePaths = { "senzorPritiska.txt", "senzorTemperature.txt", "senzorVlaznosti.txt" };

        private static bool senzoPritiskaDatabaseOpen = true;
        private static bool senzorTemperatureDatabaseOpen = true;
        private static bool senzorVlaznostiDatabaseOpen = true;

        public void TestCommunication()
        {
            Console.WriteLine("komunikacija uspesna!");
        }

        public void WriteToSenzorPritiskaDB(string report, byte[] digSignature)
        {
            throw new NotImplementedException();
        }

        public void WriteToSenzorTemperatureDB(string report, byte[] sign)
        {
            throw new NotImplementedException();
        }

        public void WriteToSenzorVlaznostiDB(string report, byte[] sign)
        {
            throw new NotImplementedException();
        }
    }
}
