namespace MediLink.Models
{
    public class HistoricalWaitTimesViewModel
    {
        public string? DayOfTheWeek {  get; set; }
        
        public Dictionary<string,string>? HistoricalWaitTimes { get; set; }
    }
}
