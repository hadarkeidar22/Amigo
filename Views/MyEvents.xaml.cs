using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.MobileServices;
using Amigo.Data;
using System.Globalization;
using System.Diagnostics;
using System.Linq;
using Amigo.Models;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyEventsPage : ContentView
	{
		public MyEventsPage()
		{
			InitializeComponent();
			CurrentPlatform.Init();
			MessagingCenter.Subscribe<MainPage>(this, "RefreshMyEvents", async (sender) =>
			{
				await RefreshItems(true);
			});
		}

		public async void OnRefresh(object sender, EventArgs e)
		{
			var list = (ListView)sender;
			Exception error = null;
			try
			{
				await RefreshItems(false);
			}
			catch (Exception ex)
			{
				error = ex;
			}
			finally
			{
				list.EndRefresh();
			}

			if (error != null)
			{
				Debug.WriteLine("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
			}
		}

		public async void OnSyncItems(object sender, EventArgs e)
		{
			await RefreshItems(true);
		}

		public async Task RefreshItems(bool showActivityIndicator)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				IEnumerable<Events> items = await DBItemManager.DefaultManager.GetEventsForMeAsync();
				eventsList.ItemsSource = items;
			}
		}

		public void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
			}
			var _selectedEvent = e.SelectedItem as Events;
			if (App.Current.MainPage is NavigationPage)
				(App.Current.MainPage as NavigationPage).PushAsync(new SelectedEvent(_selectedEvent), true);
			eventsList.SelectedItem = null;
		}

	}

}