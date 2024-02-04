namespace MediLink.Entities
{
    public class PreferedSpokenLanguages
    {
        public int Id { get; set; }

        public int PatientPreferences_ID { get; set; }

        public int Language_ID { get; set; }

        public PatientPreferences PatientPreferences { get; set; }

        public Languages Languages { get; set; }
    }
}
