using Microsoft.EntityFrameworkCore;

namespace MediLink.Entities
{
    public class MediLinkDbContext : DbContext
    {
        public MediLinkDbContext(DbContextOptions<MediLinkDbContext> options)
            : base(options)
        {
        }

        public DbSet<Languages> Languages { get; set; }

        public DbSet<OfficeType> OfficeTypes { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientDetail> PatientDetails { get; set; }

        public DbSet<PatientAddress> PatientAddress { get; set; }

        public DbSet<Practitioner> Practitioners { get; set; }

        public DbSet<PractitionerType> PractitionerTypes { get; set; }

        public DbSet<PatientOfficeType> PatientOfficeTypes { get; set; }

        public DbSet<PractitionerOfficeAddress> PractitionerAddresses { get; set; }

        public DbSet<PatientPreference> PatientPreferences { get; set; }

        public DbSet<PreferedLanguage> PreferedLanguages { get; set; }

        public DbSet<PatientSpokenLanguage> PatientSpokenLanguages { get; set; }

        public DbSet<PractitionerSpokenLanguages> PractitionerSpokenLanguages { get; set; }

        public DbSet<WalkInPractitionerSpokenLanguages> WalkInPractitionerSpokenLanguages { get; set; }

        public DbSet<WalkInClinic> WalkInClinics { get; set; }

        public DbSet<OfficeAddress> OfficeAddresses { get; set; }

        public DbSet<NewPatientRequest> NewPatientRequests { get; set; }

        public DbSet<PractitionerPatient> PractitionerPatients { get; set; }

        public DbSet<PractitionerReview> PractitionerReviews { get; set; }

        public DbSet<WaitList> WaitLists { get; set; }
        
        public DbSet<WalkInClinicCheckedInPatient> WalkInClinicCheckedInPatients { get; set; }

        public DbSet<WalkInClinicHistoricalWaitTimes> WalkInClinicHistoricalWaitTimes { get; set; } 
        
        public DbSet<WalkInClinicHours> WalkInClinicHours { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PatientDetail>().Property(us => us.DoB).HasColumnType("date");

            modelBuilder.Entity<PractitionerSpokenLanguages>().HasKey(ps => new { ps.LanguageId, ps.PractitionerId });

			modelBuilder.Entity<PreferedLanguage>().HasKey(ps => new { ps.LanguageId, ps.PatientPreferenceId });

            modelBuilder.Entity<PatientSpokenLanguage>().HasKey(ps => new { ps.LanguageId, ps.PatientDetailsId });

            modelBuilder.Entity<PatientOfficeType>().HasKey(pao => new { pao.PatientPreferenceId, pao.OfficeTypeId });

			modelBuilder.Entity<PractitionerOfficeAddress>().HasKey(pa => new { pa.PractitionerId, pa.OfficeAddressesId });

            modelBuilder.Entity<WalkInPractitionerSpokenLanguages>().HasKey(wpsp => new { wpsp.LanguageId, wpsp.WalkInPractitionerId });

            modelBuilder.Entity<Languages>().HasData(
                new Languages() { Id = 1, LanguageName = "English", IsDeleted = false},
                new Languages() { Id = 2, LanguageName = "Spanish", IsDeleted = false},
                new Languages() { Id = 3, LanguageName = "French", IsDeleted = false}
            );

            modelBuilder.Entity<OfficeType>().HasData(
                new OfficeType() { Id = 1, OfficeTypeName = "Community Center", IsDeleted = false },
                new OfficeType() { Id = 2, OfficeTypeName = "Walk In Clinic", IsDeleted = false },
			    new OfficeType() { Id = 3, OfficeTypeName = "Medical Center", IsDeleted = false },
			    new OfficeType() { Id = 4, OfficeTypeName = "Clinic", IsDeleted = false }
           );

            modelBuilder.Entity<PractitionerType>().HasData(
                new PractitionerType() { Id = 1, Name = "Family Doctor", Description = "A general health practitioner"},
				new PractitionerType() { Id = 2, Name = "Pediatrician", Description = "A general health practitioner that specializes in child healthcare" },
				new PractitionerType() { Id = 3, Name = "Walk-in Clinic", Description = "" }

			); 

		}

    }
}
