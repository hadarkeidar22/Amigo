using Amigo.Data;
using Amigo.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Amigo.ViewModels
{
    class ChatPageViewModel : INotifyPropertyChanged
	{
		public static bool ConnectionEstablished = false;
		private DateTime lastSync;
		public ChatPageViewModel()
		{
			GetMessages().Wait();
			MessagingCenter.Subscribe<SelectedEvent>(this, "EnterChatPage", async (sender) =>
			{
				Status = "Connecting...";
				lastSync = DateTime.Now;
				await InitializeHub();
				Status = "Online.";
			});
			SendMessageCommand = new Command<string>(async (parameter) =>
			{

				Amigo.Models.Messages newMessage = new Amigo.Models.Messages()
				{
					EventId = Room.Id,
					Sender = CurrentUser.AuthenticationID,
					SentTime = DateTime.Now,
					Text = parameter,
					SenderFullName = string.Format("{0} {1}", CurrentUser.FirstName, CurrentUser.LastName),
				};
				Messages.Add(newMessage);
				NewMessageText = string.Empty;
				await DBItemManager.DefaultManager.SaveMessageAsync(newMessage);

				await _chatHubProxy.Invoke("UserStopTypingInRoom", Room.Id, LoadUserDetails.CurrentUser.AuthenticationID);
				await _chatHubProxy.Invoke("Send");
			});			
		}

		public static IHubProxy _chatHubProxy;

		private async Task InitializeHub()
		{
			try
			{
				// Connect to the server
				var hubConnection = new HubConnection(Constants.ApplicationURL);

				// Create a proxy to the 'ChatHub' SignalR Hub
				_chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
				// Wire up a handler for the 'UpdateChatMessage' for the server
				// to be called on our client
				_chatHubProxy.On("broadcastMessage", async () =>
				{
					await GetNewMessages();
				});
			
				_chatHubProxy.On<string>("startTyping", s =>
				{
					if(s != LoadUserDetails.CurrentUser.AuthenticationID)
					{
						Users typer = Room.ParticipatingUsers.Where(userItem => userItem.AuthenticationID == s).FirstOrDefault();
						Status = string.Format("{0} {1} is typing...", typer.FirstName, typer.LastName);
					}
				});
			
				_chatHubProxy.On<string>("stopTyping", s =>
				{
					Status = "Online";
				});
				// Start the connection
				await hubConnection.Start();
				await _chatHubProxy.Invoke("JoinRoom", Room.Id);
				ConnectionEstablished = true;
			}
			catch(InvalidOperationException ex)
			{
				Debug.WriteLine("Error invoking GetAllStocks: {0}", ex.Message);
			}
		}
	
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private void UpdateSendersFullName()
		{
			if (Messages.Count > 0)
			{
				for(int i=0; i < Messages.Count; i++)
				{
					Users typer = Room.ParticipatingUsers.Where(userItem => userItem.AuthenticationID == Messages[i].Sender).FirstOrDefault();
					if (typer != null)
						Messages.ElementAt(i).SenderFullName = Status = string.Format("{0} {1}", typer.FirstName, typer.LastName);
				}
			}
		}
		public async Task GetMessages()
		{
			try
			{
				Messages = await DBItemManager.DefaultManager.GetMessagesAsync(Room.Id);
				UpdateSendersFullName();
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
		}
		public async Task GetNewMessages()
		{
			var newMessages = await DBItemManager.DefaultManager.GetMessagesFromDateAsync(Room.Id, lastSync);
			foreach (Amigo.Models.Messages newMessage in newMessages.ToList())
				if(newMessage.Id != Messages.Last().Id)
					Messages.Add(newMessage);
			UpdateSendersFullName();
			lastSync = DateTime.Now;
		}
		private ObservableCollection<Amigo.Models.Messages> _messages = new ObservableCollection<Amigo.Models.Messages>();
		public ObservableCollection<Amigo.Models.Messages> Messages
		{
			get { return _messages; }
			set
			{
				_messages = value;
				OnPropertyChanged("Messages");
			}
		}
		private Events _room;
		public Events Room
		{
			get { return _room; }
			set
			{
				_room = value;
				OnPropertyChanged("Room");
			}
		}

		private string _status;
		public string Status
		{
			get { return _status; }
			set {
				_status = value;
				OnPropertyChanged("Status");
			}
		}
		public Users CurrentUser
		{
			get { return LoadUserDetails.CurrentUser; }
		}

		public ICommand SendMessageCommand { protected set; get; }

		private string _newMessageText;
		public string NewMessageText
		{
			get { return _newMessageText; }
			set
			{
				_newMessageText = value;
				OnPropertyChanged("NewMessageText");
				
				if (!string.IsNullOrEmpty(NewMessageText))
				{
					_chatHubProxy.Invoke("UserStartTypingInRoom", Room.Id, LoadUserDetails.CurrentUser.AuthenticationID);
				}

			}
		}
	}
}
