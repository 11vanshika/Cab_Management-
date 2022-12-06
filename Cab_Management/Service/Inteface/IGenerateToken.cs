using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Inteface
{
    public interface IGenerateToken
      {
        string GenerateToken(TbUser user);
    }
}
