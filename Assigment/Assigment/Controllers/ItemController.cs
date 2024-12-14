using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto;
using Assigment.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace Assigment.Controllers
{
    [Route("api/item")]
    [ApiController]
    //[Authorize]
    public class ItemController : ControllerBase
    {
        private readonly IItemService itemService;
        private readonly IMapper mapper;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            this.itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("filterItems")]
        public async Task<IActionResult> GetItems([FromQuery] ItemDto itemDto, [FromQuery] string? SortBy, [FromQuery] string? SortOrder)
        {
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
            {
                return Unauthorized("User not found.");
            }
            var item = mapper.Map<Item>(itemDto);
            var itemsQuery = await itemService.GetFilteredAndSortedItems(item, userEmail, SortBy, SortOrder).ConfigureAwait(false);
            var itemsDto = mapper.Map<List<ItemDto>>(itemsQuery);

            return Ok(itemsDto);
        }

        [HttpGet]
        [Route("getAllItemsFromMyParty")]
        public async Task<IActionResult> GetAllItemsFromMyParty()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail == null)
                {
                    return Unauthorized("User not found.");
                }
                var items = await itemService.GetAllItemsFromMyParty(userEmail).ConfigureAwait(false);
                var itemsDto = mapper.Map<List<ItemDto>>(items);
                return Ok(itemsDto);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemDto itemDto)
        {
            try
            {
                var item = mapper.Map<Item>(itemDto);
                await itemService.AddItem(item).ConfigureAwait(false);

                return Created();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
