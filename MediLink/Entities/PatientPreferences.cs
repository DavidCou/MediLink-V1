namespace MediLink.Entities
{
    public class PatientPreferences
    {
        public int Id { get; set; }

        public string location { get; set; }

        public  int rating { get; set; }

        public bool IsAcceptingNePatients { get; set; }

        public bool IsDeleted { get; set; }

        public int OfficeType_ID { get; set; }

        public OfficeType OfficeType { get; set; }
        
        public ICollection<PreferedSpokenLanguages>? PreferedSpokenLanguages { get; set; }
    }
}
