using Amigo.Data;
using Amigo.Models;
using Microsoft.WindowsAzure.MobileServices;
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
    public partial class SearchEventPage : ContentPage
    {
        InterestsButtons Interests = new InterestsButtons();
        //InterestsButtons DatesOptions = new InterestsButtons();

        double FormstartingHeight;
        public static string[] HisCollege = new string[] { LoadUserDetails.CurrentUser.College };
        public SearchEventPage()
        {
            InitializeComponent();
            CurrentPlatform.Init();
			Interests.AllowMultipleButtons = true;
            Interests.Init(InterestsGrid, Constants.Interests);
           // DatesOptions.Init(UserInfoGrid, Constants.Dates);
            
           
        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
			FormstartingHeight = searchForm.Height;
		}
		public async void SearchBtn(object sender, EventArgs e)
		{
			await SearchEvents(true);

			// setup information for animation
			Action<double> callback = input => { searchForm.HeightRequest = input; }; // update the height of the layout with this callback
			double startingHeight = searchForm.Height; // the layout's height when we begin animation
			double endingHeight = 0; // final desired height of the layout
			uint rate = 16; // pace at which aniation proceeds
			uint length = 1000; // one second animation
			Easing easing = Easing.CubicOut; // There are a couple easing types, just tried this one for effect

			// now start animation with all the setup information
			searchForm.Animate("invis", callback, startingHeight, endingHeight, rate, length, easing);
		}
		
     /*   public async void whenBtn(object sender, EventArgs e)
        {
            wheno.IsVisible = false;
            UserInfoGrid.IsVisible = true;
            dCWhen.IsVisible = true;
        }
        public async void dCWhenBtn(object sender, EventArgs e)
        {
            wheno.IsVisible = true;
            UserInfoGrid.IsVisible = false;
            dCWhen.IsVisible = false;
        }
*/
	 
        public  void searchBarClick(object sender, EventArgs e)
		{
			if (searchForm.Height == 0)
			{
				// setup information for animation
				Action<double> callback = input => { searchForm.HeightRequest = input; }; // update the height of the layout with this callback
				double startingHeight = 0; // the layout's height when we begin animation
				double endingHeight = FormstartingHeight; // final desired height of the layout
				uint rate = 16; // pace at which aniation proceeds
				uint length = 1000; // one second animation
				Easing easing = Easing.CubicOut; // There are a couple easing types, just tried this one for effect

				// now start animation with all the setup information
				searchForm.Animate("show", callback, startingHeight, endingHeight, rate, length, easing);
				eventsList.Focus();
			}
		}
		public async void OnRefresh(object sender, EventArgs e)
		{
			var list = (ListView)sender;
			Exception error = null;
			try
			{
				await SearchEvents(false);
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


		public async Task SearchEvents(bool showActivityIndicator)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				string interests = Interests.CreateInterestsString();
				string search = searchBar.Text;
               // string dates = DatesOptions.CreateInterestsString();
                //dCWhen.Text = dates;
                if (search == null)
					search = "";
				IEnumerable<Events> items = await DBItemManager.DefaultManager.SearchEvents(search, interests);
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
			{
				var selectedEventPage = new SelectedEvent(_selectedEvent);
				(App.Current.MainPage as NavigationPage).PushAsync(selectedEventPage, true);
			}
		}
	}
}