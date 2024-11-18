using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using VietnameseAI.Shared.Data;
using VietnameseAI.Shared.Models;
using VietnameseAI.Shared.Services;

namespace VietnameseAI
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var apiKey = Environment.GetEnvironmentVariable("OpenAI__ApiKey__VietnameseAI") ?? "no-key";

			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				});

			builder.Services.AddMauiBlazorWebView();

#if DEBUG
			builder.Services.AddBlazorWebViewDeveloperTools();
			builder.Logging.AddDebug();
#endif

			// SQLite database
			var sqlitePreferences = new SQLitePreferences();
			sqlitePreferences.DatabasePath = Path.Combine(FileSystem.AppDataDirectory, sqlitePreferences.DatabaseFilename);

			// fix for "SSL handshake aborted" on Android in Semantic Kernel
			var httpClientHandlerCertCheckSkip = new HttpClientHandler
			{
				CheckCertificateRevocationList = false,
			};
			builder.Services.AddHttpClient("OpenAIChatCompletionHttpClient").ConfigurePrimaryHttpMessageHandler(() => httpClientHandlerCertCheckSkip);

			builder.Services.AddTransient<Kernel>(serviceProvider =>
			{
				// Use preferences screen value if set, then environment variable if not
				var openaiKey = Preferences.Default.Get("openai_key", apiKey);

				var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
				var httpClient = httpClientFactory.CreateClient("OpenAIChatCompletionHttpClient");

				// Initialize kernel.
				Kernel kernel = Kernel.CreateBuilder()
					.AddOpenAIChatCompletion(
						modelId: "gpt-4o-2024-08-06",
						apiKey: openaiKey,
						httpClient: httpClient
						)
					.Build();
				return kernel;
			});
			builder.Services.AddSingleton<LanguageChatService>();
			builder.Services.AddTransient<UserLearningDatabase>();
			builder.Services.AddSingleton<SQLitePreferences>(sqlitePreferences);

			return builder.Build();
		}
	}
}
