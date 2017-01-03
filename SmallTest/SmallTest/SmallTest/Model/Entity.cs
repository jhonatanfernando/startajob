using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SmallTest
{
	public class Entity : BaseEntity
	{
		[JsonProperty("EntityId")]
		public int EntityId { get; set;}

		[JsonProperty("FirstName")]
		public string FirstName { get; set; }
	}
}
