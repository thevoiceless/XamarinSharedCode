using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core
{
	public class CoreNetworkController
	{
		private static string url = "http://validate.jsontest.com/?json={\"key\":\"value\"}";

		public CoreNetworkController()
		{
			Console.WriteLine("Create core network controller");
		}

		public void PrintSomething()
		{
			Console.WriteLine("SOMETHING");
		}

		public async void MakeRequest()
		{
			Console.WriteLine("URL is {0}", url);

			var httpClient = new HttpClient();
			try
			{
				Task<String> contentsTask = httpClient.GetStringAsync(new Uri(url));
				// "await" returns control to the caller and the task continues to run on another thread
				String result = await contentsTask;
				Console.WriteLine(result);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}