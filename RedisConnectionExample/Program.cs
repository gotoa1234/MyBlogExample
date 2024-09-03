using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// �`�JRedis�s���r�Ŧ�
var redisConnectionString = builder.Configuration.GetSection("ConnectionStrings:RedisDb").Value;
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    // ���~
    //return ConnectionMultiplexer.Connect((redisConnectionString));
    
    // ���T
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
