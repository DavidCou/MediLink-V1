namespace MediLink.Entities
{
    public class PractitionerType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
