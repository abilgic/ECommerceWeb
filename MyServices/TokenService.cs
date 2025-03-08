using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceWebApi.myServices
{
    public static class TokenService
    {
        public static string GenerateToken(string jwtKey, DateTime expires, IEnumerable<Claim> claims, string issuer = "http://localhost:5024", string audience = "ECommerceWebApi")
        {
            byte[] key = Encoding.UTF8.GetBytes(jwtKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //List<Claim> claims = new List<Claim>
            //        {
            //            new Claim("id", account.Id.ToString()),
            //            new Claim(ClaimTypes.Name, account.Username),
            //            new Claim("type", account.Type.ToString()),
            //            new Claim(ClaimTypes.Role, account.Type.ToString())
            //        };
            JwtSecurityToken token = new JwtSecurityToken(issuer, audience, claims, expires: DateTime.Now.AddDays(30), signingCredentials: credentials
                );
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
