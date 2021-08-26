using Amigo.Data;
using Amigo.Services;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Plugin.FacebookClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WelcomePage : ContentPage
	{
		public static bool FacebookLogin = false;
		public WelcomePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			var UserClient = AuthStore.GetUserFromCache();
			if (UserClient != null)
			{
				if(UserClient.TokenExpirationDate > DateTime.Today.Date)
				{
					DBItemManager.DefaultManager.CurrentClient.CurrentUser = UserClient.MobileServiceUser;
					if (UserClient.AuthenticationType == "Facebook")
						FacebookLogin = true;
					Navigation.PushAsync(new LoadUserDetails(UserClient.AuthenticationID));
					Navigation.RemovePage(this);
				}
				else
				{
					Facebook_Clicked();
				}
			}
			theInfo.Text = "אליפות!!! " + "\n" + "גאים בכם על כך שהורדתם את האפליקציה. מי שטרם הכיר טוב את אמיגו," +
                " הי, נעים להכיר " +
                " באמיגו קל וכיף להכיר ולמצוא פרטנרים ושותפים." +
               " מפעילויות שגרתיות כמו משחקים בשכונה ועד לקבוצות לימוד, שותפים לטיולים ולדירה ועוד." +
               " כל זה בעזרתכם ובעזרת אמיגו!";
        }

        private void Info_Clicked(object sender, EventArgs e)
        {
            theInfoBox.IsVisible = true;
            closeTheInfoBox.IsVisible = true;
            infoBtn.IsVisible = false;
            facebookBtn.IsVisible = false;
            gmailBtn.IsVisible = false;
            orLbl.IsVisible = false;
            closeTheInfoBoxLbl.IsVisible = true;
            theInfo.IsVisible = true;
        }

        
         private void close_Clicked(object sender, EventArgs e)
        {
            theInfoBox.IsVisible = false;
            closeTheInfoBox.IsVisible = false;
            infoBtn.IsVisible = true;
            facebookBtn.IsVisible = true;
            gmailBtn.IsVisible = true;
            orLbl.IsVisible = true;
            closeTheInfoBoxLbl.IsVisible = false;
            theInfo.IsVisible = false;
        }

        private async void mail_Clicked(object sender, EventArgs e)
        { 
                await Navigation.PushAsync(new RegisterPage());
        }
		private async void Facebook_Clicked(object sender=null, EventArgs e=null)
		{
			FacebookResponse<string> response = await CrossFacebookClient.Current.RequestUserDataAsync(new string[] { "email" }, new string[] { "user_birthday", "user_gender" });
			if (response.Status == FacebookActionStatus.Completed)
			{
				var accessToken = CrossFacebookClient.Current.ActiveToken;
				if (accessToken != "")
				{
					var token = new JObject();
					token.Add("access_token", accessToken);
					await DBItemManager.DefaultManager.CurrentClient.LoginAsync(MobileServiceAuthenticationProvider.Facebook, token);
					string authenthicationID = (JObject.Parse(response.Data))["email"].ToString();
					AuthStore.CacheAuthToken(DBItemManager.DefaultManager.CurrentClient.CurrentUser, CrossFacebookClient.Current.TokenExpirationDate, authenthicationID, "Facebook");
					FacebookLogin = true;
					await Navigation.PushAsync(new LoadUserDetails(authenthicationID));
					Navigation.RemovePage(this);
				}
			}
		}
		public async Task<string> LoadData()
		{
			var jsonData = await CrossFacebookClient.Current.RequestUserDataAsync
			(
				  new string[] { "email" }, new string[] { }
			);
			var data = JObject.Parse(jsonData.Data);
			return data["email"].ToString();
		}
	}
}