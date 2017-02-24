using System;
using Newtonsoft.Json;

namespace SmallTest
{
	public class ResultError : BaseEntity
	{
		[JsonProperty("ResultCode")]
		public int ResultCode { get; set; }

		[JsonProperty("ResultMessage")]
		public string ResultMessage { get; set; }
	}
}
