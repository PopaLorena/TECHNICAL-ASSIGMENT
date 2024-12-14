using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Controllers;
using Assigment.ModelsDto.CreateModels;
using Assigment.Models;
using Microsoft.EntityFrameworkCore;
using Assigment.ModelsDto.GetModels;

namespace AssigmentTests.ControllerTests
{
    public class PartyControllerTests
    {
        private readonly Mock<IPartyService> mockPartyService;
        private readonly Mock<IMapper> mockMapper;
        private readonly PartyController controller;

        public PartyControllerTests()
        {
            mockPartyService = new Mock<IPartyService>();
            mockMapper = new Mock<IMapper>();
            controller = new PartyController(mockPartyService.Object, mockMapper.Object);
        }
        #region createParty
        [Test]
        public async Task CreateParty_ReturnsOk_WhenValidInput()
        {
            // Arrange
            var createPartyDto = new CreatePartyDto { Name = "Birthday Party" };
            var party = new Party { Name = "Birthday Party" };
            var partyDto = new PartyDto { Name = "Birthday Party" };

            mockMapper.Setup(m => m.Map<Party>(createPartyDto)).Returns(party);
            mockPartyService.Setup(s => s.AddParty(party)).ReturnsAsync(party);
            mockMapper.Setup(m => m.Map<PartyDto>(party)).Returns(partyDto);

            // Act
            var result = await controller.CreateParty(createPartyDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult!.StatusCode, Is.EqualTo(200));
                Assert.That(okResult.Value, Is.EqualTo(partyDto));
            });
        }

        [Test]
        public async Task CreateParty_ReturnsBadRequest_WhenDbUpdateExceptionThrown()
        {
            // Arrange
            var createPartyDto = new CreatePartyDto { Name = "Duplicate Party" };
            var party = new Party { Name = "Duplicate Party" };

            mockMapper.Setup(m => m.Map<Party>(createPartyDto)).Returns(party);
            mockPartyService.Setup(s => s.AddParty(party)).ThrowsAsync(new DbUpdateException());

            // Act
            var result = await controller.CreateParty(createPartyDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("The name of the party is already used"));
        }

        [Test]
        public async Task CreateParty_ReturnsInternalServerError_WhenUnhandledExceptionThrown()
        {
            // Arrange
            var createPartyDto = new CreatePartyDto { Name = "Party" };
            var party = new Party { Name = "Party" };

            mockMapper.Setup(m => m.Map<Party>(createPartyDto)).Returns(party);
            mockPartyService.Setup(s => s.AddParty(party)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await controller.CreateParty(createPartyDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult!.StatusCode, Is.EqualTo(500));
            StringAssert.Contains("Some error", statusCodeResult.Value!.ToString());
        }
        #endregion

    }
}