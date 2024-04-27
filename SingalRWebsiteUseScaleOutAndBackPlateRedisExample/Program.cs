using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.Redis;
using SingalRWebsiteUseScaleOutAndBackPlateRedisExample.SignalR;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[ SignalR - �åB�ҥ� Redis BackPlane (AddStackExchangeRedis �w�g����)
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));
builder.Services.AddSignalR().AddStackExchangeRedis(redisConnection, options => {
    //1-2. ���n�G���F�� Redis �Y��DB���i�H���ѩ����� Channel �i�W�[ Prefix ���j��
    options.Configuration.ChannelPrefix = "MyApp";
});

builder.Services.AddControllers();
// 1-3. �`�JRedisService �� Singleton �Ϩ���[�� 
builder.Services.AddSingleton<RedisService, RedisService>();

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
    //3. �t�m SignalR ����
    endpoints.MapHub<UpdateHub>("UpdateHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
