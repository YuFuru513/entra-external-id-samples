using BasicMfaSample.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;

namespace BasicMfaSample.Services.Auth
{
    public class EntraAuthService : IEntraAuthService
    {
        public AuthenticationProperties CreateSignInProperties(string returnUrl = "/Dashboard/Index")
        {
            return new AuthenticationProperties { RedirectUri = returnUrl };
        }

        public AuthenticationProperties CreateSignOutProperties()
        {
            return new AuthenticationProperties { RedirectUri = "/" };
        }
    }
}