using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NetCoreSwaggerJWTExample;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// 1. 增加右上角Bearer token
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "JWT Authentication",
        Description = "API如果不是[AllowAnonymous] 需要先執行Member/Login 取得JWT Token",
    });
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "驗證使用方式：Authentication輸入。 格式：[Bearer 您的Token]",
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }});
    //啟用詳細描述
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

//2-1. 建立JwtBearer
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //2-2. 配置：驗證錯誤時是否顯示錯誤資訊
    options.IncludeErrorDetails = true;
    //2-3. 配置：Swagger的Bearer的自動驗證
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // 2-4. 配置：驗證 Issuer
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

        // 2-5. 配置：驗證 Audience
        ValidateAudience = false,
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"), // 不驗證就不需要填寫

        // 2-6. 配置：驗證 Token憑證
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey"))),

        // 2-7. 配置：驗證 Token 有效時限
        ValidateLifetime = true
    };
});

//3. 註冊 Json Web Token產生器
builder.Services.AddScoped<IJsonWebTokenService, JsonWebTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

//4. 添加驗證，Attribute => [Authorize]
app.UseAuthorization();

app.MapControllers();

app.Run();
