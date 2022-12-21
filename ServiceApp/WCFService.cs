using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Manager;
using System.Security.Cryptography.X509Certificates;
using Contracts.Enums;

namespace ServiceApp
{
	public class WCFService : ICComunication
	{
        public bool SendMessage(ClientCmds cmdForClient, byte[] sign)
        {
            throw new NotImplementedException();
        }

        public void TestCommunication()
		{
			Console.WriteLine("Communication established.");
		}

	}
}
