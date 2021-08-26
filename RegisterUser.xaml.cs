using Amigo.Data;
using Amigo.Models;
using Amigo.RegisterPages;
using ImageCircle.Forms.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Amigo.Data.DBItemManager;

namespace Amigo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterUser : CarouselPage
	{
		public bool error = false;
		private string AuthenticationID;
		private string UserImage = "DefaultUserImage.png";
		public RegisterUser(string UserAuthenticationID, Users FacebookProfile = null)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			bool DetailsAdded = false;
			bool CollegeAdded = false;
			bool InterestsAdded = false;
			bool HobbiesAdded = false;
			AuthenticationID = UserAuthenticationID;

			Children.Add(new Opening());
			MessagingCenter.Subscribe<Opening>(this, "Details", (sender) => 
			{
				if (DetailsAdded  == false)
				{
					Children.Add(new UserDetails());
					DetailsAdded = true;
				}
				this.CurrentPage = Children[Children.IndexOf(CurrentPage) +1] ;
				if(FacebookProfile != null)
				{
					UserImage = FacebookProfile.Image;
					Children[1].FindByName<Entry>("FirstName").Text = FacebookProfile.FirstName;
					Children[1].FindByName<Entry>("LastName").Text = FacebookProfile.LastName;
					Children[1].FindByName<CircleImage>("UserPhoto").Source = UriImageSource.FromUri(new Uri(FacebookProfile.Image));
					if (FacebookProfile.Gender == 1)
						Children[1].FindByName<Picker>("Gender").SelectedIndex = 0;
					if (FacebookProfile.Gender == 0)
						Children[1].FindByName<Picker>("Gender").SelectedIndex = 1;
					if (FacebookProfile.Birthday != null)
						Children[1].FindByName<DatePicker>("DatePicker").Date = FacebookProfile.Birthday;
				}
			});
			MessagingCenter.Subscribe<UserDetails>(this, "College", (sender) =>
			{
				if (CollegeAdded == false)
				{
					Children.Add(new UserCollege());
					CollegeAdded = true;
				}
				this.CurrentPage = Children[Children.IndexOf(CurrentPage) + 1];
			});
			MessagingCenter.Subscribe<UserCollege>(this, "Interests", (sender) =>
			{
				if (InterestsAdded == false)
				{
					Children.Add(new UserInterests());
					InterestsAdded = true;
				}
				this.CurrentPage = Children[Children.IndexOf(CurrentPage) + 1];
			});
			MessagingCenter.Subscribe<UserInterests>(this, "Hobbies", (sender) =>
			{
				if (HobbiesAdded == false)
				{
					Children.Add(new UserHobbies());
					HobbiesAdded = true;
				}
				this.CurrentPage = Children[Children.IndexOf(CurrentPage) + 1];
			});
			MessagingCenter.Subscribe<UserHobbies>(this, "RegisterPage", (sender) =>
			{
				Users newUser = getAllData();
				if (error == false)
				{
				Navigation.PushAsync(new UserRegister(newUser), true);
				Navigation.RemovePage(this);
				}
			});
		}
		public Users getAllData()
		{
			Users newUser = new Users();
			try
			{
				newUser.AuthenticationID = AuthenticationID;
				newUser.Image = UserImage;
				newUser.MyEvents = "";
				newUser.FirstName = Children[1].FindByName<Entry>("FirstName").Text;
				newUser.LastName = Children[1].FindByName<Entry>("LastName").Text;
				newUser.Birthday = Children[1].FindByName<DatePicker>("DatePicker").Date;
				var gender = Children[1].FindByName<Picker>("Gender");
				switch (gender.Items[gender.SelectedIndex])
				{
					case "Male":
						newUser.Gender = 1;
						break;
					case "Female":
						newUser.Gender = 0;
						break;
					case "Other":
						newUser.Gender = 2;
						break;
				}
				var collegePicker = Children[2].FindByName<Picker>("College");
				newUser.College = collegePicker.Items[collegePicker.SelectedIndex];
				var collegeFieldsPicker1 = Children[2].FindByName<Picker>("FirstCollegeField");
				newUser.CollegeFields = collegeFieldsPicker1.Items[collegeFieldsPicker1.SelectedIndex];
				if (Children[2].FindByName<Picker>("SecondCollegeField").IsVisible == true)
				{
					var collegeFieldsPicker2 = Children[2].FindByName<Picker>("SecondCollegeField");
					newUser.CollegeFields += "," + collegeFieldsPicker2.Items[collegeFieldsPicker2.SelectedIndex];
				}
				newUser.ClassOf = Int32.Parse(Children[2].FindByName<Entry>("ClassOf").Text);
				newUser.Interests = Children[3].FindByName<Label>("InterestsString").Text;
				newUser.Hobbies = Children[4].FindByName<Label>("HobbiesString").Text;
				error = false;
			}
			catch
			{
				DisplayAlert("אוי לא...","בואו נבדוק מה חסר, כנראה שכחת פרט או שניים במילוי הפרטים","ok");
				error = true;
			}
			return newUser;
		}
		protected override bool OnBackButtonPressed()
		{
			if (Children.IndexOf(CurrentPage) == 0)
				return true;
			this.CurrentPage = Children[Children.IndexOf(CurrentPage) - 1];
			return true;
		}
	}
}