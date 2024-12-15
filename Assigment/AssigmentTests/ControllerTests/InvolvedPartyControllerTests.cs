using Moq;
using Assigment.Controllers;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AssigmentTests.ControllerTests
{
    [TestFixture]
    public class InvolvedPartyControllerTests
    {
        private Mock<IInvolvedPartyService> _mockInvolvedPartyService;
        private Mock<ICounterProposalService> _mockCounterProposalService;
        private Mock<IMapper> _mockMapper;
        private InvolvedPartyController _controller;

        [SetUp]
        public void Setup()
        {
            _mockInvolvedPartyService = new Mock<IInvolvedPartyService>();
            _mockCounterProposalService = new Mock<ICounterProposalService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new InvolvedPartyController(_mockInvolvedPartyService.Object, _mockMapper.Object, _mockCounterProposalService.Object);

            // Mock User Claims
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user-id-123") }))
                }
            };
        }

        #region AcceptProposal
        [Test]
        public async Task AcceptProposal_ShouldReturnOkResult_WhenValidProposalId()
        {
            // Arrange
            var proposalId = Guid.NewGuid();

            _mockInvolvedPartyService.Setup(service => service.RespondToProposal(proposalId, It.IsAny<string>(), true))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AcceptProposal(proposalId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Proposal accepted.", okResult.Value);
        }

        [Test]
        public async Task AcceptProposal_ShouldReturnUnauthorized_WhenUserIdNotFound()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.AcceptProposal(Guid.NewGuid());

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task AcceptProposal_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var proposalId = Guid.NewGuid();
            _mockInvolvedPartyService.Setup(service => service.RespondToProposal(proposalId, It.IsAny<string>(), true))
                .Throws(new Exception("Internal error"));

            // Act
            var result = await _controller.AcceptProposal(proposalId);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        #endregion

        #region RejectProposal

        [Test]
        public async Task RejectProposal_ShouldReturnOkResult_WhenValidCounterProposal()
        {
            // Arrange
            var counterProposalRequest = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };
            var counterProposal = new CounterProposal();
            var counterProposalDto = new CounterProposalDto();

            _mockInvolvedPartyService.Setup(service => service.RespondToProposal(counterProposalRequest.ProposalId, It.IsAny<string>(), false))
                .Returns(Task.CompletedTask);

            _mockMapper.Setup(mapper => mapper.Map<CounterProposal>(counterProposalRequest)).Returns(counterProposal);
            _mockCounterProposalService.Setup(service => service.AddCounterProposal(counterProposal, It.IsAny<string>()))
                .ReturnsAsync(counterProposal);

            _mockMapper.Setup(mapper => mapper.Map<CounterProposalDto>(counterProposal)).Returns(counterProposalDto);

            // Act
            var result = await _controller.RejectProposal(counterProposalRequest);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual("Proposal rejected, and counter-proposal created.", okResult.Value);
        }

        [Test]
        public async Task RejectProposal_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
        {
            // Arrange
            var counterProposalRequest = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };

            _mockInvolvedPartyService.Setup(service => service.RespondToProposal(counterProposalRequest.ProposalId, It.IsAny<string>(), false))
                .Returns(Task.CompletedTask);

            _mockCounterProposalService.Setup(service => service.AddCounterProposal(It.IsAny<CounterProposal>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Invalid data"));

            // Act
            var result = await _controller.RejectProposal(counterProposalRequest);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid data", badRequestResult.Value);
        }

        [Test]
        public async Task RejectProposal_ShouldReturnUnauthorized_WhenUserIdNotFound()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();
            var counterProposalRequest = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };

            // Act
            var result = await _controller.RejectProposal(counterProposalRequest);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }

        [Test]
        public async Task RejectProposal_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var counterProposalRequest = new CreateCounterProposalDto { ProposalId = Guid.NewGuid() };

            _mockInvolvedPartyService.Setup(service => service.RespondToProposal(counterProposalRequest.ProposalId, It.IsAny<string>(), false))
                .Returns(Task.CompletedTask);

            _mockCounterProposalService.Setup(service => service.AddCounterProposal(It.IsAny<CounterProposal>(), It.IsAny<string>()))
                .Throws(new Exception("Internal error"));

            // Act
            var result = await _controller.RejectProposal(counterProposalRequest);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }

        #endregion
    }
}
