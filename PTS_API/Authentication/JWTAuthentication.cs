using Microsoft.IdentityModel.Tokens;
using PTS_CORE.Domain.DataTransferObject.RequestModel.Account;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PTS_API.Authentication
{
    public class JWTAuthentication : IJWTAuthentication
    {
        public IConfiguration _configuration;
        private readonly string _key;
        public JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
         public JWTAuthentication(IConfiguration configuration)
        {          
            _configuration = configuration;
            _key = _configuration["Authentication:Key"];
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateToken(ApplicationUserDto model)
        {

            var tokenKey = Encoding.UTF8.GetBytes(_key);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, model.Email));
            claims.Add(new Claim(ClaimTypes.Role, model.RoleName));     
          //  claims.Add(new Claim(ClaimTypes.GivenName, $"{model.FirstName} {model.LastName}"));     

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                   // Issuer = _configuration["Authentication:Issuer"],
                    //Audience = _configuration["Authentication:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,//you might want to validate audience and issuer depending on the use cases
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)),
                ValidateLifetime = false //this means we dont care about token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var pricipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");
            return pricipal;
        }

        public void TokenValidatorHandler(string tokenInput)
        {
            var key = _key;
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var principal = tokenHandler.ValidateToken(tokenInput, tokenValidationParameters, out var validatedToken);
        }
    }
}
