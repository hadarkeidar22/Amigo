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
	public partial class UserCollege : ContentPage
	{
		public UserCollege ()
		{
			InitializeComponent ();
		}
		private void AddCollegeField(object sender, EventArgs e)
		{
			(sender as Button).IsVisible = false;
			SecondCollegeField.IsVisible = true;
			RemoveCollegeFieldButton.IsVisible = true;
            FirstCollegeField.WidthRequest = 190;
		}
		private void RemoveCollegeField(object sender, EventArgs e)
		{
			(sender as Button).IsVisible = false;
			AddCollegeFieldButton.IsVisible = true;
			SecondCollegeField.IsVisible = false;
            FirstCollegeField.WidthRequest = 150;
        }
		public void AddInterests(object sender, EventArgs args)
		{
			MessagingCenter.Send<UserCollege>(this, "Interests");
		}
	}
}