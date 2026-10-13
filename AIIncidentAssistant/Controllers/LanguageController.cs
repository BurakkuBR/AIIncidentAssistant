using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace AIIncidentAssistant.Controllers;

[Route("language")]
public class LanguageController : Controller
{
    [HttpPost("set")]
    [ValidateAntiForgeryToken]
    public IActionResult SetLanguage(
        string culture,
        string returnUrl = "/")
    {
        var supportedCultures = new[]
        {
            "pt-BR",
            "en-US",
            "es-ES"
        };

        if (!supportedCultures.Contains(culture))
        {
            culture = "en-US";
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(
                new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            });

        return LocalRedirect(
            Url.IsLocalUrl(returnUrl) ? returnUrl : "/");
    }
}