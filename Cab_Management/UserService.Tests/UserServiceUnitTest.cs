
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Moq;
using Persistence;
using Service.Inteface;
using Service.Services;
using System.Reflection.Metadata;

namespace UserDetailServiceTest
{
    public class UserServiceTest
    {
        private readonly DbContextOptions<DbCabServicesContext> dbContextOptions = new DbContextOptionsBuilder<DbCabServicesContext>()
        .UseInMemoryDatabase(databaseName: "db_CabServices")
         .Options;
        DbCabServicesContext context;
        UserService Services;
        Mock<IEncrypt> encrypt;
        public UserServiceTest()
        {
            encrypt = new Mock<IEncrypt>();
            context = new DbCabServicesContext(dbContextOptions);
            context.Database.EnsureCreated();
            Services = new UserService(context, encrypt.Object);
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
        [Fact]
        public void GetAllUsers_ReturnAll()
        {
            //Act
            SeedDatabase();
            var result = Services.GetUsersDetails();
            var items = Assert.IsType<List<TbUser>>(result);
            //Assert
            Assert.Equal(2, items.Count);

           Dispose();
        }
        [Fact]
        public void Register_AddNewUser_ReturnsOk()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new TbUser()
            {
                FirstName = "kazim",
                LastName = "mohammed",
                EmailId = "kazim@gmail.com",
                Password = "1234",
                UserRoleId = 1
            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            
            var result = Services.Register(ExistingUser);
            //Assert
            Assert.True(result);
          
            Dispose();
        }
        
        [Fact]
        public void CheckExistingUser_ReturnsGoodResponse()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new TbUser()
            {
                FirstName = "jyothi",
                LastName = "matam",
                EmailId = "jyothi@gmail.com",
                Password = "12345",
                UserRoleId = 1
            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.CheckExtistUser(ExistingUser);
            //Assert
            Assert.True(result);

            Dispose();
        }
        [Fact]
        public void CheckExistingUser_Returnsok()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "12345",
               
            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.CheckExtistUser(ExistingUser);
            //Assert
            Assert.True(result);

            Dispose();
        }
        [Fact]
        public void CheckExistingUser_WrongPassword_Returnsok()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new TbUser()
            {
                EmailId = "jyothi@gmail.com",
                Password = "123456",

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.CheckExtistUser(ExistingUser);
            //Assert
            Assert.True(result);

            Dispose();
        }
        [Fact]
        public void ConfirmPassword_CorrectPassword_Returnsok()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "12345",

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.CheckConfirmPassword(ExistingUser);
            //Assert
            Assert.False(result);

            Dispose();
        }
        [Fact]
        public void ConfirmPassword_WrongPassword_Returnsok()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "123456",

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.CheckConfirmPassword(ExistingUser);
            //Assert
            Assert.False(result);

            Dispose();
        }
        [Fact]
        public void UserLogin_Returnsok()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "12345",
                ConfirmPassword = "12345"

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.UserLogin(ExistingUser);
            //Assert
            Assert.True(result);

            Dispose();
        }

        [Fact]
        public void UserLogin_WrongEmail_ReturnsbadResponse()
        {
            //Arrange
            SeedDatabase();
            var User = new Login()
            {
                EmailId = "anshika@gmail.com",
                Password = "12345",
                ConfirmPassword = "12345"

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(User.Password)).Returns(User.Password);
            var result = Services.UserLogin(User);
            //Assert
            Assert.False(result);

            Dispose();
        }
        [Fact]
        public void UserLogin_WrongPassword_ReturnsbadResponse()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "anshika@gmail.com",
                Password = "123456",
                ConfirmPassword = "12345"

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.UserLogin(ExistingUser);
            //Assert
            Assert.False(result);

            Dispose();
        }
        [Fact]
        public void ForgotPassword__ReturnsGoodResponse()
        {
            //Arrange
            SeedDatabase();
            var ExistingUser = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "3456",
                ConfirmPassword = "3456"

            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = Services.ForgotPassword(ExistingUser);
           // Assert
            Assert.True(result);

            Dispose();
        }
    }
}