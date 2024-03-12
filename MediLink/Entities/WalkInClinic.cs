using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class WalkInClinic
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

		public int? CurrentWaitTime { get; set; }

		public int? HistoricalWaitTimeMin { get; set; } //To Be Removed - No longer needed

		public int? HistoricalWaitTimeMax { get; set; } //To Be Removed - No longer needed

        public ICollection<WalkInPractitionerSpokenLanguages> WalkInPractitionerSpokenLanguages { get; set; }

        public ICollection<WalkInClinicHours>? WalkInClinicHours { get; set; }

        public ICollection<WalkInClinicCheckedInPatient>? WalkInClinicCheckedInPatients { get; set; }

        public ICollection<WalkInClinicHistoricalWaitTimes> WalkInClinicHistoricalWaitTimes { get; set; }

        public int OfficeAddressId {  get; set; }

		public OfficeAddress OfficeAddress { get; set; }
    }
}
