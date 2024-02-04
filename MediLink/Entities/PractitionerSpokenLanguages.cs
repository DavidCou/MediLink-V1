namespace MediLink.Entities
{
    public class PractitionerSpokenLanguages
    {
        public int Id { get; set; }

        public int PractionerDetails_ID { get; set; }

        public int Laguage_ID { get; set; }

        public Languages Language { get; set; }

        public PractionerDetails PractionerDetails { get; set; }
    }
}
