using BasicMfaSample.Services.Auth;
using BasicMfaSample.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IEntraAuthService, EntraAuthService>();

//�F�؃T�[�r�X�̍\��
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        //CIAM�ɂ�����Authority�̏������i�t���[������ꂽ��A"p=�`"�Ȃǃp�����[�^�w�肷����@��B2C�̔F�ؕ��@�Ȃ̂Ł~�j
        options.Authority = $"{builder.Configuration["AzureAd:Instance"]}{builder.Configuration["AzureAd:TenantId"]}/v2.0";
    });

//�F�|���V�[�̐ݒ�
// �iFallbackPolicy�őS�y�[�W�F�ؕK�v�ɂ��Ă������s�v�ȃy�[�W�� [AllowAnonymous] �ŏ��O�j
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

//Authentication�i�F�؁F���Ȃ��N�H�j��Authorization�i�F�F���̐l�ɃA�N�Z�X������H�j�Ƃ����������厖
//UseAuthentication()�F���[�U�[�����f�[�^��HttpContext.User�iClaimsPrincipal�j�Ɋi�[
//UseAuthorization()�FHttpContext.User�����ĔF���f
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
