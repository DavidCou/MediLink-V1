namespace MediLink.Entities
{
    public class WaitList
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; } 

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }
    }
}
