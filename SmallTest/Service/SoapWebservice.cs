using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;

namespace SmallTest
{
	public class SoapService
	{
		private Uri uri = new Uri("http://isapi.mekashron.com/StartAJob/General.dll");

		public async Task<string> GetLogin(string user, string pass)
		{
			var soapString = this.ConstructSoapRequest(user, pass);
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("SOAPAction", "http://isapi.mekashron.com/StartAJob/General.dll/Login");
				var content = new StringContent(soapString, Encoding.UTF8, "text/xml");
				using (var response = await client.(uri, content))
				{
					var soapResponse = await response.Content.ReadAsStringAsync();
					return this.ParseSoapResponse(soapResponse);
				}
			}
		}

		private string ConstructSoapRequest(string a, string b)
		{
			return String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>

            <env:Envelope xmlns:env=""http://www.w3.org/2003/05/soap-envelope"" xmlns:ns1=""urn:General.Intf-IGeneral"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:enc=""http://www.w3.org/2003/05/soap-encoding"">
               <env:Body>
                  <ns1:Login env:encodingStyle=""http://www.w3.org/2003/05/soap-encoding"">
                    <UserName xsi:type=""xsd:string""></UserName>
                    <Password xsi:type=""xsd:string""></Password>
                    <IP xsi:type=""xsd:string""></IP>
                  </ns1:Login>
               </env:Body>
            </env:Envelope>", a, b);
		}


		private string ParseSoapResponse(string response)
		{
			var soap = XDocument.Parse(response);
			XNamespace ns = "General.Intf-IGeneral";
			var result = soap.Descendants(ns + "LoginResponse").First().Element(ns + "LoginResult").Value;
			return result;
		}
	}
}