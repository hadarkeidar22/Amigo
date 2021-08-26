using Amigo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Amigo.RegisterPages.UserInterests;
using Microsoft.WindowsAzure.MobileServices;
using Amigo.Views;
using Amigo.Models;

namespace Amigo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RepitEventSetDate : ContentPage
    {
        Events localPastEvent = new Events();
        Events newEvent = new Events();
        int numOfAmigosInTheEvent;

        public RepitEventSetDate(Events pastEvent)
        {
            InitializeComponent();
            CurrentPlatform.Init();
            localPastEvent = pastEvent;
            DatePicker1.MinimumDate = DateTime.Today;
            numOfAmigosInTheEvent = pastEvent.ParticipantsId.Length;
            explnationText1.HorizontalOptions = LayoutOptions.Center;

        }

        public void btn1Clicked(object sender, EventArgs e)
        {
            explnationText1.IsVisible = false;
            explnationBtn1.IsVisible = false;
            explnationText2.IsVisible = true;
            setBtn1.IsVisible = true;
		}
        public void setBtn1Clicked(object sender, EventArgs e)
        {
            explnationText2.IsVisible = false;
            setBtn1.IsVisible = false;
            DatePicker1.IsVisible = true;
            HourPicker.IsVisible = true;
            setBtn101.IsVisible = true;
        }
        public void setBtn101Clicked(object sender, EventArgs e)
        {

            string dateAndTime = "at the " + DatePicker1.Date.ToString().Substring(0,10) + " - (" + HourPicker.Time.ToString().Substring(0,5)+")";
            DatePicker1.IsVisible = false;
            HourPicker.IsVisible = false;
            setBtn101.IsVisible = false;
            dateTimeText1.IsVisible = true;
            dateTimeText2.IsVisible = true;
            setBtn2.IsVisible = true;
            saveBtn.IsVisible = true;
            dateTimeText1.Text = "All of your " + numOfAmigosInTheEvent.ToString() + " Amigos will be reinvited to this event";
            dateTimeText2.Text = dateAndTime;
        }
        public void setBtn2Clicked(object sender, EventArgs e)
        {

            DatePicker1.IsVisible = true;
            HourPicker.IsVisible = true;
            setBtn101.IsVisible = true;
            dateTimeText1.IsVisible = false;
            dateTimeText2.IsVisible = false;
            setBtn2.IsVisible = false;
            saveBtn.IsVisible = false;

        }
        public async void saveBtnClicked(object sender, EventArgs e)
        {
            if (await CreateEventFunction())
            {
                await App.Current.MainPage.DisplayAlert("Congrats!", "Event created successfully!", "OK");
                await (App.Current.MainPage as NavigationPage).PopToRootAsync();
            }
        }


        public async Task<bool> CreateEventFunction()
        {

            newEvent.EventDateTime.Add(HourPicker.Time);
            newEvent.EventDateTime = DatePicker1.Date;
            var MinNumOfParticipants = localPastEvent.MinNumOfParticipants;
            var MaxNumOfParticipants = localPastEvent.MaxNumOfParticipants;
            newEvent.Location = localPastEvent.Location;
            newEvent.Latitude = localPastEvent.Latitude;
            newEvent.Longitude = localPastEvent.Longitude;
            newEvent.NameOfEvent = localPastEvent.NameOfEvent;
            newEvent.Description = localPastEvent.Description;
            newEvent.MinNumOfParticipants = localPastEvent.MinNumOfParticipants;
            newEvent.MinNumOfParticipants = localPastEvent.MaxNumOfParticipants;
            newEvent.Interests = localPastEvent.Interests;
            newEvent.College = localPastEvent.College;
            newEvent.CollegeFields = localPastEvent.CollegeFields;
            newEvent.ClassOf = localPastEvent.ClassOf;
            newEvent.Tags = localPastEvent.Tags;
            newEvent.Gender = localPastEvent.Gender;
            newEvent.MinAge = localPastEvent.MinAge;
            newEvent.MaxAge = localPastEvent.MaxAge;
            newEvent.CreatorID = LoadUserDetails.CurrentUser.AuthenticationID;
            newEvent.ParticipantsId = "";
            newEvent.PrivateEvent = false;
            newEvent.Image = "";

            ActivityIndicator syncIndicator = new ActivityIndicator();

            using (var scope = new ActivityIndicatorScope(syncIndicator, true))
            {
                await DBItemManager.DefaultManager.SaveEventAsync(newEvent);
                MainPage.RefreshParticipatedEvents = true;
                return true;
            }
            
        }
    }

}
