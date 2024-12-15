using Assigment.Interfaces.RepositoryInterfaces;
using Assigment.Models;
using Assigment.Services;
using Moq;

namespace AssigmentTests.ServiceTests
{
    internal class ItemServiceTests
    {
        private readonly Mock<IItemRepository> _itemRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPartyRepository> _partyRepositoryMock;
        private readonly ItemService _itemService;

        public ItemServiceTests()
        {
            _itemRepositoryMock = new Mock<IItemRepository>();
            _partyRepositoryMock = new Mock<IPartyRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _itemService = new ItemService(_itemRepositoryMock.Object, _userRepositoryMock.Object, _partyRepositoryMock.Object);
        }

        #region AddItem

        [Test]
        public async Task AddItem_ShouldAddItem_WhenValidDataIsPassed()
        {
            // Arrange
            var item = new Item
            {
                Name = "Test Item",
                PartyIds = [Guid.NewGuid(), Guid.NewGuid()]
            };

            var newItemId = Guid.NewGuid();

            // Mock repository methods
            _partyRepositoryMock.Setup(repo => repo.FindParty(It.IsAny<Guid>())).ReturnsAsync(new Party());  // Simulating that parties exist
            _itemRepositoryMock.Setup(repo => repo.AddItem(It.IsAny<Item>())).ReturnsAsync(newItemId);
            _itemRepositoryMock.Setup(repo => repo.AddItemToParty(It.IsAny<List<Guid>>(), It.IsAny<Guid>())).Returns(Task.CompletedTask);
            _itemRepositoryMock.Setup(repo => repo.GetItemById(It.IsAny<Guid>())).ReturnsAsync(item);

            // Act
            var result = await _itemService.AddItem(item);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(item.Name));
            _itemRepositoryMock.Verify(repo => repo.AddItem(It.IsAny<Item>()), Times.Once);
            _partyRepositoryMock.Verify(repo => repo.FindParty(It.IsAny<Guid>()), Times.Exactly(item.PartyIds.Count));
            _itemRepositoryMock.Verify(repo => repo.AddItemToParty(It.IsAny<List<Guid>>(), It.IsAny<Guid>()), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetItemById(It.IsAny<Guid>()), Times.Once);
        }

        [Test]
        public void AddItem_ShouldThrowKeyNotFoundException_WhenPartyNotFound()
        {
            // Arrange
            var item = new Item
            {
                Name = "Test Item",
                PartyIds = [Guid.NewGuid()]
            };

            // Mock repository methods
            _partyRepositoryMock.Setup(repo => repo.FindParty(It.IsAny<Guid>())).ReturnsAsync((Party)null); 

            // Act & Assert
            Assert.ThrowsAsync<KeyNotFoundException>(() => _itemService.AddItem(item));
        }

        #endregion

        #region GetAllItemsFromMyParty

        [Test]
        public async Task GetAllItemsFromMyParty_ShouldReturnItems_WhenUserAndItemsExist()
        {
            // Arrange
            var userId = "testuser@example.com";
            var user = new UserModel
            {
                Email = userId,
                PartyId = Guid.NewGuid()
            };
            var items = new List<Item>
            {
                new() { Id = Guid.NewGuid(), Name = "Item 1" },
                new() { Id = Guid.NewGuid(), Name = "Item 2" }
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _itemRepositoryMock.Setup(repo => repo.GetAllItemsByPartyId(user.PartyId)).ReturnsAsync(items);

            // Act
            var result = await _itemService.GetAllItemsFromMyParty(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(items.Count));
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Name, Is.EqualTo(items[0].Name));
                Assert.That(result[1].Name, Is.EqualTo(items[1].Name));
            });
        }

        [Test]
        public async Task GetAllItemsFromMyParty_ShouldReturnEmptyList_WhenUserHasNoItems()
        {
            // Arrange
            var userId = "testuser@example.com";
            var user = new UserModel
            {
                Email = userId,
                PartyId = Guid.NewGuid()
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _itemRepositoryMock.Setup(repo => repo.GetAllItemsByPartyId(user.PartyId)).ReturnsAsync([]);

            // Act
            var result = await _itemService.GetAllItemsFromMyParty(userId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);

            _userRepositoryMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            _itemRepositoryMock.Verify(repo => repo.GetAllItemsByPartyId(user.PartyId), Times.Once);
        }

        #endregion

        #region GetFilteredAndSortedItems
        [Test]
        public async Task GetFilteredAndSortedItems_ShouldReturnSortedItems_WhenValidSortOrderAndSortByAreProvided()
        {
            // Arrange
            var userId = "testuser@example.com";
            var user = new UserModel { Email = userId, PartyId = Guid.NewGuid() };

            var items = new List<Item>
            {
                new() { Id = Guid.NewGuid(), Name = "Item A", CreatedDate = DateTime.UtcNow.AddDays(-1), IsShared = true },
                new() { Id = Guid.NewGuid(), Name = "Item B", CreatedDate = DateTime.UtcNow, IsShared = false }
            };

            var filterItem = new Item
            {
                SortBy = "Name",
                SortOrder = "asc"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _itemRepositoryMock.Setup(repo => repo.GetAllItemsByPartyId(user.PartyId)).ReturnsAsync(items);

            // Act
            var result = await _itemService.GetFilteredAndSortedItems(filterItem, userId);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.First().Name, Is.EqualTo("Item A"));
                Assert.That(result.Last().Name, Is.EqualTo("Item B"));
            });
        }

        [Test]
        public async Task GetFilteredAndSortedItems_ShouldThrowArgumentException_WhenInvalidSortByIsProvided()
        {
            // Arrange
            var userId = "testuser@example.com";
            var filterItem = new Item
            {
                SortBy = "InvalidSortField", // Invalid SortBy value
                SortOrder = "asc"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _itemService.GetFilteredAndSortedItems(filterItem, userId));
            Assert.That(ex.Message, Is.EqualTo("Invalid value for SortBy. Allowed values are: 'Name', 'CreatedDate', 'IsShared'."));
        }

        [Test]
        public async Task GetFilteredAndSortedItems_ShouldThrowArgumentException_WhenInvalidSortOrderIsProvided()
        {
            // Arrange
            var userId = "testuser@example.com";
            var filterItem = new Item
            {
                SortBy = "Name",
                SortOrder = "invalidOrder" // Invalid SortOrder value
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _itemService.GetFilteredAndSortedItems(filterItem, userId));
            Assert.That(ex.Message, Is.EqualTo("Invalid value for SortOrder. Allowed values are: 'asc', 'desc'."));
        }

        [Test]
        public async Task GetFilteredAndSortedItems_ShouldReturnFilteredItems_WhenValidFiltersAreProvided()
        {
            // Arrange
            var userId = "testuser@example.com";
            var user = new UserModel { Email = userId, PartyId = Guid.NewGuid() };

            var items = new List<Item>
            {
                new() { Id = Guid.NewGuid(), Name = "Item A", CreatedDate = DateTime.UtcNow.AddDays(-1), IsShared = true },
                new() { Id = Guid.NewGuid(), Name = "Item B", CreatedDate = DateTime.UtcNow, IsShared = false }
            };

            var filterItem = new Item
            {
                Name = "Item A", // Filtering by Name
                SortBy = "CreatedDate",
                SortOrder = "asc"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _itemRepositoryMock.Setup(repo => repo.GetAllItemsByPartyId(user.PartyId)).ReturnsAsync(items);

            // Act
            var result = await _itemService.GetFilteredAndSortedItems(filterItem, userId);

            // Assert
            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Item A"));
        }

        [Test]
        public async Task GetFilteredAndSortedItems_ShouldReturnEmptyList_WhenNoItemsMatchFilters()
        {
            // Arrange
            var userId = "testuser@example.com";
            var user = new UserModel { Email = userId, PartyId = Guid.NewGuid() };

            var items = new List<Item>
            {
                new() { Id = Guid.NewGuid(), Name = "Item A", CreatedDate = DateTime.UtcNow.AddDays(-1), IsShared = true },
                new() { Id = Guid.NewGuid(), Name = "Item B", CreatedDate = DateTime.UtcNow, IsShared = false }
            };

            var filterItem = new Item
            {
                Name = "NonExistentItem", // No items will match
                SortBy = "Name",
                SortOrder = "asc"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
            _itemRepositoryMock.Setup(repo => repo.GetAllItemsByPartyId(user.PartyId)).ReturnsAsync(items);

            // Act
            var result = await _itemService.GetFilteredAndSortedItems(filterItem, userId);

            // Assert
            Assert.That(result, Is.Empty);
        }

        #endregion
    }
}
