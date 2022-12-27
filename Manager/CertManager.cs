using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Contracts.Enums;

namespace Manager
{
	public class CertManager
	{
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
		{
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

            foreach (X509Certificate2 cert in certCollection)
            {
                if (cert.SubjectName.Name.Contains(string.Format("CN={0}", subjectName)))
                {
                    return cert;
                }
            }
            return null;
		}

        public static UserGroup GetMyGroupFromCert(X509Certificate2 cert)
        {
            if (cert.SubjectName.Name.Contains(string.Format("OU=SenzorPritiska")))
            {
                return UserGroup.SenzorPritiska;
            }
            else if (cert.SubjectName.Name.Contains(string.Format("OU=SenzorTemperature")))
            {
                return UserGroup.SenzorTemperature;
            }
            else if (cert.SubjectName.Name.Contains(string.Format("OU=SenzorVlaznosti")))
            {
                return UserGroup.SenzorVlaznosti;
            }
            else
            {
                return UserGroup.NULL;
            }
        }
	}
}
