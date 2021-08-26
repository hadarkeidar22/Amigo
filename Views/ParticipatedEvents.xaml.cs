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
using System.Collections.ObjectModel;
using System.Linq;
using Amigo.Controls;
using Amigo.Models;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ParticipatedEvents : ContentView
	{
		public ObservableCollection<GroupingListView<string, Events>> EventsGrouped { get; set; }
		public string CreatedByMeString = "אמיגו בניהולך";
		public string ParticipatingString = "אמיגו שלך";

		public ParticipatedEvents()
		{
			InitializeComponent();
			//CurrentPlatform.Init();
			MessagingCenter.Subscribe<MainPage>(this, "RefreshParticipatedEvents", async(sender) =>
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

		private async Task RefreshItems(bool showActivityIndicator)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				IEnumerable<Events> createdByMe = await DBItemManager.DefaultManager.GetEventsCreatedByMeAsync();
				IEnumerable<Events> Participating = await DBItemManager.DefaultManager.GetEventsParticipatingAsync();


				var sorted = from item in createdByMe
							 group item by CreatedByMeString into EventsGroup
							 select new GroupingListView<string, Events>(EventsGroup.Key, EventsGroup);

				//create a new collection of groups
				var creator = new ObservableCollection<GroupingListView<string, Events>>(sorted);

				sorted = from item in Participating
						 group item by ParticipatingString into EventsGroup
							 select new GroupingListView<string, Events>(EventsGroup.Key, EventsGroup);

				var participating = new ObservableCollection<GroupingListView<string, Events>>(sorted);

				var united = new ObservableCollection<GroupingListView<string, Events>>();
				foreach (var p in creator.Union(participating))
					united.Add(p);
				eventsList.ItemsSource = united;
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