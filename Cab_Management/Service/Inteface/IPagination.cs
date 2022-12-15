using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Inteface
{
    public interface IPagination
    {
       PagedList<TbUser> GetUserbyCreateDate(PaginationParameters ownerParameters);
       //PagedList<TbCabDetail> GetCabsbyCreateDate(PaginationParameters paginationParameters);


    }
}
