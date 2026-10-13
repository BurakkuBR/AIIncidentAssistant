using AIIncidentAssistant.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIIncidentAssistant.Services.Claude;

public class AnthropicClient
{
    private readonly HttpClient _httpClient;
    private readonly AnthropicOptions _options;

    public AnthropicClient(
        HttpClient httpClient,
        IOptions<AnthropicOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> SendMessageAsync(
        string prompt,
        int maxTokens = 1000)
    {
        var requestBody = new
        {
            model = _options.Model,
            max_tokens = maxTokens,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = prompt
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);

        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "v1/messages");

        request.Content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        request.Headers.Add("x-api-key", _options.ApiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");

        var response = await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}