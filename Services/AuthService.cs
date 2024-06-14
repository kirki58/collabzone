using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using collabzone.Models;
using Microsoft.IdentityModel.Tokens;

namespace collabzone.Services;

public class AuthService
{
    public string GenerateToken(User user){
        var rsaKey = RSA.Create();
        rsaKey.ImportRSAPrivateKey(File.ReadAllBytes("key"), out _);

        var handler = new JwtSecurityTokenHandler();
        var credentials = new SigningCredentials(new RsaSecurityKey(rsaKey), SecurityAlgorithms.RsaSha256);

        var token = handler.CreateToken(new SecurityTokenDescriptor(){
            Audience = "https://localhost:7217",
            Issuer = "https://localhost:7217",
            Subject = GetClaims(user),
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddMinutes(30)
        });
        return handler.WriteToken(token);
    }
    private static ClaimsIdentity GetClaims(User user){
        return new ClaimsIdentity(new Claim[]{
            new Claim("sub", user.Id.ToString()),
            new Claim("name", user.Name)
        });
    }

}
