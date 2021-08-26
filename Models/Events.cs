using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace Amigo.Models
{
	public class Events
	{

		[JsonProperty(PropertyName = "Id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "NameOfEvent")]
		public string NameOfEvent { get; set; }

		[JsonProperty(PropertyName = "CreatorID")]
		public string CreatorID { get; set; }

		[JsonProperty(PropertyName = "ParticipantsId")]
		public string ParticipantsId { get; set; }

		[JsonProperty(PropertyName = "Description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "Location")]
		public string Location { get; set; }

		[JsonProperty(PropertyName = "Longitude")]
		public double Longitude { get; set; }

		[JsonProperty(PropertyName = "Latitude")]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName = "MinNumOfParticipants")]
		public int MinNumOfParticipants { get; set; }

		[JsonProperty(PropertyName = "MaxNumOfParticipants")]
		public int MaxNumOfParticipants { get; set; }

		[JsonProperty(PropertyName = "EventDateTime")]
		public DateTimeOffset EventDateTime { get; set; }

		[JsonProperty(PropertyName = "NoDateEvent")]
		public bool NoDateEvent { get; set; }

		[JsonProperty(PropertyName = "NoHourEvent")]
		public bool NoHourEvent { get; set; }

		[JsonProperty(PropertyName = "College")]
		public string College { get; set; }

		[JsonProperty(PropertyName = "CollegeFields")]
		public string CollegeFields { get; set; }

		[JsonProperty(PropertyName = "ClassOf")]
		public int ClassOf { get; set; }

		[JsonProperty(PropertyName = "Interests")]
		public string Interests { get; set; }

		[JsonProperty(PropertyName = "Tags")]
		public string Tags { get; set; }

		[JsonProperty(PropertyName = "MinAge")]
		public int MinAge { get; set; }

		[JsonProperty(PropertyName = "MaxAge")]
		public int MaxAge { get; set; }

		[JsonProperty(PropertyName = "Gender")]
		public int Gender { get; set; }

		[JsonProperty(PropertyName = "Private")]
		public bool PrivateEvent { get; set; }

		[JsonProperty(PropertyName = "Image")]
		public string Image { get; set; }

		[JsonProperty(PropertyName = "numberOfParticipants")]
		public int NumberOfParticipants { get; set; }

		[Deleted]
		public bool Deleted { get; set; }

		[Version]
		public string Version { get; set; }

		[CreatedAt]
		public DateTime CreatedAt { get; set; }

		[UpdatedAt]
		public DateTime UpdatedAt { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public string DateDisplay { get { return EventDateTime.ToLocalTime().ToString("dd/MM/yyyy"); } }

		[Newtonsoft.Json.JsonIgnore]
		public string TimeDisplay { get { return EventDateTime.ToLocalTime().ToString("HH:mm"); } }

		[Newtonsoft.Json.JsonIgnore]
		public List<Users> ParticipatingUsers { get; set; }

	}
}

