using Microsoft.SemanticKernel;
using VietnameseAI.Shared.Data;
using VietnameseAI.Shared.Models;
using VietnameseAI.Shared.Services;

namespace VietnameseAI.ConsoleApp;

internal class Program
{
	static async Task Main()
	{
		var apiKey = Environment.GetEnvironmentVariable("OpenAI__ApiKey__VietnameseAI");
		if (apiKey == null)
		{
			Console.WriteLine("No API Key set (OpenAI__ApiKey__VietnameseAI)");
			return;
		}

		// Set console to support Vietnamese characters (also need to set configure console to use a font like Consolas)
		Console.OutputEncoding = System.Text.Encoding.UTF8;

		// Initialize kernel.
		Kernel kernel = Kernel.CreateBuilder()
			.AddOpenAIChatCompletion(
				modelId: "gpt-4o-2024-08-06",
				apiKey: apiKey
				)
			.Build();

		var sqlitePreferences = new SQLitePreferences();
		sqlitePreferences.DatabasePath = Path.Combine(sqlitePreferences.DatabasePath, sqlitePreferences.DatabaseFilename);
		var userLearningDatabase = new UserLearningDatabase(sqlitePreferences);
		var chatService = new LanguageChatService(kernel, userLearningDatabase);

		while (true)
		{
			Console.Write("You: ");
			string userInput = Console.ReadLine() ?? "";

			// Check for blank line to exit
			if (string.IsNullOrWhiteSpace(userInput))
			{
				break;
			}

			var responseModel = await chatService.SendMessage(userInput);

			Console.WriteLine("AI: " + responseModel.ResponseVietnamese);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("AI (English): " + responseModel.ResponseEnglish);
			//Console.WriteLine();
			//foreach (var item in responseModel.AssistantResponseVietnameseWordList.Union(responseModel.UserMessageVietnameseWordList).OrderBy(x => x.VietnameseWord))
			//{
			//	Console.WriteLine($"{item.VietnameseWord}: {item.EnglishTranslation}");
			//}
			Console.ResetColor();
		}
	}
}
