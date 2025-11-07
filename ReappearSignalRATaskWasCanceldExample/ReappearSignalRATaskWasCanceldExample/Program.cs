using Microsoft.AspNetCore.SignalR;
using ReappearSignalRATaskWasCanceldExample.Service;
using ReappearSignalRATaskWasCanceldExample.SignalR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
// 1. 添加 SignalR - 並且啟用 Redis BackPlane (AddStackExchangeRedis 已經內建)
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
builder.Services.AddSignalR().AddStackExchangeRedis(redisConnection, options => {
    //1-2. 重要：為了讓 Redis 某個DB內可以辨識彼此的 Channel 可增加 Prefix 做隔離
    options.Configuration.ChannelPrefix = "MyApp";
});
builder.Services.AddSignalR(options =>
{
    options.AddFilter<ReappearSignalRATaskWasCanceldExample.SignalR.CustomerFilter>();
});

builder.Services.AddControllers();
// 1-3. 注入RedisService 為 Singleton 使其持久化 
builder.Services.AddSingleton<RedisService, RedisService>();

var app = builder.Build();

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
    //3. 配置 SignalR 路由
    endpoints.MapHub<UpdateHub>("UpdateHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
