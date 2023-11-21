using NetCoreMvcWebSiteWithGrpcExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. 建立Grpc通信
builder.Services.AddGrpc();

var app = builder.Build();

//2. 加入設定檔的Service ※建議在中間件 middleware觸發前
app.MapGrpcService<MyGrpcTesterService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//3-1. 檢查是否為GRPC的請求，是的話則不進行 HTTPS 重定向
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

//3-2. 如果不設定3-1. 這段會讓GRPC無法通信，因為GRPC走的是HTTP/2 協議
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
