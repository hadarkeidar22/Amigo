using Amigo.Data;
using Amigo.Models;
using Amigo.ViewModels;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectedEventView : ContentView
	{
		ObservableCollection<Exclusive> _exclusive = new ObservableCollection<Exclusive>();
		ObservableCollection<Exclusive> UsersListSource = new ObservableCollection<Exclusive>();

		Events _selectedEvent;

		public SelectedEventView(Events _selectedEvent)
		{

			InitializeComponent();
			this._selectedEvent = _selectedEvent;

			if (_selectedEvent.NoDateEvent == false && _selectedEvent.EventDateTime < DateTime.Today && _selectedEvent.CreatorID == LoadUserDetails.CurrentUser.AuthenticationID)
			{
				repitEvent.IsVisible = true;
			}

			NameOfEvent.Text = _selectedEvent.NameOfEvent;

			if(_selectedEvent.MaxNumOfParticipants != -1)
				NumOfParticipantsLabel.Text = "Patricipating: ("+ _selectedEvent.NumberOfParticipants.ToString() + "/" + _selectedEvent.MaxNumOfParticipants.ToString() +")";
			else
				NumOfParticipantsLabel.Text = "Patricipating: (" + _selectedEvent.NumberOfParticipants.ToString() + ")";

			string dateandhour = "";
			if (_selectedEvent.NoDateEvent)
				dateandhour = "No specific date";
			else
				dateandhour = _selectedEvent.DateDisplay;
			if (_selectedEvent.NoHourEvent)
				dateandhour += ", at no specific Hour";
			else
				dateandhour += ", at " + _selectedEvent.TimeDisplay;
			DateAndHour.Text = dateandhour;
			Location.Text = _selectedEvent.Location;
			Description.Text = _selectedEvent.Description;
			EventImage.Source = _selectedEvent.Image;
			if (_selectedEvent.College != "")
			{
				Exclusive college = new Exclusive("College:", _selectedEvent.College);
				_exclusive.Add(college);
			}
			if (_selectedEvent.CollegeFields != "")
			{
				Exclusive collegeFileds = new Exclusive("College Fields:", _selectedEvent.CollegeFields);
				_exclusive.Add(collegeFileds);
			}
			if (_selectedEvent.ClassOf != -1)
			{
				Exclusive ClassOf = new Exclusive("Class Of:", _selectedEvent.ClassOf.ToString());
				_exclusive.Add(ClassOf);
			}
			if (_selectedEvent.MinAge != 0)
			{
				Exclusive MinAge = new Exclusive("Min Age:", _selectedEvent.MinAge.ToString());
				_exclusive.Add(MinAge);
			}
			if (_selectedEvent.MaxAge != -1)
			{
				Exclusive MaxAge = new Exclusive("Max Age:", _selectedEvent.MaxAge.ToString());
				_exclusive.Add(MaxAge);
			}
			ExclusiveList.ItemsSource = _exclusive;
			
			foreach (Users participatingUser in _selectedEvent.ParticipatingUsers)
			{
				Exclusive user = new Exclusive(participatingUser.FirstName + " " + participatingUser.LastName, participatingUser.Image);
				user.UserID = participatingUser.AuthenticationID;
				UsersListSource.Add(user);
			}
			UsersList.ItemsSource = UsersListSource;
			UsersList.SelectedCommand = new Command<Exclusive>(UserSelected);
			
			JoinCancelButton();

			MessagingCenter.Subscribe<SelectedEvent>(this, "Delete", async (sender) =>
			{
				await DeleteEvent(true);
			});
		}
		private async void UserSelected(Exclusive _selectedUser)
		{
			if (App.Current.MainPage is NavigationPage & _selectedUser != null)
			{
				var selectedUserProfile = new UserProfile(_selectedEvent.ParticipatingUsers.Where(userItem => userItem.AuthenticationID ==_selectedUser.UserID).FirstOrDefault());
				await (App.Current.MainPage as NavigationPage).PushAsync(selectedUserProfile, true);
			}
		}
		public void repitEventBtnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RepitEventSetDate(_selectedEvent), true);
        }

		private void JoinCancelButton()
		{
			var joinBtn = JoinButton;
			var cancelBtn = CancelButton;
			if (_selectedEvent.CreatorID == LoadUserDetails.CurrentUser.AuthenticationID)
			{
				joinBtn.IsVisible = false;
				cancelBtn.IsVisible = false;
			}
			else if (_selectedEvent.ParticipantsId.Split(',').Contains(LoadUserDetails.CurrentUser.AuthenticationID))
			{
				joinBtn.IsVisible = false;
				cancelBtn.IsVisible = true;
			}
			else
			{
				joinBtn.IsVisible = true;
				cancelBtn.IsVisible = false;
			}
		}
		async void JoinEvent(object sender, ItemTappedEventArgs e)
		{
			await JoinToEvent(true);
			JoinCancelButton();
			RefreshMainPage(true);
		}
		async void CancelEvent(object sender, ItemTappedEventArgs e)
		{
			await CancelEvent(true);
			JoinCancelButton();
			RefreshMainPage(true);
		}
		private async Task JoinToEvent(bool showActivityIndicator)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				_selectedEvent.ParticipantsId += LoadUserDetails.CurrentUser.AuthenticationID + ",";
				_selectedEvent.NumberOfParticipants += 1;
				await DBItemManager.DefaultManager.SaveEventAsync(_selectedEvent);
				LoadUserDetails.CurrentUser.MyEvents += _selectedEvent.Id + ",";
				await DBItemManager.DefaultManager.SaveUserAsync(LoadUserDetails.CurrentUser);

			}
		}
		private async Task CancelEvent(bool showActivityIndicator)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				_selectedEvent.ParticipantsId = _selectedEvent.ParticipantsId.Replace(LoadUserDetails.CurrentUser.AuthenticationID + ",", "");
				_selectedEvent.NumberOfParticipants -= 1;
				await DBItemManager.DefaultManager.SaveEventAsync(_selectedEvent);
				LoadUserDetails.CurrentUser.MyEvents = LoadUserDetails.CurrentUser.MyEvents.Replace(_selectedEvent.Id + ",", "");
				await DBItemManager.DefaultManager.SaveUserAsync(LoadUserDetails.CurrentUser);
			}
		}
        private async Task DeleteEvent(bool showActivityIndicator)
        {
            using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
            {
                await DBItemManager.DefaultManager.DeleteEventAsync(_selectedEvent);
				RefreshMainPage(true);
			}
        }        
		private void RefreshMainPage(bool refresh)
		{
			MainPage.RefreshMyEvents = refresh;
			MainPage.RefreshParticipatedEvents = refresh;
		}
    }
    

    public class Exclusive
	{
		public string title;
		public string content;
		public string userID;
		public Exclusive(string title, string content)
		{
			this.title = title;
			this.content = content;
		}
		public string Title
		{
			get { return title; }
			set { title = value; }
		}

		public string Content
		{
			get { return content; }
			set { content = value; }
		}
		public string UserID
		{
			get { return userID; }
			set { userID = value; }
		}
	}
}
