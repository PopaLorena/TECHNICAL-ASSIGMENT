using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Assigment.Controllers
{
    /// <summary>
    /// Web API Controller for Item operations
    /// </summary>
    [Route("api/item")]
    [ApiController]
    [Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public ItemController(IItemService itemService, IMapper mapper)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets a filtered and sorted list of items.
        /// </summary>
        /// <param name="itemFilterDto">Filter and sorting parameters for the items.</param>
        /// <returns>A list of filtered and sorted items.</returns>
        /// <response code="200">Successfully retrieved the filtered items.</response>
        /// <response code="401">Unauthorized - User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("filterItems")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets a filtered and sorted list of items.")]
        [ProducesResponseType(typeof(List<ItemDto>), 200)] 
        [ProducesResponseType(401)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> GetItems([FromQuery] ItemFilterDto itemFilterDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }
                var item = mapper.Map<Item>(itemFilterDto);
                var items = await itemService.GetFilteredAndSortedItems(item, userId).ConfigureAwait(false);
                var itemsDto = mapper.Map<List<ItemDto>>(items);

                return Ok(itemsDto);

            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Gets all items from the user's party.
        /// </summary>
        /// <returns>A list of items from the user's party.</returns>
        /// <response code="200">Successfully retrieved the items.</response>
        /// <response code="401">Unauthorized - User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet("getAllItemsFromMyParty")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Gets all items from the user's party.")]
        [ProducesResponseType(typeof(List<ItemDto>), 200)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> GetAllItemsFromMyParty()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }
                var items = await itemService.GetAllItemsFromMyParty(userId).ConfigureAwait(false);
                var itemsDto = mapper.Map<List<ItemDto>>(items);
                return Ok(itemsDto);
            }
            catch (KeyNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="createItemDto">The details of the item to create.</param>
        /// <returns>The created item.</returns>
        /// <response code="200">Successfully created the item.</response>
        /// <response code="400">Bad request - Invalid data.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new item.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ItemDto), 200)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> CreateItem(CreateItemDto createItemDto)
        {
            try
            {
                var item = mapper.Map<Item>(createItemDto);

                var newItem = await itemService.AddItem(item).ConfigureAwait(false);

                var itemDto = mapper.Map<ItemDto>(newItem);

                return Ok(itemDto);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (DbUpdateException)
            {
                return BadRequest("The name of the item is already used");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
