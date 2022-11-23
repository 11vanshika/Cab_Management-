﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Service.Inteface
{
    public interface ICabDetail
    {

        public bool AddCab(TbCabDetail tbCabDetail);
        public bool RemoveCab(TbCabDetail tbCabDetail);
        public bool CheckAdmin(TbCabDetail tbCabDetail);
        public bool CheckRegNum(TbCabDetail tbCabDetail);
        bool CheckRegNum(string RegNum);

        List<TbCabDetail> GetTbCabDetails();

        public bool UpdateCab(string RegistrationNun, int cabtype);
    }
}