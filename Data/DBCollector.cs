using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Microsoft.WindowsAzure.MobileServices;
using Amigo.Data;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Amigo.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Xamarin.Forms;

namespace Amigo.Data
{
	public class DBItemManager
	{
		MobileServiceClient client;
		IMobileServiceTable<Users> UsersTable;
		IMobileServiceTable<Amigo.Models.Messages> MessagesTable;
		IMobileServiceTable<Events> EventsTable;

		public static DBItemManager DefaultManager { get; private set; } = new DBItemManager();

		private DBItemManager()
		{
			this.client = new MobileServiceClient(
				Constants.ApplicationURL);
			this.UsersTable = client.GetTable<Users>();
			this.MessagesTable = client.GetTable<Amigo.Models.Messages>();
			this.EventsTable = client.GetTable<Events>();
		}

		public MobileServiceClient CurrentClient
		{
			get { return client; }
		}

		///    USERS FUNCTIONS


		public async Task SaveUserAsync(Users item)
		{
			if (item.ID == null)
			{
				await UsersTable.InsertAsync(item);
			}
			else
			{
				await UsersTable.UpdateAsync(item);
			}
		}
		public async Task<ObservableCollection<Users>> GetUserAsync(string authenticationId)
		{
			try
			{
				IEnumerable<Users> items = await UsersTable
					.Where(userItem => userItem.AuthenticationID == authenticationId)
					.ToEnumerableAsync();

				return new ObservableCollection<Users>(items);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}
		public async Task<List<Users>> GetUsersInEvent(string participents)
		{
			try
			{
				IEnumerable<Users> items = await UsersTable
					.Where(userItem => participents.Contains(userItem.AuthenticationID))
					.ToEnumerableAsync();

				return items.ToList();
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}
		public async Task DeleteUserAsync(Users user)
		{
			try
			{
				IEnumerable<Events> CreatedByUser = await EventsTable
					.Where(EventItem => EventItem.CreatorID == user.AuthenticationID)
					.ToEnumerableAsync();
				IEnumerable<Events> UserParticipating = await EventsTable
					.Where(EventItem => EventItem.ParticipantsId.Contains(user.AuthenticationID))
					.ToEnumerableAsync();

				foreach (Events deleteEvent in CreatedByUser)
					await EventsTable.DeleteAsync(deleteEvent);

				foreach (Events cancelParticipation in UserParticipating)
				{
					cancelParticipation.ParticipantsId = cancelParticipation.ParticipantsId.Replace(user.AuthenticationID + ",", "");
					await EventsTable.UpdateAsync(cancelParticipation);
				}
				await UsersTable.DeleteAsync(user);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
				Debug.WriteLine( msioe.Request);
				Debug.WriteLine(msioe.Response);
				Debug.WriteLine(msioe.Value);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
		}


		/////  Messages Functions


		public async Task SaveMessageAsync(Amigo.Models.Messages item)
		{
			if (item.Id == null)
			{
				await MessagesTable.InsertAsync(item);
			}
			else
			{
				await MessagesTable.UpdateAsync(item);
			}
		}
		public async Task<ObservableCollection<Amigo.Models.Messages>> GetMessagesAsync(string roomId)
		{
			try
			{
				IEnumerable<Amigo.Models.Messages> items = await MessagesTable
					.Where(messageItem => messageItem.EventId == roomId)
					.OrderBy(messageItem => messageItem.SentTime)
					.ToEnumerableAsync();

				return new ObservableCollection<Amigo.Models.Messages>(items);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}
		public async Task<IEnumerable<Amigo.Models.Messages>> GetMessagesFromDateAsync(string roomId, DateTime lastSync)
		{
			try
			{
				IEnumerable<Amigo.Models.Messages> items = await MessagesTable
					.Where(messageItem => messageItem.EventId == roomId)
					.Where(messageItem => messageItem.CreatedAt >= lastSync)
					.OrderBy(messageItem => messageItem.SentTime)
					.ToEnumerableAsync();
				return items;
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}


		/////  Events Functions
		public async Task<ObservableCollection<Events>> GetEventsForMeAsync()
		{
			try
			{
				var radiousInKm = 100D; // Some Default Value for testing					
				if (Application.Current.Properties.ContainsKey("MaxDistance"))
				{
					radiousInKm = Convert.ToDouble(Application.Current.Properties["MaxDistance"]);
				}
				var minKmPerLat = 110.566;
				var kmPerLon = Math.Cos(LoadUserDetails.CurrentUser.Latitude) * 111.3;
				var minLat = LoadUserDetails.CurrentUser.Latitude - (radiousInKm / minKmPerLat);
				var maxLat = LoadUserDetails.CurrentUser.Latitude + (radiousInKm / minKmPerLat);
				var minLon = LoadUserDetails.CurrentUser.Longitude - (radiousInKm / kmPerLon);
				var maxLon = LoadUserDetails.CurrentUser.Longitude + (radiousInKm / kmPerLon);

				IEnumerable<Events> items = await EventsTable
					//.Where(eventItem => !LoadUserDetails.CurrentUser.MyEvents.Contains(eventItem.Id))
					//Hide events I joined???
					.Where(eventItem => eventItem.CreatorID != LoadUserDetails.CurrentUser.AuthenticationID)
					.Where(eventItem => eventItem.PrivateEvent == false)
					.Where(eventItem => eventItem.NoDateEvent == true || eventItem.EventDateTime > DateTime.Now)
					.Where(eventItem => eventItem.College == LoadUserDetails.CurrentUser.College || eventItem.College == "")
					.Where(eventItem => eventItem.MinAge <= LoadUserDetails.CurrentUser.Age && (eventItem.MaxAge == -1 || eventItem.MaxAge >= LoadUserDetails.CurrentUser.Age))
					.Where(eventItem => LoadUserDetails.CurrentUser.CollegeFields.Contains(eventItem.CollegeFields) || eventItem.CollegeFields == "")
					.Where(eventItem => eventItem.ClassOf == LoadUserDetails.CurrentUser.ClassOf || eventItem.ClassOf == -1)
					.Where(eventItem => eventItem.Gender == -1 || eventItem.Gender == LoadUserDetails.CurrentUser.Gender)
					.Where(eventItem => eventItem.MaxNumOfParticipants == -1 || eventItem.MaxNumOfParticipants > eventItem.NumberOfParticipants)
					.Where(eventItem => (minLat <= eventItem.Latitude && eventItem.Latitude <= maxLat) && (minLon <= eventItem.Longitude && eventItem.Longitude <= maxLon))
					.ToEnumerableAsync();

				var result = items
					.Where(eventItem => DistanceInKm(LoadUserDetails.CurrentUser.Longitude, LoadUserDetails.CurrentUser.Latitude, eventItem.Longitude, eventItem.Latitude) <= radiousInKm)
					.Where(eventItem => eventItem.MaxNumOfParticipants == -1 || eventItem.MaxNumOfParticipants > (eventItem.ParticipantsId.Length - eventItem.ParticipantsId.Replace(",", "").Length))
					.OrderBy(eventItem => LoadUserDetails.CurrentUser.Hobbies.Split(',').ToArray().Intersect(eventItem.Tags.Split(',').ToArray()).Any());

				return new ObservableCollection<Events>(result);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
				Debug.WriteLine(msioe.Request);
				Debug.WriteLine(msioe.Response);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}
		public double ToRadians(double degrees) => degrees * Math.PI / 180.0;
		public double DistanceInKm(double lon1d, double lat1d, double lon2d, double lat2d)
		{
			var lon1 = ToRadians(lon1d);
			var lat1 = ToRadians(lat1d);
			var lon2 = ToRadians(lon2d);
			var lat2 = ToRadians(lat2d);
			var deltaLon = lon2 - lon1;
			var c = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
			var earthRadius = 3958.76;
			var distInMiles = earthRadius * c;

			return distInMiles * 1.60934;
		}
		public async Task<ObservableCollection<Events>> SearchEvents(string name, string interests)
		{
			try
			{
				IEnumerable<Events> items = await EventsTable
					//.Where(eventItem => !LoadUserDetails.CurrentUser.MyEvents.Contains(eventItem.Id))
					//Hide events I joined???
					.Where(eventItem => eventItem.CreatorID != LoadUserDetails.CurrentUser.AuthenticationID)
					.Where(eventItem => eventItem.PrivateEvent == false)
					.Where(eventItem => eventItem.NoDateEvent == true || eventItem.EventDateTime > DateTime.Now)
					.Where(eventItem => eventItem.MinAge <= LoadUserDetails.CurrentUser.Age && (eventItem.MaxAge == -1 || eventItem.MaxAge >= LoadUserDetails.CurrentUser.Age))
					.Where(eventItem => EF.Functions.Like(eventItem.NameOfEvent, name) || name == "" || eventItem.NameOfEvent.Contains(name))
					.Where(eventItem => eventItem.MaxNumOfParticipants == -1 || eventItem.MaxNumOfParticipants > eventItem.NumberOfParticipants)
					.Where(eventItem => interests.Contains(eventItem.Interests) || interests == "")
					.ToEnumerableAsync();
				var result = items
					.OrderBy(eventItem => LoadUserDetails.CurrentUser.Hobbies.Split(',').ToArray().Intersect(eventItem.Tags.Split(',').ToArray()).Any());
				return new ObservableCollection<Events>(result);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}

		public async Task<ObservableCollection<Events>> GetEventsCreatedByMeAsync()
		{
			try
			{
				IEnumerable<Events> items = await EventsTable
					.Where(eventItem => eventItem.CreatorID == LoadUserDetails.CurrentUser.AuthenticationID)
					.OrderBy(eventItem => eventItem.CreatedAt)
					.ToEnumerableAsync();

				return new ObservableCollection<Events>(items);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}

		public async Task<ObservableCollection<Events>> GetEventsParticipatingAsync()
		{
			try
			{
				IEnumerable<Events> items = await EventsTable
					.Where(eventItem => LoadUserDetails.CurrentUser.MyEvents.Contains(eventItem.Id))
					.OrderBy(eventItem => eventItem.EventDateTime)
					.ToEnumerableAsync();

				return new ObservableCollection<Events>(items);
			}
			catch (MobileServiceInvalidOperationException msioe)
			{
				Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Sync error: {0}", e.Message);
			}
			return null;
		}

		public async Task SaveEventAsync(Events item)
		{
			if (item.Id == null)
			{
				await EventsTable.InsertAsync(item);
			}
			else
			{
				await EventsTable.UpdateAsync(item);
			}
		}

		public async Task DeleteEventAsync(Events item)
		{
			if (item.Id != null)
			{
				await EventsTable.DeleteAsync(item);
			}
			IEnumerable<Amigo.Models.Messages> MessagesInEvent = await MessagesTable
				.Where(MessageItem => MessageItem.EventId == item.Id)
				.ToEnumerableAsync();

			foreach (Amigo.Models.Messages deleteMessage in MessagesInEvent)
				await MessagesTable.DeleteAsync(deleteMessage);
		}
/*
		public async Task SyncAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

			try
			{
				await this.client.SyncContext.PushAsync();

				await this.EventsTable.PullAsync(
					//The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
					//Use a different query name for each unique query in your program
					"allEvents",
					this.EventsTable.CreateQuery());
			}
			catch (MobileServicePushFailedException exc)
			{
				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;
				}
			}

			// Simple error/conflict handling. A real application would handle the various errors like network conditions,
			// server conflicts and others via the IMobileServiceSyncHandler.
			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}
		}
		public async Task PurgeEventTableAsync()
		{
			try
			{
				// PURGE TagSprachTabel in  local.db
				await this.EventsTable.PurgeAsync("allEvents", null, true, System.Threading.CancellationToken.None);
			}
			catch (Exception e)
			{
				Debug.WriteLine(@"Error executing sync operation. Item: {0}. Operation discarded.", e);
			}
		}
	*/
	}
}
