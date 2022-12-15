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

namespace Service.Services
{
    public class UserService : IUserDetails
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
            return Email != null;
        }
        public string Register(TbUser tblUser)
        {
            tblUser.Password = _encrypt.EncodePasswordToBase64(tblUser.Password);
            tblUser.CreateDate = DateTime.Now;
            tblUser.UpdateDate = null;
            tblUser.Status = 1;
            _dbContext.TbUsers.Add(tblUser);
            _dbContext.SaveChanges();
            return "Registration Successfully";
        }
        public Tuple<string, int> UserLogin(TbUser login)
        {
            TbUser Userlogin = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId && x.Password == _encrypt.EncodePasswordToBase64(login.Password)).FirstOrDefault()!;
            if (Userlogin != null)
            {
                var token = _generateToken.GenerateToken(Userlogin);
                Tuple<string, int> id = new Tuple<string, int>(token, Userlogin.UserId);
                return id;
            }
            else
            {
                return null!;
            }
        }
        public void ForgotPassword(ForgetPassword login)
        {
                TbUser UserEmail = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId).SingleOrDefault()!;
                UserEmail.Password = _encrypt.EncodePasswordToBase64(login.Password);
                UserEmail.UpdateDate = DateTime.Now;
                _dbContext.Entry(UserEmail).State = EntityState.Modified;
                _dbContext.SaveChanges();           
        }
        public void ChangingActiveStatus(string EmailId)
        {
            TbUser user  = _dbContext.TbUsers.Where(x => x.EmailId == EmailId).FirstOrDefault()!;
            user.Status = user.Status == 1 ? 0 : 1;
            user.UpdateDate = DateTime.Now;
            _dbContext.Entry(user).State = EntityState.Modified;
            _dbContext.SaveChanges(); 
        }

        public List<TbUser> Getuser(int id)
        {
            List<TbUser> list = (from user in _dbContext.TbUsers
                                 where (user.UserId == id)
                                 select new TbUser
                                 {
                                     UserId = user.UserId,
                                     FirstName = user.FirstName,
                                     LastName = user.LastName,
                                     MobileNumber = user.MobileNumber,
                                     EmailId = user.EmailId,
                                 }).ToList();
            return list.ToList();
        }
    }
}
    


