using Line.Messaging;
using LineBot2026Example.MiddleWare;
using LineBot2026Example.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// 1. 從 appsettings.json 讀取 ChannelAccessToken，建議使用安全的方式管理敏感資訊
var channelAccessToken = builder.Configuration["LineBot:ChannelAccessToken"];

// 2. 注入 LineMessagingClient 為 Singleton，確保整個應用程式只建立一個實例
builder.Services.AddSingleton<LineMessagingClient>(
    new LineMessagingClient(channelAccessToken!)
);

builder.Services.AddScoped<ILineBotService, LineBotService>();

var app = builder.Build();

// 3. 使用自訂的 ExceptionMiddleware 來統一處理例外狀況
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
