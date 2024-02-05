namespace MediLink.Entities
{
    public class PatientSpokenLanguages
    {

        public int PatientDetails_ID { get; set; }

        public int Laguage_ID { get; set; }

        public Languages Language { get; set; }

        public PatientDetails PatientDetails { get; set; }
    }
}
