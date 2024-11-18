using SQLite;
using VietnameseAI.Shared.Models;

namespace VietnameseAI.Shared.Data;

public class UserLearningDatabase
{
	private readonly SQLiteAsyncConnection Database;
	private bool initialized = false;

	public UserLearningDatabase(SQLitePreferences sqlitePreferences)
	{
		Database = new SQLiteAsyncConnection(sqlitePreferences.DatabasePath, sqlitePreferences.Flags);
	}

	async Task Init()
	{
		if (initialized)
		{
			return;
		}
		initialized = true;

		var result = await Database.CreateTableAsync<DiscoveredWord>();

		// Initial data
		if ((await GetDiscoveredWordsAsync()).Count() == 0)
		{
			await Database.InsertAsync(new DiscoveredWord
			{
				Word = "Chào mừng",
				Definition = "Welcome"
			});
		}
	}

	public async Task<List<DiscoveredWord>> GetDiscoveredWordsAsync()
	{
		await Init();
		return await Database.Table<DiscoveredWord>().ToListAsync();
	}

	public async Task<DiscoveredWord> GetItemAsync(string word)
	{
		await Init();
		return await Database.Table<DiscoveredWord>().Where(i => i.Word == word).FirstOrDefaultAsync();
	}

	public async Task<int> SaveItemAsync(DiscoveredWord item)
	{
		await Init();
		if (item.ID != 0)
		{
			return await Database.UpdateAsync(item);
		}
		else
		{
			return await Database.InsertAsync(item);
		}
	}

	public async Task<int> DeleteItemAsync(DiscoveredWord item)
	{
		await Init();
		return await Database.DeleteAsync(item);
	}
}
