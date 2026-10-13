namespace AIIncidentAssistant.Models;

public class IncidentAnalysis
{
    public string Summary { get; set; } = string.Empty;

    public string Severity { get; set; } = string.Empty;

    public List<PossibleCause> PossibleCauses { get; set; } = [];

    public List<string> RecommendedActions { get; set; } = [];

    public List<string> MissingInformation { get; set; } = [];
}