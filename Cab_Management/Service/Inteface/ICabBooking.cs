using System;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Service.Inteface
{
    public interface ICabBooking
    {
        public Task<List<TbBooking>> GetTbBookingDetails();
        public Task<List<TbTripDetail>> GetTbTripDetails(); 
        public bool checkCabForConfirmBooking(TbBooking Booking);
        public bool checkCabForBooking(TbBooking tbBooking);
        public void bookingCab(TbBooking Booking);
        public bool ConfirmBooking(TbBooking tbBooking);
        public bool UpdateCabStatus(TbBooking tbBooking);
        public Task<List<CabDisplay>> GetAvailableCabDetails();
        public Task<List<TbBooking>> GetPendingBooking();
        public bool RideCompleted(TbBooking tbBooking);
    }
}
