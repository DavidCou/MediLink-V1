using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class NewPatientRequest
    {
        public int Id { get; set; }

        public string PractitionerFirstName { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 100)]
        public string PractitionerLastName { get; set; } = null!;

        public string PractitionerEmail { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 100)]
        public string PatientFirstName { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 100)]
        public string PatientLastName { get; set; } = null!;

        public string PatientGender { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 17)]
        public string PatientPhoneNumber { get; set; } = null!;

        public string PatientEmail { get; set; } = null!;

        public DateTime PatientDoB { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 250)]
        public string PatientStreetAddress { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 7)]
        public string PatientPostalCode { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 150)]
        public string PatientCity { get; set; } = null!;

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 50)]
        public string PatientProvince { get; set; } = "Ontario";

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 7)]
        public string PatientCountry { get; set; } = "Canada";

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }
    }
}
