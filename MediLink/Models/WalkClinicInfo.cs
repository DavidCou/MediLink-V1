using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class WalkClinicInfo
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? ClinicNotes { get; set; }       
                
        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsValidated { get; set; }

        public bool passwordReset { get; set; } = false;

        public string token { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public int? CurrentWaitTime { get; set; }

        public string? fullAddress { get; set; }

        public string? OfficeName { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }

        public string? country { get; set; }

        public string? PostalCode { get; set; }

        public string? StreetAddress { get; set; }

        public string? zone { get; set; }

        public string? listLanguages { get; set; }

        public List<Languages>? languagesInfo { get; set; }
    }
}
