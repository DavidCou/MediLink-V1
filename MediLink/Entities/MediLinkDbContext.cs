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

        public DbSet<PractitionerType> PractitionerTypes  { get; set; }

        public DbSet<PatientOfficeType> PatientOfficeTypes { get; set; }		

		public DbSet<PractitionerOfficeAddress> PractitionerAddresses { get; set; }

        public DbSet<PractitionerOfficeType> PractitionerOfficeTypes { get; set; }

        public DbSet<PatientPreference> PatientPreferences { get; set; }

        public DbSet<PreferedLanguage> PreferedLanguages { get; set; }

        public DbSet<PatientSpokenLanguage> PatientSpokenLanguages { get; set; }

        public DbSet<PractitionerSpokenLanguages> PractitionerSpokenLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<PatientDetail>().Property(us => us.DoB).HasColumnType("date");

            modelBuilder.Entity<PractitionerSpokenLanguages>().HasKey(ps => new { ps.LanguageId, ps.PractitionerId });

			modelBuilder.Entity<PreferedLanguage>().HasKey(ps => new { ps.LanguageId, ps.PatientPreferenceId });

            modelBuilder.Entity<PatientSpokenLanguage>().HasKey(ps => new { ps.LanguageId, ps.PatientDetailsId });

            modelBuilder.Entity<PatientOfficeType>().HasKey(pao => new { pao.PatientPreferenceId, pao.OfficeTypeId });

			modelBuilder.Entity<PractitionerOfficeAddress>().HasKey(pa => new { pa.PractitionerId, pa.OfficeAddressesId });

            modelBuilder.Entity<PractitionerOfficeType>().HasKey(po => new { po.PractitionerId, po.OfficeTypesId });

            modelBuilder.Entity<Languages>().HasData(
                new Languages() { Id = 1, LanguageName = "English", IsDeleted = false},
                new Languages() { Id = 2, LanguageName = "Spanish", IsDeleted = false},
                new Languages() { Id = 3, LanguageName = "French", IsDeleted = false});

            modelBuilder.Entity<OfficeType>().HasData(
                new OfficeType() { Id = 1, OfficeName = "Community Center", IsDeleted = false },
                new OfficeType() { Id = 2, OfficeName = "Private", IsDeleted = false });

        }




    }
}
