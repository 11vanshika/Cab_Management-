using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using Service.Inteface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(Policy = "Cab_Admin")]
    public class CabController : BaseController
    {
        private readonly DbCabServicesContext dbCabServicesContext;
        private readonly ICabDetail cabAdmin;
        private readonly CrudStatus _crudStatus;
        public CabController(DbCabServicesContext dbCabServicesContext, ICabDetail cabAdmin):base(dbCabServicesContext)
        {
            this.dbCabServicesContext = dbCabServicesContext;
            this.cabAdmin = cabAdmin;
            _crudStatus = new CrudStatus();
        }

        [HttpPost]
        [Route("AddCabDetails")]
        public JsonResult AddCab(TbCabDetail tbCabDetail)
        {
            try
            {
                bool result = cabAdmin.CheckRegNum(tbCabDetail);
                if (result == false)
                {
                    cabAdmin.AddCab(tbCabDetail);
                    _crudStatus.Status = true;
                    _crudStatus.Message = "Cab Added Successfully";     
                }
                else
                {
                    _crudStatus.Status = false;
                    _crudStatus.Message = "cab Already exist";
                }

                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetCabDetails")]
        public async Task<ActionResult> GetTbCabDetails()
        {
            try
            {
                var cabs = await cabAdmin.GetTbCabDetails();
                return Ok(cabs);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteCab")]
        public JsonResult RemoveCab(TbCabDetail tbCabDetail)
        {
            try
            {
                TbCabDetail cabDetail = dbCabServicesContext.TbCabDetails.Where(x => x.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault();
                 bool result = cabAdmin.CheckRegNum(tbCabDetail);
                if (result == true)
                {
                    cabAdmin.RemoveCab(tbCabDetail);
                    _crudStatus.Status = true;
                    _crudStatus.Message = "Removed Cab  Successfully";

                }
                else
                {
                    _crudStatus.Status = false;
                    _crudStatus.Message = "cab RegNum not matched";
                }

                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);            
            }
        }

        [HttpPut]
        [Route("UpdateCabDetails")]
        public JsonResult UpdateCab(TbCabDetail tbCabDetail)
        {
            try
            {
                   TbCabDetail cabDetail = dbCabServicesContext.TbCabDetails.Where(x => x.RegistrationNun == tbCabDetail.RegistrationNun).FirstOrDefault()!;
                    bool result = cabAdmin.CheckRegNum(tbCabDetail);
                    if (result == true)
                    {
                        cabAdmin.UpdateCab(tbCabDetail);
                        _crudStatus.Status = true;
                        _crudStatus.Message = "updated Cab  Successfully";
                    }
                    else
                    {
                        _crudStatus.Status = false;
                        _crudStatus.Message = "cab RegNum not matched";
                    }
                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}
