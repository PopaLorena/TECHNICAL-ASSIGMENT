using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assigment.Controllers
{
    [Route("api/item")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetItems([FromQuery] ItemFilterDto itemFilterDto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail == null)
                {
                    return Unauthorized("User not found.");
                }
                var item = mapper.Map<Item>(itemFilterDto);
                var items = await itemService.GetFilteredAndSortedItems(item, userEmail).ConfigureAwait(false);
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
            catch (KeyNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
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
