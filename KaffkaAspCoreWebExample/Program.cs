using KaffkaAspCoreWebExample.Services.Background;
using KaffkaAspCoreWebExample.Services;
using KaffkaAspCoreWebExample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 註冊 Kafka 配置選項
builder.Services.Configure<KafkaConfigOptions>(builder.Configuration.GetSection("KafkaConfig"));

// 註冊 Kafka 服務
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

builder.Services.AddHostedService<KafkaConsumerHostedService>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
