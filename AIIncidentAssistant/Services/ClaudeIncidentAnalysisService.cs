using AIIncidentAssistant.Configuration;
using AIIncidentAssistant.Models;
using AIIncidentAssistant.Services.Claude;
using Microsoft.Extensions.Options;

namespace AIIncidentAssistant.Services;

public class ClaudeIncidentAnalysisService : IIncidentAnalysisService
{
    private readonly AnthropicClient _client;
    private readonly AnthropicOptions _options;

    public ClaudeIncidentAnalysisService(
        AnthropicClient client,
        IOptions<AnthropicOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<IncidentAnalysis> AnalyzeAsync(Incident incident)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            return new IncidentAnalysis
            {
                Summary = "Claude API is not configured yet. No AI analysis was performed.",
                Severity = "Not analyzed",
                RecommendedActions =
                [
                    "Configure an Anthropic API key in User Secrets to enable AI analysis."
                ]
            };
        }

        try
        {
            string prompt = IncidentPromptBuilder.Build(incident);

            string response = await _client.SendMessageAsync(prompt);

            return ClaudeResponseParser.Parse(response);
        }
        catch (HttpRequestException)
        {
            return new IncidentAnalysis
            {
                Summary = "Unable to communicate with the Claude API.",
                Severity = "Not analyzed",
                RecommendedActions =
                [
                    "Check the API configuration and network connection.",
                        "Try again later."
                ]
            };
        }
        catch (TaskCanceledException)
        {
            return new IncidentAnalysis
            {
                Summary = "The Claude API request timed out or was canceled.",
                Severity = "Not analyzed",
                RecommendedActions =
                [
                    "Try again later."
                ]
            };
        }
        catch (System.Text.Json.JsonException)
        {
            return new IncidentAnalysis
            {
                Summary = "Claude returned a response that could not be processed.",
                Severity = "Not analyzed",
                RecommendedActions =
                [
                    "Try analyzing the incident again."
                ]
            };
        }
        catch (InvalidOperationException)
        {
            return new IncidentAnalysis
            {
                Summary = "Claude returned an incomplete or invalid analysis.",
                Severity = "Not analyzed",
                RecommendedActions =
                [
                    "Try analyzing the incident again."
                ]
            };
        }
    }
}