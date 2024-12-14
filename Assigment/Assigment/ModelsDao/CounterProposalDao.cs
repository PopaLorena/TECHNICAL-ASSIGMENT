using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assigment.ModelsDao
{
    public class CounterProposalDao
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; } 
        public Guid ProposalId { get; set; } 
        public Guid CreatedByUserId { get; set; }
        public string Payment { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        public ICollection<InvolvedPartiesDao> InvolvedParties { get; } = [];
    }
}
