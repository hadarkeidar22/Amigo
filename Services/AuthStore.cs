using Amigo.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Diagnostics;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;


namespace Amigo.Services
{
	public class AuthStore
	{
		private static string TokenKeyName = "AmigoToken";
		private static string AuthenticationIDName = "AmigoAuthenticationID";
		private static string AuthenticationTypeName = "AmigoAuthenticationType";
		private static string TokenExpirationDateName = "AmigoTokenExpirationDate";

		public static void CacheAuthToken(MobileServiceUser user, DateTime ExpirationDate, string AuthenticationID, string LoginType = "")
		{
			var account = new Account(user.UserId);
			account.Properties.Add(TokenKeyName, user.MobileServiceAuthenticationToken);
			account.Properties.Add(AuthenticationIDName, AuthenticationID);
			account.Properties.Add(AuthenticationTypeName, LoginType);
			account.Properties.Add(TokenExpirationDateName, ExpirationDate.ToString("MM/dd/yyyy"));
			GetAccountStore().Save(account, App.AppName);

			Debug.WriteLine($"Cached auth token: {user.MobileServiceAuthenticationToken}");
		}

		public static AuthStoreUserData GetUserFromCache()
		{
			var account = GetAccountStore().FindAccountsForService(App.AppName).FirstOrDefault();

			if (account == null)
			{
				return null;
			}

			var token = account.Properties[TokenKeyName];
			Debug.WriteLine($"Retrieved token from account store: {token}");
			
			var Service =  new MobileServiceUser(account.Username)
			{
				MobileServiceAuthenticationToken = token
			};
			return new AuthStoreUserData(Service, account.Properties[TokenExpirationDateName], account.Properties[AuthenticationIDName], account.Properties[AuthenticationTypeName]);
		}

		public static void DeleteTokenCache()
		{
			var accountStore = GetAccountStore();
			var account = accountStore.FindAccountsForService(App.AppName).FirstOrDefault();
			if (account != null)
			{
				accountStore.Delete(account, App.AppName);
			}
		}

		private static AccountStore GetAccountStore()
		{
			return AccountStore.Create();
		}
	}
}

