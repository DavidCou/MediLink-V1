using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class SearchPractitionerRequest
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? gender { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public List<OfficeInfo> OfficeAddresses { get; set; }

        public string PractitionerSpokenLanguages { get; set; }

        public string PractitionerType { get; set; }

        public string IsAcceptingNewPatients { get; set; }

        public string LastAcceptedPatientDate { get; set; }

        public int? Rating { get; set; } = 0;

        public int PatientId { get; set; }
    }
}
