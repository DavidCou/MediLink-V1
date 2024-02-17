namespace MediLink.Entities
{
    public class PractitionerOfficeType
    {
        public int PractitionerId { get; set; }

        public int OfficeTypesId { get; set; }

        public OfficeType OfficeTypes { get; set; } = null!;

        public Practitioner Practitioner { get; set; } = null!;
    }
}
