using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MediLink.Entities;

namespace MediLink.Models
{
    public class PractitionerUpdateViewModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Gender { get; set; }

        public string IsAcceptingNewPatients { get; set; }

        public List<Languages> Languages { get; set; }

        public List<int> CurrentSpokenLanguageIds { get; set; }

        public List<PractitionerType> PractitionerTypes { get; set; }

        public int CurrentPractitionerTypeId { get; set; }




    }
}
