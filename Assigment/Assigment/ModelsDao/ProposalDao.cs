using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class ProposalDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; } 
        public Guid ItemId { get; set; } 
        public Guid CreatedByUserId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string Payment { get; set; } = string.Empty;

        public ICollection<InvolvedPartiesDao> InvolvedParties { get; } = [];
        public ICollection<CounterProposalDao> CounterProposals { get; } = [];
    }
}
