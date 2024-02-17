namespace MediLink.Entities
{
    public class PreferedLanguage
    {

        public int PatientPreferenceId { get; set; }

        public int LanguageId { get; set; }

        public Languages Language { get; set; } = null!;

        public PatientPreference PatientPreference { get; set; } = null!;
    }
}
