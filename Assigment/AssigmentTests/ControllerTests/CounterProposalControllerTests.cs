using Moq;
using Assigment.Controllers;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AssigmentTests.ControllerTests
{
    [TestFixture]
    public class CounterProposalControllerTests
    {
        private Mock<ICounterProposalService> _mockCounterProposalService;
        private Mock<IMapper> _mockMapper;
        private Mock<ILogger<CounterProposalController>> _mockLogger;
        private CounterProposalController _controller;

        [SetUp]
        public void Setup()
        {
            _mockCounterProposalService = new Mock<ICounterProposalService>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CounterProposalController>>();
            _controller = new CounterProposalController(_mockCounterProposalService.Object, _mockMapper.Object, _mockLogger.Object);

            // Mock User Claims
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user-id-123") }))
                }
            };
        }

        [Test]
        public async Task CreateProposal_ShouldReturnOkResult_WhenValidInput()
        {
            // Arrange
            var createDto = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };
            var counterProposal = new CounterProposal();
            var counterProposalDto = new CounterProposalDto();

            _mockMapper.Setup(m => m.Map<CounterProposal>(createDto)).Returns(counterProposal);
            _mockCounterProposalService.Setup(s => s.AddCounterProposal(counterProposal, It.IsAny<string>()))
                .ReturnsAsync(counterProposal);
            _mockMapper.Setup(m => m.Map<CounterProposalDto>(counterProposal)).Returns(counterProposalDto);

            // Act
            var result = await _controller.CreateProposal(createDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(counterProposalDto, okResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            var createDto = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };

            // Act
            var result = await _controller.CreateProposal(createDto);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
            Assert.AreEqual("User not found.", unauthorizedResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
        {
            // Arrange
            var createDto = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };
            _mockMapper.Setup(m => m.Map<CounterProposal>(createDto)).Throws(new ArgumentException("Invalid data"));

            // Act
            var result = await _controller.CreateProposal(createDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid data", badRequestResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnNotFound_WhenKeyNotFoundExceptionOccurs()
        {
            // Arrange
            var createDto = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };
            var counterProposal = new CounterProposal();

            _mockMapper.Setup(m => m.Map<CounterProposal>(createDto)).Returns(counterProposal);
            _mockCounterProposalService.Setup(s => s.AddCounterProposal(counterProposal, It.IsAny<string>()))
                .ThrowsAsync(new KeyNotFoundException("Proposal not found"));

            // Act
            var result = await _controller.CreateProposal(createDto);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
            Assert.AreEqual("Proposal not found", notFoundResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var createDto = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };
            var counterProposal = new CounterProposal();

            _mockMapper.Setup(m => m.Map<CounterProposal>(createDto)).Returns(counterProposal);
            _mockCounterProposalService.Setup(s => s.AddCounterProposal(counterProposal, It.IsAny<string>()))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.CreateProposal(createDto);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
            Assert.AreEqual("An unexpected error occurred.", statusCodeResult.Value);
        }
    }
}

