using System;
using System.IO;
using SQLite;

namespace Core
{
	public class DBManager
	{
		private const string DB_NAME = "test.db";

		private static SQLiteConnection db;

		public DBManager()
		{
			Console.WriteLine("*************** DBManager");
		}

		public void init()
		{
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			db = new SQLiteConnection(Path.Combine(folder, DB_NAME));

			CreateOrUpdateTables();
		}

		// Create or update tables based on object annotations
		private void CreateOrUpdateTables()
		{
			db.CreateTable<ValidatedJSON>();
		}

		// Return number of rows affected by the operation
		public int Insert<T>(T obj) where T : new()
		{
			return db.Insert(obj);
		}

		public int GetRowsInTable<T>() where T : new()
		{
			return db.Table<T>().Count();
		}
	}
}

