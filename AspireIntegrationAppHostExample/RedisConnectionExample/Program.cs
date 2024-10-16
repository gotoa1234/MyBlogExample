using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ª`¤JRedis³s±µ¦r²Å¦ê
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

app.Run();
