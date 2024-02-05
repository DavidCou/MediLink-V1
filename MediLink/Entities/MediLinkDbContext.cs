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

        public DbSet<PatientAddress> PatientAddresses { get; set; }

        public DbSet<Practitioner> Practitioners { get; set; }

        public DbSet<PractitionerType> PractitionerTypes  { get; set; }

        public DbSet<PractionerAddress> PractionerAddresses  { get; set; }

        public DbSet<PractionerDetails> PractionerDetails { get; set; }

        public DbSet<PatientDetails> PatientDetails { get; set; }

        public DbSet<PatientPreferences> PatientPreferences { get; set; }

        public DbSet<PatientSpokenLanguages> PatientSpokenLanguages { get; set; }

        public DbSet<PreferedSpokenLanguages> PreferedSpokenLanguages { get; set; }

        public DbSet<PractitionerSpokenLanguages> PractitionerSpokenLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PractitionerSpokenLanguages>().HasKey(pr => new { pr.Laguage_ID, pr.PractionerDetails_ID });

            modelBuilder.Entity<PractitionerSpokenLanguages>().HasOne(pr => pr.Language).WithMany(l => l.PractitionerSpokenLanguages).HasForeignKey(pr => pr.Laguage_ID);

            modelBuilder.Entity<PractitionerSpokenLanguages>().HasOne(pr => pr.PractionerDetails).WithMany(l => l.PractitionerSpokenLanguages).HasForeignKey(pr => pr.PractionerDetails_ID);

            modelBuilder.Entity<PatientSpokenLanguages>().HasKey(p => new { p.Laguage_ID, p.PatientDetails_ID });

            modelBuilder.Entity<PatientSpokenLanguages>().HasOne(p => p.Language).WithMany(l => l.PatientSpokenLanguages).HasForeignKey(p => p.Laguage_ID);

            modelBuilder.Entity<PatientSpokenLanguages>().HasOne(p => p.PatientDetails).WithMany(l => l.PatientSpokenLanguages).HasForeignKey(pr => pr.PatientDetails_ID);

            modelBuilder.Entity<PreferedSpokenLanguages>().HasKey(p => new { p.Language_ID, p.PatientPreferences_ID });

            modelBuilder.Entity<PreferedSpokenLanguages>().HasOne(p => p.Language).WithMany(l => l.PreferedSpokenLanguages).HasForeignKey(p => p.Language_ID);

            modelBuilder.Entity<PreferedSpokenLanguages>().HasOne(p => p.PatientPreferences).WithMany(l => l.PreferedSpokenLanguages).HasForeignKey(pr => pr.PatientPreferences_ID);
        }




    }
}
