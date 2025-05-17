using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PropertyControl.Commons;
using PropertyControl.Databases.InitDb;
using PropertyControl.Extensions;
using PropertyControl.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<StartupState>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDbInitializer, DbInitializer>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDataContext(builder.Configuration);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(
                    builder.Configuration["JWTSetting:SecretKey"]!
                )
            ),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWTSetting:Issuer"]!,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWTSetting:Audience"],

        };
    });
builder.Services.AddAuthorization();

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument();

var app = builder.Build();

StartupState.Instance.WebHostEnvironment = app.Environment;
StartupState.Instance.Configuration = app.Configuration;

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    await dbInitializer.Initialize(); // or .Seed()
}

app
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.Run();