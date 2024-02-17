namespace MediLink.Entities
{
    public class PractitionerSpokenLanguages
    {
        public int PractitionerId { get; set; }

        public int LanguageId { get; set; }

        public Languages Language { get; set; } = null!;

        public Practitioner Practitioner { get; set; } = null!;
    }
}
