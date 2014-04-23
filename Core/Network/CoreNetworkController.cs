using System;
using System.Net.Http;
using System.Threading.Tasks;

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

		public async void ValidateJSON(NetworkCallbacks callbacks, string jsonText)
		{
			string requestUrl = String.Format(JSON_URL, jsonText);
			Console.WriteLine("URL is {0}", requestUrl);

			var httpClient = new HttpClient();
			try
			{
				Task<string> contentsTask = httpClient.GetStringAsync(new Uri(requestUrl));
				string result = await contentsTask;
				Console.WriteLine(result);
				callbacks.OnSuccess(result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				callbacks.OnFail();
			}
		}

		public async void GetAreaCode(NetworkCallbacks callbacks, double lat, double lon)
		{
			string requestUrl = String.Format(AREA_CODE_URL, lat, lon);
			Console.WriteLine("URL is {0}", requestUrl);

			var httpClient = new HttpClient();
			try
			{
				Task<string> contentsTask = httpClient.GetStringAsync(new Uri(requestUrl));
				string result = await contentsTask;
				Console.WriteLine(result);
				callbacks.OnSuccess(result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				callbacks.OnFail();
			}
		}
	}
}