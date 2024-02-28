using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class OfficeType
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 200)]
		public string OfficeTypeName { get; set; } = null!;

        public bool IsDeleted { get; set; }	= false;

		public ICollection<PatientOfficeType>? PatientOfficeTypes { get; set; }

        public ICollection<OfficeAddress>? OfficeAddresses { get; set; }




    }
}
