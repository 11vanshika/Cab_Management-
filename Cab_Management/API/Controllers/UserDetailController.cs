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
        private readonly DbCabServicesContext dbCabservice;
        private readonly IUserDetails IuserDetails;

        public UserDetailsController(DbCabServicesContext dbContext, IUserDetails iuserDetails)
        {
            dbCabservice = dbContext;
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
        public JsonResult UserRegister(TbUser tblUser)
        {
            try
            {
                bool result = IuserDetails.CheckExtistUser(tblUser);
                if (result == false)
                {
                    
                        result = IuserDetails.Register(tblUser);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Registered successfully" });
                        }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Email Already Exist" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost()]
        [Route("Login")]
        public JsonResult UserLogin(Login login)
        {
            try
            {
                bool result = IuserDetails.UserLogin(login);
                if (result == true)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = "User Login successfull" });

                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "User id not Mached" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public JsonResult Forgot_password(Login login)
        {
            try
            {
                bool result = IuserDetails.CheckExtistUser(login);
                if (result == true)
                {
                    result = IuserDetails.CheckConfirmPassword(login);
                    if (result == true)
                    {
                        result = IuserDetails.ForgotPassword(login);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = true, Message = "Password updated successfully" });
                        }
                    }
                    else
                    {
                        return new JsonResult(new CrudStatus() { Status = result, Message = "Password and Confirm password not matched" });
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Email not  registered Please Sign up" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

    }
}

