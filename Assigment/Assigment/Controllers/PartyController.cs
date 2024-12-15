using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Assigment.Controllers
{
    /// <summary>
    /// Web API Controller for Party operations.
    /// </summary>
    [Route("api/Party")]
    [ApiController]
    public class PartyController : ControllerBase
    {
        private readonly IPartyService partyService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public PartyController(IPartyService partyService, IMapper mapper)
        {
            this.partyService = partyService ?? throw new ArgumentNullException(nameof(partyService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Creates a new party.
        /// </summary>
        /// <param name="createPartyDto">The details of the party to create.</param>
        /// <returns>The created party.</returns>
        /// <response code="200">Successfully created the party.</response>
        /// <response code="400">Bad request - The name of the party is already used.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Creates a new party.")]
        [ProducesResponseType(typeof(PartyDto), 200)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
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
