using AIIncidentAssistant.Models;
using AIIncidentAssistant.Services.Claude;
using System.Globalization;
using Xunit;

namespace AIIncidentAssistant.Tests;

public class IncidentPromptBuilderTests
{
    [Theory]
    [InlineData("pt-BR", "Brazilian Portuguese")]
    [InlineData("en-US", "English")]
    [InlineData("es-ES", "Spanish")]
    public void Build_UsesSelectedLanguage(
        string culture,
        string expectedLanguage)
    {
        var originalCulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentUICulture =
                new CultureInfo(culture);

            var incident = new Incident
            {
                Application = "Customer Portal",
                Environment = "Production",
                ErrorMessage = "Database connection failed"
            };

            string prompt = IncidentPromptBuilder.Build(incident);

            Assert.Contains(
                $"Write all user-facing analysis content in {expectedLanguage}.",
                prompt);

            Assert.Contains(
                "Keep all JSON property names in English.",
                prompt);

            Assert.Contains(
                "Keep severity values in English:",
                prompt);

            Assert.Contains(
                "Database connection failed",
                prompt);
        }
        finally
        {
            CultureInfo.CurrentUICulture = originalCulture;
        }
    }
}