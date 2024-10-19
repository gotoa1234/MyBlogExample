using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 1. [加入結構化、計量、追蹤] Aspire 要能可觀察此專案，需要增加以下服務
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

// 注入Redis連接字符串
var redisConnectionString = builder.Configuration.GetSection("ConnectionStrings:RedisDb").Value;

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{        
    return ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(redisConnectionString));
});

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

//// 2. [加入結構化、計量、追蹤] Aspire 要能可觀察此專案，需要增加以下服務
app.MapDefaultEndpoints();

app.Run();


