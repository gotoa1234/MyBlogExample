using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 添加gRPC 
builder.Services.AddGrpc();
builder.Services.AddCors();  // 1. 增加 CORS 服務

// 2. 取得當前gRPC Https連線配置
IConfigurationRoot baseBuilderData = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"), optional: true, reloadOnChange: true)
    .Build();

// 3. 啟用 Kestrel Server ，並且設定 IP 這邊為了閱讀使用HardCode，建議放進 appsetting.json 中
NetCoreGRPCChattingRoomForHttpExample.Controllers.GlobalConst.Self_GRPC_URL = "http://localhost:50051";
builder.WebHost.ConfigureKestrel((context, options) =>
{
    // 3. 配置 Web Endpoint（HTTP/1.1） => 連網站用
    options.Listen(IPAddress.Any, 5099, listenOptions => { });

    // 4. 配置 gRPC Endpoint（HTTP/2） => 連gRPC用
    options.Listen(IPAddress.Any, 50051, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});


var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// 5. 實現重定向，讓gRPC Server 與 Web Server 走同個port
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("content-type") &&
        context.Request.Headers["content-type"].ToString().StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase)
        )
    {
        context.Request.Scheme = "https";
        await next();
    }
    else
    {
        await next();
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// 6. 啟用跨域設定，走Http的情況下，便於使用
app.UseCors(builder =>
{    
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});


app.UseAuthorization();

// Configure the HTTP request pipeline.
app.UseEndpoints(endpoints =>
{
    // 7. 使用不安全通道
    endpoints.MapGrpcService<NetCoreGRPCChattingRoomForHttpExample.Service.ChatService>().RequireCors("AllowAll");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
