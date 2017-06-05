using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace mondaydinner
{
	public class Yodel
	{
		string id;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value;}
		}

        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted { get; set; }

        [Version]
        public string Version { get; set; }
	}
}

