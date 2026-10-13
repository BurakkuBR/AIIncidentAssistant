using AIIncidentAssistant.Models;
using System.Text.Json;

namespace AIIncidentAssistant.Services.Claude;

public static class ClaudeResponseParser
{
    public static IncidentAnalysis Parse(string apiResponse)
    {
        using var document = JsonDocument.Parse(apiResponse);

        var content = document.RootElement
            .GetProperty("content");

        string responseText = string.Join(
            "\n",
            content.EnumerateArray()
                .Where(item =>
                    item.GetProperty("type").GetString() == "text")
                .Select(item =>
                    item.GetProperty("text").GetString())
        );

        if (string.IsNullOrWhiteSpace(responseText))
        {
            throw new InvalidOperationException(
                "Claude returned an empty response.");
        }

        var analysis = JsonSerializer.Deserialize<IncidentAnalysis>(
            responseText,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (analysis == null)
        {
            throw new InvalidOperationException(
                "Unable to deserialize Claude analysis.");
        }

        return analysis;
    }
}