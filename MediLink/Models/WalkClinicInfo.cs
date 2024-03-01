using MediLink.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MediLink.Models
{
    public class WalkClinicInfo
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string? ClinicNotes { get; set; }

        public int? CurrentWaitTime { get; set; }

        public int? HistoricalWaitTimeMin { get; set; }

        public int? HistoricalWaitTimeMax { get; set; }

        public string? fullAddress { get; set; }

        public string? OfficeName { get; set; }

        public string? City { get; set; }

        public string? Province { get; set; }

        public string? country { get; set; }

        public string? PostalCode { get; set; }

        public string? StreetAddress { get; set; }

        public string? zone { get; set; }
    }
}
