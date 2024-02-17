using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class Languages
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 50)]
		public string LanguageName { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public ICollection<PractitionerSpokenLanguages>? PractitionerSpokenLanguages { get; set; }   

        public ICollection<PreferedLanguage>? PreferedLanguages { get; set; }

        public ICollection<PatientSpokenLanguage>? PatientSpokenLanguages { get; set; }


    }
}
