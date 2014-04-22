using System;
using System.IO;
using System.Collections.Generic;
using SQLite;

namespace Core
{
	public class DBManager
	{
		private const string DB_NAME = "test.db";

		private static DBManager instance;
		private static SQLiteConnection db;

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

		public void init()
		{
			string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			db = new SQLiteConnection(Path.Combine(folder, DB_NAME));

			CreateOrUpdateTables();
		}

		public void close()
		{
			if (db != null)
			{
				db.Close();
			}
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

		public int GetRowCountForTable<T>() where T : new()
		{
			return db.Table<T>().Count();
		}

		// Return a list of all objects from the specified table
		public List<T> GetAll<T>() where T : new()
		{
			return new List<T>(db.Table<T>());
		}

		// Return number of rows deleted
		public int ClearTable<T>() where T : new()
		{
			return db.DeleteAll<T>();
		}
	}
}

