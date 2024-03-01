using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class PatientUpdateViewModel
    {
        public List<Languages> Languages { get; set; }

        public string Email { get; set; }

        public List<int>? SpokenLanguageIds { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? gender { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime? DoB { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }

        public string? country { get; set; }

        public string? PostalCode { get; set; }

        public string? StreetAddress { get; set; }
    }
}
