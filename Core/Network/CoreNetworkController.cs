using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core
{
	public interface NetworkCallbacks
	{
		void OnSuccess(Object data);
		void OnFail();
	}

	public class CoreNetworkController
	{
		const string JSON_URL = "http://validate.jsontest.com/?json={0}";

		const string GEO_USERNAME = "rimoses";
		const string GEO_JSON_KEY = "postalCodes";
		const int GEO_MAXROWS = 10;
		static string GEO_CONSTS = String.Format("username={0}&maxRows={1}", GEO_USERNAME, GEO_MAXROWS);
		static readonly string AREA_CODE_URL = "http://api.geonames.org/findNearbyPostalCodesJSON?" + GEO_CONSTS + "&lat={0}&lng={1}";

		static CoreNetworkController instance;

		public static CoreNetworkController GetInstance()
		{
			if (instance == null)
			{
				instance = new CoreNetworkController();
			}
			return instance;
		}

		CoreNetworkController()
		{
		}

		delegate string ProcessResponse(string response);

		async void MakeRequest(Uri requestUrl, NetworkCallbacks callbacks, ProcessResponse process)
		{
			var httpClient = new HttpClient();
			try
			{
				Task<string> contentsTask = httpClient.GetStringAsync(requestUrl);
				string result = await contentsTask;
				// Process the result using the ProcessResponse function that was passed in
				string processedResult = process(result);
				Console.WriteLine(processedResult);
				callbacks.OnSuccess(processedResult);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				callbacks.OnFail();
			}
		}

		public void ValidateJSON(NetworkCallbacks callbacks, string jsonText)
		{
			Uri requestUrl = new Uri(String.Format(JSON_URL, jsonText));
			Console.WriteLine("URL is {0}", requestUrl);

			MakeRequest(requestUrl, callbacks, x => x);
		}

		public void GetAreaCode(NetworkCallbacks callbacks, double lat, double lon)
		{
			Uri requestUrl = new Uri(String.Format(AREA_CODE_URL, lat, lon));
			Console.WriteLine("URL is {0}", requestUrl);

			MakeRequest(requestUrl, callbacks, x => {
				try
				{
					JObject obj = JObject.Parse(x);
					JArray codes = (JArray) obj.GetValue(GEO_JSON_KEY);
					return codes.ToString();
				}
				catch (Exception e)
				{
					Console.WriteLine("Error parsing area codes: {0}", e);
					return "[{}]";
				}
			});
		}
	}
}