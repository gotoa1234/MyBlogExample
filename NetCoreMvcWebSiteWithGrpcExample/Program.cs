using NetCoreMvcWebSiteWithGrpcExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. �إ�Grpc�q�H
builder.Services.AddGrpc();

var app = builder.Build();

//2. �[�J�]�w�ɪ�Service ����ĳ�b������ middlewareĲ�o�e
app.MapGrpcService<MyGrpcTesterService>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//3-1. �ˬd�O�_��GRPC���ШD�A�O���ܫh���i�� HTTPS ���w�V
app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("content-type") &&
        context.Request.Headers["content-type"].ToString().StartsWith("application/grpc", StringComparison.OrdinalIgnoreCase)
        )
    {
        // �p�G���O gRPC �ШD�A���� HTTPS ���w�V
        context.Request.Scheme = "https";
        await next();
    }
    else
    {
        await next();
    }
});

//3-2. �p�G���]�w3-1. �o�q�|��GRPC�L�k�q�H�A�]��GRPC�����OHTTP/2 ��ĳ
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
