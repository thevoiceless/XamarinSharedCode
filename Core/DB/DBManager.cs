using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Core
{
	public class DBManager
	{
		private static string DB_NAME = "test.db";
		private static Type[] TABLES = new Type[] {
			typeof(ValidatedJSON)
		};

		private static DBManager instance;
		private static SQLiteAsyncConnection db;

		public static DBManager GetInstance()
		{
			if (instance == null)
			{
				instance = new DBManager();
			}
			return instance;
		}

		private DBManager()
		{
			Console.WriteLine("*************** DBManager created, should now call init()");
		}

		public Task Init()
		{
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			db = new SQLiteAsyncConnection(Path.Combine(folder, DB_NAME));

			return CreateOrUpdateTables();
		}

		// Create or update tables based on object annotations
		private Task CreateOrUpdateTables()
		{
			return db.CreateTablesAsync(TABLES);
		}

		// Return number of rows affected by the operation
		public Task<int> Insert<T>(T obj) where T : new()
		{
			return db.InsertAsync(obj);
		}

		public Task<int> GetRowCountForTable<T>() where T : new()
		{
			return db.Table<T>().CountAsync();
		}
//
//		// Return a list of all objects from the specified table
//		public List<T> GetAll<T>() where T : new()
//		{
//			return new List<T>(db.Table<T>());
//		}
//
//		// Return number of rows deleted
//		public int ClearTable<T>() where T : new()
//		{
//			return db.DeleteAll<T>();
//		}
	}
}

