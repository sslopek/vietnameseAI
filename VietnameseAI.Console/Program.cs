using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;

#pragma warning disable SKEXP0010 // OpenAIPromptExecutionSettings ResponseFormat is for evaluation purposes only and is subject to change or removal in future updates.

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

		var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

		// Create a history to store the conversation
		var history = new ChatHistory();

		var executionSettings = new OpenAIPromptExecutionSettings
		{
			// Structured output - https://openai.com/index/introducing-structured-outputs-in-the-api/
			ResponseFormat = typeof(ChatResponseModel),
			// Prompt - Location affects dialect. Gender and age of participants are important for kinship terms.
			ChatSystemPrompt = """
				You are a Vietnamese language coach that helps users learn and practice Vietnamese.
				Engage the user in conversations to help them improve their language skill.
				If the user asks questions about you, make up an interesting and specific response.
				You are a native Vietnamese speaker living in Southern Vietnam.  You are an older than the user.  The user is male.
				The output word lists should include every word from the response or message in lowercase without any grammatical marks, keeping sets of words like "Việt Nam" together and respecting capitalization rules.
			"""
		};


		while (true)
		{
			Console.Write("You: ");
			string userInput = Console.ReadLine() ?? "";

			// Check for blank line to exit
			if (string.IsNullOrWhiteSpace(userInput))
			{
				break;
			}

			history.AddUserMessage(userInput);

			var response = await chatCompletionService.GetChatMessageContentAsync(
				 history,
				 executionSettings: executionSettings,
				 kernel: kernel);

			// Deserialize string response
			var responseModel = JsonSerializer.Deserialize<ChatResponseModel>(response.ToString())!;

			Console.WriteLine("AI: " + responseModel.AssistantResponseInVietnamese);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("AI (English): " + responseModel.AssistantResponseInEnglish);
			Console.WriteLine();
			foreach (var item in responseModel.AssistantResponseVietnameseWordList.Union(responseModel.UserMessageVietnameseWordList).OrderBy(x => x.VietnameseWord))
			{
				Console.WriteLine($"{item.VietnameseWord}: {item.EnglishTranslation}");
			}
			Console.ResetColor();

			// Keep track of just the English version
			history.AddAssistantMessage(responseModel.AssistantResponseInEnglish);
		}

	}
}
