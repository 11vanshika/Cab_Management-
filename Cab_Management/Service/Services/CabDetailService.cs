﻿using System;
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

        public bool CheckAdmin(TbCabDetail tbCabDetail)
        {
            TbUser user = _dbCabServicesContext.TbUsers.Where(x => x.UserId == tbCabDetail.UserId).FirstOrDefault();
            int Userrole = Convert.ToInt32(user.UserRoleId);
            if (Userrole == 2)
            {
                return true;
            }
            return false;
        }

        public bool CheckRegNum(TbCabDetail tbCabDetail)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault();
            if (cab == null)
            {
                return false;
            }
            return true;
        }

        public bool CheckRegNum(string RegNum)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == RegNum).FirstOrDefault();
            if (cab == null)
            {
                return false;
            }
            return true;
        }
        public bool AddCab(TbCabDetail tbCabDetail)
        {
                    tbCabDetail.CreateDate = DateTime.Now;
                    tbCabDetail.UpdateDate = null;
                    tbCabDetail.Status = 1;
                    _dbCabServicesContext.TbCabDetails.Add(tbCabDetail);
                    _dbCabServicesContext.SaveChanges();
                    return true;
        }
        public List<TbCabDetail> GetTbCabDetails()
        {
            List<TbCabDetail> tbCabDetails = _dbCabServicesContext.TbCabDetails.ToList();
            return tbCabDetails;
        }
        public bool RemoveCab(TbCabDetail tbCabDetail)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault();
            _dbCabServicesContext.Remove(cab);
            _dbCabServicesContext.SaveChanges();
            return true;
        }
        public bool UpdateCab(string RegistrationNun, int cabtype)
        {
            TbCabDetail cab = _dbCabServicesContext.TbCabDetails.Where(y => y.RegistrationNun == RegistrationNun).FirstOrDefault();
            cab.CabTypeId = cabtype;
            cab.UpdateDate = DateTime.Now;
            _dbCabServicesContext.Entry(cab).State = EntityState.Modified;
            _dbCabServicesContext.SaveChanges();
            return true;
        }
    }
}


