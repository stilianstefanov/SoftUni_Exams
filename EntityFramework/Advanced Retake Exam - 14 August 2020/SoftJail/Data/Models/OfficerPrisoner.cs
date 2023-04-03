﻿namespace SoftJail.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class OfficerPrisoner
    {
        [ForeignKey(nameof(Prisoner))]
        public int PrisonerId { get; set; }

        public Prisoner Prisoner { get; set; } = null!;

        [ForeignKey(nameof(Officer))]
        public int OfficerId { get; set; }

        public Officer Officer { get; set; } = null!;
    }
}