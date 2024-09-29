using Example.Common.FakeDataBase;
using Example.Common.MinIO;
using Example.Common.MinIO.Factory;
using Example.Common.MinIO.Model;
using MinIOWebSiteExample.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// 1. �`�J�ۨ� - MinIO �Ҭ����
builder.Services.AddSingleton<IMinIOClientFactory, MinIOClientFactory>();
builder.Services.AddSingleton<FakeDataBase>();
builder.Services.AddSingleton<MinIOClientInstance>();
builder.Services.AddSingleton<MinIOConnectionModel>();

// 2. �`�J�ۨ� - Service �� Scoped
builder.Services.AddScoped<ITeacherManageService, TeacherManageService>();

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
