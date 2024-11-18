using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;
using VietnameseAI.Shared.Models;

#pragma warning disable SKEXP0010 // OpenAIPromptExecutionSettings ResponseFormat is for evaluation purposes only and is subject to change or removal in future updates.

namespace VietnameseAI.Shared.Services;

public class LanguageChatService
{
	private readonly Kernel kernel;

	private readonly IChatCompletionService chatCompletionService;
	private readonly OpenAIPromptExecutionSettings openAIPromptExecutionSettings;
	// History to store the conversation
	readonly private ChatHistory history = [];

	public LanguageChatService(Kernel kernel)
	{
		this.kernel = kernel;
		chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

		openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
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
	}

	public async Task<ChatResponseModel> SendMessage(string userMessage)
	{
		history.AddUserMessage(userMessage);

		var response = await chatCompletionService.GetChatMessageContentAsync(
			 history,
			 executionSettings: openAIPromptExecutionSettings,
			 kernel: kernel);

		// Deserialize string response
		var responseModel = JsonSerializer.Deserialize<ChatResponseModel>(response.ToString())!;

		// Keep track of just the English version
		history.AddAssistantMessage(responseModel.AssistantResponseInEnglish);

		return responseModel;
	}
}
