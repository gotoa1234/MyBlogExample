using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 添加 SignalR - 並且啟用 Redis BackPlane
//var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IDatabase>(provider =>
{
    // 這裡配置 Redis 連接
    var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");//ConnectionMultiplexer.Connect("YourRedisConnectionString");
    var redis = ConnectionMultiplexer.Connect(redisConnection);
    return redis.GetDatabase();
});
builder.Services.AddSignalR();

builder.Services.AddControllers();

var app = builder.Build();

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
    //3. 配置 SignalR 路由
    endpoints.MapHub<UpdateHub>("UpdateHub");


});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
