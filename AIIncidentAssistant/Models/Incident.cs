using System.ComponentModel.DataAnnotations;

namespace AIIncidentAssistant.Models;

public class Incident
{
    [Required(ErrorMessage = "ApplicationRequired")]
    [StringLength(100, ErrorMessage = "ApplicationTooLong")]
    public string Application { get; set; } = string.Empty;

    [Required(ErrorMessage = "EnvironmentRequired")]
    [StringLength(50, ErrorMessage = "EnvironmentTooLong")]
    public string Environment { get; set; } = string.Empty;

    [Required(ErrorMessage = "ErrorMessageRequired")]
    [StringLength(2000, ErrorMessage = "ErrorMessageTooLong")]
    public string ErrorMessage { get; set; } = string.Empty;

    [StringLength(10000, ErrorMessage = "LogsTooLong")]
    public string? Logs { get; set; }

    [StringLength(2000, ErrorMessage = "RecentChangesTooLong")]
    public string? RecentChanges { get; set; }
}