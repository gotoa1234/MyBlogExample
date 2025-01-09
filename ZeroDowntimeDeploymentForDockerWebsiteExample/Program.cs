using StackExchange.Redis;
using ZeroDowntimeDeploymentForDockerWebsiteExample.GlobalSetting;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Redis;
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

builder.Services.AddSingleton<RedisService, RedisService>();

var app = builder.Build();

// 2. 設定全域變數，由啟動時決定
FieldSettings.EnviromentName = app.Environment.EnvironmentName;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{    
    endpoints.MapHub<UpdateHub>("UpdateHub");// 配置 SignalR 路由
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
