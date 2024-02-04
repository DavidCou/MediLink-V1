namespace MediLink.Entities
{
    public class Languages
    {
        public int Id { get; set; }

        public string LanguageName { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<PractitionerSpokenLanguages>? PractitionerSpokenLanguages { get; set; }   

        public ICollection<PatientSpokenLanguages>? PatientSpokenLanguages { get; set; }

        public ICollection<PreferedSpokenLanguages>? PreferedSpokenLanguages { get; set; }
    }
}
