using SingalRWebsiteUseAzureSignalRServiceBackPlateExample.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[ Azure Signalr Service �]�w
var azureConnection = builder.Configuration["Azure:SignalR:ConnectionString"];
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    // 2. �o��n�Ѧ� Azure �W���s�u�r��
    options.ConnectionString = azureConnection;
});

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

app.UseEndpoints(endpoints =>
{
    //3. �t�m SignalR ����
    endpoints.MapHub<UpdateHub>("UpdateHub");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();