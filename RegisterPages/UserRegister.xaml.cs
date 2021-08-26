using Amigo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Amigo.Data.DBItemManager;
using Amigo.Models;

namespace Amigo.RegisterPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserRegister : ContentPage
	{
		Users newUser;

		public UserRegister (Users newUser)
		{
			InitializeComponent ();
			CurrentPlatform.Init();
			this.newUser = newUser;
       
        }
		protected override async void OnAppearing()
		{
			// Set syncItems to true in order to synchronize the data on startup when running in offline mode
			base.OnAppearing();
			await SaveUser(true, syncItems: true);
			await Navigation.PushAsync(new LoadUserDetails(newUser.AuthenticationID));
			Navigation.RemovePage(this);
		}
		private async Task SaveUser(bool showActivityIndicator, bool syncItems)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				await DBItemManager.DefaultManager.SaveUserAsync(newUser);
			}
		}
	}
}