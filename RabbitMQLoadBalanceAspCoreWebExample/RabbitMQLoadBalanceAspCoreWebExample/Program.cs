using Example.Common.FakeDataBase;
using Example.Common.RabbitMQ.Factory;
using RabbitMQLoadBalanceAspCoreWebExample.RabbitMQ;
using RabbitMQLoadBalanceAspCoreWebExample.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// 1. 注入相依
builder.Services.AddSingleton<IRabbitMqFactory, RabbitMqFactory>();
builder.Services.AddSingleton<FakeDataBase>();
builder.Services.AddScoped<IAccountTradeOrder, AccountTradeOrder>();

// 2. 注入 RabbitMQ Subscriber
builder.Services.AddRabbitMqSubscriber();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 3. RabbitMQ Subscriber 啟用交換器
app.InitMqSubscriber();

app.Run();
