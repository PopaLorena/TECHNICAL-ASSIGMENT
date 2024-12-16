using Assigment.Controllers;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AssigmentTests.ControllerTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<IMapper> _mockMapper;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockMapper = new Mock<IMapper>();

            // Initialize controller with mocks
            _controller = new UserController(_mockUserService.Object, _mockMapper.Object);
        }

        #region Login

        [Test]
        public async Task Login_ValidUser_ReturnsOk()
        {
            // Arrange
            var loginUserDto = new LoginUserDto
            {
                Email = "Email@gmail.com",
                Password = "password"
            };
            var userModel = new UserModel
            {
                Email = "Email@gmail.com",
                Password = "password"
            };
            var token = "fake_token";

            _mockMapper.Setup(m => m.Map<UserModel>(loginUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Login(userModel)).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(loginUserDto);

            // Assert
            var actionResult = result as OkObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(200));
                Assert.That(actionResult.Value, Is.EqualTo(token));
            });
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var loginUserDto = new LoginUserDto
            {
                Email = "Email@gmail.com",
                Password = "password"
            };
            var userModel = new UserModel
            {
                Email = "Email@gmail.com",
                Password = "password"
            };

            _mockMapper.Setup(m => m.Map<UserModel>(loginUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Login(userModel)).ThrowsAsync(new InvalidOperationException("Invalid credentials"));

            // Act
            var result = await _controller.Login(loginUserDto);

            // Assert
            var actionResult = result as BadRequestObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(400));
                Assert.That(actionResult.Value, Is.EqualTo("Invalid credentials"));
            });
        }

        [Test]
        public async Task Login_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var loginUserDto = new LoginUserDto
            {
                Email = "Email@gmail.com",
                Password = "password"
            };
            var userModel = new UserModel
            {
                Email = "Email@gmail.com",
                Password = "password"
            };

            _mockMapper.Setup(m => m.Map<UserModel>(loginUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Login(userModel)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.Login(loginUserDto);

            // Assert
            var actionResult = result as ObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        }
        #endregion

        #region Register

        [Test]
        public async Task Register_ValidModel_ReturnsOk()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                PartyId = Guid.NewGuid()
            };

            var userModel = new UserModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                PartyId = Guid.NewGuid()
            }; 

            var userDto = new UserDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                PartyId = Guid.NewGuid()
            };

            _mockMapper.Setup(m => m.Map<UserModel>(createUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Register(userModel)).ReturnsAsync(userModel);
            _mockMapper.Setup(m => m.Map<UserDto>(userModel)).Returns(userDto);

            // Act
            var result = await _controller.Register(createUserDto);

            // Assert
            var actionResult = result as OkObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(200));
                Assert.That(actionResult.Value, Is.EqualTo(userDto));
            });
        }

        [Test]
        public async Task Register_EmailAlreadyUsed_ReturnsBadRequest()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                PartyId = Guid.NewGuid()
            };

            var userModel = new UserModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                PartyId = Guid.NewGuid()
            };

            _mockMapper.Setup(m => m.Map<UserModel>(createUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Register(userModel)).ThrowsAsync(new DbUpdateException());

            // Act
            var result = await _controller.Register(createUserDto);

            // Assert
            var actionResult = result as BadRequestObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(actionResult.StatusCode, Is.EqualTo(400));
                Assert.That(actionResult.Value, Is.EqualTo("The email is used by another user"));
            });
        }

        [Test]
        public async Task Register_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var createUserDto = new CreateUserDto
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                PartyId = Guid.NewGuid()
            };

            var userModel = new UserModel
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Password = "Password123!",
                PartyId = Guid.NewGuid()
            };

            _mockMapper.Setup(m => m.Map<UserModel>(createUserDto)).Returns(userModel);
            _mockUserService.Setup(s => s.Register(userModel)).ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.Register(createUserDto);

            // Assert
            var actionResult = result as ObjectResult;
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult.StatusCode, Is.EqualTo(500));
        }
        #endregion
    }
}
