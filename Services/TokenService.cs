using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi.Models;
using Microsoft.IdentityModel.Tokens;

namespace webapi.Services{
    public static class TokenService{
        public static string GenerateToken(User user){

            var tokenHandler  = new JwtSecurityTokenHandler();
            //minha chave
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            //descrição do que terá dentro do token
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials =  new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
