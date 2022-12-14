using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Persistence;
using Service.Services;
using Domain;
using Service.Inteface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;

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
        private readonly IConfiguration _configuration;
        private readonly IPagination _pagination;
       

        public UserDetailsController(DbCabServicesContext dbContext, IUserDetails iuserDetails, IConfiguration configuration, IPagination pagination)
        {
            dbCabservice = dbContext;
            IuserDetails = iuserDetails;
            _configuration = configuration;
            _pagination = pagination;
          
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
        [HttpGet]
        [Route("PaginatedbyCreatedate")]
        public IActionResult GetUsersCreateDate([FromQuery] PaginationParameters ownerParameters)
        {
            var pages = _pagination.GetUserbyCreateDate(ownerParameters);

            var metadata = new
            {
                pages.TotalCount,
                pages.PageSize,
                pages.CurrentPage,
                pages.TotalPages,
                pages.HasNext,
                pages.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(pages);
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
                string result=IuserDetails.UserLogin(login) ;
                if (result != null)
                {
                    return new JsonResult(new CrudStatus() { Status = true, Message = result });
                }
                else
                {
                    return new JsonResult(new CrudStatus() { Status = false, Message = "User id not Mached" });
                }
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

