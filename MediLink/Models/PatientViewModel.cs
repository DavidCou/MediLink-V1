using MediLink.Entities;

namespace MediLink.Models
{
    public class PatientViewModel
    {
        public string Email { get; set; }

        public List<PatientSpokenLanguage>? SpokenLanguages { get; set; }

        public List<Languages> Languages { get; set; }

        public PatientDetail PatientDetail { get; set; }

        public PatientAddress? PatientAddress { get; set; }
    }
}
