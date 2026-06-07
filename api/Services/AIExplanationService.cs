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

            var prompt = $@"
You are a cybersecurity assistant.

User: {userName}
File: {fileName}
Risk Score: {riskScore}

Explain the risk in ONE SHORT SENTENCE.

Rules:
- Maximum 15 words.
- No introductions.
- No detailed explanations.
- No legal, financial, or compliance discussions.
- Return only the risk reason.

Example:
'Sensitive file accessed with elevated risk score. Review recommended.'
";
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