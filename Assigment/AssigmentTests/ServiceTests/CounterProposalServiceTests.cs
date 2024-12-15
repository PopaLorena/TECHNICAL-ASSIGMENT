using Moq;
using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.Services;
using System.Globalization;

namespace AssigmentTests.ServiceTests
{
    [TestFixture]
    public class CounterProposalServiceTests
    {
        private Mock<ICounterProposalRepository> _mockCounterProposalRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IInvolvedPartyRepository> _mockInvolvedPartyRepository;
        private Mock<IProposalRepository> _mockProposalRepository;
        private CounterProposalService _counterProposalService;

        [SetUp]
        public void Setup()
        {
            _mockCounterProposalRepository = new Mock<ICounterProposalRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockInvolvedPartyRepository = new Mock<IInvolvedPartyRepository>();
            _mockProposalRepository = new Mock<IProposalRepository>();
            _counterProposalService = new CounterProposalService(
                _mockCounterProposalRepository.Object,
                _mockUserRepository.Object,
                _mockInvolvedPartyRepository.Object,
                _mockProposalRepository.Object);
        }

        [Test]
        public async Task AddCounterProposal_ShouldThrowKeyNotFoundException_WhenProposalDoesNotExist()
        {
            // Arrange
            var counterProposal = new CounterProposal { ProposalId = Guid.NewGuid() };
            var userId = "user-123";
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId))
                .ReturnsAsync((Proposal)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _counterProposalService.AddCounterProposal(counterProposal, userId));
            Assert.AreEqual("the Proposal does not exist", ex.Message);
        }

        [Test]
        public async Task AddCounterProposal_ShouldThrowInvalidOperationException_WhenUserOwnsProposal()
        {
            // Arrange
            var counterProposal = new CounterProposal { ProposalId = Guid.NewGuid() };
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = user.PartyId };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId)).ReturnsAsync(proposal);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _counterProposalService.AddCounterProposal(counterProposal, userId));
            Assert.AreEqual("Can’t create counter proposal to my own proposal", ex.Message);
        }

        [Test]
        public async Task AddCounterProposal_ShouldThrowArgumentException_WhenPaymentIsInvalid()
        {
            // Arrange
            var counterProposal = new CounterProposal
            {
                ProposalId = Guid.NewGuid(),
                PaymentType = "Percentages",
                Payment = "InvalidPercentage"
            };
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = Guid.NewGuid() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId)).ReturnsAsync(proposal);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _counterProposalService.AddCounterProposal(counterProposal, userId));
            Assert.AreEqual("Invalid value for Payment.", ex.Message);
        }

        [Test]
        public async Task AddCounterProposal_ShouldFormatPayment_WhenPaymentTypeIsPercentages()
        {
            // Arrange
            var counterProposal = new CounterProposal
            {
                ProposalId = Guid.NewGuid(),
                PaymentType = "Percentages",
                Payment = "50"
            };
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = Guid.NewGuid() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId)).ReturnsAsync(proposal);
            _mockCounterProposalRepository.Setup(repo => repo.AddCounterProposal(It.IsAny<CounterProposal>()))
                .ReturnsAsync(counterProposal);
            _mockCounterProposalRepository.Setup(repo => repo.GetCounterProposalById(It.IsAny<Guid>()))
               .ReturnsAsync(counterProposal);

            // Act
            var result = await _counterProposalService.AddCounterProposal(counterProposal, userId);

            // Assert
            Assert.AreEqual("50%", result.Payment);
        }

        [Test]
        public async Task AddCounterProposal_ShouldFormatPayment_WhenPaymentTypeIsAmounts()
        {
            // Arrange
            var counterProposal = new CounterProposal
            {
                ProposalId = Guid.NewGuid(),
                PaymentType = "Amounts",
                Payment = "1000"
            };
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = Guid.NewGuid() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId)).ReturnsAsync(proposal);
            _mockCounterProposalRepository.Setup(repo => repo.AddCounterProposal(It.IsAny<CounterProposal>()))
                .ReturnsAsync(counterProposal);
            _mockCounterProposalRepository.Setup(repo => repo.GetCounterProposalById(It.IsAny<Guid>()))
               .ReturnsAsync(counterProposal);

            // Act
            var result = await _counterProposalService.AddCounterProposal(counterProposal, userId);

            // Assert
            var expectedPayment = string.Format(CultureInfo.GetCultureInfo("fr-FR"), "{0:C} Euro", "1000");
            Assert.AreEqual(expectedPayment, result.Payment);
        }

        [Test]
        public async Task AddCounterProposal_ShouldAddCounterProposal_WhenValid()
        {
            // Arrange
            var counterProposal = new CounterProposal
            {
                ProposalId = Guid.NewGuid(),
                PaymentType = "Percentages",
                Payment = "25"
            };
            var userId = "user-123";
            var user = new UserModel { Id = Guid.NewGuid(), PartyId = Guid.NewGuid() };
            var proposal = new Proposal { CreatedByUserId = Guid.NewGuid() };

            _mockUserRepository.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _mockProposalRepository.Setup(repo => repo.GetProposalById(counterProposal.ProposalId)).ReturnsAsync(proposal);
            _mockCounterProposalRepository.Setup(repo => repo.AddCounterProposal(It.IsAny<CounterProposal>()))
                .ReturnsAsync(counterProposal);
            _mockCounterProposalRepository.Setup(repo => repo.GetCounterProposalById(It.IsAny<Guid>()))
               .ReturnsAsync(counterProposal);
            // Act
            var result = await _counterProposalService.AddCounterProposal(counterProposal, userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(counterProposal.ProposalId, result.ProposalId);
            Assert.AreEqual(counterProposal.Payment, result.Payment);
        }
    }
}