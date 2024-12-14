using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class UserDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }

        public Guid PartyId { get; set; }
        public ICollection<ProposalDao> Proposals { get; } = [];
        public ICollection<CounterProposalDao> CounterProposals { get; } = [];
    }
}
