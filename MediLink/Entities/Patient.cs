namespace MediLink.Entities
{
    public class Patient
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int PatietDetails_ID { get; set; }

       public PatientDetails PatientDetails { get; set; }
    }
}
