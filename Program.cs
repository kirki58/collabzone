using System.Security.Cryptography;
using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var frontendOrigin = builder.Configuration["FrontendOrigin"];

//This is for development purposes only. In production, you should use a secure method to store the key.
if(!File.Exists("key")){
    var rsa = RSA.Create();
    var privateKey = rsa.ExportRSAPrivateKey();
    File.WriteAllBytes("key", privateKey);
}

var rsaKey = RSA.Create();
rsaKey.ImportRSAPrivateKey(File.ReadAllBytes("key"), out _);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new RsaSecurityKey(rsaKey),
        ValidateIssuer = true,
        ValidIssuer = "https://localhost:7217",
        ValidateAudience = true,
        ValidAudience = "https://localhost:7217",
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
                .WithOrigins(frontendOrigin)
               .AllowAnyHeader() 
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

//Also for development purposes only. In production, you should use a secure method to store the connection strin.
builder.Services.AddDbContext<CZContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();