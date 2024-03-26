using MediLink.Entities;

namespace MediLink.Models
{
    public class WalkClinicViewModel
    {
        public string CurrentWaitTime { get; set; }

        public WalkInClinic WalkInClinic { get; set; }

        public List<WalkInPractitionerSpokenLanguages> WalkInPractitionerSpokenLanguages { get; set; }

        public OfficeAddress OfficeAddress { get; set; }

        public WalkInClinicHoursInfo WalkInClinicHours { get; set; }
    }

}
