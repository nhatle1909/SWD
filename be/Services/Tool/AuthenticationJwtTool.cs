using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Repositories.Model;
using Repositories.ModelView;
using Repositories.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Bcpg.Attr.ImageAttrib;

namespace Services.Tool
{
    public class AuthenticationJwtTool
    {
        private readonly IConfiguration _configuration;
        //private IUnitOfWork _unit;
        public AuthenticationJwtTool(IConfiguration configuration)
        {
            _configuration = configuration;
            //_unit = unit;
        }
        public string GenerateJwtToken(string userId, string role)
        {
            var jwtKey = _configuration["JWT:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? ""));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id",userId),
                new Claim("role", role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["JWT:Issure"],
                _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signingCredentials
                );
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            //var refreshToken = GenerateRefreshToken();
            ////save database
            //var refreshTokenEntity = new RefreshToken
            //{
            //    RefreshTokenId = ObjectId.GenerateNewId().ToString(),
            //    AccountId = userId,
            //    Refresh_Token = refreshToken,
            //    JwtId = accessToken,
            //    IsUsed = false,
            //    IsRevoked = false,
            //    IssuedAt = DateTime.Now,
            // ExpiredAt = DateTime.Now.AddMonths(1)
            //};
            //await _unit.RefreshTokenRepo.AddOneItem(refreshTokenEntity);
            return accessToken;
        }

        //public async Task<string> RenewToken(string refreshToken, string accessToken)
        //{
        //    var jwtKey = _configuration["JWT:Key"];
        //    var jwtTokenhandler = new JwtSecurityTokenHandler();
        //    var secretKeyBytes = Encoding.UTF8.GetBytes(jwtKey ?? "");
        //    var tokenValidateParams = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? throw new ArgumentNullException("builder.Configuration[\"Jwt:Key\"]", "Jwt:Key is null"))),
        //        ValidIssuer = _configuration["JWT:Issure"],
        //        ValidAudience = _configuration["JWT:Audience"],
        //        ValidateLifetime = false//ko kiem tra 
        //    };
        //    try
        //    {
        //        //check 1: access token valid format
        //        var tokenInverification = jwtTokenhandler.ValidateToken(accessToken, tokenValidateParams, out var validatedToken);
        //        //check 2: check Algorithm
        //        if(validatedToken is JwtSecurityToken jwtSecurityToken)
        //        {
        //            var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
        //                                                            StringComparison.InvariantCultureIgnoreCase);
        //            if (!result)
        //            {
        //                return "Invalid token";
        //            }
        //        }
        //        //check 3: check access token expire
        //        var utcExpireDate = long.Parse(tokenInverification.Claims.FirstOrDefault(x => 
        //                                       x.Type == JwtRegisteredClaimNames.Exp).Value);
        //        var expireDate = ConvertUnitTimeToDateTime(utcExpireDate);
        //        if (expireDate > DateTime.Now)
        //        {
        //            return "Access token have not expired!";
        //        }
        //        //check 4: check refresh token exist in DB
        //        var storedToken = (await _unit.RefreshTokenRepo.GetFieldsByFilterAsync([],
        //                                 s => s.Refresh_Token.Equals(refreshToken))).FirstOrDefault();
        //        if (storedToken is null)
        //        {
        //            return "Refresh token does not exist!";
        //        }
        //        //check 5: check refresh token isUsed or isRevoke
        //        if (storedToken.IsUsed)
        //        {
        //            return "Refresh token has been used!";
        //        }
        //        if (storedToken.IsRevoked)
        //        {
        //            return "Refresh token has been revoked!";
        //        }
        //        //check 6: check model.AccessToken == JwtId in RefreshToken Model
        //        //var jti = tokenInverification.Claims.FirstOrDefault(x => x.Type ==
        //        //                    JwtRegisteredClaimNames.Jti).Value;
        //        if (storedToken.JwtId != accessToken)
        //        {
        //            return "Access Token does not match!";
        //        }
        //        //Update token is used
        //        storedToken.IsUsed = true;
        //        storedToken.IsRevoked = true;
        //        await _unit.RefreshTokenRepo.UpdateItemByValue("RefreshTokenId", storedToken.RefreshTokenId, storedToken);
        //        //create new token
        //        var getUserStatus = (await _unit.AccountStatusRepo.GetFieldsByFilterAsync(["IsRole"],
        //                    g => g.AccountId.Equals(storedToken.AccountId))).FirstOrDefault();
        //        var token = await GenerateJwtToken(storedToken.AccountId, getUserStatus.IsRole.ToString());
        //        return token;
        //    }
        //    catch (Exception)
        //    {
        //        return "Something went wrong";
        //    }
        //}

        private DateTime ConvertUnitTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public static string GetUserIdFromJwt(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var token = tokenHandler.ReadJwtToken(jwtToken);
                var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
                return idClaim?.Value ?? "Can not get id from token";
            }
            catch (ArgumentException)
            {
                return "Invalid JWT token format";
            }
        }
    }
}
