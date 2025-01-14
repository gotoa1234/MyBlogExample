using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StackExchange.Redis;
using ZeroDowntimeDeploymentForDockerWebsiteExample.GlobalSetting;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Redis;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Service.Interface;
using ZeroDowntimeDeploymentForDockerWebsiteExample.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// 1. �K�[ SignalR - �åB�ҥ� Redis BackPlane (AddStackExchangeRedis �w�g����)
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
builder.Services.AddSignalR().AddStackExchangeRedis(redisConnection, options => {    
    options.Configuration.ChannelPrefix = "MyApp";//Redis �Y��DB���i�H���ѩ����� Channel �i�W�[ Prefix ���j��
});

// [�t�mSession] 1. �t�m Session
// �� Nuget �w�� => Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "MySession_";
});

// [�t�mSession] 2-1. �t�m Session + �ݩ�
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1.5);    
    options.Cookie.IsEssential = true;
});

// ���U HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<RedisService, RedisService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IMyAuthenticationService, MyAuthenticationService>();
builder.Services.AddScoped<IMyUserSessionService, MyUserSessionService>();
builder.Services.AddScoped<IUserBalanceService, UserBalanceService>();


#region [�t�mSession] 2-2. �t�m Session + �{��
// �K�[�{�Ұt�m
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // ���n�J�ɾɦV�����|        
    });

// �K�[���v�t�m
builder.Services.AddAuthorization();

#endregion


var app = builder.Build();

// 2. �]�w�����ܼơA�ѱҰʮɨM�w
FieldSettings.EnviromentName = app.Environment.EnvironmentName;

// [�t�mSession] 3. �ҥ� Session
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{    
    endpoints.MapHub<UpdateHub>("/UpdateHub");// �t�m SignalR ����
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
