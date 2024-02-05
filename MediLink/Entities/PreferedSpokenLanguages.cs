namespace MediLink.Entities
{
    public class PreferedSpokenLanguages
    {

        public int PatientPreferences_ID { get; set; }

        public int Language_ID { get; set; }

        public PatientPreferences PatientPreferences { get; set; }

        public Languages Language { get; set; }
    }
}
