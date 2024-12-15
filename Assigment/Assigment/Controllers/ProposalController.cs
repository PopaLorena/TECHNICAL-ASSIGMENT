using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Assigment.Controllers
{
    /// <summary>
    /// Web API Controller for Proposal operations.
    /// </summary>
    [Route("api/Proposal")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService proposalService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public ProposalController(IProposalService proposalService, IMapper mapper)
        {
            this.proposalService = proposalService ?? throw new ArgumentNullException(nameof(proposalService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Retrieves all negotiation details for a specific item.
        /// </summary>
        /// <param name="itemId">The ID of the item to retrieve negotiations for.</param>
        /// <returns>A list of proposals related to the item.</returns>
        /// <response code="200">Successfully retrieved negotiation details.</response>
        /// <response code="400">Bad request due to invalid item ID or other issues.</response>
        /// <response code="500">Internal server error.</response>
        [HttpGet]
        [Route("getAllNegotiationDetails")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Retrieves all negotiation details for a specific item.")]
        [ProducesResponseType(typeof(List<ProposalDto>), 200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllNegotiationDetails([FromQuery] Guid itemId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }

                var negotiations = await proposalService.GetAllNegotiationDetails(userId, itemId).ConfigureAwait(false);
                var negotiationsDto = mapper.Map<List<ProposalDto>>(negotiations);

                return Ok(negotiationsDto);
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
        /// Creates a new proposal.
        /// </summary>
        /// <param name="createProposalDto">The details of the proposal to create.</param>
        /// <returns>The created proposal.</returns>
        /// <response code="200">Successfully created the proposal.</response>
        /// <response code="400">Bad request - Invalid proposal data or item already has a proposal.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Creates a new proposal.")]
        [ProducesResponseType(typeof(ProposalDto), 200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateProposal(CreateProposalDto createProposalDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }
                var proposal = mapper.Map<Proposal>(createProposalDto);
                proposal = await proposalService.AddProposal(proposal, userId).ConfigureAwait(false);

                var proposalDto = mapper.Map<ProposalDto>(proposal);
                return Ok(proposalDto);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (DbUpdateException)
            {
                return BadRequest("This Item already has a proposal, please submit a counter proposal");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
    }
}
