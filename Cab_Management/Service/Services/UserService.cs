﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;
using Service.Inteface;
using Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Service.Services
{
    public class UserService : IUserDetails
    {
        private readonly DbCabServicesContext _dbContext;
        private readonly IEncrypt _encrypt;
        private readonly IConfiguration _configuration;

        public UserService(DbCabServicesContext dbcontext, IEncrypt encrypt,IConfiguration configuration)
        {
            _dbContext = dbcontext;
            _encrypt = encrypt;
            _configuration = configuration;
        }
        public List<TbUser> GetUsersDetails()
        {
            List<TbUser> users = _dbContext.TbUsers.ToList();
            return users;
        }

        public bool CheckExtistUser(TbUser tblUser)
        {
            var Email = _dbContext.TbUsers.Where(x => x.EmailId == tblUser.EmailId).FirstOrDefault();
            if (Email == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckExtistUser(Login login)
        {
            var Email = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId).FirstOrDefault();
            if (Email == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckConfirmPassword(Login login)
        {
            if (login.Password == login.ConfirmPassword)
            {
                return true;
            }
            return false;
        }

        public bool Register(TbUser tblUser)
        {
            tblUser.Password = _encrypt.EncodePasswordToBase64(tblUser.Password);
            tblUser.CreateDate = DateTime.Now;
            tblUser.UpdateDate = null;
            tblUser.Status = 1;
            _dbContext.TbUsers.Add(tblUser);
            _dbContext.SaveChanges();
            return true;
        }

        public string UserLogin(Login login)
        {
            TbUser Userlogin = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId && x.Password == _encrypt.EncodePasswordToBase64(login.Password)).FirstOrDefault()!;
            if (Userlogin != null)
            {
                var token = GenerateToken(Userlogin);
                return token;
            }
            else
            {
                return "User EmailId or Password not matched";
            }
        }
        private string GenerateToken(TbUser user)
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
        public bool ForgotPassword(Login login)
        {
            TbUser UserEmail = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId).SingleOrDefault();
            UserEmail.Password = _encrypt.EncodePasswordToBase64(login.Password);
            UserEmail.UpdateDate = DateTime.Now;
            _dbContext.Entry(UserEmail).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }
    }
}
    


