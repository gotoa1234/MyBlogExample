using Microsoft.AspNetCore.HttpOverrides;
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // 1. 添加 ForwardedHeaders 
    var forwardedHeadersOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        ForwardLimit = 5
    };

    // 明確清空預設值
    forwardedHeadersOptions.KnownProxies.Clear();
    forwardedHeadersOptions.KnownNetworks.Clear();

    app.UseForwardedHeaders(forwardedHeadersOptions);

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
