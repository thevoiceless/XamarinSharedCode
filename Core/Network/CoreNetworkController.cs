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
		const string url = "http://validate.jsontest.com/?json={0}";
		const string badurl = "http://bad.jsontest.com/?json={0}";

		public async void ValidateJSON(string jsonText, NetworkCallbacks callbacks)
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