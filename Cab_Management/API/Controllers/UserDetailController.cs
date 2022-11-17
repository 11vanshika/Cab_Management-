using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Persistence;
using Service.Services;
using Domain;
using Service.Inteface;

//using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly DbCabManagementContext dbCabmanagement;
        private readonly IUserDetails IuserDetails;
        public UserDetailsController(DbCabManagementContext dbContext, IUserDetails iuserDetails)
        {
            dbCabmanagement = dbContext;
            IuserDetails = iuserDetails;
        }
        // GET: api/<UserController>
        [HttpGet()]
        public JsonResult GetUserDetail()
        {
            try
            {
                return new JsonResult(IuserDetails.GetUsersDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost()]
        [Route("Register")]
        public JsonResult UserRegister(TabUsersDetail tblUser)
        {
            try
            {
                return new JsonResult(IuserDetails.Register(tblUser));
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost()]
        [Route("Login")]
        public JsonResult UpdateUser(Login login)
        {
            try
            {
                bool result = IuserDetails.UserLogin(login);
                if (result == true)
                {
                    return new JsonResult("User Details Updated successfully");
                }
                else
                {
                    return new JsonResult("User id not Mached");
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
     
    }
}

