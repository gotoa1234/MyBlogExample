using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 注入Redis連接字符串
var redisConnectionString = builder.Configuration.GetSection("ConnectionStrings:RedisDb").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    // 錯誤
    //return ConnectionMultiplexer.Connect((redisConnectionString));
    
    // 正確
    return ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(redisConnectionString));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
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
