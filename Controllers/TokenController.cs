using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;

namespace WebAPI_JWT_AuthenticationDemo.Controllers
{
    public class TokenController : Controller
    {
        private IConfiguration Configuration;
        public TokenController(IConfiguration config)
        {
            Configuration = config;
        }
        public IActionResult CreateToken(string username="admin",string password="admin")
        {
            //In real example use LoginModel ,this is just for dummy purpose so that
            //we can focus on relevant code

            IActionResult response = Unauthorized();
            if(username.Equals(password))
            {
                //create jwt token here and send it with response
                var jwttoken = JwtTokenBuilder();

                response = Ok(new { access_token=jwttoken});
            }

            return response;
        }

        private string JwtTokenBuilder()
        {
            //prepare key and credentials

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(Configuration["JWT:key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(issuer:Configuration["JWT:issuer"],
                audience:Configuration["JWT:audience"],signingCredentials:credentials,
                expires:DateTime.Now.AddMinutes(10)
                );
            


            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}