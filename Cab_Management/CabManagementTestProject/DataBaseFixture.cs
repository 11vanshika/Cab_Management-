using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace CabManagementTestProject
{
    public class DataBaseFixture : IDisposable
    {
        private static DbContextOptions<DbCabServicesContext> dbContextOptions=new DbContextOptionsBuilder<DbCabServicesContext>()
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
            var tbCabDetails = new List<TbCabDetail>()
            {

                new TbCabDetail(){Cabid=5,RegistrationNun="456987",CabTypeId=2,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null},
                new TbCabDetail(){Cabid=9,RegistrationNun="657890",CabTypeId=1,UpdateDate=null,Status=1,CreateDate=DateTime.Now,UserId=15,CabType=null,User=null}

            };
            context.TbCabDetails.AddRange(tbCabDetails);
            context.SaveChanges();
            var tbUsers = new List<TbUser>()
            {
              new TbUser(){CreateDate=DateTime.Now,UserId=15,FirstName="Sahil",LastName="khan",EmailId="sh@gmail.com", UpdateDate=null,Password="1234",UserRoleId=2,Status=1},
              new TbUser(){CreateDate=DateTime.Now,UserId=1,FirstName="Rahil",LastName="khan",EmailId="Rh@gmail.com",UpdateDate=null,Password="12345",UserRoleId=1,Status=0}
            };
            context.TbUsers.AddRange(tbUsers);
            context.SaveChanges();

        }

        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
