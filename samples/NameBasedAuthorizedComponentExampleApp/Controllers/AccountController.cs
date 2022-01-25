using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace NameBasedAuthorizedComponentExampleApp.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    public IActionResult SignIn([FromQuery] string returnUrl)
    {
        if (User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        //this if you are using okta nuget
        //return Challenge(OktaDefaults.MvcAuthenticationScheme); 

        return Challenge();
    }

    public  async Task<IActionResult> SignOut([FromQuery]string returnUrl)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        await HttpContext.SignOutAsync();

        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
    
    [HttpPost("Logout")]
    [ValidateAntiForgeryToken]
    public IActionResult PostLogout([FromQuery]string returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Content("~/");

        if (User.Identity.IsAuthenticated)
        {
            HttpContext.SignOutAsync();
        }

        return LocalRedirect(returnUrl);
    }
    
    // public IActionResult SignOut([FromQuery] string returnUrl)
    // {
    //     if (!User.Identity.IsAuthenticated)
    //     {
    //         return LocalRedirect(returnUrl ?? Url.Content("~/"));
    //     }
    //
    //     return SignOut(
    //         new AuthenticationProperties() { RedirectUri = Url.Content("~/") },
    //         new[]
    //         {
    //             OktaDefaults.MvcAuthenticationScheme,
    //             CookieAuthenticationDefaults.AuthenticationScheme,
    //         });
    // }
}