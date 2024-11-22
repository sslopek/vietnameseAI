namespace VietnameseAI.Shared.Models;
public class ChatResult
{
	public required string RequestVietnamese { get; set; }
	public required string ResponseVietnamese { get; set; }
	public required string ResponseEnglish { get; set; }
	public int NewRequestWordCount { get; set; }
	public int NewResponseWordCount { get; set; }
}
