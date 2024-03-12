namespace MediLink.Entities
{
    public class PractitionerReview
    {
        public int Id { get; set; }

        public string PractitionerFirstName { get; set; }

        public string PractitionerLastName { get; set; }

        public string PractitionerEmail { get; set; }

        public string PatientFirstName { get; set; }

        public string? Review { get; set; }

        public bool IsPractitionerOnTime { get; set; }

        public int Rating { get; set; }

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }
    }
}
