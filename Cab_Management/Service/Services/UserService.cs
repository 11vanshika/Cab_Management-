using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;
using Service.Inteface;
using Persistence;


namespace Service.Services
{
    public class UserService : IUserDetails
    {
        private readonly DbCabServicesContext _dbContext;
        private readonly IEncrypt _encrypt;

        public UserService(DbCabServicesContext dbcontext, IEncrypt encrypt)
        {
            _dbContext = dbcontext;
            _encrypt = encrypt;
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

        public bool UserLogin(Login login)
        {
            TbUser Userlogin = _dbContext.TbUsers.Where(x => x.EmailId == login.EmailId && x.Password == _encrypt.EncodePasswordToBase64(login.Password)).FirstOrDefault();
            if (Userlogin != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    


