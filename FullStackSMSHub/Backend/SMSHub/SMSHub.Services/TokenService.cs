using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SMSHub.Core.Entities.Identity;
using SMSHub.Core.Services;

namespace SMSHub.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser>userManager)
        {
          //token 3bara 3n header,payload,keys
          //payload>>register claim or private claim
          //create private claim [user - defined]>> 
          var AuthClaims = new List<Claim>()//claim>> properties of user like name,email,pass
          {
            //object of claim class
            new Claim(ClaimTypes.GivenName,user.DisplayName),
            new Claim(ClaimTypes.Email,user.Email), 
          };
            //bzwed s3obet eltoken by adding roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role,role));
            }
            //key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));//btreturn byte f h3ml encoding
            //object of token not token
            var token = new JwtSecurityToken(
                    //registered claim>>need some properties, i will put them in appsetting
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                    claims: AuthClaims,
                    signingCredentials: new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature)

                ) ;
            //func that returns token
            return new JwtSecurityTokenHandler().WriteToken(token) ;
        }
    }
}
