using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class OfficeType
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 200)]
		public string OfficeName { get; set; } = null!;

        public bool IsDeleted { get; set; }	= false;

		public ICollection<PatientOfficeType>? PatientOfficeTypes { get; set; }

        //public OfficeAddress? OfficeAddressed { get; set; }

        public ICollection<PractitionerOfficeType>? PractitionerOfficeTypes { get; set; }



    }
}
