using ClaimsOnToken.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace ClaimsOnToken.Services.Auth
{
    public class EntraAuthService : IEntraAuthService
    {
        public AuthenticationProperties CreateSignInProperties(string returnUrl = "/Dashboard/Index")
        {
            return new AuthenticationProperties { RedirectUri = returnUrl };
        }

        public AuthenticationProperties CreateSignOutProperties(string returnUrl = "/")
        {
            return new AuthenticationProperties { RedirectUri = returnUrl };
        }
    }
}