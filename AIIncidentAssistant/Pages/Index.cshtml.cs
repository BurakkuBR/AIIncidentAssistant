using AIIncidentAssistant.Models;
using AIIncidentAssistant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AIIncidentAssistant.Pages;

public class IndexModel : PageModel
{
    private readonly IIncidentAnalysisService _incidentAnalysisService;
    public bool IsDemoMode { get; private set; }

    private readonly IConfiguration _configuration;

    public IndexModel(
        IIncidentAnalysisService incidentAnalysisService,
        IConfiguration configuration)
    {
        _incidentAnalysisService = incidentAnalysisService;
        _configuration = configuration;
    }
    [BindProperty]
    public Incident Incident { get; set; } = new();

    public IncidentAnalysis? Analysis { get; private set; }

    public Task OnGetAsync()
    {
        IsDemoMode = _configuration.GetValue<bool>("DemoMode");

        return Task.CompletedTask;
    }

    public async Task OnPostAsync()
    {
        IsDemoMode = _configuration.GetValue<bool>("DemoMode");

        if (!ModelState.IsValid)
        {
            return;
        }

        Analysis = await _incidentAnalysisService.AnalyzeAsync(Incident);
    }
}