using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assigment.Controllers
{
    [Route("api/Party")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService partyService;
        private readonly IMapper mapper;

        public PartyController(IPartyService partyService, IMapper mapper)
        {
            this.partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost]
        public async Task<IActionResult> CreateParty(CreatePartyDto createPartyDto)
        {
            try
            {
                var party = mapper.Map<Party>(createPartyDto);
                party = await partyService.AddParty(party).ConfigureAwait(false);

                var partyDto = mapper.Map<PartyDto>(party);
                return Ok(partyDto);
            }
            catch(DbUpdateException)
            {
                return BadRequest("The name of the party is already used");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
