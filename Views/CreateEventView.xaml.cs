using Amigo.Data;
using Amigo.Models;
using Amigo.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateEventView : ContentView
	{
		int MinimumSearchText = 3;

		Events newEvent = new Events();
		PlacesAPI PlacesSearch = new PlacesAPI();
		TagsButtons Tags = new TagsButtons();
		InterestsButtons Interests = new InterestsButtons();
		bool showCollegeForm = false;
		bool showTagsForm = false;
		bool showOtherInfoForm;
		bool FirstTime = true;
		ObservableCollection<AddressInfo> places = new ObservableCollection<AddressInfo>();
		bool skipTextChange = false;
		double Longitude { get; set; } = 0;
		double Latitude { get; set; } = 0;
		string Location { get; set; } = "";
        List<Button> btnList = new List<Button> { };
        int counter = 0;

        public CreateEventView(Events updateEvent = null)
		{
			InitializeComponent();
			CurrentPlatform.Init();
			DatePicker.MinimumDate = DateTime.Today;
			Tags.SetColumnsNumInRow = 3;
			Interests.Init(InterstsButtons, Constants.Interests);
			if(updateEvent != null & FirstTime)
			{
				UpdateFormFromEvent(updateEvent);
				newEvent = updateEvent;
				FirstTime = false;
			}
			MessagingCenter.Subscribe<SelectedEvent>(this, "Update", async(sender) =>
			{
				if(await CreateEventFunction())
					MessagingCenter.Send(this, "Update Success");
			});
            tagsEX.Text = "הוספת תגיות עוזרת לאמיגו להתפרסם אצל קהל היעד שלך" +
                "\n" +
                "לדוגמא: כדורסל, יוגה, טיול..";

            collegeEX.Text = "באפשרותך לבחור מסינונים אלו כדי לברור " +
                "\n" +
                "את קהל היעד שלך";

            foreach(string title in Constants.imagesLinks)
            {
                //var imaged = new Image();
                Button btn = new Button();
                btn.Image = title;
                btn.HeightRequest = 64;
                btn.WidthRequest = 64;
                btn.BorderRadius = 32;
                btn.Clicked += thisImageClicked;
                btnList.Add(btn);
                imagesGrid.Children.Add(btn, counter, 0);
                counter++;
            }           

        }
		private async void OnTextChanged(object sender=null, EventArgs eventArgs=null)
		{
			if (!string.IsNullOrWhiteSpace(PlacesSearchBar.Text) && PlacesSearchBar.Text.Length > MinimumSearchText && !skipTextChange)
			{
				try
				{
					places.Clear();
					spinner.IsVisible = true;
					spinner.IsRunning = true;
					places = await PlacesSearch.GetPlacesPredictionsAsync(PlacesSearchBar.Text);
					spinner.IsRunning = false;
					spinner.IsVisible = false;
					if (places != null && places.Count() > 0)
					{
						PlacesList.IsVisible = true;
						PlacesList.ItemsSource = places;
					}
				}
				catch(Exception ex)
				{
					Debug.WriteLine("Error invoking GetPlacesPreditcions: {0}", ex.Message);
					Thread.Sleep(2000);
					OnTextChanged();
				}
			}
		}
		private void UnFocusSearchBar(object sender, EventArgs eventArgs)
		{
			PlacesList.IsVisible = false;
		}
		private async void FocusSearchBar(object sender, EventArgs eventArgs)
		{
			PlacesList.IsVisible = true;
			await newEventForm.ScrollToAsync(PlacesSearchBarLayout, ScrollToPosition.Start, true);
			PlacesSearchBar.Text = "";
			Location = "";
			Latitude = 0;
			Longitude = 0;
		}
		async void PlacesListSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
				return;

			var prediction = (AddressInfo)e.SelectedItem;

			var place = await PlacesSearch.GetPlaceGeo(prediction.PlaceID);

			if (place != null)
			{
				Longitude = place.Longitude;
				Latitude = place.Latitude;
				Location = prediction.Address;
			}
			skipTextChange = true;
			PlacesSearchBar.Text = Location;				
			PlacesList.IsVisible = false;
			skipTextChange = false;
		}

		public void UpdateFormFromEvent(Events updateEvent)
		{
			EventName.Text = updateEvent.NameOfEvent;
			Description.Text = updateEvent.Description;
			if (updateEvent.NoDateEvent)
				NoDate.IsToggled = true;
			else
				DatePicker.Date = updateEvent.EventDateTime.ToLocalTime().Date;
			if (updateEvent.NoHourEvent)
				NoHour.IsToggled = true;
			else
				HourPicker.Time = updateEvent.EventDateTime.ToLocalTime().TimeOfDay;
			if (updateEvent.MinNumOfParticipants != 0)
				MinParticipants.Text = updateEvent.MinNumOfParticipants.ToString();
			if (updateEvent.MaxNumOfParticipants != -1)
				MaxParticipants.Text = updateEvent.MaxNumOfParticipants.ToString();
			skipTextChange = true;
			PlacesSearchBar.Text = updateEvent.Location;
			Longitude = updateEvent.Longitude;
			Latitude = updateEvent.Latitude;
			Location = updateEvent.Location;
			skipTextChange = false;
			var buttonsListForLoop = Interests.TotalbuttonsList.ToList();
			foreach (Button interest in buttonsListForLoop)
				if (updateEvent.Interests == interest.Text)
					Interests.ChooseInterest(interest, null);
			if (updateEvent.College != "" || updateEvent.CollegeFields != "" || updateEvent.ClassOf != -1)
			{
				CollegeForms.IsVisible = true;
				if (updateEvent.College != "")
					CollegeSwitch.IsToggled = true;
				if (updateEvent.CollegeFields != "")
					CollegeField.IsToggled = true;
				if (updateEvent.ClassOf != -1)
					ClassSwitch.IsToggled = true;
			}
			if (updateEvent.Tags != "")
			{
				AddTagsForm.IsVisible = true;
				foreach (string tag in updateEvent.Tags.Split(','))
					Tags.addTag(Morehobbiesgrid, new Entry() { Text = tag });
			}
			if (updateEvent.Gender != -1 || updateEvent.MinAge != 0 || updateEvent.MaxAge != -1)
			{
				AddOtherInfoForm.IsVisible = true;
				if (updateEvent.Gender != -1)
					genderFilterSwitch.IsToggled = true;
				if (updateEvent.MinAge != 0)
					MinAgeEntery.Text = updateEvent.MinAge.ToString();
				if (updateEvent.MaxAge != -1)
					MaxAgeEntery.Text = updateEvent.MaxAge.ToString();
			}
			lastImage.Image = updateEvent.Image;
			///
			lastImage.IsVisible = true;
			imagePicker.IsVisible = false;
			theImagesStack.HeightRequest = 0;
			imagesButton.IsVisible = false;
			///
			CreateBtn.IsVisible = false;
		}
		public async void CreateEvent(object sender, EventArgs e)
		{
			if (await CreateEventFunction())
			{
				await App.Current.MainPage.DisplayAlert("כל הכבוד!", "יצרנו אמיגו והרוב בזכותך! אפשר להמשיך לעקוב אחריו במסך האמיגואים בניהולך", "מעולה");
				//await (App.Current.MainPage as NavigationPage).PopToRootAsync();
			}

		}
		public async Task<bool> CreateEventFunction()
		{
			string ErrorMsg = "";
			if (string.IsNullOrWhiteSpace(EventName.Text))
				ErrorMsg += "\nשם האמיגו,";
			if (string.IsNullOrWhiteSpace(Description.Text))
				ErrorMsg += "\nפירוט לאמיגו,";
			string interests = Interests.CreateInterestsString();
			if (interests == "")
				ErrorMsg += "\nנצטרך לבחור לפחות קטגוריה אחת,";
			if (Location == "" || Latitude == 0 || Longitude == 0)
				ErrorMsg += "\nבחירת מיקום,";
			if (ErrorMsg == "")
			{
				if (NoDate.IsToggled)
				{
					newEvent.NoDateEvent = true;
					newEvent.EventDateTime = Constants.DummyDate;
				}
				else
					newEvent.EventDateTime = DatePicker.Date;
				if (NoHour.IsToggled)
				{
					newEvent.NoHourEvent = true;
					newEvent.EventDateTime.Add(Constants.DummyHour);
				}
				else
					newEvent.EventDateTime.Add(HourPicker.Time);
				newEvent.EventDateTime = DateTime.SpecifyKind(newEvent.EventDateTime.DateTime, DateTimeKind.Utc);
				var MinNumOfParticipants = MinParticipants.Text;
				var MaxNumOfParticipants = MaxParticipants.Text;
				newEvent.Location = Location;
				newEvent.Latitude = Latitude;
				newEvent.Longitude = Longitude;
				newEvent.NameOfEvent = EventName.Text;
				newEvent.Description = Description.Text;
				if (!string.IsNullOrWhiteSpace(MinNumOfParticipants))
					newEvent.MinNumOfParticipants = Int32.Parse(MinNumOfParticipants);
				else
					newEvent.MinNumOfParticipants = 0;
				if (!string.IsNullOrWhiteSpace(MaxNumOfParticipants))
					newEvent.MaxNumOfParticipants = Int32.Parse(MaxNumOfParticipants);
				else
					newEvent.MaxNumOfParticipants = -1;
				newEvent.Interests = interests;
				newEvent.College = "";
				newEvent.CollegeFields = "";
				newEvent.ClassOf = -1;
				if (CollegeSwitch.IsToggled)
					newEvent.College = LoadUserDetails.CurrentUser.College;
				if (CollegeField.IsToggled)
					newEvent.CollegeFields = LoadUserDetails.CurrentUser.CollegeFields;
				if (ClassSwitch.IsToggled)
					newEvent.ClassOf = LoadUserDetails.CurrentUser.ClassOf;
				newEvent.Tags = Tags.CreateTagsString();
				newEvent.Gender = -1;
				newEvent.NumberOfParticipants = 1;
				if (!string.IsNullOrWhiteSpace(MinAgeEntery.Text))
					newEvent.MinAge = Int32.Parse(MinAgeEntery.Text);
				else
					newEvent.MinAge = 0;
				if (!string.IsNullOrWhiteSpace(MaxAgeEntery.Text))
					newEvent.MaxAge = Int32.Parse(MaxAgeEntery.Text);
				else
					newEvent.MaxAge = -1;
				if (genderFilterSwitch.IsToggled)
					newEvent.Gender = LoadUserDetails.CurrentUser.Gender;
				newEvent.CreatorID = LoadUserDetails.CurrentUser.AuthenticationID;
				newEvent.ParticipantsId = "";
				newEvent.PrivateEvent = false;
				newEvent.Image = lastImage.Image;
				using (var scope = new ActivityIndicatorScope(syncIndicator, true))
				{
					await DBItemManager.DefaultManager.SaveEventAsync(newEvent);
					MainPage.RefreshParticipatedEvents = true;
					return true;
				}
               
            }
			else
			{
				await App.Current.MainPage.DisplayAlert("אוי לא...", "נשאר לנו למלא את הפרטים הבאים:" + ErrorMsg.Substring(0, ErrorMsg.Length - 1), "אוקיי");
			}
			return false;

		}

        // Page UI Functions:
        public void putImages(object sender, EventArgs e)
        {
            imagePicker.IsVisible = true;
            theImagesStack.HeightRequest = 70;
        }
        public void thisImageClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            lastImage.IsVisible = true;
            //mainImage.IsVisible = true;
            imagePicker.IsVisible = false;
            lastImage.Image = btn.Image;
            theImagesStack.HeightRequest = 0;
            imagesButton.IsVisible = false;
		}
		public void NoDateSwitch(object sender, EventArgs e)
		{
			DatePicker.IsVisible = !NoDate.IsToggled;
            App.Current.MainPage.DisplayAlert("חשוב לדעת", "אמיגואים ללא ציון תאריך ישארו במערכת לתקופה של שבועיים ולאחר מכן לא יפורסמו", "לקחתי לתשומת ליבי");

        }
		public void NoHourSwitch(object sender, EventArgs e)
		{
			HourPicker.IsVisible = !NoHour.IsToggled;
            var thatDay = DateTime.Now;
            var afterDay = thatDay.AddDays(1);

        }
		private void CheckRange(string min, string max)
		{
			if (!string.IsNullOrWhiteSpace(min))
			{
				if (Int32.Parse(this.FindByName<Entry>(min).Text) > Int32.Parse(this.FindByName<Entry>(max).Text))
				{
					App.Current.MainPage.DisplayAlert("אוי לא...", "כנראה רשמנו שהכמות המינימאלית להזמנת אנשים הינה גבוהה מהכמות המקסימאלית", "מיד לתקן");
					this.FindByName<Entry>(max).Text = this.FindByName<Entry>(min).Text;
				}
			}
		}
		private void CheckMinMax(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(this.FindByName<Entry>("MinParticipants").Text))
				CheckRange("MinParticipants", "MaxParticipants");
		}
		async void ShowCollegeForms(object sender, EventArgs e)
		{
			var btn = this.FindByName<Button>("CollegeBtn");
			showCollegeForm = !showCollegeForm;
			if (!showCollegeForm)
			{
				newEvent.College = "";
				newEvent.CollegeFields = "";
				newEvent.ClassOf = -1;
				await newEventForm.ScrollToAsync(CollegeBtn, ScrollToPosition.End, true);
				CollegeForms.IsVisible = false;
			}
			else
			{
				CollegeForms.IsVisible = true;
				await Task.Yield();
				await newEventForm.ScrollToAsync(CollegeForms, ScrollToPosition.End, true);
			}
		}

		async void ShowTagsForms(object sender, EventArgs e)
		{
			var btn = TagsBtn;
			showTagsForm = !showTagsForm;
			if (!showTagsForm)
			{
				newEvent.Tags = "";
				await newEventForm.ScrollToAsync(TagsBtn, ScrollToPosition.End, true);
				AddTagsForm.IsVisible = false;
			}
			else
			{
				AddTagsForm.IsVisible = true;
				await Task.Yield();
				await newEventForm.ScrollToAsync(AddTagsForm, ScrollToPosition.End, true);
			}
		}

		void HobbiesaddedButtonClicked(object sender2, EventArgs e)
		{
			if (Morehobbiescell.Text == null)
				Morehobbiescell.Text = "";
			if (Morehobbiescell.Text.Trim() != "")
			{
				Tags.addTag(Morehobbiesgrid, Morehobbiescell);
				Morehobbiescell.Text = "";
			}
		}

		async void ShowOtherInfoForms(object sender, EventArgs e)
		{
			var btn = OtherInfoBtn;
			showOtherInfoForm = !showOtherInfoForm;
			if (!showOtherInfoForm)
			{
				newEvent.College = "";
				newEvent.CollegeFields = "";
				newEvent.ClassOf = -1;
				await newEventForm.ScrollToAsync(OtherInfoBtn, ScrollToPosition.End, true);
				AddOtherInfoForm.IsVisible = false;
			}
			else
			{
				AddOtherInfoForm.IsVisible = true;
				await Task.Yield();
				await newEventForm.ScrollToAsync(AddOtherInfoForm, ScrollToPosition.End, true);
			}
		}
		private void CheckAgeRange(object sender, EventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(MinAgeEntery.Text))
				CheckRange("MinAgeEntery", "MaxAgeEntery");
		}
	}

}