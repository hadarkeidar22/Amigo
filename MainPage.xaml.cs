using Amigo.Data;
using Amigo.Models;
using CarouselView.FormsPlugin.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
        
        Color selectedItem = Color.White;
		Color other = Color.Black;
		string searchBarText = "חפש אירועים ושותפים חדשים!";
		List<ContentView> CarouseleViews = new List<ContentView>();
		public static bool RefreshMyEvents = true;
		public static bool RefreshParticipatedEvents = true;

		Users currentUser = new Users();
		public MainPage()
		{
			NavigationPage.SetHasNavigationBar(this, false);
			InitializeComponent();
			topSearchBar.Text = searchBarText;
            topSearchBar.FontSize = 10;
			CarouseleViews.Add(new MyEventsPage());
			CarouseleViews.Add(new ParticipatedEvents());
            mainCarouselView.ItemsSource = CarouseleViews;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (RefreshMyEvents)
			{
				MessagingCenter.Send<MainPage>(this, "RefreshMyEvents");
				RefreshMyEvents = false;
			}
			if (RefreshParticipatedEvents)
			{
				MessagingCenter.Send<MainPage>(this, "RefreshParticipatedEvents");
				RefreshParticipatedEvents = false;
			}
		}
		public void AddEvent(object sender, EventArgs e)
		{
			Navigation.PushAsync(new CreateEventIntro(), true);
		}

		public void firstPageClick(object sender, TappedEventArgs e)
		{
			mainCarouselView.Position = 0;
			setColors();
		}
		public void secondPageClick(object sender, TappedEventArgs e)
		{
			mainCarouselView.Position = 1;
			setColors();
		}
        public void goToSettings(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPages.SettingsPage(), true);
        }
        public void setColors(object sender = null, PositionSelectedEventArgs e = null)
        {
            switch (mainCarouselView.Position)

            {
                case 0:
                    firstPage.TextColor = selectedItem;
                    secondPage.TextColor = other;
                    firstPage.Image = "groupblack";
                    secondPage.Image = "sketch32";
                    break;

                case 1:
                    firstPage.TextColor = other;
                    secondPage.TextColor = selectedItem;
                    firstPage.Image = "group";
                    secondPage.Image = "sketchBlack32";
                    break;
            }
        }
		
		public void keepSearchButtonText(object sender, PropertyChangedEventArgs e)
		{
			topSearchBar.Text = searchBarText;
		}
		public void openSearchPage(object sender, FocusEventArgs e)
		{
			Navigation.PushAsync(new SearchEventPage(), true);
		}

	}
}