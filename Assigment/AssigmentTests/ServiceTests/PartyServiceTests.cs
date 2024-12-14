using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.Services;
using Moq;

namespace AssigmentTests.ServiceTests
{
    public class PartyServiceTests
    {
        private readonly Mock<IPartyRepository> _mockPartyRepository;
        private readonly PartyService _partyService;

        public PartyServiceTests()
        {
            _mockPartyRepository = new Mock<IPartyRepository>();
            _partyService = new PartyService(_mockPartyRepository.Object);
        }

        [Test]
        public async Task AddParty_ShouldCallPartyRepositoryAddParty_WhenPartyIsProvided()
        {
            // Arrange
            var party = new Party { Id = new Guid(), Name = "Test Party" };
            _mockPartyRepository.Setup(repo => repo.AddParty(It.IsAny<Party>())).ReturnsAsync(party);

            // Act
            var result = await _partyService.AddParty(party);

            // Assert
            _mockPartyRepository.Verify(repo => repo.AddParty(It.IsAny<Party>()), Times.Once);
            Assert.That(result, Is.EqualTo(party));
        }
    }
}
