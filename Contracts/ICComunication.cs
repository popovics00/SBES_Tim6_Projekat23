using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts.Enums;

namespace Contracts
{
	[ServiceContract]
	public interface ICComunication
	{
		[OperationContract]
		bool SendMessage(ClientCmds cmdForClient, byte[] sign);

		[OperationContract]
		void TestCommunication();

	}
}
