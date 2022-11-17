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
            List<TbUser> users =_dbContext.TbUsers.ToList();
            return users;
        }
        public bool Register(TbUser tblUser)
        {
            var Email = _dbContext.TbUsers.Where(x=>x.EmailId==tblUser.EmailId).FirstOrDefault();
            if (Email == null)
            {
                tblUser.Password = _encrypt.EncodePasswordToBase64(tblUser.Password);
                tblUser.CreateDate = DateTime.Now;
                tblUser.UpdateDate = null;
                tblUser.Status = 1;
                _dbContext.TbUsers.Add(tblUser);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }  
        }
       public bool UserLogin(Login login)
        {  
           string encryptPassword = _encrypt.EncodePasswordToBase64(login.Password);
            string decryptPassword = _encrypt.Decrypt_Password(encryptPassword);
             TbUser Userlogin = _dbContext.TbUsers.Where(x=>x.EmailId==login.EmailId && x.Password==encryptPassword).FirstOrDefault();
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
            if (UserEmail == null)
            {
                return false;
            }
            else
            {
                string encryptnewPassword = _encrypt.EncodePasswordToBase64(login.Password);
                string encryptconfirmPassword = _encrypt.EncodePasswordToBase64(login.ConfirmPassword);             
                if(encryptnewPassword == encryptconfirmPassword)
                {
                    UserEmail.Password = encryptnewPassword;
                    UserEmail.UpdateDate = DateTime.Now;
                    _dbContext.Entry(UserEmail).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }               
            }
        }
    }      
}
    


