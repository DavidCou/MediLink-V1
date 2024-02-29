using MediLink.Entities;

namespace MediLink.Models
{
    public class PatientViewModel
    {
        public string? Email { get; set; }

        public List<string>? SpokenLanguages { get; set; }

        public List<string>? LanguageNames { get; set; }

        public PatientDetail? PatientDetail { get; set; }

        public PatientAddress? PatientAddress { get; set; }
    }
}
