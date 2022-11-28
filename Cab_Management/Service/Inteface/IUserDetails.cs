using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service.Inteface
{
    public interface IUserDetails
    {
        List<TbUser> GetUsersDetails();
        public bool Register(TbUser tbUsers);
       public bool CheckExtistUser(TbUser tblUser);

       public bool CheckExtistUser(Login login);

       public bool CheckConfirmPassword(Login login);

        public string UserLogin(Login login);  

        public bool ForgotPassword(Login login);

       

    }
}
