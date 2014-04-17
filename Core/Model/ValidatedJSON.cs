using System;
using SQLite;
using Newtonsoft.Json;

namespace Core
{
	public class ValidatedJSON
	{
		// These are attributes
		[PrimaryKey, AutoIncrement]
		// This is a property
		public int Id { get; set; }
		public String ObjOrArr { get; set; }
		public bool Empty { get; set; }
		public long ParseTimeNs { get; set; }
		public bool Valid { get; set; }
		public int Size { get; set; }

		public ValidatedJSON()
		{
		}

		public static ValidatedJSON createObject(string json)
		{
			ValidatedJSON resultObj = JsonConvert.DeserializeObject<ValidatedJSON>(json);
			return resultObj;
		}
	}
}

