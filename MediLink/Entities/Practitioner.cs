namespace MediLink.Entities
{
    public class Practitioner
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsValidated { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int PractitionerType_ID { get; set; }

        public int PractionerDetails_ID { get; set; }

        public PractionerDetails PractionerDetails { get; set; }

        public PractitionerType PractitionerType { get; set; }
    }
}
