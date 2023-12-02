using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[gRPC 
builder.Services.AddGrpc();
builder.Services.AddCors();  // 1. �W�[ CORS �A��

// 2. ���o��egRPC Https�s�u�t�m
IConfigurationRoot baseBuilderData = new ConfigurationBuilder()
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"), optional: true, reloadOnChange: true)
    .Build();

// 3. �ҥ� Kestrel Server �A�åB�]�w IP �o�䬰�F�\Ū�ϥ�HardCode�A��ĳ��i appsetting.json ��
NetCoreGRPCChattingRoomForHttpExample.Controllers.GlobalConst.Self_GRPC_URL = "http://localhost:50051";
builder.WebHost.ConfigureKestrel((context, options) =>
{
    // 3. �t�m Web Endpoint�]HTTP/1.1�^ => �s������
    options.Listen(IPAddress.Any, 5099, listenOptions => { });

    // 4. �t�m gRPC Endpoint�]HTTP/2�^ => �sgRPC��
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

// 5. ��{���w�V�A��gRPC Server �P Web Server ���P��port
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
// 6. �ҥθ��]�w�A��Http�����p�U�A�K��ϥ�
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
    // 7. �ϥΤ��w���q�D
    endpoints.MapGrpcService<NetCoreGRPCChattingRoomForHttpExample.Service.ChatService>().RequireCors("AllowAll");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
