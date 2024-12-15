using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.ModelsDao;
using Assigment.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Text;

namespace AssigmentTests.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> configurationMock;
        private readonly UserService userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            configurationMock = new Mock<IConfiguration>();

            // Setup mock configuration values
            configurationMock.Setup(c => c["Jwt:Key"]).Returns("testkey123");
            configurationMock.Setup(c => c["Jwt:Issuer"]).Returns("testIssuer");
            configurationMock.Setup(c => c["Jwt:Audience"]).Returns("testAudience");

            userService = new UserService(configurationMock.Object, _userRepositoryMock.Object);
        }

        #region Login
        [Test]
        public async Task Login_InvalidUser_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userModel = new UserModel
            {
                Name = "John Doe",
                Email = "wrongemail@example.com",
                Password = "wrongpassword",
                PasswordHash = new byte[64],
                PasswordSalt = new byte[64],
                PartyId = new Guid()
            };
            _userRepositoryMock.Setup(r => r.GetUserById(userModel.Email)).ReturnsAsync(userModel);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await userService.Login(userModel));
            Assert.That(ex.Message, Is.EqualTo("Wrong credentials"));
        }

        [Test]
        public async Task Login_InvalidPassword_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var userModel = new UserModel { Email = "user@example.com", Password = "wrongpassword", PasswordHash = Encoding.UTF8.GetBytes("correctHash"), PasswordSalt = new byte[64] };
            _userRepositoryMock.Setup(r => r.GetUserById(userModel.Email)).ReturnsAsync(userModel);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await userService.Login(userModel));
            Assert.That(ex.Message, Is.EqualTo("Wrong credentials"));
        }

        #endregion

        #region Register
        [Test]
        public async Task Register_ValidUser_ShouldReturnUserModel()
        {
            // Arrange
           var userModel = new UserModel
            {
                Name = "John Doe",
                Email = "user@example.com",
                PasswordHash = new byte[64], 
                PasswordSalt = new byte[64], 
                PartyId = new Guid()
            };

            _userRepositoryMock.Setup(r => r.Register(It.IsAny<UserDao>())).ReturnsAsync(userModel);

            // Act
            var result = await userService.Register(userModel);
            
            // Assert
            Assert.Multiple(() =>
            {
               
                Assert.That(result.Email, Is.EqualTo(userModel.Email));
                Assert.That(result.Name, Is.EqualTo(userModel.Name));
                Assert.That(result.PartyId, Is.EqualTo(userModel.PartyId));
            });
        }
        #endregion
    }
}
