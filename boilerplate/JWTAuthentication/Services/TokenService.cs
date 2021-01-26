using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using JWTAuthentication.Helpers;

//Ver Refresh Token
//https://renatogroffe.medium.com/asp-net-core-3-1-jwt-refresh-tokens-exemplo-de-implementa%C3%A7%C3%A3o-cc8a7cbf69db
namespace JWTAuthentication.Services
{
    public class TokenService {

        public static string GenerateToken(User user) {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JWTSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}