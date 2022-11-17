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
        List<TabUsersDetail> GetUsersDetails();
        public bool Register(TabUsersDetail tabUsersDetail);

        public bool UserLogin(Login login);  

       //public bool ForgotPassword(ConfirmPassword tabUsersDetail);

    }
}
