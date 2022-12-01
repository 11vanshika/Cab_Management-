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
                new TbUser(){UserId = 1, FirstName = "jyothi",LastName = "matam",EmailId = "jyothi@gmail.com",Password = "12345",UserRoleId = 1,CreateDate = DateTime.Now,UpdateDate = null,Status = 1},
                new TbUser(){UserId = 15, FirstName = "avez",LastName = "md",EmailId = "avz@gmail.com",Password = "123456",UserRoleId = 2,CreateDate = DateTime.Now,UpdateDate = null,Status = 1}
            };
            context.TbUsers.AddRange(user);
            context.SaveChanges();
            var tbCabDetails = new List<TbCabDetail>()
            {

                new TbCabDetail(){Cabid=5,RegistrationNun="456987",CabTypeId=2,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null},
                new TbCabDetail(){Cabid=9,RegistrationNun="657890",CabTypeId=1,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null}

            };
            context.TbCabDetails.AddRange(tbCabDetails);
            context.SaveChanges();

        }
    
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}

