using System;
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
using Microsoft.AspNetCore.Mvc;

namespace Service.Services
{
    public class UserService : IUserDetails,IPagination
    {
        private readonly DbCabServicesContext _dbContext;
        private readonly IEncrypt _encrypt;
        private readonly IGenerateToken _generateToken;
        public UserService(DbCabServicesContext dbcontext, IEncrypt encrypt, IGenerateToken generateToken)
        {
            _dbContext = dbcontext;
            _encrypt = encrypt;
            _generateToken = generateToken;
        }

        public List<TbUser> GetUsersDetails()
        {
            List<TbUser> users = _dbContext.TbUsers.ToList();
            return users;
        }
        public IQueryable<TbUser> FindAll()
        {
            return this._dbContext.Set<TbUser>()
                .AsNoTracking();
        }
        public PagedList<TbUser> GetUserbyCreateDate(PaginationParameters paginationParameters)
        {
            bool isDescending= paginationParameters.IsDescending;
            if (isDescending == true)
            {
                return PagedList<TbUser>.ToPagedList(FindAll().OrderByDescending(on => on.CreateDate),
                                paginationParameters.PageNumber,
                                paginationParameters.PageSize);
            }
            else
            {
                return PagedList<TbUser>.ToPagedList(FindAll().OrderBy(on => on.CreateDate),
                    paginationParameters.PageNumber,
                    paginationParameters.PageSize);
            }
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
                var token = _generateToken.GenerateToken(Userlogin);
                return token;
            }
            else
            {
                return "User EmailId or Password not matched";
            }
        }
        public bool ForgotPassword(Login login)
        {
            TbUser UserEmail = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId).SingleOrDefault()!;
            UserEmail.Password = _encrypt.EncodePasswordToBase64(login.Password);
            UserEmail.UpdateDate = DateTime.Now;
            _dbContext.Entry(UserEmail).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }
    }
}
    


