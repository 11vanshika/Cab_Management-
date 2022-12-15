using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service.Inteface
{
    public interface ICabDetail
    {
        public void AddCab(TbCabDetail tbCabDetail);
        public void RemoveCab(TbCabDetail tbCabDetail);
        public bool CheckRegNum(TbCabDetail tbCabDetail);
        List<TbCabDetail> GetTbCabDetails();
        public void UpdateCab(TbCabDetail tbCabDetail);
    }
}
