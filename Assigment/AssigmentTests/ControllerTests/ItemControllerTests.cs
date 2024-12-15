using Assigment.Controllers;
using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
namespace AssigmentTests.ControllerTests
{
    public class ItemControllerTests
    {
        private readonly Mock<IItemService> _itemServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ItemController _controller;

        public ItemControllerTests()
        {
            _itemServiceMock = new Mock<IItemService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ItemController(_itemServiceMock.Object, _mapperMock.Object);
        }

        #region GetItems
        [Test]
        public async Task GetItems_ReturnsOk_WhenValidRequest()
        {
            // Arrange
            var itemFilterDto = new ItemFilterDto();
            var item = new Item();
            var items = new List<Item> { new() };
            var itemsDto = new List<ItemDto> { new() };

            _mapperMock.Setup(m => m.Map<Item>(itemFilterDto)).Returns(item);
            _itemServiceMock.Setup(s => s.GetFilteredAndSortedItems(item, It.IsAny<string>())).ReturnsAsync(items);
            _mapperMock.Setup(m => m.Map<List<ItemDto>>(items)).Returns(itemsDto);

            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };

            // Act
            var result = await _controller.GetItems(itemFilterDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult!.Value, Is.EqualTo(itemsDto));
        }

        [Test]
        public async Task GetItems_ReturnsUnauthorized_WhenUserNotAuthenticated()
        {
            // Arrange
            var itemFilterDto = new ItemFilterDto();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.GetItems(itemFilterDto);

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult!.Value, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task GetItems_ReturnsBadRequest_WhenArgumentExceptionThrown()
        {
            // Arrange
            var itemFilterDto = new ItemFilterDto();
            var item = new Item();

            _mapperMock.Setup(m => m.Map<Item>(itemFilterDto)).Returns(item);
            _itemServiceMock.Setup(s => s.GetFilteredAndSortedItems(item, It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException("Invalid argument"));

            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };

            // Act
            var result = await _controller.GetItems(itemFilterDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Invalid argument"));
        }

        [Test]
        public async Task GetItems_ReturnsInternalServerError_WhenExceptionThrown()
        {
            // Arrange
            var itemFilterDto = new ItemFilterDto();
            var item = new Item();

            _mapperMock.Setup(m => m.Map<Item>(itemFilterDto)).Returns(item);
            _itemServiceMock.Setup(s => s.GetFilteredAndSortedItems(item, It.IsAny<string>()))
                .ThrowsAsync(new Exception("Internal server error"));

            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };

            // Act
            var result = await _controller.GetItems(itemFilterDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult!.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult.Value, Is.EqualTo("Internal server error"));
            });
        }
        #endregion

        #region GetAllItemsFromMyParty
        [Test]
        public async Task GetAllItemsFromMyParty_ReturnsUnauthorized_WhenUserEmailIsNull()
        {
            // Arrange: Create a controller with no user email claim
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.GetAllItemsFromMyParty();

            // Assert
            Assert.That(result, Is.InstanceOf<UnauthorizedObjectResult>());
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.That(unauthorizedResult?.Value, Is.EqualTo("User not found."));
        }

        [Test]
        public async Task GetAllItemsFromMyParty_ReturnsOk_WithItems()
        {
            // Arrange
            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
            var items = new List<Item>
            {
                new() { Id = Guid.NewGuid(), Name = "Item 1", CreatedDate = DateTime.Now, IsShared = true, PartyIds = [Guid.NewGuid()] }
            };
            var itemDtos = new List<ItemDto>
            {
                new() { Id = items[0].Id, Name = items[0].Name!, CreatedDate = items[0].CreatedDate, IsShared = items[0].IsShared, PartyIds = items[0].PartyIds }
            };

            // Mock service and mapper
            _itemServiceMock.Setup(service => service.GetAllItemsFromMyParty(It.IsAny<string>())).ReturnsAsync(items);
            _mapperMock.Setup(mapper => mapper.Map<List<ItemDto>>(items)).Returns(itemDtos);

            // Act
            var result = await _controller.GetAllItemsFromMyParty();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(itemDtos));
        }

        [Test]
        public async Task GetAllItemsFromMyParty_ReturnsBadRequest_WhenKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
            _itemServiceMock.Setup(service => service.GetAllItemsFromMyParty(It.IsAny<string>())).ThrowsAsync(new KeyNotFoundException("Items not found"));

            // Act
            var result = await _controller.GetAllItemsFromMyParty();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Items not found"));
        }

        [Test]
        public async Task GetAllItemsFromMyParty_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var userId = "test@example.com";
            var claims = new List<Claim> { new(ClaimTypes.Email, userId) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) }
            };
            _itemServiceMock.Setup(service => service.GetAllItemsFromMyParty(It.IsAny<string>())).ThrowsAsync(new Exception("Internal error"));

            // Act
            var result = await _controller.GetAllItemsFromMyParty();

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
                Assert.That(objectResult?.Value, Is.EqualTo("Internal error"));
            });
        }
        #endregion

        #region CreateItem
        [Test]
        public async Task CreateItem_ReturnsOk_WhenItemIsCreatedSuccessfully()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "New Item" };
            var item = new Item { Id = Guid.NewGuid(), Name = createItemDto.Name };
            var itemDto = new ItemDto { Id = item.Id, Name = item.Name };

            _mapperMock.Setup(mapper => mapper.Map<Item>(createItemDto)).Returns(item);
            _itemServiceMock.Setup(service => service.AddItem(item)).ReturnsAsync(item);
            _mapperMock.Setup(mapper => mapper.Map<ItemDto>(item)).Returns(itemDto);

            // Act
            var result = await _controller.CreateItem(createItemDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo(itemDto));
        }

        [Test]
        public async Task CreateItem_ReturnsNotFound_WhenKeyNotFoundExceptionIsThrown()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "New Item" };
            _mapperMock.Setup(mapper => mapper.Map<Item>(createItemDto)).Returns(new Item { Name = createItemDto.Name });
            _itemServiceMock.Setup(service => service.AddItem(It.IsAny<Item>())).ThrowsAsync(new KeyNotFoundException("Item not found"));

            // Act
            var result = await _controller.CreateItem(createItemDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult?.Value, Is.EqualTo("Item not found"));
        }

        [Test]
        public async Task CreateItem_ReturnsBadRequest_WhenItemNameAlreadyExists()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "Existing Item" };
            _mapperMock.Setup(mapper => mapper.Map<Item>(createItemDto)).Returns(new Item { Name = createItemDto.Name });
            _itemServiceMock.Setup(service => service.AddItem(It.IsAny<Item>())).ThrowsAsync(new DbUpdateException("The name of the item is already used"));

            // Act
            var result = await _controller.CreateItem(createItemDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("The name of the item is already used"));
        }

        [Test]
        public async Task CreateItem_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createItemDto = new CreateItemDto { Name = "New Item" };
            _mapperMock.Setup(mapper => mapper.Map<Item>(createItemDto)).Returns(new Item { Name = createItemDto.Name });
            _itemServiceMock.Setup(service => service.AddItem(It.IsAny<Item>())).ThrowsAsync(new Exception("Internal error"));

            // Act
            var result = await _controller.CreateItem(createItemDto);

            // Assert
            Assert.That(result, Is.InstanceOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult?.StatusCode, Is.EqualTo(500));
        }
        #endregion
    }
}
