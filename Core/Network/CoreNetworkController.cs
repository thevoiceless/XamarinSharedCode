using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core
{
	public interface NetworkCallbacks
	{
		void OnSuccess(Object data);
		void OnFail();
	}

	public class CoreNetworkController
	{
		private static string url = "http://validate.jsontest.com/?json={0}";

		public CoreNetworkController()
		{
		}

		public async void MakeRequest(string jsonText, NetworkCallbacks callbacks)
		{
			string requestUrl = String.Format(url, jsonText);
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