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

        public static bool senzoPritiskaDatabaseOpen = true;
        public static bool senzorTemperatureDatabaseOpen = true;
        public static bool senzorVlaznostiDatabaseOpen = true;
        public static object senzoPritiskaDatabaseOpenLock = new object();
        public static object senzorTemperatureDatabaseOpenLock = new object();
        public static object senzorVlaznostiDatabaseOpenLock = new object();

        public void TestCommunication()
        {
            Console.WriteLine("INFO | Ovom porukom server potvrdjuje da je komunikacije sa bazom ostvarena!");
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
