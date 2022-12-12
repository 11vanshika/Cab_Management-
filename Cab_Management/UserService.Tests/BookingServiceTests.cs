using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using Moq;
using Service.Inteface;
using Service.Services;
using UserDetailService.Tests;
using Xunit;
namespace Bookingservice.Tests
{
    [Collection("Database Collection")]
    public class BookingServiceTests
    {
        private readonly DataBaseFixture _fixture;
        private readonly CabBookingService bookingService;
        private readonly Mock<ISendNotification> notification;
        public BookingServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            notification = new Mock<ISendNotification>();
            bookingService = new CabBookingService(fixture.context, notification.Object);
        }

        [Fact]
        public void GetAll_BookingDetails()
        {
            //Arrange
            var result = bookingService.GetTbBookingDetails();
            var count = _fixture.context.TbBookings.Count();
            //Act
            var items = Assert.IsType<List<TbBooking>>(result);
            //Assert
            Assert.Equal(count, items.Count());
        }

        [Fact]
        public void GetAll_TripDetails()
        {
            //Arrange
            var result = bookingService.GetTbTripDetails();
            var count = _fixture.context.TbTripDetails.Count();
            //Act
            var items = Assert.IsType<List<TbTripDetail>>(result);
            //Assert
            Assert.Equal(count, items.Count());
        }

        [Fact]
        public void CheckCabBookings_ReturnsGoodResponse()
        {
            //Arrange
            var cab_booking = new TbBooking()
            {
                CabId = 5,
                BookingId = 8
            };
            //Act
            var result = bookingService.checkCabForBooking(cab_booking);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckCabBookings_ReturnsBadResponse()
        {
            //Arrange
            var cab_booking = new TbBooking()
            {
                CabId = 9,
                BookingId = 5
            };
            //Act
            var result = bookingService.checkCabForBooking(cab_booking);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void CheckCabForConfirmBooking()
        {
            //Arrange
            var ConfirmBooking = new TbBooking()
            {
                BookingId = 10,
                CabId = 5
            };
            //Act
            var result = bookingService.checkCabForConfirmBooking(ConfirmBooking);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckCabForConfirmBooking_ReturnFalse()
        {
            //Arrange
            var ConfirmBooking = new TbBooking()
            {
                BookingId = 1
            };
            //Act
            var result = bookingService.checkCabForConfirmBooking(ConfirmBooking);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void BookingCab()
        {
            var booking = new TbBooking()
            {
                CabId = 5,
                UserId = 15,
                TripId = 2,
                ScheduleDate = "2022-12-17",
                ScheduleTime = "07-30.14"
            };
            //Act
            var result = bookingService.bookingCab(booking);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ConfirmBooking_Accept()
        {
            //Arrange
            var confirmbooking = new TbBooking()
            {
                BookingId = 5,
                CabId = 5,
                TripId = 2,
                ScheduleDate = "2022-12-17",
                ScheduleTime = "07-30.14",
                Status = 1
            };
            //Act
            var result = bookingService.ConfirmBooking(confirmbooking);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ConfirmBooking_Rejected()
        {
            //Arrange
            var confirmbooking = new TbBooking()
            {
                BookingId = 11,
                CabId = 5,
                TripId = 3,
                ScheduleDate = "2022-12-19",
                ScheduleTime = "09-30.14",
                Status = 2
            };
            //Act
            var result = bookingService.ConfirmBooking(confirmbooking);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void ConfirmBooking_Pending()
        {
            //Arrange
            var bookingpending = new TbBooking()
            {
                BookingId = 10,
                CabId = 5,
                TripId = 9,
                ScheduleDate = "2022-12-17",
                ScheduleTime = "07-30.14",
                Status = 0
            };
            //Act
            var result = bookingService.ConfirmBooking(bookingpending);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateCabStatus_ReturnsTrue()
        {
            //Arrange
            var bookingpending = new TbBooking()
            {
                BookingId = 7,
                CabId = 8,
            };
            //Act
            var result = bookingService.UpdateCabStatus(bookingpending);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateCabStatus_ReturnsFalse()
        {
            //Arrange
            var bookingpending = new TbBooking()
            {
                BookingId = 5,
                CabId = 9,
            };
            //Act
            var result = bookingService.UpdateCabStatus(bookingpending);
            //Assert
            Assert.False(result);
        }

        [Fact]
        public void GetAvailableCabDetail()
        {
            //Arrange
            var expected = _fixture.context.TbCabDetails.Where(x => x.Status == 1).Count();
            //Act
            var result = bookingService.GetAvailableCabDetails();
            //Assert
            var items = Assert.IsType<List<CabDisplay>>(result);
            Assert.Equal(expected, items.Count);
        }

        [Fact]
        public void GetPendingBooking()
        {
            //Arrange
            var expected = _fixture.context.TbBookings.Where(x => x.Status == 0).Count();
            //Act
            var result = bookingService.GetPendingBooking();
            //Assert
            var items = Assert.IsType<List<TbBooking>>(result);
            Assert.Equal(expected, items.Count);
        }

        [Fact]
        public void RideCompleted_ReturnTrue()
        {
            //Arrange
            var ride = new TbBooking()
            {
                BookingId = 8,
                CabId = 5
            };
            //Act
            var result = bookingService.RideCompleted(ride);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void RideCompleted_ReturnFalse()
        {
            //Arrange
            var ride = new TbBooking()
            {
                BookingId = 10,
                CabId = 5
            };
            //Act
            var result = bookingService.RideCompleted(ride);
            //Assert
            Assert.False(result);
        }
    }
}

