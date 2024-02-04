namespace MediLink.Entities
{
    public class PractionerDetails
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? gender { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime lastPatientAcceptedDate { get; set; }

        public bool IsAcceptingNewPatients { get; set; }

        public bool IsDeleted { get; set; }

        public int OfficeType_ID { get; set; }

        public int PractionerAddress_ID { get; set; }

        public OfficeType OfficeType { get; set; }

        public PractionerAddress PractionerAddress { get; set; }

        public ICollection<PractitionerSpokenLanguages>? PractitionerSpokenLanguages { get; set; }

    }
}
