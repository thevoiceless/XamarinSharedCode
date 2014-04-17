using System;
using SQLite;

namespace Core
{
	public class JSONResponse
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
	}
}

