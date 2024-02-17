using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
	public class PatientDetail
	{
		public int Id { get; set; }
		
		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 100)]
		public string FirstName { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 100)]
		public string LastName { get; set; } = null!;

		public string gender { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 17)]
		public string PhoneNumber { get; set; } = null!;

		public DateTime creation_Date { get; set; } = DateTime.Now;

        public DateTime DoB { get; set; }

		public bool IsDeleted { get; set; } = false;

        public int PatientAddressesId { get; set; }

        public PatientAddress? PatientAddresses { get; set; }

        public Patient Patients { get; set; } = null!;

        public ICollection<PatientSpokenLanguage>? PatientSpokenLanguages { get; set; }


    }
}
