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
        public string EncodePasswordToBase64(string password);
        public string Decrypt_Password(string encodeData);
    }
}
