using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigo.Models
{
	public class Messages
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "sender")]
		public string Sender { get; set; }

		[JsonProperty(PropertyName = "sentTime")]
		public DateTime SentTime { get; set; }

		[JsonProperty(PropertyName = "eventId")]
		public string EventId { get; set; }

		[Deleted]
		public bool Deleted { get; set; }

		[Version]
		public string Version { get; set; }

		[CreatedAt]
		public DateTime CreatedAt { get; set; }

		[UpdatedAt]
		public DateTime UpdatedAt { get; set; }

		[Newtonsoft.Json.JsonIgnore]
		public string SentTimeAsString => SentTime.ToString("g");

		[Newtonsoft.Json.JsonIgnore]
		public string SenderFullName { get; set; } = "";
	}
}
