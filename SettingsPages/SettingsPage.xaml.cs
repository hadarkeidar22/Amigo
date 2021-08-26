using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.WindowsAzure.MobileServices;
using Amigo.Data;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Linq;
using Amigo.Models;
using Amigo.Services;
using ImageCircle.Forms.Plugin.Abstractions;

namespace Amigo.SettingsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        ObservableCollection<Users> myUser = new ObservableCollection<Users>();
        List<Label> LabelList = new List<Label>();
        Users current = new Users();
        bool FirstTime = true;

        TagsButtons Tags = new TagsButtons();
        TagsButtons Tags2 = new TagsButtons();

        public SettingsPage()
        {
            InitializeComponent();
            current = LoadUserDetails.CurrentUser;
            DisplayPage();
        }

        public void DisplayPage()
        {
            sadbox1.WidthRequest = happyBox.Width;
            hisPhoto.Source = UriImageSource.FromUri(new Uri(current.Image));
            fullname.Text = current.FirstName + " " + current.LastName;
            hisAge.Text = current.Age.ToString();
            uniName.Text = current.College + "(" + current.CollegeFields + ")";
            	if (Application.Current.Properties.ContainsKey("MaxDistance"))
                {
                    MaxDistance.Value = Convert.ToDouble(Application.Current.Properties["MaxDistance"]);
                }                       

                string allInterests = current.Interests;
                List<string> hisInterests = allInterests.Split(',').ToList();
                foreach (string item in hisInterests)
                {
                    gridInterestsHelp.Text = item;
                    Tags.addTag(interestsGrid, gridInterestsHelp);

                }
                if (hisInterests.Count() == 4)
                {
                    gridInterestsHelp.Text = "";
                    Tags.addTag(interestsGrid, gridInterestsHelp);
                    Tags.addTag(interestsGrid, gridInterestsHelp);
                    interestsGrid.Children[5].IsVisible = false;
                    interestsGrid.Children[6].IsVisible = false;
                }
                if (hisInterests.Count() == 5)
                {
                    gridInterestsHelp.Text = "";
                    Tags.addTag(interestsGrid, gridInterestsHelp);
                    interestsGrid.Children[6].IsVisible = false;

                }
                string allHobbies = current.Hobbies;
                List<string> hisHobbies = allHobbies.Split(',').ToList();
                gridHobbiesHelp.Text = "";
                Tags2.addTag(HobbiesGrid, gridHobbiesHelp);
                Tags2.addTag(HobbiesGrid, gridHobbiesHelp);
                Tags2.addTag(HobbiesGrid, gridHobbiesHelp);

            foreach (string item in hisHobbies)
                {
                    gridHobbiesHelp.Text = item;
                    Tags2.addTag(HobbiesGrid, gridHobbiesHelp);
                }       
        }
        EventArgs f = new EventArgs();

            void OnTap(View arg1, object arg2)
        {
            editImageHelperclicked(arg2, f);
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            Application.Current.Properties["MaxDistance"] = value;
            if (FirstTime)
                FirstTime = false;
            else
                MainPage.RefreshMyEvents = true;
        }

            public void HobbiesBtnclicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPages.EditHobbies(current), true);
        }
            public void interestsBtnclicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPages.EditInterests(current), true);
        }
            public void editImageHelperclicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SettingsPages.EditInfo(current), true);
        }
        public async void deleteUser(object sender, EventArgs e)
        {
			var answer = await DisplayAlert("שים לב!", "פעולה זו הינה תמידית ולא ניתנת לשחזור! האם אתה בטוח שברצונך למחוק את החשבון?", "כן", "לא");
			if (answer)
			{
				await DBItemManager.DefaultManager.DeleteUserAsync(LoadUserDetails.CurrentUser);
				Application.Current.Properties.Clear();
				logOut();
			}
		}
        public void logOut(object sender = null, EventArgs e = null)
        {
			AuthStore.DeleteTokenCache();
			Navigation.InsertPageBefore(new WelcomePage(), this.Navigation.NavigationStack[0]);
			Navigation.PopToRootAsync();
		}

	}
    }










