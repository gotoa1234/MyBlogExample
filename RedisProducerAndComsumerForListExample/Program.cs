using RedisProducerAndComsumerForListExample.Background;
using RedisProducerAndComsumerForListExample.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �̿�`�J Redis �Ͳ��� ( 127.0.0.1:6379 �O���a�� Redis)
builder.Services.AddSingleton(new RedisQueueService("127.0.0.1:6379"));
// 2. �̿�`�J Redis ���O�̭I���A��
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
