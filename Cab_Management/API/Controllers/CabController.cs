﻿using Domain;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Inteface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CabController : ControllerBase
    {
        private readonly DbCabServicesContext dbCabServicesContext;
        private readonly ICabDetail cabAdmin;

        public CabController(DbCabServicesContext dbCabServicesContext, ICabDetail cabAdmin)
        {
            this.dbCabServicesContext = dbCabServicesContext;
            this.cabAdmin = cabAdmin;
        }

        [HttpPost]
        public JsonResult AddCab(TbCabDetail tbCabDetail)
        {
            try
            {
                bool result = cabAdmin.CheckAdmin(tbCabDetail);
                if (result == true)
                {
                    result = cabAdmin.CheckRegNum(tbCabDetail);
                    if (result == false)
                    {
                        result = cabAdmin.AddCab(tbCabDetail);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Cab Added Successfully" });
                        }
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "UnAuthorized user" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet]
        public JsonResult GetTbCabDetails()
        {
            try
            {
                return new JsonResult(cabAdmin.GetTbCabDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete]
        public JsonResult RemoveCab(TbCabDetail tbCabDetail)
        {
            try
            {
                TbCabDetail cabDetail = dbCabServicesContext.TbCabDetails.Where(x => x.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault();
                bool result = cabAdmin.CheckAdmin(cabDetail);
                if (result == true)
                {
                    result = cabAdmin.CheckRegNum(tbCabDetail);
                    if (result == true)
                    {
                        result = cabAdmin.RemoveCab(tbCabDetail);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Removed cab Successfully" });
                        }
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "UnAuthorized User" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);            
            }
        }

        [HttpPut]
        public JsonResult UpdateCab(string RegistrationNun, int cabtype)
        {
            try
            {
                TbCabDetail cabDetail = dbCabServicesContext.TbCabDetails.Where(x=>x.RegistrationNun== RegistrationNun).FirstOrDefault();   
                bool result = cabAdmin.CheckAdmin(cabDetail);
                if (result == true)
                {
                    result = cabAdmin.CheckRegNum(RegistrationNun);
                    if (result == true)
                    {
                        result = cabAdmin.UpdateCab(RegistrationNun, cabtype);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Updated cab Successfully" });
                        }
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "UnAuthorized User" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}