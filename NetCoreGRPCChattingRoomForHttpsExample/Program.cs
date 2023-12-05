using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 添加gRPC 
builder.Services.AddGrpc();

// 2. 取得當前gRPC Https連線配置
IConfigurationRoot baseBuilderData = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"), optional: true, reloadOnChange: true)
    .Build();

// 3. 啟用 Kestrel Server ，並且設定 IP 這邊為了閱讀使用HardCode，建議放進 appsetting.json 中
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Self_GRPC_URL = "https://localhost:50051";
builder.WebHost.UseKestrel(options =>
{
    // 3. 配置 Web Endpoint（HTTP/1.1） => 連網站用
    options.Listen(IPAddress.Any, 5099, listenOptions => { });

    // 4-1. 配置Https 
    options.Listen(IPAddress.Any, 50051, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        // 4-2. 開啟Https 並且指向Server 的 HTTPS 憑證位置 
        listenOptions.UseHttps(@"/etc/nginx/certificate.pfx");        
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

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

//5. 基本的Grpc注入使用服務
app.UseEndpoints(endpoints =>
{    
    endpoints.MapGrpcService<NetCoreGRPCChattingRoomForHttpsExample.Service.ChatService>();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
