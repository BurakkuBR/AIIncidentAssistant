using AIIncidentAssistant.Services;
using AIIncidentAssistant.Configuration;
using AIIncidentAssistant.Services.Claude;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<AnthropicOptions>(
    builder.Configuration.GetSection("Anthropic"));

builder.Services.AddHttpClient<AnthropicClient>(client =>
{
    client.BaseAddress = new Uri("https://api.anthropic.com/");
});

builder.Services.AddScoped<MockIncidentAnalysisService>();
builder.Services.AddScoped<ClaudeIncidentAnalysisService>();

bool demoMode = builder.Configuration.GetValue<bool>("DemoMode");

if (demoMode)
{
    builder.Services.AddScoped<IIncidentAnalysisService, MockIncidentAnalysisService>();
}
else
{
    builder.Services.AddScoped<IIncidentAnalysisService, ClaudeIncidentAnalysisService>();
}

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});

builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider =
            (type, factory) =>
                factory.Create(
                    typeof(AIIncidentAssistant.Localization.SharedResource));
    });

builder.Services.AddControllers();

var app = builder.Build();

var supportedCultures = new[]
{
    new CultureInfo("pt-BR"),
    new CultureInfo("en-US"),
    new CultureInfo("es-ES")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

localizationOptions.RequestCultureProviders =
[
    new CookieRequestCultureProvider(),
    new AcceptLanguageHeaderRequestCultureProvider()
];

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapControllers();

app.Run();
