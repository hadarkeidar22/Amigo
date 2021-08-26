using Amigo.Data;
using Amigo.Models;
using Amigo.Views;
using CarouselView.FormsPlugin.Abstractions;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Amigo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedEvent : ContentPage
    {
		Color selectedItem = Color.White;
		Color other = Color.Black;
		Events _selectedEvent;
		ToolbarItem editToolBar;
		ToolbarItem deleteToolBar;
		ToolbarItem saveToolBar;
		List<ContentView> CarouseleViews = new List<ContentView>();

		public SelectedEvent(Events _selectedEvent)
		{
			InitializeComponent();
			this._selectedEvent = _selectedEvent;

			if(_selectedEvent.CreatorID == LoadUserDetails.CurrentUser.AuthenticationID)
			{
				editToolBar = new ToolbarItem("Edit", "editBtn.png", () => { EditEvent(); });
				deleteToolBar = new ToolbarItem("Delete", "deleteBtn.png", () => { DeleteEvent(); });
				saveToolBar = new ToolbarItem("Save", "saveBtn.png", () => { SaveChanges(); });
				this.ToolbarItems.Add(editToolBar);
				this.ToolbarItems.Add(deleteToolBar);
			}
		}
		protected override async void OnAppearing()
		{
			_selectedEvent.ParticipatingUsers = await DBItemManager.DefaultManager.GetUsersInEvent(_selectedEvent.CreatorID + "," + _selectedEvent.ParticipantsId);

			CarouseleViews.Add(new SelectedEventView(_selectedEvent));
			CarouseleViews.Add(new MessegesView(_selectedEvent));
			SelectedCarouselView.ItemsSource = CarouseleViews;
		}
		protected override async void OnDisappearing()
		{
			if(ViewModels.ChatPageViewModel.ConnectionEstablished)
				await ViewModels.ChatPageViewModel._chatHubProxy.Invoke("LeaveRoom", _selectedEvent.Id);

		}
		private void firstPageClick(object sender, TappedEventArgs e)
		{
			SelectedCarouselView.Position = 0;
			SetColors();
		}
		private void secondPageClick(object sender, TappedEventArgs e)
		{
			SelectedCarouselView.Position = 1;
			SetColors();
			MessagingCenter.Send(this, "EnterChatPage");
		}

		private void EditEvent()
		{
			CarouseleViews.RemoveRange(0,2);
			CarouseleViews.Add(new CreateEventView(_selectedEvent));
			SelectedCarouselView.ItemsSource = CarouseleViews;
			BottomMenu.IsVisible = false;
			this.ToolbarItems.RemoveAt(0);
			this.ToolbarItems.RemoveAt(0);
			this.ToolbarItems.Add(saveToolBar);
		}
		private void SaveChanges()
		{
			MessagingCenter.Send(this, "Update");
			MessagingCenter.Subscribe<CreateEventView>(this, "Update Success", async (sender) =>
			{
				await DisplayAlert("Congrats!", "Event updated successfully!", "OK");

				CarouseleViews.RemoveAt(0);
				CarouseleViews.Add(new SelectedEventView(_selectedEvent));
				CarouseleViews.Add(new MessegesView(_selectedEvent));
				SelectedCarouselView.ItemsSource = CarouseleViews;
				BottomMenu.IsVisible = true;
				this.ToolbarItems.RemoveAt(0);
				this.ToolbarItems.Add(editToolBar);
				this.ToolbarItems.Add(deleteToolBar);
			});
		}
		private async void DeleteEvent()
		{
			var answer = await DisplayAlert("This action is permanent!", "Are you sure you want to delete this activity?", "I'm sure", "Cancel");
			if (answer)
			{
				MessagingCenter.Send(this, "Delete");
				await Navigation.PopToRootAsync();
			}

		}
		public void SetColors(object sender = null, PositionSelectedEventArgs e = null)
		{
			switch (SelectedCarouselView.Position)
			{
				case 0:
					firstPage.TextColor = selectedItem;
					secondPage.TextColor = other;
					break;

				case 1:
					firstPage.TextColor = other;
					secondPage.TextColor = selectedItem;
					break;
			}
		}
	}
}
