using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Contracts
{
	[ServiceContract]
	public interface IDatabase
	{
        [OperationContract]
        void WriteToSenzorTemperatureDB(string report, byte[] digSignature); // pisanje u bazu senzora temperature

        [OperationContract]
        void WriteToSenzorVlaznostiDB(string report, byte[] digSignature); 

        [OperationContract]
        void WriteToSenzorPritiskaDB(string report, byte[] digSignature);

        [OperationContract]
        void TestCommunication(); // iniicjalna test komunikacija za testiranje servisa komunikacije sa bazom
    }
}
