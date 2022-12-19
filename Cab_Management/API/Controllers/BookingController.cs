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
    ///<summary>
    ///Api consist of BookingController that derive from controllerBase
    /// </summary>
    public class BookingController : BaseController
    {
        /// <summary>
        /// By using the dependency injection calling all the methods
        /// </summary>
       
        private readonly DbCabServicesContext _dbCabServicesContext;
        private readonly ICabBooking _cabbooking;
        private readonly ISendNotification _sendNotification;
        private readonly CrudStatus _crudStatus;

        /// <summary>
        /// here we passed these parameters inside the constructor
        /// </summary>
        /// <param name="dbCabServicesContext"></param>
        /// <param name="cabbooking"></param>
        /// <param name="sendNotification"></param>
        public BookingController(DbCabServicesContext dbCabServicesContext, ICabBooking cabbooking, ISendNotification sendNotification):base(dbCabServicesContext)
        {
            _dbCabServicesContext = dbCabServicesContext;
            _cabbooking = cabbooking;
            _sendNotification = sendNotification;
            _crudStatus = new CrudStatus();
        }

        /// <summary>
        /// calling GetTbTripDetails() method from CabBookingService 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTripDetails")]

        [Authorize]
        public async Task<ActionResult> GetTbTripDetails()
        {
            try
            {
                var trips = await _cabbooking.GetTbTripDetails();
                return Ok(trips);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling GetAvailableCabDetails method from CabBookingService 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Available Cab")]

        [Authorize]
        public async Task<ActionResult> GetAvailableCabDetails()
        {
            try
            {
                var cabs = await _cabbooking.GetAvailableCabDetails();  
                return Ok(cabs);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling the checkCabForBooking to check cab availability then calling the bookingCab method from CabBookingService
        /// </summary>
        /// <param name="tbBooking"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CabBooking")]
        [Authorize(Policy = "Customer")]
        public JsonResult BookingCab(TbBooking tbBooking)
        {
            try
            {
                bool result = _cabbooking.checkCabForBooking(tbBooking);
                if (result == true)
                {
                    _cabbooking.bookingCab(tbBooking);
                    _crudStatus.Status = true;
                    _crudStatus.Message = "Booking request send Successfully";
                }
                else
                {
                    _crudStatus.Status = false;
                    _crudStatus.Message = "Cab not available";
                }
                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling checkCabForConfirmBooking to check the cab availability then calling the ConfirmBooking if the booking is confirmed the cab status get updated
        /// </summary>
        /// <param name="tbBooking"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ConfirmBooking")]
       [Authorize(Policy = "Cab_Admin")]
        public JsonResult ConfirmBooking(TbBooking tbBooking)
        {
            try
            {
                bool result = _cabbooking.checkCabForConfirmBooking(tbBooking);
                if (result == true)
                {
                    result = _cabbooking.ConfirmBooking(tbBooking);
                    if (result == true)
                    {
                        result = _cabbooking.UpdateCabStatus(tbBooking);
                        if (result == true)
                        {
                            _crudStatus.Status = true;
                            _crudStatus.Message = "Booking Confirmed Successfully";
                        }
                        else
                        {
                            _crudStatus.Status = false;
                            _crudStatus.Message = "Cab is not available";
                        }
                    }
                }
                else
                {
                    _crudStatus.Status = false;
                    _crudStatus.Message = "Booking rejected";
                }
                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling the RideCompleted method from CabBookingService
        /// </summary>
        /// <param name="tbBooking"></param>
        /// <returns></returns>
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
                    _crudStatus.Status = true;
                    _crudStatus.Message = "Ride completed Successfully";
                }
                else
                {
                    _crudStatus.Status = false;
                    _crudStatus.Message = "Ride not completed Successfully";
                } 
                return new JsonResult(_crudStatus);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling the GetPendingBooking method to get the pending bookings from CabBookingService
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("BookingPending")]
        [Authorize(Policy = "Cab_Admin")]
        public async Task<ActionResult> GetPendingBookings()
        {
            try
            {
                var bookingPendings = await _cabbooking.GetPendingBooking();
                return Ok(bookingPendings);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        /// <summary>
        /// calling the GetTbBookingDetails method to get the bookings details from CabBookingService
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBookingDetails")]
        [Authorize(Policy = "Cab_Admin")]
        public async Task<ActionResult> GetTbBookingDetails()
        {
            try
            {
                var bookings = await _cabbooking.GetTbBookingDetails();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}