using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace MediLink.Models
{
    public class WalkInHoursViewModel
    {
        public string MondayOpeningTime { get; set; }

        public string MondayClosingTime { get; set; }

        public string TuesdayOpeningTime { get; set; }

        public string TuesdayClosingTime { get; set; }

        public string WednesdayOpeningTime { get; set; }

        public string WednesdayClosingTime { get; set; }

        public string ThursdayOpeningTime { get; set; }

        public string ThursdayClosingTime { get; set; }

        public string FridayOpeningTime { get; set; }

        public string FridayClosingTime { get; set; }

        public string SaturdayOpeningTime { get; set; }

        public string SaturdayClosingTime { get; set; }

        public string SundayOpeningTime { get; set; }

        public string SundayClosingTime { get; set; }
    }
}
