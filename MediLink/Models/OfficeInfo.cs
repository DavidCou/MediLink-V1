﻿using MediLink.Entities;

namespace MediLink.Models
{
    public class OfficeInfo
    {
        public int Id { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string Province { get; set; } = "Ontario";

        public string country { get; set; } = "Canada";

        public string? PostalCode { get; set; }

        public bool IsDeleted { get; set; }

        public string? OfficeName { get; set; }

        public string? OfficeTypeName { get; set; }

        public OfficeType? OfficeType { get; set; }

        public string? fullAddress { get; set; }

        public bool isRequested { get; set; } = false;

        public string DateRequest { get; set; } = "";

        public string DateApproved { get; set; } = "";

        public string statusRequest { get; set; } = "";
    }
}
