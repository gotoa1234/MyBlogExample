using DistributedeSAGAWithMysql.Extension;
using DistributedeSAGAWithMysql.Repository.Instance;
using DistributedeSAGAWithMysql.Repository.Interface;
using DistributedeSAGAWithMysql.Service.Instance;
using DistributedeSAGAWithMysql.Service.Interface;
using Framework.Database.Enum;
using Framework.Database.Implementations.Shared;
using Framework.Database.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 依賴注入所需的項目
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IUnitOfWorkAccessor, UnitOfWorkAccessor>();

// 2. 依賴注入 Quartz.NET
builder.Services.AddQuartzNETJob(DateTime.Now);

// 3. 引用資料庫 
var dbSettings = new Dictionary<MysqlDbConnectionEnum, string>
{
    // 從 appsettings.json 讀取連線資訊並對應到 Enum
    { MysqlDbConnectionEnum.Member, builder.Configuration.GetConnectionString("Member") ?? "" },
    { MysqlDbConnectionEnum.Balance, builder.Configuration.GetConnectionString("Balance") ?? "" },
    { MysqlDbConnectionEnum.Log, builder.Configuration.GetConnectionString("Log") ?? "" }
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
