using Microsoft.AspNetCore.HttpOverrides;
try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // 1. �K�[ ForwardedHeaders 
    var forwardedHeadersOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        ForwardLimit = 5
    };

    // ���T�M�Źw�]��
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
