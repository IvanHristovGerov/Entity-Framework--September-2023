﻿using Medicines.Data.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medicines.Data.Models
{
    public class Medicine
    {

     
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public DateTime ProductionDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string Producer { get; set; }

        [Required]
        public int PharmacyId { get; set; }
        [ForeignKey(nameof(PharmacyId))]
        public Pharmacy Pharmacy { get; set; }

        public ICollection<PatientMedicine> PatientsMedicines { get; set; }

    }
}