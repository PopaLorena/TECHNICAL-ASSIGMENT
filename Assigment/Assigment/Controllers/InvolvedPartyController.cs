using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Assigment.Controllers
{
    /// <summary>
    /// Web API Controller for Involved Party
    /// </summary>
    [Route("api/InvolvedParty")]
    [ApiController]
    public class InvolvedPartyController : ControllerBase
    {
        private readonly IInvolvedPartyService involvedPartyService;
        private readonly ICounterProposalService counterProposalService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public InvolvedPartyController(IInvolvedPartyService involvedPartyService, IMapper mapper, ICounterProposalService counterProposalService)
        {
            this.counterProposalService = counterProposalService ?? throw new ArgumentNullException(nameof(counterProposalService));
            this.involvedPartyService = involvedPartyService ?? throw new ArgumentNullException(nameof(involvedPartyService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Accepts the proposal for the current user.
        /// </summary>
        /// <param name="proposalId">The proposal ID to accept.</param>
        /// <returns>Returns a success message.</returns>
        /// <response code="200">Successfully accepted the proposal.</response>
        /// <response code="401">Unauthorized - User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("accept/{proposalId}")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Accepts a proposal for the user.")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> AcceptProposal(Guid proposalId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }

                await involvedPartyService.RespondToProposal(proposalId, userId, true).ConfigureAwait(false);

                return Ok("Proposal accepted.");
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        /// <summary>
        /// Rejects the proposal and creates a counter-proposal.
        /// </summary>
        /// <param name="counterProposalRequest">The counter proposal data.</param>
        /// <returns>Returns a message indicating the proposal rejection and counter-proposal creation.</returns>
        /// <response code="200">Successfully rejected the proposal and created a counter-proposal.</response>
        /// <response code="400">Bad request - Invalid data provided.</response>
        /// <response code="401">Unauthorized - User not found.</response>
        /// <response code="500">Internal server error.</response>
        [HttpPost("reject")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Rejects the proposal and creates a counter-proposal.")]
        [ProducesResponseType(200)] 
        [ProducesResponseType(400)] 
        [ProducesResponseType(401)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> RejectProposal(CreateCounterProposalDto counterProposalRequest)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }

                await involvedPartyService.RespondToProposal(counterProposalRequest.ProposalId, userId, false).ConfigureAwait(false);

                var counterProposal = mapper.Map<CounterProposal>(counterProposalRequest);
                counterProposal = await counterProposalService.AddCounterProposal(counterProposal, userId).ConfigureAwait(false);

                var counterProposalDto = mapper.Map<CounterProposalDto>(counterProposal);

                return Ok("Proposal rejected, and counter-proposal created.");
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
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
