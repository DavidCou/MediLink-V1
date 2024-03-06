using MediLink.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediLink.Models
{
    public class PractitionerSearchViewModel
    {

        public string city {  get; set; }

        public MultiSelectList languages { get; set; }

        public MultiSelectList officeTypes { get; set; }

        public List<int> selectedLanguageIds { get; set; } = new List<int>();

        public List<int> selectedOfficeTypeIds { get; set; } = new List<int>();

        public int minimumRating { get; set; }

        public List<Practitioner> practitioners { get; set; } = new List<Practitioner>();
    }
}
