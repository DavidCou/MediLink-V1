using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class Practitioner
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
		[StringLength(maximumLength: 250)]
		public string FirstName { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 250)]
		public string LastName { get; set; } = null!;

		public string? gender { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 12)]
		public string PhoneNumber { get; set; } = null!;

        public DateTime? lastPatientAcceptedDate { get; set; }

        public bool IsAcceptingNewPatients { get; set; }

        public ICollection<PractitionerSpokenLanguages> PractitionerSpokenLanguages { get; set; }
				
		public ICollection<PractitionerOfficeAddress> PractitionerAddress { get; set; }

        public ICollection<NewPatientRequest>? NewPatientRequests { get; set; }

        public ICollection<PractitionerPatient>? PractitionerPatients { get; set; }

        public ICollection<PractitionerReview>? PractitionerReviews { get; set; }

        public ICollection<WaitList>? WaitLists { get; set; }

        public int PractitionerTypeId { get; set; }

		public PractitionerType PractitionerType { get; set; } = null!;

        public double? rating { get; set; }

    }
}
