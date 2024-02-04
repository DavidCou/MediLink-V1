namespace MediLink.Entities
{
    public class PatientSpokenLanguages
    {
        public int Id { get; set; }

        public int PatientDetails_ID { get; set; }

        public int Laguage_ID { get; set; }

        public Languages Language { get; set; }

        public PractionerDetails PatientDetails { get; set; }
    }
}
