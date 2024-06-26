﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class PatientAddress
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 150)]
		public string? City { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 50)]
		public string? Province { get; set; } = "Ontario";

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 7)]
		public string? country { get; set; } = "Canada";

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 7)]
		public string? PostalCode { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 250)]
		public string? StreetAddress { get; set; } = null!;

		public bool IsDeleted { get; set; } = false;

        public PatientDetail? PatientDetails { get; set; }








    }
}
