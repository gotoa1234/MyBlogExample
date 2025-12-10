using System.Xml.Linq;
using Typesense;
using Typesense.Setup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var typesenseSection = builder.Configuration.GetSection("Typesense");

// 註冊 Typesense Client
if (typesenseSection != null)
{
    var apiKey = typesenseSection.GetSection("ApiKey").Value ?? string.Empty;
    var host = typesenseSection.GetSection("Host").Value ?? string.Empty;
    var port = typesenseSection.GetSection("Port").Value ?? string.Empty;
    var protocol = typesenseSection.GetSection("Protocol").Value ?? string.Empty;

    // 註冊 Typesense Client - 最新方式
    builder.Services.AddTypesenseClient(config =>
    {
        config.ApiKey = apiKey;
        config.Nodes = new List<Node>
        {
            new Node(host, port, protocol)
        };
    }, enableHttpCompression: false); // 如果使用 Typesense Cloud 建議設為 true
}

var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
