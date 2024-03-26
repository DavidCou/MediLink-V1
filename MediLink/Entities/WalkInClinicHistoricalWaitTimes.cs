namespace MediLink.Entities
{
    public class WalkInClinicHistoricalWaitTimes
    {
        public int Id { get; set; }

        public DateTime PatientCheckInTime { get; set; }

        public string DayOfTheWeek { get; set; }

        public int WaitTimeInSeconds { get; set; }

        public int WalkInClinicId { get; set; }

        public WalkInClinic WalkInClinic { get; set; }
    }
}
