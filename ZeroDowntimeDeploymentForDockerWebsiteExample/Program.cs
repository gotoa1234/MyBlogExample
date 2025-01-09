using StackExchange.Redis;
using ZeroDowntimeDeploymentForDockerWebsiteExample.GlobalSetting;
using ZeroDowntimeDeploymentForDockerWebsiteExample.Redis;
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

builder.Services.AddSingleton<RedisService, RedisService>();

var app = builder.Build();

// 2. �]�w�����ܼơA�ѱҰʮɨM�w
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
    endpoints.MapHub<UpdateHub>("UpdateHub");// �t�m SignalR ����
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
