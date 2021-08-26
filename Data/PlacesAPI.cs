using Amigo.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amigo.Data
{
	public class PlacesAPI
	{
		public const string GooglePlacesApiAutoCompletePath = "https://maps.googleapis.com/maps/api/place/autocomplete/json?key={0}&input={1}"; //Adding country:us limits results to us
		public static string GooglePlacesApiKey = Constants.PlacesApiKey;

		public const string GoogleGeoApiAutoCompletePath = "https://maps.googleapis.com/maps/api/geocode/json?key={0}&place_id={1}"; //Adding country:us limits results to us

		private static HttpClient _httpClientInstance;
		public static HttpClient HttpClientInstance => _httpClientInstance ?? (_httpClientInstance = new HttpClient());

		public ObservableCollection<AddressInfo> Addresses = new ObservableCollection<AddressInfo>();
		public AddressInfo AdressObject;

		public async Task<ObservableCollection<AddressInfo>> GetPlacesPredictionsAsync(string search)
		{

			// TODO: Add throttle logic, Google begins denying requests if too many are made in a short amount of time

			CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token;

			using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(GooglePlacesApiAutoCompletePath, GooglePlacesApiKey, WebUtility.UrlEncode(search))))
			{ //Be sure to UrlEncode the search term they enter

				using (HttpResponseMessage message = await HttpClientInstance.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false))
				{
					if (message.IsSuccessStatusCode)
					{
						string json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

						PlacesLocationPredictions predictionList = await Task.Run(() => JsonConvert.DeserializeObject<PlacesLocationPredictions>(json)).ConfigureAwait(false);

						if (predictionList.Status == "OK")
						{

							if (predictionList.Predictions.Count > 0)
							{
								foreach (Prediction prediction in predictionList.Predictions)
								{
									Addresses.Add(new AddressInfo
									{
										Address = prediction.Description,
										PlaceID = prediction.PlaceId
									});
								}
							}
						}
						else
						{
							throw new Exception(predictionList.Status);
						}
					}
					return Addresses;
				}
			}
		}

		public async Task<AddressInfo> GetPlaceGeo(string Place_ID)
		{

			// TODO: Add throttle logic, Google begins denying requests if too many are made in a short amount of time

			CancellationToken cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(2)).Token;

			using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format(GoogleGeoApiAutoCompletePath, GooglePlacesApiKey, WebUtility.UrlEncode(Place_ID))))
			{ //Be sure to UrlEncode the search term they enter

				using (HttpResponseMessage message = await HttpClientInstance.SendAsync(request, HttpCompletionOption.ResponseContentRead, cancellationToken).ConfigureAwait(false))
				{
					if (message.IsSuccessStatusCode)
					{
						string json = await message.Content.ReadAsStringAsync().ConfigureAwait(false);

						GeoResults resultsList = await Task.Run(() => JsonConvert.DeserializeObject<GeoResults>(json)).ConfigureAwait(false);

						if (resultsList.Status == "OK")
						{

							if (resultsList.Results.Count > 0)
							{
								foreach (Results result in resultsList.Results)
								{
									AdressObject = new AddressInfo
									{
										Address = result.Formatted_address,
										Longitude = result.Geo.Location.Lng,
										Latitude = result.Geo.Location.Lat,
										PlaceID = Place_ID
									};
									return AdressObject;
								}
							}
						}
						else
						{
							throw new Exception(resultsList.Status + " : " + Place_ID);
						}
					}
					return AdressObject;
				}
			}
		}

	}
}
