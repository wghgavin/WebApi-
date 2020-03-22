using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyWebApiProject.AuthHelper
{
    public class JwtToken
    {
        public static dynamic BuildJwtToken(Claim[] claim, PermissionRequirement permissionRequirement)
        {
            var now = DateTime.Now;
            //实例化jwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer:permissionRequirement.Issuer,
                audience:permissionRequirement.Audience,
                claims:claim,
                notBefore:now,
                expires:now.Add(permissionRequirement.Expiration),
                signingCredentials:permissionRequirement.SigningCredentials
                );
            //生产Token
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //打包返回前台
            var responseJson = new
            {
                success = true,
                token = encodedJwt,
                expires_in = permissionRequirement.Expiration.TotalSeconds,
                token_type = "Bearer"
            };
            return responseJson;
        }
    }
}
