using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediLink.Entities
{
    public class OfficeAddress
    {
        public int Id { get; set; }

        public string OfficeName { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 150)]
        public string City { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 50)]
        public string Province { get; set; } = "Ontario";

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 7)]
        public string country { get; set; } = "Canada";

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 7)]
        public string PostalCode { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 250)]
        public string StreetAddress { get; set; } = null!;

        public bool IsDeleted { get; set; }        

        public ICollection<PractitionerOfficeAddress>? PractitionerAddress { get; set; }

        public WalkInClinic? WalkInClinic { get; set; }

        public int OfficeTypeId { get; set; }

        public OfficeType OfficeType { get; set; }
    }
}
