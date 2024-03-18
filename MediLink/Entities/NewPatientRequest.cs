using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Entities
{
    public class NewPatientRequest
    {
        public int Id { get; set; }

        public int officePractitionerId { get; set; }

        public DateTime DateRequest { get; set; } = DateTime.Now;

        public DateTime DateApproved { get; set; } = DateTime.Now;

        public int PatientId { get; set; }

        public int PractitionerId { get; set; }

        public Practitioner Practitioner { get; set; }

        public Patient Patient { get; set; }

        public string status { get; set; } = "pending";
    }
}
