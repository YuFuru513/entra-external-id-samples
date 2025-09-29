using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClaimsOnToken.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // ユーザーの基本情報を取得
            var userInfo = new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                AuthenticationType = User.Identity.AuthenticationType,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Country = User.FindFirst(ClaimTypes.Country)?.Value,
                State = User.FindFirst(ClaimTypes.StateOrProvince)?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("emails")?.Value,
                DisplayName = User.FindFirst("name")?.Value,
                // 全てのクレーム情報
                Claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList()
            };

            return View(userInfo);
        }
    }
}
