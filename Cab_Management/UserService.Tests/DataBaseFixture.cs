using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UserDetailService.Tests
{
    public class DataBaseFixture : IDisposable
    {
        private readonly DbContextOptions<DbCabServicesContext> dbContextOptions = new DbContextOptionsBuilder<DbCabServicesContext>()
        .UseInMemoryDatabase(databaseName: "db_CabServices")
         .Options;
        public DbCabServicesContext context;
        public DataBaseFixture()
        {
            context = new DbCabServicesContext(dbContextOptions);
            context.Database.EnsureCreated();
            SeedDatabase();
        }
        public void SeedDatabase()
        {
            var user = new List<TbUser>()
            {
                new TbUser(){UserId = 1, FirstName = "jyothi",LastName = "matam",EmailId = "jyothi@gmail.com",Password = "12345",UserRoleId = 1,CreateDate = DateTime.Now,UpdateDate = null,Status = 1,MobileNumber = "919380742987"},
                new TbUser(){UserId = 15, FirstName = "avez",LastName = "md",EmailId = "avz@gmail.com",Password = "123456",UserRoleId = 2,CreateDate = DateTime.Now,UpdateDate = null,Status = 1,MobileNumber = "7204091381"}
            };
            context.TbUsers.AddRange(user);
            context.SaveChanges();
            var tbCabDetails = new List<TbCabDetail>()
            {
                new TbCabDetail(){Cabid=5,RegistrationNun="456987",CabTypeId=2,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null},
                new TbCabDetail(){Cabid=9,RegistrationNun="657890",CabTypeId=1,UpdateDate=null,Status=0,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null},
                new TbCabDetail(){Cabid=8,RegistrationNun="6578901",CabTypeId=1,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null},
                new TbCabDetail(){Cabid=1,RegistrationNun="6578901",CabTypeId=1,UpdateDate=null,Status=0,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null}
            };
            context.TbCabDetails.AddRange(tbCabDetails);
            context.SaveChanges();
            var tbcabtype = new List<TbCabType>()
            {
                new TbCabType(){CabTypeId=1,CabName="Mini"},
                new TbCabType(){CabTypeId=2,CabName="Prime Sedan"},
                new TbCabType(){CabTypeId=3,CabName="Prime SUV"},
            };
            context.TbCabTypes.AddRange(tbcabtype);
            context.SaveChanges();
            var booking = new List<TbBooking>()
            {
                new TbBooking(){BookingId=10,CabId=5,UserId=15,TripId=2,ScheduleDate="2022-12-17",ScheduleTime="07-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=0},
                new TbBooking(){BookingId=5,CabId=9,UserId=1,TripId=1,ScheduleDate="2022-12-18",ScheduleTime="08-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=0},
                new TbBooking(){BookingId=8,CabId=5,UserId=15,TripId=1,ScheduleDate="2022-12-20",ScheduleTime="10-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=1},
                new TbBooking(){BookingId=11,CabId=5,UserId=15,TripId=3,ScheduleDate="2022-12-19",ScheduleTime="09-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=2},
                new TbBooking(){BookingId=7,CabId=8,UserId=15,TripId=3,ScheduleDate="2022-12-16",ScheduleTime="06-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=1},
                new TbBooking(){BookingId=1,CabId=1,UserId=15,TripId=3,ScheduleDate="2022-12-16",ScheduleTime="06-30.14",CreateDate=DateTime.Now,UpdateDate=DateTime.Now,Status=1}
            };
            context.TbBookings.AddRange(booking);
            context.SaveChanges();
            var TripDetails = new List<TbTripDetail>()
            {
               new TbTripDetail(){TripDetailId=1,SourceAddress="HSR Layout",DestinationAddress="BTM Layout",Distance=5,TotalFare=100},
               new TbTripDetail(){TripDetailId=2,SourceAddress="BTM Layout",DestinationAddress="Bommanahalli",Distance=3,TotalFare=80},
            };
            context.TbTripDetails.AddRange(TripDetails);
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}