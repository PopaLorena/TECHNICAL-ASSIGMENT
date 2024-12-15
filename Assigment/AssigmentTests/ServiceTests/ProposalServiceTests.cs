using Moq;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.Services;

namespace AssigmentTests.ServiceTests
{
    [TestFixture]
    public class ProposalServiceTests
    {
        private Mock<IProposalRepository> _mockProposalRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IInvolvedPartyRepository> _mockInvolvedPartyRepository;
        private Mock<IPartyRepository> _mockPartyRepository;
        private ProposalService _proposalService;

        [SetUp]
        public void Setup()
        {
            _mockProposalRepository = new Mock<IProposalRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockInvolvedPartyRepository = new Mock<IInvolvedPartyRepository>();
            _mockPartyRepository = new Mock<IPartyRepository>();
            _proposalService = new ProposalService(
                _mockProposalRepository.Object,
                _mockUserRepository.Object,
                _mockInvolvedPartyRepository.Object,
                _mockPartyRepository.Object);
        }

        #region AddProposal

        [Test]
        public async Task AddProposal_ShouldAddProposalAndReturnCreatedProposal()
        {
            // Arrange
            var userId = "user-123";
            var proposal = new Proposal { PaymentType = "Percentages", Payment = "50" };
            var createdProposal = new Proposal { Id = Guid.NewGuid(), Payment = "50%" };
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var parties = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

            _mockUserRepository.Setup(r => r.GetUserById(userId))
                .ReturnsAsync(user);

            _mockProposalRepository.Setup(r => r.AddProposal(It.IsAny<Proposal>()))
                .ReturnsAsync(createdProposal);

            _mockPartyRepository.Setup(r => r.GetPartiesByItemIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(parties);

            _mockProposalRepository.Setup(r => r.GetProposalById(It.IsAny<Guid>()))
                .ReturnsAsync(createdProposal);

            

            // Act
            var result = await _proposalService.AddProposal(proposal, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createdProposal.Id, result.Id);
            _mockInvolvedPartyRepository.Verify(r => r.AddInvolvedParty(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Exactly(parties.Count));
        }

        [Test]
        public void AddProposal_ShouldThrowException_WhenInvalidPaymentType()
        {
            // Arrange
            var userId = "user-123";
            var proposal = new Proposal { PaymentType = "invalid", Payment = "50" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _proposalService.AddProposal(proposal, userId));
            Assert.AreEqual("Invalid value for PaymentType. Allowed values are: 'Amounts', 'Percentages'.", ex.Message);
        }

        [Test]
        public void AddProposal_ShouldThrowException_WhenInvalidPercentage()
        {
            // Arrange
            var userId = "user-123";
            var proposal = new Proposal { PaymentType = "Percentages", Payment = "500" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _proposalService.AddProposal(proposal, userId));
            Assert.AreEqual("Invalid value for Payment.", ex.Message);
        }

        #endregion
    }
}

