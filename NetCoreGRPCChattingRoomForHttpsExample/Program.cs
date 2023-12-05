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

// 3. �ҥ� Kestrel Server �A�åB�]�w IP �o�䬰�F�\Ū�ϥ�HardCode�A��ĳ��i appsetting.json ��
NetCoreGRPCChattingRoomForHttpsExample.Controllers.GlobalConst.Self_GRPC_URL = "https://localhost:50051";
builder.WebHost.UseKestrel(options =>
{
    // 3. �t�m Web Endpoint�]HTTP/1.1�^ => �s������
    options.Listen(IPAddress.Any, 5099, listenOptions => { });

    // 4-1. �t�mHttps 
    options.Listen(IPAddress.Any, 50051, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
        // 4-2. �}��Https �åB���VServer �� HTTPS ���Ҧ�m 
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

//5. �򥻪�Grpc�`�J�ϥΪA��
app.UseEndpoints(endpoints =>
{    
    endpoints.MapGrpcService<NetCoreGRPCChattingRoomForHttpsExample.Service.ChatService>();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
