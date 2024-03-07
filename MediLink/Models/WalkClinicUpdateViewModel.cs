using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class WalkClinicUpdateViewModel
    {
        public List<Languages>? Languages { get; set; }

        public List<WalkInPractitionerSpokenLanguages> CurrentSpokenLanguages { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; } = null!;

        public string? ClinicNotes { get; set; }       

        public string OfficeName { get; set; }

        public string StreetAddress { get; set; }

        public string? UnitNumber { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public string Province { get; set; }

        public string Country { get; set; }
    }
}
