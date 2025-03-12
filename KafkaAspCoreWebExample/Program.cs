using KafkaAspCoreWebExample.Services.Background;
using KafkaAspCoreWebExample.Services;
using KafkaAspCoreWebExample.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. ���U Kafka �t�m�ﶵ
builder.Services.Configure<KafkaConfigOptions>(builder.Configuration.GetSection("KafkaConfig"));

// 2-1. ���U Kafka �A��
builder.Services.AddSingleton<IKafkaConsumerService, KafkaConsumerService>();
builder.Services.AddSingleton<IKafkaProducerService, KafkaProducerService>();

// 2-2. ���U Kafka �I���A��
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
