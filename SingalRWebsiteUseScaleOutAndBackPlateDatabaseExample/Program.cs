using MySql.Data.MySqlClient;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Backround;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.DbConnection;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.Repository;
using SingalRWebsiteUseScaleOutAndBackPlateDatabaseExample.SignalR;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[ SignalR
builder.Services.AddSignalR();

builder.Services.AddControllers();

// 2. �W�[�I���A�� - ���� Push SignalR ���C�x Server ���� Backplane �u�@
builder.Services.AddHostedService<PageBackroundUpdaterService>();

// 4. �W�[�`�J�A�t�m Mysql �s�u�r�� / SignalR 
builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new MySqlConnection(connectionString);
});
builder.Services.AddScoped<IMyDb, MyDb>();
builder.Services.AddScoped<ISignalRMessagesRepository, SignalRMessagesRepository>();

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
    //3. �t�m SignalR ����
    endpoints.MapHub<UpdateHub>("UpdateHub");
});


app.Run();
