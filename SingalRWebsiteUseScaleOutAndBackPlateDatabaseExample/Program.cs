using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Backround;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 添加 SignalR
builder.Services.AddSignalR();

builder.Services.AddControllers();

// 2. 增加背景服務 - 輪詢 Push SignalR
builder.Services.AddHostedService<PageBackroundUpdaterService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
    //3. 配置 SignalR 路由
    endpoints.MapHub<UpdateHub>("UpdateHub");
});


app.Run();
