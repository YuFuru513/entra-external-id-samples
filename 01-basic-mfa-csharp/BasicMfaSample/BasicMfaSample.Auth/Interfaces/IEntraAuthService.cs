using Microsoft.AspNetCore.Authentication;

namespace BasicMfaSample.Services.Interfaces
{
    public interface IEntraAuthService
    {
        AuthenticationProperties CreateSignInProperties(string returnUrl = "/Dashboard/Index");
        AuthenticationProperties CreateSignOutProperties();
    }
}