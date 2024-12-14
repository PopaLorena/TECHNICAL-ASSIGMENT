using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> CreateParty(PartyDto partyDto)
        {
            try
            {
                var party = mapper.Map<Party>(partyDto);
                await partyService.AddParty(party).ConfigureAwait(false);

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
