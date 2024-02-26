namespace MediLink.Entities
{
    public class WalkInPractitionerSpokenLanguages
	{
        public int WalkInPractitionerId { get; set; }

        public int LanguageId { get; set; }

        public Languages Language { get; set; } = null!;

        public WalkInClinic WalkInPractitioner { get; set; } = null!;
    }
}
