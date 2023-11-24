using ChatApp;
using NetCoreGRPCChattingRoomExample.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<ChatApp.ChatService.ChatServiceClient>();
builder.Services.AddGrpcClient<ChatService.ChatServiceClient>(options =>
{
    options.Address = new Uri("https://localhost:7184"); // Your gRPC server address
});

var app = builder.Build();
app.MapGrpcService<NetCoreGRPCChattingRoomExample.Service.ChatService>();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("content-type") &&
        context.Request.Headers["content-type"].ToString().StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase)
        )
    {
        // 如果不是 gRPC 請求，應用 HTTPS 重定向
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
