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

// 3. 啟用 Kestrel Server ，內部Server指向本地的https
int httpsPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpsPort", 50051);
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Self_GRPC_URL = @$"https://localhost:{httpsPort}";
var certPath = builder.Configuration.GetValue<string>("CertPath", string.Empty);
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Cert_Path = certPath;
builder.WebHost.UseKestrel(options =>
{
    // 4. 配置 Web Endpoint（HTTP/1.1） => 連網站用
    var httpPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpPort", 5099);   
    options.Listen(IPAddress.Any, httpPort, listenOptions => { });


    // 5. 配置Https + 使用 Server 上的產生Https憑證 (此pfx 是該Server域名產生，所以Local Debug啟動會異常)
    var httpsPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpsPort", 50051);
    var pfxFile = builder.Configuration.GetValue<string>("PfxPath", string.Empty);
    options.Listen(IPAddress.Any, httpsPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        // 5-2. 開啟Https 並且指向Server 的 HTTPS 憑證位置 
        listenOptions.UseHttps(pfxFile);        
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
