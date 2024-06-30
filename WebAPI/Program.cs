using Application.Interfaces;
using Application.Services;
using Domain.Interfaces.ConnectorRepository;
using Domain.Interfaces.EVStationRepository;
using Domain.Interfaces.PaymentRepository;
using Domain.Interfaces.PaymentTransactionRepository;
using Domain.Interfaces.RegisteredCompaniesRepository;
using Infrastructure.Data;
using Infrastructure.Mappings;
using Infrastructure.Repositories.CompaniesRepository;
using Infrastructure.Repositories.ConnectorRepository;
using Infrastructure.Repositories.EVStationRepository;
using Infrastructure.Repositories.PaymentRepository;
using Infrastructure.Repositories.RefreshTokenRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

#region MiddlewareConfigs
// For Entity Framework
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("conn") ?? throw new InvalidOperationException("Connection string not found"));
});

// For Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });
builder.Services.AddCors(options =>
{
    options.AddPolicy("defaultCors", builder =>
    {
        builder.WithOrigins("*")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(typeof(Program), typeof(MappingProfile));
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
});
#endregion

#region DI Container
// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IEVStationService, EVStationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Repositories
builder.Services.AddScoped<IEVStationRepository, EVStationRepository>();
builder.Services.AddScoped<ICompaniesRepository, CompaniesRepository>();
builder.Services.AddScoped<IConnectorDetailsRepository, ConnectorDetailsRepository>();
builder.Services.AddScoped<IConnectorStatusRepository, ConnectorStatusRepository>();
builder.Services.AddScoped<IPaymentMethodsRepository, PaymentMethodsRepository>();
builder.Services.AddScoped<IPaymentTransactionRepository, PaymentTransactions>();
builder.Services.AddScoped<IRefrehTokenRepository, RefreshTokenRepository>();
#endregion

#region MiddlewarePipe
var app = builder.Build();
app.UseHttpLogging();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("defaultCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion