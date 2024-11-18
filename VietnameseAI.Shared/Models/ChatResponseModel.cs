namespace VietnameseAI.Shared.Models;

public class ChatResponseModel
{
	public string UserMessageInEnglish { get; set; } = string.Empty;
	public string UserMessageInVietnamese { get; set; } = string.Empty;
	public string AssistantResponseInEnglish { get; set; } = string.Empty;
	public string AssistantResponseInVietnamese { get; set; } = string.Empty;
	public string ResponseGeneralTopic { get; set; } = string.Empty;
	public List<WordInfo> AssistantResponseVietnameseWordList { get; set; } = new List<WordInfo>();
	public List<WordInfo> UserMessageVietnameseWordList { get; set; } = new List<WordInfo>();
}

public class WordInfo
{
	public string VietnameseWord { get; set; } = string.Empty;
	public string EnglishTranslation { get; set; } = string.Empty;
}
