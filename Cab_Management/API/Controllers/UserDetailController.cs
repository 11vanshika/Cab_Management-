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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Repository;
using FluentNHibernate.Automapping;
using NHibernate.Mapping;
using Twilio.Jwt.AccessToken;
using static NHibernate.Engine.Query.CallableParser;

//using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : BaseController
    {
        private readonly DbCabServicesContext dbCabservice;
        private readonly IUserDetails IuserDetails;
        private readonly IConfiguration _configuration;
        private const string Sessionkey = "userId";
        private readonly IMapper _mapper; 
        private readonly CrudStatus crudStatus;
        public UserDetailsController(DbCabServicesContext dbContext, IUserDetails iuserDetails, IConfiguration configuration , IMapper mapper):base(dbContext)
        {
            dbCabservice=dbContext;
            IuserDetails = iuserDetails;
            _configuration = configuration; 
            _mapper = mapper;
            crudStatus=new CrudStatus();
        }

        [HttpPost()]
        [Route("Register")]
        public JsonResult UserRegister(Registration tblUser)
        {
            try
            {
                var RegIndto = Automapper<Registration, TbUser>.MapClass(tblUser);
                bool result = IuserDetails.CheckExtistUser(tblUser);
                if (result == false)
                {
                    string message = IuserDetails.Register(RegIndto);
                    crudStatus.Status = true;
                    crudStatus.Message = message;
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Email Already Exist";
                }
                return new JsonResult(crudStatus);
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
                var logIndto = Automapper<Login, TbUser>.MapClass(login);
                var result = IuserDetails.UserLogin(logIndto);
                if (result != null)
                {
                    HttpContext.Session.SetInt32(Sessionkey, result.Item2);
                    loginID(Sessionkey);
                    crudStatus.Status = true;
                    crudStatus.Message = result.Item1; 
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "User id not Mached";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public JsonResult Forgot_password(ForgetPassword password)
        {
            try
            {
                var ForgetIndto = Automapper<ForgetPassword, Registration>.MapClass(password);
                bool result = IuserDetails.CheckExtistUser(ForgetIndto);
                if (result == true)
                {
                    result = IuserDetails.ConfirmPassword(ForgetIndto);
                    if (result == true)
                    {
                        IuserDetails.ForgotPassword(password);
                        crudStatus.Status = true;
                        crudStatus.Message = "Password updated successfully";
                    }
                    else
                    {
                        crudStatus.Status = false;
                        crudStatus.Message = "Password and Confirm Password not matched";
                    }
                }
                else
                {
                    crudStatus.Status = false;
                    crudStatus.Message = "Email not  registered Please Sign up";
                }
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpPut("Changing_Active_Status")]
        [Authorize]
        public JsonResult ChangingActiveStatus(string EmailId)
        {
            try
            {
                IuserDetails.ChangingActiveStatus(EmailId);
                crudStatus.Status = true;
                crudStatus.Message = "Your Account Deactivated";
                return new JsonResult(crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        [HttpGet()]
        [Route("GetUserDetails")]
        [Authorize(Policy = "Cab_Admin")]
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
        [MapToApiVersion("2")]
        [Route("V2")]
        [Authorize(Policy = "Cab_Admin")]
        public ActionResult<List<UserDisplay>> UserDetails2()
        {
            var users = IuserDetails.GetUsersDetail().Select(x=> _mapper.Map<UserDisplay>(x));
            return Ok(users);
        }

        [HttpGet]
        [Route("V3")]
        [MapToApiVersion("3")]
        [Authorize(Policy = "Cab_Admin")]
        public JsonResult UserDetails3()
        {
            var c = new MapperConfiguration(cfg => cfg.CreateProjection<TbUser, UserDisplay>()
                                                      .ForMember(dto => dto.UserRoleName, conf =>
                                                  conf.MapFrom(ol => ol.UserRole.UserRoleName)));

            return new JsonResult(dbCabservice.TbUsers.ProjectTo<UserDisplay>(c).ToList());
        }

        [HttpGet]
        [Route("V4")]
        [MapToApiVersion("4")]
        [Authorize(Policy = "Cab_Admin")]
        public List<UserDisplay> UserDetails4()
        {
            List<UserDisplay> users = Automapper<UserView, UserDisplay>.MapList(IuserDetails.GetUsersDetail());
            return users;
        }

        [HttpGet]
        [Route("V5")]
        [MapToApiVersion("5")]
        [Authorize]
        public JsonResult GetUser(int id)
        {
            try
            {
                return new JsonResult(IuserDetails.Getuser(id).ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}

