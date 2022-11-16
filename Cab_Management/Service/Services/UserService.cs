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
        public string Register(TabUsersDetail tblUser)
        {
            var Email = _dbContext.TabUsersDetails.Find(tblUser.EmailId);
            if (Email != null)
            {
                return "false";
            }
            else
            {
                _dbContext.TabUsersDetails.Add(tblUser);
                var password = _dbContext.TabUsersDetails.Find(tblUser.Password);
                if(password != null)
                {
                    Encrypt encrypt = new Encrypt();
                    encrypt.Decrypt_Password(tblUser.Password);
                }
                _dbContext.SaveChanges();
                return "";
            }
        }

       public bool Update(TabUsersDetail tblUser)
        {  
            _dbContext.TabUsersDetails.Update(tblUser);
            _dbContext.SaveChanges();
            return true;
        }
        public bool Delete(TabUsersDetail tabUsers)
        {
            _dbContext.Remove(Delete(tabUsers));
            return true;
        }
      }        
    }
    


