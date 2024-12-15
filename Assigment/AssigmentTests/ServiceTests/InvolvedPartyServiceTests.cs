using Moq;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.Services;

namespace AssigmentTests.ServiceTests
{
    [TestFixture]
    public class InvolvedPartyServiceTests
    {
        private Mock<IProposalRepository> _mockProposalRepository;
        private Mock<ICounterProposalRepository> _mockCounterProposalRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IInvolvedPartyRepository> _mockInvolvedPartyRepository;
        private InvolvedPartyService _involvedPartyService;

        [SetUp]
        public void Setup()
        {
            _mockProposalRepository = new Mock<IProposalRepository>();
            _mockCounterProposalRepository = new Mock<ICounterProposalRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockInvolvedPartyRepository = new Mock<IInvolvedPartyRepository>();
            _involvedPartyService = new InvolvedPartyService(
                _mockCounterProposalRepository.Object,
                _mockProposalRepository.Object,
                _mockInvolvedPartyRepository.Object,
                _mockUserRepository.Object);
        }

        [Test]
        public async Task RespondToProposal_ShouldThrowKeyNotFoundException_WhenProposalDoesNotExist()
        {
            // Arrange
            var proposalId = Guid.NewGuid();
            var userId = "user-123";
            _mockProposalRepository.Setup(repo => repo.GetProposalById(proposalId))
                .ReturnsAsync((Proposal)null);
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await _involvedPartyService.RespondToProposal(proposalId, userId, true));
            Assert.That(ex.Message, Is.EqualTo("the Proposal does not exist"));
        }

        [Test]
        public async Task RespondToProposal_ShouldThrowInvalidOperationException_WhenUserOwnsProposal()
        {
            // Arrange
            var proposalId = Guid.NewGuid();
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = user.PartyId, InvolvedParties = new List<InvolvedParties>() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(proposalId)).ReturnsAsync(proposal);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _involvedPartyService.RespondToProposal(proposalId, userId, true));
            Assert.AreEqual("Can’t accept my own proposal", ex.Message);
        }

        [Test]
        public async Task RespondToProposal_ShouldThrowKeyNotFoundException_WhenPartyNotInvolved()
        {
            // Arrange
            var proposalId = Guid.NewGuid();
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal
            {
                CreatedByUserId = Guid.NewGuid(),
                InvolvedParties = new List<InvolvedParties>()
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(proposalId)).ReturnsAsync(proposal);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _involvedPartyService.RespondToProposal(proposalId, userId, true));
            Assert.AreEqual("Your Party is not involved in this Proposal", ex.Message);
        }

        [Test]
        public async Task RespondToProposal_ShouldCallRepository_WhenValidResponse()
        {
            // Arrange
            var proposalId = Guid.NewGuid();
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var involvedParty = new InvolvedParties { PartyId = user.PartyId };
            var proposal = new Proposal
            {
                CreatedByUserId = Guid.NewGuid(),
                InvolvedParties = new List<InvolvedParties> { involvedParty }
            };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(proposalId)).ReturnsAsync(proposal);
            _mockInvolvedPartyRepository.Setup(repo => repo.RespontToProposal(involvedParty, user, true))
                .Returns(Task.CompletedTask);

            // Act
            await _involvedPartyService.RespondToProposal(proposalId, userId, true);

            // Assert
            _mockInvolvedPartyRepository.Verify(repo => repo.RespontToProposal(involvedParty, user, true), Times.Once);
        }
    }
}
