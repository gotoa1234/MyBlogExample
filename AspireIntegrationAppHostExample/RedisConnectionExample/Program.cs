using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// 1. [�[�J���c�ơB�p�q�B�l��] Aspire �n��i�[��M�סA�ݭn�W�[�H�U�A��
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddControllersWithViews();

// �`�JRedis�s���r�Ŧ�
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

//// 2. [�[�J���c�ơB�p�q�B�l��] Aspire �n��i�[��M�סA�ݭn�W�[�H�U�A��
app.MapDefaultEndpoints();

app.Run();

