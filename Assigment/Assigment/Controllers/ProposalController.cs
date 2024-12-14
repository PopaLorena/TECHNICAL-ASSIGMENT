using Assigment.Interfaces.ServiceInterfaces;
using Assigment.Models;
using Assigment.ModelsDto.CreateModels;
using Assigment.ModelsDto.GetModels;
using Assigment.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Assigment.Controllers
{
    [Route("api/Proposal")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService proposalService;
        private readonly IMapper mapper;

        public ProposalController(IProposalService proposalService, IMapper mapper)
        {
            this.proposalService = proposalService ?? throw new ArgumentNullException(nameof(proposalService));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Route("getAllNegotiationDetails")]
        public async Task<IActionResult> GetAllNegotiationDetails([FromQuery] Guid itemId)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail == null)
                {
                    return Unauthorized("User not found.");
                }

                var negotiations = await proposalService.GetAllNegotiationDetails(userEmail, itemId).ConfigureAwait(false);
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

        [HttpPost]
        public async Task<IActionResult> CreateProposal(CreateProposalDto createProposalDto)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (userEmail == null)
                {
                    return Unauthorized("User not found.");
                }
                var proposal = mapper.Map<Proposal>(createProposalDto);
                proposal = await proposalService.AddProposal(proposal, userEmail).ConfigureAwait(false);

                var partyDto = mapper.Map<ProposalDto>(proposal);
                return Ok(partyDto);
            }
            catch (DbUpdateException)
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
