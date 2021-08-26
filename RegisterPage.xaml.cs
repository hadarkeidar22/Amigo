using Amigo.Data;
using Amigo.Services;
using Amigo.Models;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Plugin.FacebookClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
			var UserClient = AuthStore.GetUserFromCache();
			if (UserClient != null)
			{
				DBItemManager.DefaultManager.CurrentClient.CurrentUser = UserClient.MobileServiceUser;
				Navigation.PushAsync(new LoadUserDetails(UserClient.AuthenticationID));
			}
		}
        
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

		private async void Facebook_Clicked(object sender, EventArgs e)
		{
			FacebookResponse<bool> response = await CrossFacebookClient.Current.LoginAsync(new string[] { "user_birthday", "user_gender" });
			if (response.Data)
			{
				var accessToken = CrossFacebookClient.Current.ActiveToken;
				if (accessToken != "")
				{
					var token = new JObject();
					// Replace access_token_value with actual value of your access token obtained
					// using the Facebook or Google SDK.
					token.Add("access_token", accessToken);
					await DBItemManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token);
					string authenthicationID = await LoadData();
					AuthStore.CacheAuthToken(DBItemManager.DefaultManager.CurrentClient.CurrentUser, CrossFacebookClient.Current.TokenExpirationDate, authenthicationID, "Facebook");

					await Navigation.PushAsync(new LoadUserDetails(authenthicationID));
				}
			}
		}
		public async Task<string> LoadData()
		{
			var jsonData = await CrossFacebookClient.Current.RequestUserDataAsync
			(
				  new string[] { "email"}, new string[] { }
			);
			var data = JObject.Parse(jsonData.Data);
			return data["email"].ToString();
		}
		/*
		public async Task LoadData()
		{

			var jsonData = await CrossFacebookClient.Current.RequestUserDataAsync
			(
				  new string[] {"first_name", "last_name", "email", "picture", "user_birthday", "user_gender" }, new string[] { }
			);

			var data = JObject.Parse(jsonData.Data);
			Users CurrentUser = new Users()
			{
				AuthenticationID = data["email"].ToString(),
				FirstName = data["first_name"].ToString(),
				LastName = data["last_name"].ToString(),
				Image = data["picture"]["data"]["url"].ToString()
			};
		}
		*/
	}
}