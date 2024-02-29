using MediLink.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediLink.Models
{
    public class PreferencesViewModel
    {
        public PatientPreference preferences { get; set; }

        public MultiSelectList languages { get; set; }

        public MultiSelectList officeTypes { get; set; }

        public List<int> selectedLanguageIds { get; set; } = new List<int>();

        public List<int> selectedOfficeTypeIds { get; set; } = new List<int>();

        public List<int> previousLanguages { get; set; }

        public List<int> previousOfficeTypes { get; set; }
    }
}
