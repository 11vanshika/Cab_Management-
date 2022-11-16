using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Service.Inteface;
using Persistence;


namespace Service.Services
{
    public class UserService : Encrypt,IUserDetails
    {
        private readonly DbCabManagementContext _dbContext;
        public UserService(DbCabManagementContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        public List<TabUsersDetail> GetUsersDetails()
        {
            List<TabUsersDetail> users = new List<TabUsersDetail>();
            return users;
        }
        public bool Register(TabUsersDetail tblUser)
        {
            Encrypt encrypt1 = new Encrypt();
            string encryptPassword = encrypt1.EncodePasswordToBase64(tblUser.Password);
            tblUser.Password = encryptPassword;
            tblUser.CreateDate = DateTime.Now;
            tblUser.UpdateDate = null;
            tblUser.Status = 1;
            var Email = _dbContext.TabUsersDetails.Find(tblUser.EmailId);
            if (Email == null)
            {
                _dbContext.TabUsersDetails.Add(tblUser);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
  
        }
       public bool Login(TabUsersDetail tblUser)
        {  

            Encrypt encrypt1 = new Encrypt();
            string encryptPassword = encrypt1.EncodePasswordToBase64(tblUser.Password);
            TabUsersDetail Userlogin = _dbContext.TabUsersDetails.Where(x => x.EmailId == tblUser.EmailId && x.Password == encryptPassword).FirstOrDefault();
            if (Userlogin != null)
            {

                return true;
            }
            return false;

        }
        //public bool ForgotPassword(ConfirmPassword changepassword)
        //{
        //    var Email = _dbContext.TabUsersDetails.Find(changepassword.EmailId);
        //    if(Email == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        Encrypt encrypt1 = new Encrypt();

        //        string encryptPassword = encrypt1.EncodePasswordToBase64(changepassword.Password);

        //        changepassword.Password = encryptPassword;
        //        return true;
        //    }
           
        //}

      }        
    }
    


