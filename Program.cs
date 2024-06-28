using System.Security.Cryptography;
using collabzone.DBAccess.Context;
using collabzone.DBAccess.Repositories;
using collabzone.Hubs;
using collabzone.Models;
using collabzone.Repositories;
using collabzone.Services;
using collabzone.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var frontendOrigin = builder.Configuration["FrontendOriginSettings:FrontendOrigin"];
var frontTest = builder.Configuration["FrontendOriginSettings:FrontendOriginTest"];

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

// Registering settings
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<FrontendOriginSettings>(builder.Configuration.GetSection("FrontendOriginSettings"));

builder.Services.AddControllers();
builder.Services.AddSignalR();

//Users controller services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IVerificationTokenRepository, VerificationTokenRepository>();
builder.Services.AddTransient<EmailService>();

//Images controller services
builder.Services.AddScoped<IImageRepository, ImageRepository>();

//Projects controller services
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUsersProjectRepository, UsersProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Tasks controller services
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

//Confiugre CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder
                .WithOrigins(frontTest)
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

app.MapHub<ProjectChatHub>("/projectChatHub");

app.Run();