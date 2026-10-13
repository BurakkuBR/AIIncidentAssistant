using AIIncidentAssistant.Models;

namespace AIIncidentAssistant.Services;

public interface IIncidentAnalysisService
{
    Task<IncidentAnalysis> AnalyzeAsync(Incident incident);
}