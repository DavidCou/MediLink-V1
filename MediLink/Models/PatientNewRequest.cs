using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class PatientNewRequest
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(maximumLength: 150)]
        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsEmailConfirmed { get; set; } = false;

        public string token { get; set; } = string.Empty;


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

        public DateTime DoB { get; set; }

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

    }
}
