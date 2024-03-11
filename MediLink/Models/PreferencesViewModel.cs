using MediLink.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediLink.Models
{
    public class PreferencesViewModel
    {
        public PatientPreference preferences { get; set; }

        public MultiSelectList languages { get; set; }

        public List<OfficeType> officeTypes { get; set; }

        public List<int> selectedLanguageIds { get; set; }

        public List<int> selectedOfficeTypeIds { get; set; } = new List<int>();

        public List<int> previousLanguages { get; set; } = new List<int>();

        public List<int> previousOfficeTypes { get; set; } = new List<int>();

        public string previousCity { get; set; }

        public int previousRating { get; set; }
    }
}
