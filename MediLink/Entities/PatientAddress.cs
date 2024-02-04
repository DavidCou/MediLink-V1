namespace MediLink.Entities
{
    public class PatientAddress
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Province { get; set; } = "Ontario";

        public string country { get; set; } = "Canada";

        public string PostalCode { get; set; }

        public string StreetAddress { get; set; }

        public bool IsDeleted { get; set; }

    }
}
