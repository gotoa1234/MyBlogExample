using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.Backround;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 添加 SignalR
builder.Services.AddSignalR();

// 2-1. 注入-增加背景服務 - 輪詢 Push SignalR 讓每台 Server 完成 Backplane 工作
builder.Services.AddHostedService<PageBackroundUpdaterService>();

// 2-2. 注入-RabbitMQ 單例模式
builder.Services.AddSingleton<RabbitMqService>();

// 2-3. 應用程式結束時，釋放 RabbitMQ 連線
builder.Services.AddSingleton<IHostedService, RabbitMqServiceHostedService>();

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
    // 3. 配置 SignalR 路由
    endpoints.MapHub<UpdateHub>("UpdateHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
