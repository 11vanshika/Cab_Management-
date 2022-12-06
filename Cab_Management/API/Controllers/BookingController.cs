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
    public class BookingController : ControllerBase
    {
        private readonly DbCabServicesContext _dbCabServicesContext;
        private readonly ICabBooking _cabbooking;
        private readonly ISendNotification _sendNotification;
        public BookingController(DbCabServicesContext dbCabServicesContext, ICabBooking cabbooking, ISendNotification sendNotification)
        {
            _dbCabServicesContext = dbCabServicesContext;
            _cabbooking = cabbooking;
            _sendNotification = sendNotification;
        }
        [HttpGet]
        [Route("GetTripDetails")]
        [Authorize]
        public JsonResult GetTbTripDetails()
        {
            try
            {
                return new JsonResult(_cabbooking.GetTbTripDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet]
        [Route("Available Cab")]
        [Authorize]
        public JsonResult GetAvailableCabDetails()
        {
            try
            {
                return new JsonResult(_cabbooking.GetAvailableCabDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        [Route("CabBooking")]
        [Authorize]
        public JsonResult BookingCab(TbBooking tbBooking)
        {
            try
            {
                bool result = _cabbooking.checkCabForBooking(tbBooking);
                if (result == true)
                {
                    result = _cabbooking.bookingCab(tbBooking);
                    if (result == true)
                    {
                        return new JsonResult(new CrudStatus() { Status = result, Message = "Booking Request Send Successfully" });
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Cab not available" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPost]
        [Route("ConfirmBooking")]
        [Authorize(Policy = "Cab_Admin")]
        public JsonResult ConfirmBooking(TbBooking tbBooking)
        {
            try
            {
                bool result = _cabbooking.checkCabForConfirmBooking(tbBooking);
                if (result == true)
                {
                      result = _cabbooking.UpdateCabStatus(tbBooking);
                    if (result == true)
                    {
                        result = _cabbooking.ConfirmBooking(tbBooking);
                        if (result == true)
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Booking Confirmed Successfully" });
                        }
                        else
                        {
                            return new JsonResult(new CrudStatus() { Status = result, Message = "Cab is not available" });
                        }
                    }
                }
                return new JsonResult(new CrudStatus() { Status = false, Message = "Booking rejected" });
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpPut]
        [Route("RideCompleted")]
       [Authorize(Policy = "Customer")]
        public JsonResult RideCompleted(TbBooking tbBooking)
        {
            try
            {
                bool result = _cabbooking.RideCompleted(tbBooking);
                if (result == true)
                {
                    return new JsonResult(new CrudStatus() { Status = result, Message = "Ride completed Successfully" });
                }
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet]
        [Route("BookingPending")]
        [Authorize(Policy = "Cab_Admin")]
        public JsonResult GetPendingBooking()
        {
            try
            {
                return new JsonResult(_cabbooking.GetPendingBooking().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetBookingDetails")]
        [Authorize(Policy = "Cab_Admin")]
        public JsonResult GetTbBookingDetails()
        {
            try
            {
                return new JsonResult(_cabbooking.GetTbBookingDetails().ToList());
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}