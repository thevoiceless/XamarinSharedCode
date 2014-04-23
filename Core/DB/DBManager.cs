using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;

namespace Core
{
	public class DBManager
	{
		const string DB_NAME = "test.db";
		static Type[] TABLES = {
			typeof(ValidatedJSON)
		};

		static DBManager instance;
		static SQLiteAsyncConnection db;

		public static DBManager GetInstance()
		{
			if (instance == null)
			{
				instance = new DBManager();
			}
			return instance;
		}

		DBManager()
		{
			Console.WriteLine("*************** DBManager created, should now call Init()");
		}

		public Task Init()
		{
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			db = new SQLiteAsyncConnection(Path.Combine(folder, DB_NAME));

			return CreateOrUpdateTables();
		}

		// Create or update tables based on object annotations
		Task CreateOrUpdateTables()
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

		// Return a list of all objects from the specified table
		public Task<List<T>> GetAll<T>() where T : new()
		{
			return db.Table<T>().ToListAsync();
		}

		public void ClearTable<T>() where T : new()
		{
			db.DropTableAsync<T>().ContinueWith((r) => {
				db.CreateTableAsync<T>();
			});
		}
	}
}

