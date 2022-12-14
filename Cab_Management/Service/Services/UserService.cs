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
        
        public List<UserView> GetUsersDetail()
        {
            List<UserView> users = _dbContext.UserViews.ToList();
            return users;
        }

        public bool ConfirmPassword(Registration tblUser)
        {
            return tblUser.Password == tblUser.ConfirmPassword;
        }

        public bool CheckExtistUser(Registration login)
        {
            var Email = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId).FirstOrDefault();
            if (Email == null)
            {
                return false;
            }
            return true;
     
        }
        public string Register(TbUser tblUser)
        {
            tblUser.Password = _encrypt.EncodePasswordToBase64(tblUser.Password);
            tblUser.CreateDate = DateTime.Now;
            tblUser.UpdateDate = null;
            tblUser.Status = 1;
            _dbContext.TbUsers.Add(tblUser);
            _dbContext.SaveChanges();
            var token = _generateToken.GenerateToken(tblUser);
            return token;
        }

        public Tuple<string, int> UserLogin(TbUser login)
        {
            TbUser Userlogin = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId && x.Password == _encrypt.EncodePasswordToBase64(login.Password)).FirstOrDefault()!;
            if (Userlogin != null)
            {
                //var token = GenerateToken(Userlogin);
                var token = _generateToken.GenerateToken(Userlogin);
                Tuple<string, int> id = new Tuple<string, int>(token, Userlogin.UserId);
                return id;
            }
            else
            {
                return null!;
            }
        }
        public bool ForgotPassword(ForgetPassword login)
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
    


