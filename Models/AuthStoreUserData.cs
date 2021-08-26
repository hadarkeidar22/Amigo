using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigo.Models
{
	public class AuthStoreUserData
	{
		public AuthStoreUserData(MobileServiceUser MobileUser, string expiration, string ID, string type)
		{
			MobileServiceUser = MobileUser;
			AuthenticationID = ID;
			AuthenticationType = type;
			TokenExpirationDate = DateTime.ParseExact(expiration, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
		}
		public MobileServiceUser MobileServiceUser { get; set; }
		public DateTime TokenExpirationDate { get; set; }
		public string AuthenticationID { get; set; }
		public string AuthenticationType { get; set; }
	}
}
