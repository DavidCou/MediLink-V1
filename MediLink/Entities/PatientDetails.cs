namespace MediLink.Entities
{
    public class PatientDetails
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? gender { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public int PatientAddress_ID { get; set; }

        public int PatientPreferences_ID { get; set; }

        public PatientAddress PatientAddress { get; set; }

        public PatientPreferences PatientPreferences { get; set; }

        public ICollection<PatientSpokenLanguages>? PatientSpokenLanguages { get; set; }
    }
}
