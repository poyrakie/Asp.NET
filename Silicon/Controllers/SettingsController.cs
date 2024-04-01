using Microsoft.AspNetCore.Mvc;

namespace Silicon.Controllers;

public class SettingsController : Controller
{
    public IActionResult ChangeTheme(string theme)
    {
        var option = new CookieOptions
        {
            Expires = DateTime.Now.AddDays(365),
        };
        Response.Cookies.Append("ThemeMode", theme, option);
        return Ok();
    }
}
