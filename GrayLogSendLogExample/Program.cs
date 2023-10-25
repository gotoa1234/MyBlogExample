//1. まノ
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;

var builder = WebApplication.CreateBuilder(args);

//2. builder ミLog
Log.Logger = new LoggerConfiguration()        
    .MinimumLevel.Information()//程糷琌Infomation    
    //4-1. ミ单跋だ戈Ж常琌ノぱ计ㄓ癘魁
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
            .WriteTo.File(@"Logs\Info\Info.log", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
            .WriteTo.File(@"Logs\Error\Error.log", rollingInterval: RollingInterval.Day))    
    .WriteTo.Graylog(new GraylogSinkOptions()//块 Graylog
    {
        HostnameOrAddress = builder.Configuration.GetSection("Graylog:Host").Value,
        Port = int.Parse(builder.Configuration.GetSection("Graylog:Port").Value),
    })
    .CreateLogger();

//3. 糶翴Logず甧
Log.Information($@"MilkTeaGreen Test GrayLogSendLogExample Log");

//4-2. Information セ
Log.Information($@"MilkTeaGreen Test GrayLogSendLogExample Log => INF");
//4-3. Error セ
Log.Error($@"MilkTeaGreen Test GrayLogSendLogExample Log => ERR");

// Add services to the container.
builder.Services.AddControllersWithViews();

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