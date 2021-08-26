using System;
using System.Collections.Generic;
using System.Text;

namespace Amigo.Services
{
	public class AddressInfo
	{

		public string Address { get; set; }

		public string PlaceID { get; set; }

		public double Longitude { get; set; }

		public double Latitude { get; set; }
	}

	public class PlacesMatchedSubstring
	{

		[Newtonsoft.Json.JsonProperty("length")]
		public int Length { get; set; }

		[Newtonsoft.Json.JsonProperty("offset")]
		public int Offset { get; set; }
	}

	public class PlacesTerm
	{

		[Newtonsoft.Json.JsonProperty("offset")]
		public int Offset { get; set; }

		[Newtonsoft.Json.JsonProperty("value")]
		public string Value { get; set; }
	}

	public class Prediction
	{

		[Newtonsoft.Json.JsonProperty("id")]
		public string Id { get; set; }

		[Newtonsoft.Json.JsonProperty("description")]
		public string Description { get; set; }

		[Newtonsoft.Json.JsonProperty("matched_substrings")]
		public List<PlacesMatchedSubstring> MatchedSubstrings { get; set; }

		[Newtonsoft.Json.JsonProperty("place_id")]
		public string PlaceId { get; set; }

		[Newtonsoft.Json.JsonProperty("reference")]
		public string Reference { get; set; }

		[Newtonsoft.Json.JsonProperty("terms")]
		public List<PlacesTerm> Terms { get; set; }

		[Newtonsoft.Json.JsonProperty("types")]
		public List<string> Types { get; set; }
	}

	public class PlacesLocationPredictions
	{

		[Newtonsoft.Json.JsonProperty("predictions")]
		public List<Prediction> Predictions { get; set; }

		[Newtonsoft.Json.JsonProperty("status")]
		public string Status { get; set; }
	}



	public class Geometry
	{
		[Newtonsoft.Json.JsonProperty("location")]
		public Location Location { get; set; }

		[Newtonsoft.Json.JsonProperty("location_type")]
		public string Location_type { get; set; }

		[Newtonsoft.Json.JsonProperty("viewport")]
		public Viewport Viewport { get; set; }
	}

	public class Location
	{
		[Newtonsoft.Json.JsonProperty("lat")]
		public float Lat { get; set; }

		[Newtonsoft.Json.JsonProperty("lng")]
		public float Lng { get; set; }
	}

	public class Viewport
	{
		[Newtonsoft.Json.JsonProperty("northeast")]
		public Northeast Northeast { get; set; }

		[Newtonsoft.Json.JsonProperty("southwest")]
		public Southwest Southwest { get; set; }
	}

	public class Northeast
	{
		[Newtonsoft.Json.JsonProperty("lat")]
		public float Lat { get; set; }

		[Newtonsoft.Json.JsonProperty("lng")]
		public float Lng { get; set; }
	}

	public class Southwest
	{
		[Newtonsoft.Json.JsonProperty("lat")]
		public float Lat { get; set; }

		[Newtonsoft.Json.JsonProperty("lng")]
		public float Lng { get; set; }
	}

	public class Address_Components
	{
		[Newtonsoft.Json.JsonProperty("long_name")]
		public string Long_name { get; set; }

		[Newtonsoft.Json.JsonProperty("short_name")]
		public string Short_name { get; set; }

		[Newtonsoft.Json.JsonProperty("types")]
		public string[] Types { get; set; }
	}

	public class GeoResults
	{

		[Newtonsoft.Json.JsonProperty("results")]
		public List<Results> Results { get; set; }

		[Newtonsoft.Json.JsonProperty("status")]
		public string Status { get; set; }
	}

	public class Results
	{
		[Newtonsoft.Json.JsonProperty("address_components")]
		public Address_Components[] Adresses { get; set; }

		[Newtonsoft.Json.JsonProperty("formatted_address")]
		public string Formatted_address;

		[Newtonsoft.Json.JsonProperty("geometry")]
		public Geometry Geo { get; set; }

		[Newtonsoft.Json.JsonProperty("place_id")]
		public string Place_ID;

		[Newtonsoft.Json.JsonProperty("types")]
		public string[] Types { get; set; }

	}

}
