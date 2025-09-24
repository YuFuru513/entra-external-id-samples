using BasicMfaSample.Services.Auth;
using BasicMfaSample.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEntraAuthService, EntraAuthService>();

//認証サービスの構成
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        //CIAMにおけるAuthorityの書き方（フロー名を入れたり、"p=～"などパラメータ指定する方法はB2Cの認証方法なので×）
        options.Authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/v2.0";
    });

//認可ポリシーの設定
// （FallbackPolicyで全ページ認証必要にしておく→不要なページは [AllowAnonymous] で除外）
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();//ko

app.UseRouting();

//Authentication（認証：あなた誰？）→Authorization（認可：この人にアクセス権ある？）という順序が大事
//UseAuthentication()：ユーザー属性データをHttpContext.User（ClaimsPrincipal）に格納
//UseAuthorization()：HttpContext.Userを見て認可判断
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
