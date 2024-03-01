using MediLink.Entities;

namespace MediLink.Models
{
    public class PractitionerViewModel
    {
        public Practitioner Practitioner { get; set; }

        public List<OfficeAddress> OfficeAddresses { get; set; }

        public List<PractitionerSpokenLanguages> PractitionerSpokenLanguages { get; set; }

        public PractitionerType PractitionerType { get; set; }

    }
}
