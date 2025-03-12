using RedisProducerAndComsumerForListExample.Background;
using RedisProducerAndComsumerForListExample.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 依賴注入 Redis 生產者 ( 127.0.0.1:6379 是本地的 Redis)
builder.Services.AddSingleton(new RedisQueueService("127.0.0.1:6379"));
// 2. 依賴注入 Redis 消費者背景服務
builder.Services.AddHostedService<RedisQueueConsumerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
