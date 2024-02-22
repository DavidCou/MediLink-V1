using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class PatientPreference
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 200)]
		public string? location { get; set; }

		public int? rating { get; set; }
                
        public bool IsDeleted { get; set; } = false;

		public Patient Patients { get; set; } = null!;

		public ICollection<PatientOfficeType>? PatientOfficeType { get; set; }

		public ICollection<PreferedLanguage>? PreferedLanguages { get; set; }

	}
}
