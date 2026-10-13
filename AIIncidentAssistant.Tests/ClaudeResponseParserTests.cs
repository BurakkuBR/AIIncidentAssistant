using AIIncidentAssistant.Services.Claude;
using System.Text.Json;
using Xunit;

namespace AIIncidentAssistant.Tests;

public class ClaudeResponseParserTests
{
    [Fact]
    public void Parse_ValidResponse_ReturnsIncidentAnalysis()
    {
        // Arrange: preparar uma resposta simulada
        var analysis = new
        {
            summary = "Database connection failure",
            severity = "High",
            possibleCauses = new[]
            {
                new
                {
                    cause = "Database unavailable",
                    reason = "Connection timeout detected"
                }
            },
            recommendedActions = new[]
            {
                "Check database availability"
            },
            missingInformation = new[]
            {
                "Database server logs"
            }
        };

        var apiResponse = new
        {
            content = new[]
            {
                new
                {
                    type = "text",
                    text = JsonSerializer.Serialize(analysis)
                }
            }
        };

        string json = JsonSerializer.Serialize(apiResponse);

        // Act: executar o parser
        var result = ClaudeResponseParser.Parse(json);

        // Assert: verificar o resultado
        Assert.NotNull(result);
        Assert.Equal("Database connection failure", result.Summary);
        Assert.Equal("High", result.Severity);
        Assert.Single(result.PossibleCauses);
        Assert.Equal("Database unavailable", result.PossibleCauses[0].Cause);
        Assert.Single(result.RecommendedActions);
        Assert.Single(result.MissingInformation);
    }
}