using ChatApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �K�[gRPC 
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<ChatApp.ChatService.ChatServiceClient>();

// 2. ���o��egRPC Https�s�u�t�m
IConfigurationRoot baseBuilderData = new ConfigurationBuilder()    
    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"), optional: true, reloadOnChange: true)
    .Build();

// 3. �]�w��eGrpc�s����]�w����
string apiUrls = baseBuilderData["profiles:NetCoreGRPCChattingRoomExample:applicationUrl"];
if (apiUrls != null)
{
    var splitApi = apiUrls.Split(';').Where(item => item.ToLower().Contains("https://")).Select(item => item).FirstOrDefault();
    NetCoreGRPCChattingRoomExample.Controllers.GlobalConst.Self_GRPC_URL = splitApi ?? "";

    builder.Services.AddGrpcClient<ChatService.ChatServiceClient>(options =>
    {
        options.Address = new Uri(NetCoreGRPCChattingRoomExample.Controllers.GlobalConst.Self_GRPC_URL); // Your gRPC server address
    });
}

var app = builder.Build();
app.MapGrpcService<NetCoreGRPCChattingRoomExample.Service.ChatService>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// 4-1. ��{���w�V�A��gRPC Server �P Web Server ���P��port
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("content-type") &&
        context.Request.Headers["content-type"].ToString().StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase)
        )
    {
        // 4-2. �p�G���O gRPC �ШD�A���� HTTPS ���w�V
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
