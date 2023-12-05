using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[gRPC 
builder.Services.AddGrpc();

// 2. ���o��egRPC Https�s�u�t�m
IConfigurationRoot baseBuilderData = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"), optional: true, reloadOnChange: true)
    .Build();

// 3. �ҥ� Kestrel Server �A����Server���V���a��https
int httpsPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpsPort", 50051);
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Self_GRPC_URL = @$"https://localhost:{httpsPort}";
var certPath = builder.Configuration.GetValue<string>("CertPath", string.Empty);
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Cert_Path = certPath;
builder.WebHost.UseKestrel(options =>
{
    // 4. �t�m Web Endpoint�]HTTP/1.1�^ => �s������
    var httpPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpPort", 5099);   
    options.Listen(IPAddress.Any, httpPort, listenOptions => { });


    // 5. �t�mHttps + �ϥ� Server �W������Https���� (��pfx �O��Server��W���͡A�ҥHLocal Debug�Ұʷ|���`)
    var httpsPort = builder.Configuration.GetValue<int>("Kestrel:Ports:HttpsPort", 50051);
    var pfxFile = builder.Configuration.GetValue<string>("PfxPath", string.Empty);
    options.Listen(IPAddress.Any, httpsPort, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        // 5-2. �}��Https �åB���VServer �� HTTPS ���Ҧ�m 
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

//5. �򥻪�Grpc�`�J�ϥΪA��
app.UseEndpoints(endpoints =>
{    
    endpoints.MapGrpcService<NetCoreGRPCChattingRoomForHttpsExample.Service.ChatService>();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
