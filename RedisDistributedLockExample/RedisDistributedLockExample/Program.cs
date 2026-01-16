using Framework.Database.Enum;
using RedisDistributedLockExample.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 依賴注入 RedLock Redis 實現分布式工作
builder.Services.AddDistributedLock(builder.Configuration);

// 2. 依賴注入 Quartz.NET
builder.Services.AddQuartzNETJob(new DateTime(2026, 1, 16, 13, 06, 00));

// 3. 引用資料庫 
var dbSettings = new Dictionary<MysqlDbConnectionEnum, string>
{
    // 從 appsettings.json 讀取連線資訊並對應到 Enum
    { MysqlDbConnectionEnum.MilkTeaGreen, builder.Configuration.GetConnectionString("MilkTeaGreen") ?? "" },
};
builder.Services.AddDatabase(dbSettings);



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

