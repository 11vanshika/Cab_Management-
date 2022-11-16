using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service.Inteface
{
    public interface IEncrypt
    {
        public string Decrypt_Password(TabUsersDetail tabUsersDetail);
    }
}
