
using AIIncidentAssistant.Localization;
using AIIncidentAssistant.Models;
using Microsoft.Extensions.Localization;

namespace AIIncidentAssistant.Services;

public class MockIncidentAnalysisService : IIncidentAnalysisService
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public MockIncidentAnalysisService(
        IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
    }

    public Task<IncidentAnalysis> AnalyzeAsync(Incident incident)
    {
        string error = incident.ErrorMessage ?? "";
        string logs = incident.Logs ?? "";

        string combinedText = $"{error} {logs}".ToLowerInvariant();

        var analysis = new IncidentAnalysis
        {
            Summary = _localizer[
                "DemoSummary",
                incident.Application,
                incident.Environment,
                incident.ErrorMessage
            ].Value,

            Severity = "Medium"
        };

        if (combinedText.Contains("database") ||
            combinedText.Contains("sql") ||
            combinedText.Contains("connection"))
        {
            analysis.Severity = "High";

            analysis.PossibleCauses.Add(new PossibleCause
            {
                Cause = _localizer["DatabaseCause"].Value,
                Reason = _localizer["DatabaseReason"].Value
            });

            analysis.RecommendedActions.Add(
                _localizer["CheckDatabase"].Value);

            analysis.RecommendedActions.Add(
                _localizer["CheckConnection"].Value);

            analysis.MissingInformation.Add(
                _localizer["DatabaseMissing"].Value);
        }
        else if (combinedText.Contains("timeout"))
        {
            analysis.PossibleCauses.Add(new PossibleCause
            {
                Cause = _localizer["TimeoutCause"].Value,
                Reason = _localizer["TimeoutReason"].Value
            });

            analysis.RecommendedActions.Add(
                _localizer["CheckResponseTime"].Value);

            analysis.RecommendedActions.Add(
                _localizer["CheckDependencies"].Value);

            analysis.MissingInformation.Add(
                _localizer["TimeoutMissing"].Value);
        }
        else
        {
            analysis.PossibleCauses.Add(new PossibleCause
            {
                Cause = _localizer["UnknownCause"].Value,
                Reason = _localizer["UnknownReason"].Value
            });

            analysis.RecommendedActions.Add(
                _localizer["CheckLogs"].Value);

            analysis.MissingInformation.Add(
                _localizer["UnknownMissing"].Value);
        }

        return Task.FromResult(analysis);
    }
}
