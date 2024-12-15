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
    /// Web API Controller for Counter Proposal
    /// </summary>
    [Route("api/CounterProposal")]
    [ApiController]
    public class CounterProposalController : ControllerBase
    {
        private readonly ICounterProposalService counterProposalService;
        private readonly IMapper mapper;
        private readonly ILogger<CounterProposalController> logger;

        /// <summary>
        /// Constructor of the Controller
        /// </summary>
        public CounterProposalController(ICounterProposalService counterProposalService, IMapper mapper, ILogger<CounterProposalController> logger)
        {
            this.counterProposalService = counterProposalService ?? throw new ArgumentNullException(nameof(counterProposalService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates a new counter proposal.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the creation of a new counter proposal. The request body must contain all the required information 
        /// for the counter proposal. If successful, it returns the details of the created counter proposal.
        /// </remarks>
        /// <param name="createCounterProposalDto">The details of the counter proposal to be created.</param>
        /// <returns>A newly created counter proposal.</returns>
        /// <response code="200">Successfully created counter proposal</response>
        /// <response code="400">Bad request due to invalid input</response>
        /// <response code="401">Unauthorized if user is not found</response>
        /// <response code="404">Not found if resources like Proposal or Party are not found</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [SwaggerOperation(Summary = "Creates a new counter proposal", Description = "This endpoint allows the creation of a new counter proposal. The request body must contain all the required information for the counter proposal. If successful, it returns the details of the created counter proposal.")]
        [SwaggerResponse(200, "Successfully created counter proposal", typeof(CounterProposalDto))]
        [SwaggerResponse(400, "Bad request due to invalid input")]
        [SwaggerResponse(401, "Unauthorized if user is not found")]
        [SwaggerResponse(404, "Not found if resources like Proposal or Party are not found")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> CreateProposal([FromBody] CreateCounterProposalDto createCounterProposalDto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not found.");
                }
                var counterProposal = mapper.Map<CounterProposal>(createCounterProposalDto);
                counterProposal = await counterProposalService.AddCounterProposal(counterProposal, userId).ConfigureAwait(false);

                var counterProposalDto = mapper.Map<CounterProposalDto>(counterProposal);
                return Ok(counterProposalDto);
            }
            catch (ArgumentException e)
            {
                logger.LogError(e, "Invalid argument exception occurred while creating counter proposal.");
                return BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                logger.LogError(e, "Key not found exception occurred while creating counter proposal.");
                return NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                logger.LogError(e, "Invalid operation exception occurred while creating counter proposal.");
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogError(e, "An unexpected error occurred while creating counter proposal.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
