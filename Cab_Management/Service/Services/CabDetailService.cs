using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Persistence;
using Service.Inteface;

namespace Service.Services
{
    public class CabDetailService : ICabDetail
    {
        private readonly DbCabServicesContext _dbCabServicesContext;
        public CabDetailService(DbCabServicesContext dbcontext)
        {
            _dbCabServicesContext = dbcontext;

        }
        public bool CheckRegNum(TbCabDetail tbCabDetail)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault();
            return  cab != null;
        }
        public void AddCab(TbCabDetail tbCabDetail)
        {
                    tbCabDetail.CreateDate = DateTime.Now;
                    tbCabDetail.UpdateDate = null;
                    tbCabDetail.Status = 1;
                    _dbCabServicesContext.TbCabDetails.Add(tbCabDetail);
                    _dbCabServicesContext.SaveChanges();
        }
        public List<TbCabDetail> GetTbCabDetails()
        {
            List<TbCabDetail> tbCabDetails = _dbCabServicesContext.TbCabDetails.ToList();
            return tbCabDetails;
        }
        public void RemoveCab(TbCabDetail tbCabDetail)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault()!;
            _dbCabServicesContext.Remove(cab);
            _dbCabServicesContext.SaveChanges();
        }
        public void UpdateCab(TbCabDetail tbCabDetail)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault()!;
            cab.CabTypeId = tbCabDetail.CabTypeId;
            cab.UpdateDate = DateTime.Now;
            _dbCabServicesContext.Entry(cab).State = EntityState.Modified;
            _dbCabServicesContext.SaveChanges();
        }
    }
}



