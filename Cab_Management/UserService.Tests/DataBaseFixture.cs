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
                new TbUser(){UserId = 22, FirstName = "jyothi",LastName = "matam",EmailId = "jyothi@gmail.com",Password = "12345",UserRoleId = 1,CreateDate = DateTime.Now,UpdateDate = null,Status = 1},
                new TbUser(){UserId = 23, FirstName = "avez",LastName = "md",EmailId = "avz@gmail.com",Password = "123456",UserRoleId = 1,CreateDate = DateTime.Now,UpdateDate = null,Status = 1}
            };
            context.TbUsers.AddRange(user);
            context.SaveChanges();
        }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}

