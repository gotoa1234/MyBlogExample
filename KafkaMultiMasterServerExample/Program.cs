using KafkaAspCoreWebExample.Models;
using KafkaAspCoreWebExample.Services.Background;
using KafkaAspCoreWebExample.Services;
using KafkaMultiMasterServerExample.Service.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 註冊 Kafka 配置選項
builder.Services.Configure<KafkaConfigOptions>(builder.Configuration.GetSection("KafkaConfig"));

// 2-1. 註冊 Kafka 服務
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

// 2-2. 註冊 Kafka 主服務
builder.Services.AddHostedService<KafkaMultiMasterConsumerHostedService>();


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
