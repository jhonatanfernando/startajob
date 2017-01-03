using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SmallTest
{
	public class ServiceSoap
	{
		public BaseEntity Execute(string user, string pass)
		{
			XNamespace ns = "http://www.w3.org/2003/05/soap-envelope";
			XNamespace myns = "ns1";

			XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
			XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
			XNamespace enc = "http://www.w3.org/2003/05/soap-encoding";

			XDocument soapRequest = new XDocument(
				new XDeclaration("1.0", "UTF-8", "no"),
				new XElement(ns + "Envelope",
					new XAttribute(XNamespace.Xmlns + "xsi", xsi),
					new XAttribute(XNamespace.Xmlns + "xsd", xsd),
							 new XAttribute(XNamespace.Xmlns + "enc", enc),
					new XElement(ns + "Body",
						new XElement(myns + "Login",
								new XElement(myns + "Username", user),
								new XElement(myns + "Password", pass),
									new XElement(myns + "IP", ""))


					)
				));


			try
			{
				using (var client = new HttpClient())
				{
					var request = new HttpRequestMessage()
					{
						RequestUri = new Uri("http://isapi.mekashron.com/StartAJob/General.dll/soap/IGeneral"),
						Method = HttpMethod.Post
					};

					request.Content = new StringContent(soapRequest.ToString(), Encoding.UTF8, "text/xml");

					request.Headers.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
					request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
					//request.Headers.Add("SOAPAction", "urn:General.Intf-IGeneral#Login");

					HttpResponseMessage response = client.SendAsync(request).Result;

					if (!response.IsSuccessStatusCode)
					{
						throw new Exception();
					}

					Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
					Stream stream = streamTask.Result;
					var sr = new StreamReader(stream);
					var soapResponse = XDocument.Load(sr);

					if (soapResponse.Root.Value.Contains("ResultCode"))
						return JsonConvert.DeserializeObject<ResultError>(soapResponse.Root.Value);
					else
						return JsonConvert.DeserializeObject<Entity>(soapResponse.Root.Value);
				}
			}
			catch (AggregateException ex)
			{
				if (ex.InnerException is TaskCanceledException)
				{
					throw ex.InnerException;
				}
				else
				{
					throw ex;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

	}
}
