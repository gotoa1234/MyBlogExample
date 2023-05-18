using InjectReflectionForTranslateLanguageExample.Implement;
using InjectReflectionForTranslateLanguageExample.Implement.Nation;
using InjectReflectionForTranslateLanguageExample.Interface;
using InjectReflectionForTranslateLanguageExample.Interface.Nation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 注入Interface對應實例
builder.Services.AddScoped<ILanguageService, LanguageService>();
builder.Services.AddScoped<INationAmericaService, NationAmericaService>();
builder.Services.AddScoped<INationChinaService, NationChinaService>();
builder.Services.AddScoped<INationJapanService, NationJapanService>();
builder.Services.AddScoped<INationGermanyService, NationGermanyService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
