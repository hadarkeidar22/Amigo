using Amigo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amigo.Models;
using Newtonsoft.Json.Linq;
using Plugin.FacebookClient;
using Xamarin.Essentials;
using System.Diagnostics;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]

	public partial class LoadUserDetails : ContentPage
	{
		public static Users CurrentUser { get; set; }
		string authenticationId;
		public LoadUserDetails (string authenticationId)
		{
			this.authenticationId = authenticationId;
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			IEnumerable<Users> user = await DBItemManager.DefaultManager.GetUserAsync(authenticationId);
			if (user.Count() > 0)
			{
				CurrentUser = user.ElementAt(0);
				if (!await LoadLoacation())
				{
					//try again
					if (!await LoadLoacation())
						await DisplayAlert("Error!","Can't get user location","OK");
				}
			}
			else
			{
				if (WelcomePage.FacebookLogin){
					Users FacebookDetails = await LoadData();
					await Navigation.PushAsync(new RegisterUser(FacebookDetails.AuthenticationID, FacebookDetails));
				}
				else
					await Navigation.PushAsync(new RegisterUser("EmailInputOfEmailRegister"));
			}
			Navigation.RemovePage(this);
		}
		public async Task<bool> LoadLoacation()
		{
			try
			{
				Debug.WriteLine("Got User Details:" + CurrentUser.AuthenticationID);
				var request = new GeolocationRequest(GeolocationAccuracy.Medium, new TimeSpan(0, 0, 15));
				var geoTask = Geolocation.GetLocationAsync(request);
				if (await Task.WhenAny(geoTask, Task.Delay(10000)) == geoTask)
				{
					var position = await geoTask;
					CurrentUser.Latitude = position.Latitude;
					CurrentUser.Longitude = position.Longitude;
					Debug.WriteLine("Got User Location:" + position.Latitude.ToString() + "," + position.Longitude.ToString());
					if (!Application.Current.Properties.ContainsKey("MaxDistance"))
					{
						await Navigation.PushAsync(new ChooseEventsRadius(CurrentUser.Latitude, CurrentUser.Longitude));
					}
					else
						await Navigation.PushAsync(new MainPage());
					Navigation.RemovePage(this);
					return true;
				}
				return false;
			}
			catch (FeatureNotSupportedException fnsEx)
			{
				Debug.WriteLine(fnsEx.Data + "////" + fnsEx.Message);
				return false;
				// Handle not supported on device exception
			}
			catch (PermissionException pEx)
			{
				Debug.WriteLine(pEx.Data + "////" + pEx.Message);
				return false;
				// Handle permission exception
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Data + "////" + ex.Message);
				return false;
				// Unable to get location
			}
		}
		public async Task<Users> LoadData()
		{

			if(CrossFacebookClient.Current.HasPermissions(new string[] { "user_birthday", "user_gender" }))
			{
				var jsonData = await CrossFacebookClient.Current.QueryDataAsync("me/", new string[] { "user_birthday", "user_gender" }, new Dictionary<string, string>()
				{
					{"fields", "email,first_name,last_name,picture.type(large),birthday,gender"}
				});
				
				var data = JObject.Parse(jsonData.Data);
				Users UserDetails = new Users()	
				{
					AuthenticationID = data["email"].ToString(),
					FirstName = data["first_name"].ToString(),
					LastName = data["last_name"].ToString(),
					Image = data["picture"]["data"]["url"].ToString()
				};
				if (data["birthday"] != null)
					UserDetails.Birthday = DateTime.ParseExact(data["birthday"].ToString(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
				if (data["gender"] != null)
				{
					switch (data["gender"].ToString())
					{
						case "male":
							UserDetails.Gender = 1;
							break;
						case "female":
							UserDetails.Gender = 0;
							break;
					}
				}
				return UserDetails;			
			}
			else
			{
				await DisplayAlert("אופס!", "בעיית הרשאה, חוזר לחוף מבטחים", "אוקיי");
			}
			return null;
		}
	}
}