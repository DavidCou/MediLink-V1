using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MediLink.Entities;

namespace MediLink.Models
{
    public class PractitionerUpdateViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? gender { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public bool IsAcceptingNewPatients { get; set; }

        public int PractitionerTypesId { get; set; }

        public List<PractitionerType>? practitionerTypes { get; set; }
    }
}
