using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class WalkInPractitioner
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 100)]
		public string Password { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 100)]
		public string Email { get; set; } = null!;

        public bool IsValidated { get; set; }

        public bool passwordReset { get; set; } = false;

        public string token { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 12)]
		public string PhoneNumber { get; set; } = null!;

		public string? ClinicNotes { get; set; }

		public DateTime? CurrentWaitTime { get; set; }

		public DateTime? HistoricalWaitTimeMin { get; set; }

		public DateTime? HistoricalWaitTimeMax { get; set; }

		public ICollection<WalkInPractitionerSpokenLanguages> WalkInPractitionerSpokenLanguages { get; set; }
				
		public ICollection<PractitionerOfficeAddress> PractitionerAddress { get; set; }

        public int? PractitionerTypeId { get; set; }

        public PractitionerType PractitionerType { get; set; } = null!;

        

    }
}
