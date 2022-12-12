using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistence;
using System.Runtime.CompilerServices;
namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly DbCabServicesContext dbCabServicesContext;
        private const string Sessionkey = "userId";
        public BaseController(DbCabServicesContext dbCabServicesContext)
        {
            this.dbCabServicesContext = dbCabServicesContext;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public void loginID(string sessionkey)
        {
            var test = HttpContext.Session.GetInt32(sessionkey);
        }
    }
}
