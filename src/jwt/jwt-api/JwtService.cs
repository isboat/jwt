using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt_api
{
    public class JwtService : IJwtService
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;
        private readonly string _jwtSigningKey;

        public JwtService()
        {
            _jwtSecurityTokenHandler = new();

            _jwtIssuer = "http://mysite.com";
            _jwtAudience = "http://myaudience.com";
            _jwtSigningKey =  "asdv234234^&%&^%&^hjsdfb2%%%";
        }

        public string GenerateToken()
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSigningKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("clientid", "123456"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _jwtIssuer,
                Audience = _jwtAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public void ValidateToken(string token)
        {
            _ = _jwtSecurityTokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidIssuer = _jwtIssuer,
                    ValidAudience = _jwtAudience,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSigningKey))
                }, out _);
        }

        public JwtSecurityToken ReadToken(string token)
        {
            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
            return jwtToken;
        }
    }

    public interface IJwtService
    {
        string GenerateToken();

        void ValidateToken(string token);

        JwtSecurityToken ReadToken(string token);
    }
}
