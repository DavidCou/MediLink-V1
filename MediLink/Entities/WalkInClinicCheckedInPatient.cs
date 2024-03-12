namespace MediLink.Entities
{
    public class WalkInClinicCheckedInPatient
    {
        public int Id { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set;}

        public DateTime PatientCheckInTime { get; set;}

        public int WalkInClinicId { get; set; }

        public WalkInClinic WalkInClinic { get; set; }
    }
}
