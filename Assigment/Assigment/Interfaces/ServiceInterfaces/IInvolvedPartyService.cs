namespace Assigment.Interfaces.ServiceInterfaces
{
    /// <summary>
    /// Handling the response of involved parties to proposals.
    /// </summary>
    public interface IInvolvedPartyService
    {
        /// <summary>
        /// Allows an involved party to respond to a proposal.
        /// </summary>
        /// <param name="proposalId">The ID of the proposal to respond to.</param>
        /// <param name="userId">The ID of the user responding to the proposal.</param>
        /// <param name="response">The response to the proposal (true for acceptance, false for rejection).</param>
        Task RespondToProposal(Guid proposalId, string userId, bool response);
    }

}
