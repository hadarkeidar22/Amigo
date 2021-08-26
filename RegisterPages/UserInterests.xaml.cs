using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amigo.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.RegisterPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserInterests : ContentPage
	{
		InterestsButtons Interests = new InterestsButtons();
		public UserInterests()
		{
			InitializeComponent();
			Interests.AllowMultipleButtons = true;
			Interests.Init(InterestsGrid, Constants.Interests);
        }
		public void AddHobbies(object sender, EventArgs args)
		{
			InterestsString.Text = Interests.CreateInterestsString();
			if(InterestsString.Text=="")
				DisplayAlert("אוי לא...", "אמיגו מעוניין להכיר אותך, בחר לפחות תחום עיניין אחד ונוכל להמשיך", "אוקיי");
			else
				MessagingCenter.Send<UserInterests>(this, "Hobbies");
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			InterestsString.Text = Interests.CreateInterestsString();
		}
	}
}