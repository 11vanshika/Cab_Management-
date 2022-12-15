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
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Repository;
using FluentNHibernate.Automapping;
using NHibernate.Mapping;

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
        private readonly IPagination _pagination;
        private const string Sessionkey = "userId";
        private readonly IMapper _mapper;

        public UserDetailsController(DbCabServicesContext dbContext, IUserDetails iuserDetails, IConfiguration configuration, IPagination pagination, IMapper mapper) : base(dbContext)
        {
            dbCabservice=dbContext;
            IuserDetails = iuserDetails;
            _configuration = configuration;
            _pagination = pagination;
            _mapper = mapper;
        }

        // GET: api/<UserController>
        [HttpGet()]
        [Authorize]
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
       

        [HttpGet]
        [MapToApiVersion("2")]
        [Route("V2")]
        [Authorize]
        public ActionResult<List<UserDisplay>> UserDetails2()
        {
            var users = IuserDetails.GetUsersDetail().Select(x=> _mapper.Map<UserDisplay>(x));
            return Ok(users);
        }

        [HttpGet]
        [Route("V3")]
        [MapToApiVersion("3")]
        [Authorize]
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

        public List<UserDisplay> UserDetails4()
        {
            List<UserDisplay> users = Automapper<UserView, UserDisplay>.MapList(IuserDetails.GetUsersDetail());
            return users;
        }

        [HttpPost()]
        [Route("Register")]
        public JsonResult UserRegister(Registration tblUser)
        {
            try
            {
                var logIndto = Automapper<Registration, TbUser>.MapClass(tblUser);
                bool result = IuserDetails.CheckExtistUser(tblUser);
                if (result == false)
                {
                        IuserDetails.Register(logIndto);
                         return new JsonResult(new CrudStatus() { Status = true , Message = "Registered successfully" });
                        
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
                var logIndto = Automapper<Login, TbUser>.MapClass(login);
                var result=IuserDetails.UserLogin(logIndto) ;
                if (result != null)
                {
                    //inbuild func to set a value
                    HttpContext.Session.SetInt32(Sessionkey, result.Item2);
                    loginID(Sessionkey);
                    return new JsonResult(new CrudStatus() { Status = true, Message = result.Item1 });
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
        [Authorize]
        public JsonResult Forgot_password(ForgetPassword login)
        {
            try
            {
                var ForgetIndto = Automapper<ForgetPassword, Registration>.MapClass(login);
                bool result = IuserDetails.CheckExtistUser(ForgetIndto);
                if (result == true)
                {                  
                    result = IuserDetails.ForgotPassword(login);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = true, Message = "Password updated successfully" });
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

