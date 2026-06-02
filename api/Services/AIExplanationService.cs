using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

namespace FileAccessSystem.Services
{
    public class AIExplanationService
    {
        private readonly IConfiguration _configuration;

        public AIExplanationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetAIExplanation(
            string userName,
            string fileName,
            string sensitivity,
            int riskScore)
        {
            var apiKey = _configuration["OpenRouter:ApiKey"];

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var prompt =
                $"User {userName} accessed file {fileName}. " +
                $"File sensitivity is {sensitivity}. " +
                $"Calculated risk score is {riskScore}. " +
                $"Explain in 1-2 sentences why this activity may be risky.";

            var requestBody = new
            {
                model = "openai/gpt-4o-mini",
                messages = new[]
                {
                    new
                    {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var response = await client.PostAsJsonAsync(
                "https://openrouter.ai/api/v1/chat/completions",
                requestBody);

            if (!response.IsSuccessStatusCode)
            {
                return "AI explanation unavailable.";
            }

            var json = await response.Content.ReadAsStringAsync();

            var result = JObject.Parse(json);

            return result["choices"]?[0]?["message"]?["content"]?.ToString()
                   ?? "AI explanation unavailable.";
        }
    }
}