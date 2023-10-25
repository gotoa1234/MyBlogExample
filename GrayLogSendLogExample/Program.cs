//1. �ޥ�
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;

var builder = WebApplication.CreateBuilder(args);

//2. builder ��إ�Log
Log.Logger = new LoggerConfiguration()        
    .MinimumLevel.Information()//�̤p�h�ŬOInfomation    
    //4-1. �إߥH���ŰϤ�����Ƨ��A�åB���O�ΤѼƨӰO��
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
            .WriteTo.File(@"Logs\Info\Info.log", rollingInterval: RollingInterval.Day))
    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
            .WriteTo.File(@"Logs\Error\Error.log", rollingInterval: RollingInterval.Day))    
    .WriteTo.Graylog(new GraylogSinkOptions()//��X�� Graylog
    {
        HostnameOrAddress = builder.Configuration.GetSection("Graylog:Host").Value,
        Port = int.Parse(builder.Configuration.GetSection("Graylog:Port").Value),
    })
    .CreateLogger();

//3. �g�ILog���e
Log.Information($@"MilkTeaGreen Test GrayLogSendLogExample Log");

//4-2. Information ���a
Log.Information($@"MilkTeaGreen Test GrayLogSendLogExample Log => INF");
//4-3. Error ���a
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