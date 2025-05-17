using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using PropertyControl.Commons.Schemas;

namespace PropertyControl.Commons
{
    public class Security
    {
        public static string GenerateJWTCode(JwtData data)
        {
            var now = DateTime.Now;
            var configuration = StartupState.Instance.Configuration.GetSection("JWTSetting");
            // Khởi tạo Claim
            var claims = new Claim[] {
                new Claim ("UserId", data.UserId.ToString()),
                new Claim ("Username", data.Username),
                new Claim ("RoleId", data.RoleId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, data.Username),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim (JwtRegisteredClaimNames.Iat, now.ToUniversalTime ().ToString (), ClaimValueTypes.Integer64)
            };

            // Khởi tạo SymmetricSecurityKey
            var symmetricKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["SecretKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Issuer = configuration["Issuer"],
                Audience = configuration["Audience"],
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha512Signature)
            };
            
            var tokenHandler= new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}