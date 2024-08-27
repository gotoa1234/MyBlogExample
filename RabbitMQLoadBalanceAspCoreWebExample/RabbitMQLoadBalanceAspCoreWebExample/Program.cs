using Example.Common.FakeDataBase;
using Example.Common.RabbitMQ.Factory;
using RabbitMQLoadBalanceAspCoreWebExample.RabbitMQ;
using RabbitMQLoadBalanceAspCoreWebExample.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// 1-1. ª`¤J¬Û¨Ì
builder.Services.AddSingleton<IRabbitMqFactory, RabbitMqFactory>();
builder.Services.AddSingleton<FakeDataBase>();
builder.Services.AddScoped<IAccountTradeOrder, AccountTradeOrder>();

// 1-2. ª`¤J RabbitMQ Subscriber
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

app.InitMqSubscriber();

app.Run();
