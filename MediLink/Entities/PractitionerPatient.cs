namespace MediLink.Entities
{
    public class PractitionerPatient
    {
        public int Id { get; set; }

        public bool IsCurrent { get; set; }

        public bool IsDeleted { get; set; }

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }
    }
}
