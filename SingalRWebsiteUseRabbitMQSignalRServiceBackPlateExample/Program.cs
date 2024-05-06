using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.Backround;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.RabbitMQ;
using SingalRWebsiteUseRabbitMQSignalRServiceBackPlateExample.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[ SignalR
builder.Services.AddSignalR();

// 2-1. �`�J-�W�[�I���A�� - ���� Push SignalR ���C�x Server ���� Backplane �u�@
builder.Services.AddHostedService<PageBackroundUpdaterService>();

// 2-2. �`�J-RabbitMQ ��ҼҦ�
builder.Services.AddSingleton<RabbitMqService>();

// 2-3. ���ε{�������ɡA���� RabbitMQ �s�u
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
    // 3. �t�m SignalR ����
    endpoints.MapHub<UpdateHub>("UpdateHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
