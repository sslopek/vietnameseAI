namespace VietnameseAI.ConsoleApp
{
	public class ChatResponseModel
	{
		public string UserMessageEnglish { get; set; } = "";
		public string UserMessageVietnamese { get; set; } = "";
		public string ChatReplyEnglish { get; set; } = "";
		public string ChatReplyVietnamese { get; set; } = "";
		public List<WordInfo> ChatReplyVietnameseWords { get; set; } = new List<WordInfo>();
	}

	public class WordInfo
	{
		public string VietnameseWord { get; set; } = "";

		public string EnglishTranslation { get; set; } = "";
	}
}
