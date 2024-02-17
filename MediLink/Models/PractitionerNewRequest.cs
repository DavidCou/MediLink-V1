using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class PractitionerNewRequest
    {
        public int Id { get; set; }
        
        public string Username { get; set; } = null!;
       
        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsValidated { get; set; }

        public bool passwordReset { get; set; } = false;

        public string token { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        
        public string FirstName { get; set; } = null!;
       
        public string LastName { get; set; } = null!;

        public string? gender { get; set; }
       
        public string PhoneNumber { get; set; } = null!;

        public DateTime lastPatientAcceptedDate { get; set; }

        public bool IsAcceptingNewPatients { get; set; }
    }
}
