using Example.Common.FakeDataBase;
using Example.Common.MinIO;
using Example.Common.MinIO.Factory;
using Example.Common.MinIO.Model;
using MinIOWebSiteExample.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IMinIOClientFactory, MinIOClientFactory>();
builder.Services.AddSingleton<FakeDataBase>();
builder.Services.AddScoped<ITeacherManageService, TeacherManageService>();
builder.Services.AddSingleton<MinIOClientInstance>();
builder.Services.AddSingleton<MinIOConnectionModel>();

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
