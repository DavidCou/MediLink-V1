namespace MediLink.Entities
{
    public class PatientSpokenLanguage
    {
        public int PatientDetailsId { get; set; }

        public int LanguageId { get; set; }

        public Languages Language { get; set; } = null!;

        public PatientDetail PatientDetails { get; set; } = null!;
    }
}
