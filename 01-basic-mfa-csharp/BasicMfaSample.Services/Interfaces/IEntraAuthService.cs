using Microsoft.AspNetCore.Authentication;

namespace BasicMfaSample.Services.Interfaces
{
    public interface IEntraAuthService
    {
        /// <summary>
        /// サインイン時の認証プロパティを生成する
        /// </summary>
        /// <param name="returnUrl">認証完了後の戻り先</param>
        /// <returns>認証プロパティ</returns>
        AuthenticationProperties CreateSignInProperties(string returnUrl = "/Dashboard/Index");

        /// <summary>
        /// サインアウト時の認証プロパティを生成する
        /// </summary>
        /// <param name="returnUrl">認証完了後の戻り先</param>
        /// <returns>認証プロパティ</returns>
        AuthenticationProperties CreateSignOutProperties(string returnUrl = "/");
    }
}