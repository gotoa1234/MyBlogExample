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


// 1. 添加 SignalR - 並且啟用 Redis BackPlane (AddStackExchangeRedis 已經內建)
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
builder.Services.AddSignalR().AddStackExchangeRedis(redisConnection, options => {    
    options.Configuration.ChannelPrefix = "MyApp";//Redis 某個DB內可以辨識彼此的 Channel 可增加 Prefix 做隔離
});

// [配置Session] 1. 配置 Session
// ※ Nuget 安裝 => Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "MySession_";
});

// [配置Session] 2-1. 配置 Session + 屬性
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1.5);    
    options.Cookie.IsEssential = true;
});

// 註冊 HttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<RedisService, RedisService>();
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IMyAuthenticationService, MyAuthenticationService>();
builder.Services.AddScoped<IMyUserSessionService, MyUserSessionService>();
builder.Services.AddScoped<IUserBalanceService, UserBalanceService>();


#region [配置Session] 2-2. 配置 Session + 認證
// 添加認證配置
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";  // 未登入時導向的路徑        
    });

// 添加授權配置
builder.Services.AddAuthorization();

#endregion


var app = builder.Build();

// 2. 設定全域變數，由啟動時決定
FieldSettings.EnviromentName = app.Environment.EnvironmentName;

// [配置Session] 3. 啟用 Session
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
    endpoints.MapHub<UpdateHub>("/UpdateHub");// 配置 SignalR 路由
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
