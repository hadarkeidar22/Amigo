using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChooseEventsRadius : ContentPage
	{
		private Position CurrentPosition;
		public ChooseEventsRadius (double latitude, double longitude)
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
			CurrentPosition = new Position(latitude, longitude);
			MaxDistance.Value = 15;
			MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(
					CurrentPosition, Distance.FromKilometers(MaxDistance.Value)));
		}
		private void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
		{
			double value = args.NewValue;
			MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(
					CurrentPosition, Distance.FromKilometers(value)));
		}
		private async void ChooseRadius(object sender, EventArgs args)
		{
			Application.Current.Properties["MaxDistance"] = MaxDistance.Value;
			await Navigation.PushAsync(new MainPage());
			Navigation.RemovePage(this);
		}
	}
}