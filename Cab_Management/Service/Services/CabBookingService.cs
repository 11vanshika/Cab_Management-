using Domain;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Service.Services
{
    public class CabBookingService : ICabBooking
    {
        private readonly DbCabServicesContext _dbCabServicesContext;
        private readonly ISendNotification _sendNotification;
        public CabBookingService(DbCabServicesContext dbcontext, ISendNotification sendNotification)
        {
            _dbCabServicesContext = dbcontext;
            _sendNotification = sendNotification;
        }
        public List<TbBooking> GetTbBookingDetails()
        {
            List<TbBooking> tbBookings = _dbCabServicesContext.TbBookings.ToList();
            return tbBookings;
        }
        public List<TbTripDetail> GetTbTripDetails()
        {
            List<TbTripDetail> tbTripDetails = _dbCabServicesContext.TbTripDetails.ToList();
            return tbTripDetails;
        }
        public bool checkCabForConfirmBooking(TbBooking tbBooking)
        {
            TbBooking booking = _dbCabServicesContext.TbBookings.Where(x => x.BookingId == tbBooking.BookingId).FirstOrDefault()!;
            int tbCab = Convert.ToInt32(booking.CabId);
            TbCabDetail cabDetail = _dbCabServicesContext.TbCabDetails.Where(x => x.Cabid == tbCab).FirstOrDefault()!;
            int CabStatus = Convert.ToInt32(cabDetail.Status);
            if (CabStatus == 1)
            {
                return true;
            }
            return false;
        }

        public bool checkCabForBooking(TbBooking tbBooking)
        {
            TbBooking booking = _dbCabServicesContext.TbBookings.Where(x =>x.BookingId == tbBooking.BookingId).FirstOrDefault()!;
            TbCabDetail cabDetail = _dbCabServicesContext.TbCabDetails.Where(x => x.Cabid == booking.CabId).FirstOrDefault()!;
            int CabStatus = Convert.ToInt32(cabDetail.Status);
            if (CabStatus == 1)
            {
                return true;
            }
            return false;
        }

        public bool bookingCab(TbBooking tbBooking)
        {
            tbBooking.CreateDate = DateTime.Now;
            tbBooking.UpdateDate = null;
            tbBooking.Status = 0;
            _dbCabServicesContext.TbBookings.Add(tbBooking);
            _dbCabServicesContext.SaveChanges();
            return true;
        }
        public bool ConfirmBooking(TbBooking tbBooking)
        {
            var booking = _dbCabServicesContext.TbBookings.Where(x => x.BookingId == tbBooking.BookingId).FirstOrDefault()!;
            if (booking != null)
            {
                if (tbBooking.Status == 1)
                {
                    booking.UpdateDate = DateTime.Now;
                    booking.Status = 1;
                    _dbCabServicesContext.Entry(booking).State = EntityState.Modified;
                    _dbCabServicesContext.SaveChanges();
                    SendingNotification sendingNotification = GenerateMessage(tbBooking);
                    _sendNotification.SendWhatsAppMessage(sendingNotification);
                    _sendNotification.SendSMSMessage(sendingNotification);
                    return true;
                }
                else
                {
                    booking.Status = 2;
                    booking.UpdateDate = DateTime.Now;
                    _dbCabServicesContext.Entry(booking).State = EntityState.Modified;
                    _dbCabServicesContext.SaveChanges();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool UpdateCabStatus(TbBooking tbBooking)
        {
            TbBooking booking = _dbCabServicesContext.TbBookings.Where(x => x.BookingId == tbBooking.BookingId).FirstOrDefault()!;
            if (booking != null)
            {
                int tbCab = Convert.ToInt32(booking.CabId);
                TbCabDetail cabDetail = _dbCabServicesContext.TbCabDetails.Where(x => x.Cabid == tbCab).FirstOrDefault()!;
                int CabStatus = Convert.ToInt32(cabDetail.Status);
                if (CabStatus == 1)
                {
                    cabDetail.Status = 0;
                    cabDetail.UpdateDate = DateTime.Now;
                    _dbCabServicesContext.Entry(booking).State = EntityState.Modified;
                    _dbCabServicesContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public List<CabDisplay> GetAvailableCabDetails()
        {
            List<CabDisplay> list = (from cab in _dbCabServicesContext.TbCabDetails
                                     join cabtype in _dbCabServicesContext.TbCabTypes on cab.CabTypeId equals cabtype.CabTypeId
                                     orderby cab.Cabid
                                     where (cab.Status == 1)
                                     select new CabDisplay
                                     {
                                         Cabid = cab.Cabid,
                                         RegistrationNun = cab.RegistrationNun,
                                         CabType = cabtype.CabName!
                                     }).ToList();
            return list.ToList();
        }
        public List<TbBooking> GetPendingBooking()
        {
            List<TbBooking> list = (from booking in _dbCabServicesContext.TbBookings
                                    where (booking.Status == 0)
                                    select new TbBooking
                                    {
                                        BookingId = booking.BookingId,
                                        CabId = booking.CabId,
                                        UserId = booking.UserId,
                                        TripId = booking.TripId,
                                        ScheduleDate = booking.ScheduleDate,
                                        ScheduleTime = booking.ScheduleTime
                                    }).ToList();
            return list.ToList();
        }
        public bool RideCompleted(TbBooking tbBooking)
        {
            TbBooking booking = _dbCabServicesContext.TbBookings.Where(x => x.BookingId == tbBooking.BookingId).FirstOrDefault()!;
            if (booking != null)
            {
                int tbCab = Convert.ToInt32(booking.CabId);
                TbCabDetail cabDetail = _dbCabServicesContext.TbCabDetails.Where(x => x.Cabid == tbCab).FirstOrDefault()!;
                int bookingStatus = Convert.ToInt32(booking.Status);
                if (bookingStatus == 1)
                {
                    booking.Status = 3;
                    booking.UpdateDate = DateTime.Now;
                    _dbCabServicesContext.Entry(booking).State = EntityState.Modified;
                    _dbCabServicesContext.SaveChanges();
                    cabDetail.Status = 1;
                    cabDetail.UpdateDate = DateTime.Now;
                    _dbCabServicesContext.Entry(cabDetail).State = EntityState.Modified;
                    _dbCabServicesContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public SendingNotification GenerateMessage(TbBooking tbBooking)
        {
            SendingNotification sendingNotification = new SendingNotification();
            TbBooking tbBooking1 = _dbCabServicesContext.TbBookings.Where(x => x.BookingId == tbBooking.BookingId).FirstOrDefault();
            TbCabDetail tbCabDetail = _dbCabServicesContext.TbCabDetails.Where(x=>x.Cabid == tbBooking1.CabId).FirstOrDefault();  
            TbUser tbUser = _dbCabServicesContext.TbUsers.Where(x => x.UserId == tbBooking1.UserId).FirstOrDefault();
            sendingNotification.Message = "Hello" + tbUser.FirstName + " your Booking Confirm " + " Cab Number is " + tbCabDetail.RegistrationNun +" Booking Id is " + tbBooking1.BookingId;
            sendingNotification.MobileNumber = tbUser.MobileNumber.ToString();
            return sendingNotification;
        }
    }
}