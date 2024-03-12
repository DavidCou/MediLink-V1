using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class Patient
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 150)]
		public string Password { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsEmailConfirmed { get; set; } = false;

        public bool passwordReset { get; set; } = false;

        public string token { get; set; } = string.Empty;

        public int? PatientPreferencesId { get; set; }
               
        public PatientPreference? PatientPreferences { get; set; }

        public int? PatientDetailsId { get; set; }

		public PatientDetail? PatientDetails { get; set; }

        public ICollection<NewPatientRequest>? NewPatientRequests { get; set; }

        public ICollection<PractitionerPatient>? PractitionerPatients { get; set; }

        public ICollection<PractitionerReview>? PractitionerReviews { get; set; }

		public ICollection<WaitList>? WaitLists { get; set; }


	}
}
