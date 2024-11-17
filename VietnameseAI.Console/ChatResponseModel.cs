namespace VietnameseAI.ConsoleApp
{
	public class ChatResponseModel
	{
		public string UserMessageInEnglish { get; set; } = "";
		public string UserMessageInVietnamese { get; set; } = "";
		public string AssistantResponseInEnglish { get; set; } = "";
		public string AssistantResponseInVietnamese { get; set; } = "";
		public string ResponseGeneralTopic { get; set; } = "";
		public List<WordInfo> AssistantResponseVietnameseWordList { get; set; } = new List<WordInfo>();
		public List<WordInfo> UserMessageVietnameseWordList { get; set; } = new List<WordInfo>();
	}

	public class WordInfo
	{
		public string VietnameseWord { get; set; } = "";
		public string EnglishTranslation { get; set; } = "";
	}
}
