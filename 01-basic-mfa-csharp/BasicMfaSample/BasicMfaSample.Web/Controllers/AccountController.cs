using BasicMfaSample.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace BasicMfaSample.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEntraAuthService _authService;

        public AccountController(IEntraAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult SignIn(string returnUrl = "/Dashboard/Index")
        {
            var properties = _authService.CreateSignInProperties(returnUrl);
            return Challenge(properties, OpenIdConnectDefaults.AuthenticationScheme);
        }

        public IActionResult SignOut()
        {
            var properties = _authService.CreateSignOutProperties();
            return SignOut(properties, OpenIdConnectDefaults.AuthenticationScheme, "Cookies");
        }
    }
}