using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Service.Inteface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GenerateTokenServices : IGenerateToken
    {
        private readonly DbCabServicesContext _dbContext;
        private readonly IConfiguration _configuration;
        public GenerateTokenServices(DbCabServicesContext dbcontext, IConfiguration configuration)
        {
            _dbContext = dbcontext;
            _configuration = configuration;
        }
        public string GenerateToken(TbUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            TbUserRole role = _dbContext.TbUserRoles.Where(x => x.UserRoleId == user.UserRoleId).FirstOrDefault()!;
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.EmailId!),
                 new Claim(ClaimTypes.NameIdentifier,user.Password!),
                 new Claim(ClaimTypes.Role,role.UserRoleName!)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
