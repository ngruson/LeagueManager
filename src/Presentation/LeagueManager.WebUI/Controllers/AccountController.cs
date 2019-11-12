using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace LeagueManager.WebUI.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "Home/Index";

            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                }, "oidc");
        }

        public IActionResult SignOut()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}