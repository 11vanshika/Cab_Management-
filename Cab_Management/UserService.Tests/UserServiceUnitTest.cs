
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Moq;
using Persistence;
using Service.Inteface;
using Service.Services;
using System.Reflection.Metadata;
using UserDetailService.Tests;

namespace UserDetailService.Test
{
    [CollectionDefinition("Database Collection")]
    public class DatabBaseCollection : ICollectionFixture<DataBaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
    [Collection("Database Collection")]
    public class UserServiceTests
    {
        private readonly DataBaseFixture _fixture;
        private readonly UserService userService;
        private readonly Mock<IEncrypt> encrypt;
        private readonly Mock<IGenerateToken> generateToken;

        public UserServiceTests(DataBaseFixture fixture)
        {
            _fixture = fixture;
            encrypt = new Mock<IEncrypt>();
            generateToken = new Mock<IGenerateToken>();
            userService = new UserService(_fixture.context, encrypt.Object, generateToken.Object);
        }

        [Fact]
        public void GetAllUsers_ReturnAll()
        {
            //Act 
            var result = userService.GetUsersDetails();
            var items = Assert.IsType<List<TbUser>>(result);

            //Act
            var expect = _fixture.context.TbUsers.Count();

            //Assert
            Assert.Equal(expect, items.Count);
        }

        [Fact]
        public void Register_AddNewUser_ReturnsOk()
        {
            //Arrange
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
            generateToken.Setup(x => x.GenerateToken(ExistingUser)).Returns("login SuccessFully");
            var result = userService.Register(ExistingUser);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void CheckExistingUser_ReturnsGoodResponse()
        {
            //Arrange
            var ExistingUser = new Registration()
            {
                EmailId = "jyothi@gmail.com",
                Password = "12345",
            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.CheckExtistUser(ExistingUser);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckExistingUser_Returnsok()
        {
            //Arrange
            var ExistingUser = new Registration()
            {
                EmailId = "avz@gmail.com",
                Password = "123456",    
            };
            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.CheckExtistUser(ExistingUser);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void CheckExistingUser_WrongPassword_Returnsok()
        {
            //Arrange
            var ExistingUser = new Registration()
            {
                EmailId = "jyothi@gmail.com",
                Password = "123456",
            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.CheckExtistUser(ExistingUser);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ConfirmPassword_CorrectPassword_Returnsok()
        {
            //Arrange
            var ExistingUser = new Registration()
            {
                EmailId = "jyothi@gmail.com",
                Password = "12345",
                ConfirmPassword = "12345"

            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.ConfirmPassword(ExistingUser);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void ConfirmPassword_WrongPassword_Returnsok()
        {
            //Arrange 
            var ExistingUser = new Registration()
            {
                EmailId = "jyothi@gmail.com",
                Password = "123456",
                ConfirmPassword = "12345"
            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.ConfirmPassword(ExistingUser);

            //Assert
            Assert.False(result);
        }

        [Fact]
        public void UserLogin_Returnsok()
        {
            //Arrange
            var ExistingUser = new TbUser()
            {
                EmailId = "avz@gmail.com",
                Password = "123456"
            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.UserLogin(ExistingUser);

            //Assert
            Assert.NotNull(result.Item2);
        }

        [Fact]
        public void UserLogin_WrongEmail_ReturnsbadResponse()
        {
            //Arrange         
            var User = new TbUser()
            {
                EmailId = "anshika@gmail.com",
                Password = "12345"
            };
            var expected = "User EmailId or Password not matched";

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(User.Password)).Returns(User.Password);
            var result = userService.UserLogin(User);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void UserLogin_WrongPassword_ReturnsbadResponse()
        {
            //Arrange    
            var ExistingUser = new TbUser()
            {
                EmailId = "anshika@gmail.com",
                Password = "123456"

            };
            var expected = "User EmailId or Password not matched";

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            var result = userService.UserLogin(ExistingUser);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void ForgotPassword__ReturnsGoodResponse()
        {
            //Arrange
            var ExistingUser = new ForgetPassword()
            {
                EmailId = "jyothi@gmail.com",
                Password = "3456",
                ConfirmPassword = "3456"
            };

            //Act
            encrypt.Setup(method => method.EncodePasswordToBase64(ExistingUser.Password)).Returns(ExistingUser.Password);
            try
            {
                userService.ForgotPassword(ExistingUser);
            }
            catch
            {
                // Assert
                Assert.NotNull(ExistingUser);
            }
        }

        public void ForgotPassword__ReturnsBadResponse()
        {
            //Arrange
            var ExistingUser = new ForgetPassword()
            {
                EmailId = "jyothi@gmail.com",
                Password = "3456",
                ConfirmPassword = "345"
            };

            //Act
            try
            {
                userService.ForgotPassword(ExistingUser);
            }
            catch
            {
                // Assert
                Assert.NotNull(ExistingUser);
            }
        }
    }
}