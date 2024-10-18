using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

// 1. [加入結構化、計量、追蹤] Aspire 要能可觀察此專案，需要增加以下服務
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

// 依賴注入 MySQL 連線服務
builder.Services.AddTransient<MySqlConnection>(sp =>
{
    // 讀取 ConnectionString，並設定 MySQL DbContext
    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
    return new MySqlConnection(connectionString);
});

var app = builder.Build();

// 2. [加入結構化、計量、追蹤] Aspire 要能可觀察此專案，需要增加以下服務
app.MapDefaultEndpoints();

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
