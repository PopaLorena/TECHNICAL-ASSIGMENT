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
using Microsoft.EntityFrameworkCore;

namespace AssigmentTests.ControllerTests
{
    [TestFixture]
    public class ProposalControllerTests
    {
        private Mock<IProposalService> _mockProposalService;
        private Mock<IMapper> _mockMapper;
        private ProposalController _controller;

        [SetUp]
        public void Setup()
        {
            _mockProposalService = new Mock<IProposalService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ProposalController(_mockProposalService.Object, _mockMapper.Object);

            // Mock User Claims
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "user-id-123") }))
                }
            };
        }

        #region GetAllNegotiationDetails
        [Test]
        public async Task GetAllNegotiationDetails_ShouldReturnOkResult_WhenValidItemId()
        {
            // Arrange
            var itemId = Guid.NewGuid();
            var negotiations = new List<Proposal> { new Proposal() };
            var negotiationsDto = new List<ProposalDto> { new ProposalDto() };

            _mockProposalService.Setup(service => service.GetAllNegotiationDetails(It.IsAny<string>(), itemId))
                .ReturnsAsync(negotiations);

            _mockMapper.Setup(mapper => mapper.Map<List<ProposalDto>>(It.IsAny<List<Proposal>>()))
                .Returns(negotiationsDto);

            // Act
            var result = await _controller.GetAllNegotiationDetails(itemId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(negotiationsDto, okResult.Value);
        }

        [Test]
        public async Task GetAllNegotiationDetails_ShouldReturnUnauthorized_WhenUserIdNotFound()
        {
            // Arrange
            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal();

            // Act
            var result = await _controller.GetAllNegotiationDetails(Guid.NewGuid());

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorizedResult);
            Assert.AreEqual(401, unauthorizedResult.StatusCode);
        }
        #endregion

        #region CreateProposal

        [Test]
        public async Task CreateProposal_ShouldReturnOkResult_WhenValidData()
        {
            // Arrange
            var createProposalDto = new CreateProposalDto();
            var proposal = new Proposal();
            var proposalDto = new ProposalDto();

            _mockMapper.Setup(mapper => mapper.Map<Proposal>(createProposalDto)).Returns(proposal);
            _mockProposalService.Setup(service => service.AddProposal(proposal, It.IsAny<string>()))
                .ReturnsAsync(proposal);
            _mockMapper.Setup(mapper => mapper.Map<ProposalDto>(proposal)).Returns(proposalDto);

            // Act
            var result = await _controller.CreateProposal(createProposalDto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(proposalDto, okResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnBadRequest_WhenItemAlreadyHasProposal()
        {
            // Arrange
            var createProposalDto = new CreateProposalDto();
            var proposal = new Proposal();

            _mockMapper.Setup(mapper => mapper.Map<Proposal>(createProposalDto)).Returns(proposal);
            _mockProposalService.Setup(service => service.AddProposal(proposal, It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            // Act
            var result = await _controller.CreateProposal(createProposalDto);

            // Assert
            var badRequestResult = result as ObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnBadRequest_WhenArgumentException()
        {
            // Arrange
            var createProposalDto = new CreateProposalDto();
            var proposal = new Proposal();

            _mockMapper.Setup(mapper => mapper.Map<Proposal>(createProposalDto)).Returns(proposal);
            _mockProposalService.Setup(service => service.AddProposal(proposal, It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Invalid proposal data"));

            // Act
            var result = await _controller.CreateProposal(createProposalDto);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual(400, badRequestResult.StatusCode);
            Assert.AreEqual("Invalid proposal data", badRequestResult.Value);
        }

        [Test]
        public async Task CreateProposal_ShouldReturnInternalServerError_WhenException()
        {
            // Arrange
            var createProposalDto = new CreateProposalDto();
            var proposal = new Proposal();

            _mockMapper.Setup(mapper => mapper.Map<Proposal>(createProposalDto)).Returns(proposal);
            _mockProposalService.Setup(service => service.AddProposal(proposal, It.IsAny<string>()))
                .ThrowsAsync(new Exception("Internal error"));

            // Act
            var result = await _controller.CreateProposal(createProposalDto);

            // Assert
            var statusCodeResult = result as ObjectResult;
            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(500, statusCodeResult.StatusCode);
        }
        #endregion
    }
}
