﻿namespace Assigment.Models
{
    public class Party
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<InvolvedParties> InvolvedParties { get; set; } = [];

    }
}
