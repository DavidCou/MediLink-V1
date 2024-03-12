namespace MediLink.Entities
{
    public class WalkInClinicHours
    {
        public int Id { get; set; }

        public string DayOfTheWeek { get; set; }

        public string OpeningTime { get; set; }

        public string ClosingTime { get; set;}

        public int WalkInClinicId { get; set; }

        public WalkInClinic WalkInClinic { get; set; }
    }
}
