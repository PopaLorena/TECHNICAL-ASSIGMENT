﻿using Assigment.ModelsDto.CreateModels;

namespace Assigment.Models
{
    public class Proposal
    {
        public Guid Id { get; set; } 
        public Guid ItemId { get; set; }

        public Guid CreatedByUserId { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public string Payment { get; set; } = string.Empty;
        public string PaymentType { get; set; } = string.Empty;

        public List<InvolvedParties> InvolvedParties { get; set; } = [];

    }
}
