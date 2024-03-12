namespace MediLink.Entities
{
    public class WaitList
    {
        public int Id { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PatientEmail { get; set; } 

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }
    }
}
