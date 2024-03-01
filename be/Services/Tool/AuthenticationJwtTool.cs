using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace Services.Tool
{
    public class AuthenticationJwtTool
    {
        private readonly IConfiguration _configuration;
        public AuthenticationJwtTool(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(string userId, float hour)
        {
            var jwtKey = _configuration["JWT:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? ""));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("_id",userId)
            };

            var token = new JwtSecurityToken(
                _configuration["JWT:Issure"],
                _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(hour),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GetUserIdFromJwt(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.ReadJwtToken(jwtToken);
                var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == "_id");
                return idClaim?.Value ?? "Can not get id from token";
            }
            catch (ArgumentException)
            {
                return "Invalid JWT token format";
            }
        }
    }
}
