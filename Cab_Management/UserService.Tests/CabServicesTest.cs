using System;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Persistence;
using Service.Services;
using UserDetailService.Tests;

namespace CabManagementTestProject
{
    [Collection("Database Collection")]
    public class CabServicesTest
    {
       
        private readonly DataBaseFixture dataBaseFixture;
        private readonly CabDetailService service;
      public CabServicesTest(DataBaseFixture dataBaseFixture)
        {
            this.dataBaseFixture = dataBaseFixture;
            service = new CabDetailService(dataBaseFixture.context);
        }

        [Fact]
        public void Add_NewCab()
        {
            //Arrange
            var cab = new TbCabDetail()
            {

                Cabid = 6,
                RegistrationNun = "4569878",
                CabTypeId = 1,
                UpdateDate = null,
                Status = 1,
                CreateDate = DateTime.Now,
                UserId = 15,
                CabType = null,
                User = null
            };
            //Act
            try
            {
                service.AddCab(cab);
                Assert.True(true);
            }
            catch
            {
                Assert.False(false);
            }
        }

        [Fact]
        public void GetAll_CabDetails()
        {
            //Arrange
            var result = service.GetTbCabDetails();
            var count=dataBaseFixture.context.TbCabDetails.Count();
            //Act
            var items = Assert.IsType<List<TbCabDetail>>(result);
            //Assert
            Assert.Equal(count, items.Count);
        }

        [Fact]
        public void RemoveCab()
        {
            //Arrange
            var cab = new TbCabDetail()
            {
                Cabid = 9,
                RegistrationNun = "657890",
                CabTypeId = 1,
                UpdateDate = null,
                Status = 1,
                CreateDate = DateTime.Now,
                UserId = 15,
                CabType = null,
                User = null
            };

            //Act
            try
            {
                service.RemoveCab(cab);
                Assert.True(true);
            }
            //Assert
            catch
            {
                Assert.False(false);
            }
        }

        [Fact]

        public void UpdateCab()
        {
           //Arrange
            var updatecab = new TbCabDetail()
            {
                Cabid = 5,
                RegistrationNun = "456987",
                CabTypeId = 1,
            };

            //Act
            try
            {
                service.UpdateCab(updatecab);
                Assert.True(true);
            }
            catch
            {
                //Assert
                Assert.False(false);
            }
        }

        [Fact]

        public void CheckRegNum()
        {
           //Arrange
            var RegNum = new TbCabDetail()
            {
                RegistrationNun = "456987",
                CabTypeId = 2

            };

            //ACT
            var result = service.CheckRegNum(RegNum);

            //Assert
            Assert.True(result);
        }
        [Fact]
        public void newCheckRegNum()
        {
            //Arrange
            var regNum = new TbCabDetail()
            {
                RegistrationNun = "45698",
                CabTypeId = 1
            };

            //Act
            var result = service.CheckRegNum(regNum);

            //Assert
            Assert.False(result);       
        }
    }
}