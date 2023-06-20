
using BlazorJWTLoginExample2.Middleware;
using BlazorJWTLoginExample2.Repository;
using BlazorJWTLoginExample2.Service;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<JsonWebTokenService>();
builder.Services.AddSingleton<ISqliteRepository, SqliteRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();

// 添加身份驗證提供者
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "YourApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
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
app.UseSession(); // 添加 Session 中介軟體

app.UseMiddleware<UserLoginSessionHandlerMiddleware>();// 添加 Middleware
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
