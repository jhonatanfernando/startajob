using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SmallTest
{
	public class StartJobService<Entity>
	{
		private const string MarvelDns = "http://isapi.mekashron.com/StartAJob/General.dll/IGeneral/";
	
		public async Task<Entity> GetEntity(string user, string pass, string ip)
		{
			var querystring = string.Empty;
				querystring += $"&UserName={user}";
				querystring += $"&Password={pass}";
				querystring += $"&IP={ip}";

			var result = await this.MakeHttpCall<Entity>(querystring);
			return result;

		}

		private async Task<TOutput> MakeHttpCall<TOutput>(string filter)
		{

			HttpClient client = new HttpClient();
	
			string url = $"{MarvelDns}Login?{filter}";


			HttpResponseMessage response = new HttpResponseMessage();
			try
			{

				response = await client.GetAsync(url);

				string responseText = await response.Content.ReadAsStringAsync();
				if (response.IsSuccessStatusCode)
				{
					return JsonConvert.DeserializeObject<TOutput>(responseText);
				}
				else {
					throw new Exception(string.Format("Response Statuscode for {0}: {1}", url, response.StatusCode));
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				throw ex;
			}
		}
	}
}