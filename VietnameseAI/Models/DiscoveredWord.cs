using SQLite;

namespace VietnameseAI.Models;

public class DiscoveredWord
{
	[PrimaryKey, AutoIncrement]
	public int ID { get; set; }
	public string Word { get; set; } = string.Empty;
	//TODO: definition to separate table
	public string Definition { get; set; } = string.Empty;
	public bool IsFavorited { get; set; }
	public bool IsMuted { get; set; }
	public int TimesDiscovered { get; set; } = 1;
	public DateTime DiscoveredDate { get; set; } = DateTime.Now;
	public DateTime LastSeenDate { get; set; } = DateTime.Now;
}
