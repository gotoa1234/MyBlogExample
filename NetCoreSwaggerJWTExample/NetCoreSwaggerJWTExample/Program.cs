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


// 1. �W�[�k�W��Bearer token
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "JWT Authentication",
        Description = "API�p�G���O[AllowAnonymous] �ݭn������Member/Login ���oJWT Token",
    });
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "���ҨϥΤ覡�GAuthentication��J�C �榡�G[Bearer �z��Token]",
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
    //�ҥθԲӴy�z
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    o.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
});

//2-1. �إ�JwtBearer
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    //2-2. �t�m�G���ҿ��~�ɬO�_��ܿ��~��T
    options.IncludeErrorDetails = true;
    //2-3. �t�m�GSwagger��Bearer���۰�����
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // 2-4. �t�m�G���� Issuer
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),

        // 2-5. �t�m�G���� Audience
        ValidateAudience = false,
        ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"), // �����ҴN���ݭn��g

        // 2-6. �t�m�G���� Token����
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey"))),

        // 2-7. �t�m�G���� Token ���Įɭ�
        ValidateLifetime = true
    };
});

//3. ���U Json Web Token���;�
builder.Services.AddScoped<IJsonWebTokenService, JsonWebTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

//4. �K�[���ҡAAttribute => [Authorize]
app.UseAuthorization();

app.MapControllers();

app.Run();
