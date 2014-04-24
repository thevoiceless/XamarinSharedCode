using System;
using SQLite;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core
{
	public class AreaCode
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public String postalCode { get; set; }
		public double distance { get; set; }
		public String placeName { get; set; }   // City
		public String adminCode1 { get; set; }  // Abbreviated state name
		public String adminName1 { get; set; }  // Full state name
		public String countryCode { get; set; }

		public static AreaCode CreateObject(string json)
		{
			AreaCode resultObj = JsonConvert.DeserializeObject<AreaCode>(json,
				new JsonSerializerSettings
				{
					Error = delegate(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e) {
						e.ErrorContext.Handled = true;
					}
				});

			return resultObj ?? new AreaCode();
		}

		public static List<AreaCode> CreateList(string jsonArray)
		{
			List<AreaCode> areaCodes = JsonConvert.DeserializeObject<List<AreaCode>>(jsonArray,
				new JsonSerializerSettings
				{
					Error = delegate(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e) {
						e.ErrorContext.Handled = true;
					}
				});

			return areaCodes ?? new List<AreaCode>();
		}
	}
}

