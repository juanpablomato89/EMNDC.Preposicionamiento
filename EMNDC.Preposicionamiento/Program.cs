using System.Globalization;
using System.Text;
using Askmethat.Aspnet.JsonLocalizer.Extensions;
using AutoMapper;
using EMNDC.Preposicionamiento.DB;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Middlewares;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Services;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger, los nuget son Swashbuckle.AspNetCore, Swashbuckle.AspNetCore.SwaggerUI
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Telescopy",
        Version = "v1",
        Description = "API Telescopy"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter JWT Bearer authorization token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "Bearer {token}",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PreposicionamientoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentityCore<UserModel>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PreposicionamientoDbContext>()
    .AddDefaultTokenProviders();

// Configuración
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!)
            ),
            NameClaimType = "sub"
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("Telescopy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

//Todo lo relacionado con IStringLocalizer, el nuget es Askmethat.Aspnet.JsonLocalizer
builder.Services.AddControllers().AddDataAnnotationsLocalization().AddViewLocalization();
CultureInfo[] supportedCultures = new[]
    {
                        new CultureInfo("en-US"),
                        new CultureInfo("es-ES")
                };
builder.Services.AddJsonLocalization(options => options.ResourcesPath = "i18n");
builder.Services.AddLocalization(options => options.ResourcesPath = "i18n");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddSingleton(provider =>
{
    var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<MappingProfiles>();
    }, loggerFactory);

    return config.CreateMapper();
});

//Inyeccion de dependencia
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IMailKitService, MailService>();

var app = builder.Build();

app.UseRequestLocalization();
app.UseMiddleware<ErrorWrappingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}

app.UseRouting();
app.UseCors("Telescopy");

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Telescopy V1");
    });
}

app.MapControllers();
app.Run();