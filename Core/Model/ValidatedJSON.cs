using System;
using SQLite;
using Newtonsoft.Json;

namespace Core
{
	public class ValidatedJSON
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public String object_or_array { get; set; }
		public bool empty { get; set; }
		public long parse_time_nanoseconds { get; set; }
		public bool validate { get; set; }
		public int size { get; set; }
		public string error { get; set; }
		public string error_info { get; set; }

		public static ValidatedJSON CreateObject(string json)
		{
			ValidatedJSON resultObj = JsonConvert.DeserializeObject<ValidatedJSON>(json,
				new JsonSerializerSettings
				{
					Error = delegate(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e) {
						e.ErrorContext.Handled = true;
					}
				});

			return (resultObj == null) ? new ValidatedJSON() : resultObj;
		}

		public ValidatedJSON()
		{
		}

		public bool IsValid()
		{
			return validate;
		}

		public string AsJSON()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}
	}
}

