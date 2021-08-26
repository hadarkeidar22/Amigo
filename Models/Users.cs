using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigo.Models
{
	public class Users
	{
		[JsonProperty(PropertyName = "id")]
		public string ID { get; set; }

		[JsonProperty(PropertyName = "firstName")]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "lastName")]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "interests")]
		public string Interests { get; set; }

		[JsonProperty(PropertyName = "myEvents")]
		public string MyEvents { get; set; }

		[JsonProperty(PropertyName = "hobbies")]
		public string Hobbies { get; set; }

		[JsonProperty(PropertyName = "image")]
		public string Image { get; set; }

		[JsonProperty(PropertyName = "birthday")]
		public DateTime Birthday { get; set; }

		[JsonProperty(PropertyName = "gender")]
		public int Gender { get; set; }

		[JsonProperty(PropertyName = "college")]
		public string College { get; set; }

		[JsonProperty(PropertyName = "collegeFields")]
		public string CollegeFields { get; set; }

		[JsonProperty(PropertyName = "classOf")]
		public int ClassOf { get; set; }

		[JsonProperty(PropertyName = "authenticationID")]
		public string AuthenticationID { get; set; }

		[JsonProperty(PropertyName = "location")]
		public string Location { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public double Latitude { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public double Longitude { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public int Age {
			get {
				var today = DateTime.Today;
				var age = today.Year - Birthday.Year;
				if (Birthday > today.AddYears(-age))
					age--;
				return age; 
					}
		}

		[Deleted]
		public bool Deleted { get; set; }

		[Version]
		public string Version { get; set; }

		[CreatedAt]
		public DateTime CreatedAt { get; set; }

		[UpdatedAt]
		public DateTime UpdatedAt { get; set; }

	}
}
