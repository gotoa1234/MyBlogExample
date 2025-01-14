using RedisSolveDistributedSessionExample.Service;
using RedisSolveDistributedSessionExample.Service.IService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. ���o Redis �s�u�t�m
var redisConnection = builder.Configuration.GetConnectionString("RedisConnection");

// 2. �t�m Session + Redis
// �� Nuget �w�� => Microsoft.Extensions.Caching.StackExchangeRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
    options.InstanceName = "MySession_";
});

// 3. �t�m Session ��l�t�m
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(1.5);
    options.Cookie.IsEssential = true;
});


// 4. ���U HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// 5. �̿�ʪ`�J���� Scope Singleton
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IUserBalanceService, UserBalanceService>();


var app = builder.Build();

// 6. ���ε{���ҥ� Session
app.UseSession();

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
    pattern: "{controller=Balance}/{action=Index}/{id?}");

app.Run();
