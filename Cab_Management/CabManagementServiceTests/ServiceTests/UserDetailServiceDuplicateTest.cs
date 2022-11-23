using Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services;
using CabManagementServiceTests.Service;
using Domain;
using Microsoft.Identity.Client;

namespace CabManagementServiceTests.ServiceTests
{
    public class UserDetailServiceDuplicateTest
    {
        private readonly IUserDetails _service;

        public UserDetailServiceDuplicateTest()
        {
            _service = new UserDetailServiceDuplicate();
        }
        [Fact]
        public void Get_WhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _service.GetUsersDetails();

            // Assert
            var items = Assert.IsType<List<TbUser>>(okResult);
            Assert.Equal(3, items.Count);
        }
        [Fact]
        public void Check_ExtistingUser_ReturnsGoodRequest()
        {
            // Arrange
            var ExtistingUser = new TbUser()
            {
                FirstName = "xxx",
                LastName = "xxx",
                EmailId = "gokul@gmail.com",
                Password = "yyy",
                UserRoleId = 1,
            };

            // Act
            var goodResponse = _service.CheckExtistUser(ExtistingUser);

            // Assert
            Assert.False(goodResponse);
        }
        [Fact]
        public void Check_NewUser_ReturnsGoodRequest()
        {
            // Arrange
            var NewUser = new TbUser()
            {
                FirstName = "anshika",
                LastName = "agarwal",
                EmailId = "anshika@gmail.com",
                Password = "3456",
               UserRoleId = 2,
            };

            // Act
            var goodResponse = _service.CheckExtistUser(NewUser);

            // Assert
            Assert.False(goodResponse);
        }
        [Fact]
        public void Add_correct_ConfirmPassword_ReturnsOkResult()
        {
            // Arrange
            var testUser = new Login()
            {
                EmailId = "gokul@gmail.com",
                Password = "yyy",
                ConfirmPassword = "yyy"  
            };

            // Act
            var goodResponse = _service.CheckConfirmPassword(testUser);

            // Assert
            Assert.True(goodResponse);
        }
        [Fact]
        public void Add_Incorrect_ConfirmPassword_ReturnsBadRequest()
        {
            // Arrange
            var ExtistingUser = new Login()
            {
                
                EmailId = "anshika@gmail.com",
                Password = "3456",
                ConfirmPassword = "1234",
               
            };

            // Act
            var badResponse = _service.CheckConfirmPassword(ExtistingUser);

            // Assert
            Assert.False(badResponse);
        }
        [Fact]
        public void Register_newUser_ReturnsOkResult()
        {
            //Arrange
            var newUser = new TbUser()
            {
                FirstName = "diptesh",
                LastName = "Patra",
                EmailId = "diptesh@gmail.com",
                Password = "4567",
                UserRoleId = 1
            };
            //Act
            var goodResponse = _service.Register(newUser);
            //Assert
            Assert.True(goodResponse);
        }
        [Fact]
        public void Register_ExistingUser_ReturnsBadRequest()
        {
            //Arrange
            var User = new TbUser()
            {
                FirstName = "jyothi",
                LastName = "V",
                EmailId = "jyothi@gmail.com",
                Password = "1234",
                UserRoleId = 1
            };
            //Act
            var badResponse = _service.Register(User);
            //Assert
            Assert.True(badResponse);
        }
        [Fact]
        public void Add_New_User_Passed_ReturnsCreatedResponse()
        {
            // Arrange
            var testUser = new Login()
            {
                EmailId = "anshika@gmail.com",
                Password = "3456",
                ConfirmPassword = "3456",

            };
            // Act
            var createdResponse = _service.CheckConfirmPassword(testUser);

            // Assert
            Assert.True(createdResponse);
        }

        [Fact]
        public void Login_Correct_Password_ReturnOkRequest()
        {
            // Arrange
            var CorrectPassword = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "1234",
            };
            // Act
            var goodResponse = _service.CheckExtistUser(CorrectPassword);

            // Assert
            Assert.True(goodResponse);
        }

        [Fact]
        public void Login_Exiting_User_ReturnOkRequest()
        {
            // Arrange
            var CorrectPassword = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "1234",
            };
            // Act
            var goodResponse = _service.UserLogin(CorrectPassword);

            // Assert
            Assert.False(goodResponse);
        }

        [Fact]
        public void Login_NotExiting_User_ReturnOkRequest()
        {
            // Arrange
            var CorrectPassword = new Login()
            {
                EmailId = "anshika@gmail.com",
                Password = "1234",
            };
            // Act
            var badResponse = _service.UserLogin(CorrectPassword);

            // Assert
            Assert.False(badResponse);
        }
        [Fact]
        public void Login_NonExiting_User_ReturnOkRequest()
        {
            // Arrange
            var CorrectPassword = new Login()
            {
                EmailId = "diptesh@gmail.com",
                Password = "4567",
            };
            // Act
            var goodResponse = _service.UserLogin(CorrectPassword);

            // Assert
            Assert.False(goodResponse);
        }
        [Fact]
        public void Login_Wrong_Password_ReturnsBadRequest()
        {
            // Arrange
            var WrongPassword = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "4321",
            };

            // Act
            var badResponse = _service.UserLogin(WrongPassword);

            // Assert
            Assert.False(badResponse);
        }

        [Fact]
        public void Forgot_Password_ReturnsUpdatedResponse()
        {
            // Arrange
            var WrongPassword = new Login()
            {
                EmailId = "jyothi@gmail.com",
                Password = "4321",
                ConfirmPassword = "4321"
            };

            // Act
            var badResponse = _service.ForgotPassword(WrongPassword);

            // Assert
            Assert.True(badResponse);
        }
    }
}

    

