namespace VietnameseAI.Shared.Models
{
	public class SQLitePreferences
	{
		public string DatabaseFilename { get; set; } = "UserLearningData.db3";
		public SQLite.SQLiteOpenFlags Flags { get; set; } =
			// open the database in read/write mode
			SQLite.SQLiteOpenFlags.ReadWrite |
			// create the database if it doesn't exist
			SQLite.SQLiteOpenFlags.Create |
			// enable multi-threaded database access
			SQLite.SQLiteOpenFlags.SharedCache;

		public string DatabasePath { get; set; } = "./";
	}
}
