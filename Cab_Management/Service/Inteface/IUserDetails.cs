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
        public string Register(TabUsersDetail tabUsersDetail);

        public bool Update(TabUsersDetail tabUsersDetail);  

       public bool Delete(TabUsersDetail tabUsersDetail);

    }
}
