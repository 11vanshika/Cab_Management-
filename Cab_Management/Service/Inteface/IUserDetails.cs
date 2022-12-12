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
        List<UserView> GetUsersDetail();
        public List<TbUser> GetUsersDetails();
        public string Register(TbUser tbUsers);
       public bool ConfirmPassword(Registration tblUser);

       public bool CheckExtistUser(Registration user);

       public bool CheckConfirmPassword(Login login);

        public Tuple<string, int> UserLogin(TbUser login);

        public bool ForgotPassword(ForgetPassword changePassword);
       

    }
}
