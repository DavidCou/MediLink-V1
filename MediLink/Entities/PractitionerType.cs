using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class PractitionerType
    {
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 250)]
		public string Name { get; set; } = null!;

		[Column(TypeName = "VARCHAR")]
		[StringLength(maximumLength: 350)]
		public string Description { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;

        public Practitioner? Practitioners { get; set; }
    }
}
