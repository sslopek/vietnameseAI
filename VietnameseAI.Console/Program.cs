using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Encodings.Web;
using System.Text.Json;

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

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
		var executionSettings = new OpenAIPromptExecutionSettings
		{
			// Structured output - https://openai.com/index/introducing-structured-outputs-in-the-api/
			ResponseFormat = typeof(ChatResponseModel),
			// Prompt - Location affects dialect. Gender and age of participants are important for kinship terms.
			ChatSystemPrompt = "Assistant is acting as a native Vietnamese speaker living in Southern Vietnam.  It is an older colleague of the user.  The user is male."
		};
#pragma warning restore SKEXP0010

		while (true)
		{
			Console.Write("You: ");
			string userInput = Console.ReadLine() ?? "";

			// Check for blank line to exit
			if (string.IsNullOrWhiteSpace(userInput))
			{
				break;
			}

			var response = await kernel.InvokePromptAsync(userInput, new(executionSettings));

			// Deserialize string response
			var responseModel = JsonSerializer.Deserialize<ChatResponseModel>(response.ToString())!;
			// Serialize again for easy viewing on console
			var prettyPrinted = JsonSerializer.Serialize(responseModel, new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping })!;

			Console.WriteLine("AI: " + responseModel.ChatReplyVietnamese);
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine("AI (English): " + responseModel.ChatReplyEnglish);
			Console.WriteLine();
			Console.WriteLine(prettyPrinted);
			Console.ResetColor();
		}

	}
}
