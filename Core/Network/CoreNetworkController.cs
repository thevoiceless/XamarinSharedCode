﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

		public async Task<string> MakeRequest()
		{
			Console.WriteLine("URL is {0}", url);

			var httpClient = new HttpClient();
			try
			{
				Task<string> contentsTask = httpClient.GetStringAsync(new Uri(url));
				string result = await contentsTask;
				Console.WriteLine(await contentsTask);
				return result;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return "";
			}
		}
	}
}