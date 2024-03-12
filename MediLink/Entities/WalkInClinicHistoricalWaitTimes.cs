namespace MediLink.Entities
{
    public class WalkInClinicHistoricalWaitTimes
    {
        public int Id { get; set; }

        public string DayOfTheWeek { get; set; }

        public string TimeOfDay { get; set; }

        public string WaitTime { get; set; }

        public int WalkInClinicId { get; set; }

        public WalkInClinic WalkInClinic { get; set; }
    }
}
