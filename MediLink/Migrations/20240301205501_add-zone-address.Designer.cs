﻿// <auto-generated />
using System;
using MediLink.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MediLink.Migrations
{
    [DbContext(typeof(MediLinkDbContext))]
    [Migration("20240301205501_add-zone-address")]
    partial class addzoneaddress
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MediLink.Entities.Languages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LanguageName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            LanguageName = "English"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            LanguageName = "Spanish"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            LanguageName = "French"
                        });
                });

            modelBuilder.Entity("MediLink.Entities.OfficeAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OfficeTypeId")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("country")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("zone")
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.HasIndex("OfficeTypeId");

                    b.ToTable("OfficeAddresses");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("OfficeTypeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.ToTable("OfficeTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsDeleted = false,
                            OfficeTypeName = "Community Center"
                        },
                        new
                        {
                            Id = 2,
                            IsDeleted = false,
                            OfficeTypeName = "Walk In Clinic"
                        },
                        new
                        {
                            Id = 3,
                            IsDeleted = false,
                            OfficeTypeName = "Medical Center"
                        },
                        new
                        {
                            Id = 4,
                            IsDeleted = false,
                            OfficeTypeName = "Clinic"
                        });
                });

            modelBuilder.Entity("MediLink.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<int?>("PatientDetailsId")
                        .HasColumnType("int");

                    b.Property<int?>("PatientPreferencesId")
                        .HasColumnType("int");

                    b.Property<bool>("passwordReset")
                        .HasColumnType("bit");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PatientDetailsId")
                        .IsUnique()
                        .HasFilter("[PatientDetailsId] IS NOT NULL");

                    b.HasIndex("PatientPreferencesId")
                        .IsUnique()
                        .HasFilter("[PatientPreferencesId] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MediLink.Entities.PatientAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(7)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Province")
                        .HasMaxLength(50)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("StreetAddress")
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("country")
                        .HasMaxLength(7)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.ToTable("PatientAddress");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("DoB")
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<int?>("PatientAddressesId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(17)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("creation_Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PatientAddressesId")
                        .IsUnique()
                        .HasFilter("[PatientAddressesId] IS NOT NULL");

                    b.ToTable("PatientDetails");
                });

            modelBuilder.Entity("MediLink.Entities.PatientOfficeType", b =>
                {
                    b.Property<int>("PatientPreferenceId")
                        .HasColumnType("int");

                    b.Property<int>("OfficeTypeId")
                        .HasColumnType("int");

                    b.HasKey("PatientPreferenceId", "OfficeTypeId");

                    b.HasIndex("OfficeTypeId");

                    b.ToTable("PatientOfficeTypes");
                });

            modelBuilder.Entity("MediLink.Entities.PatientPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("location")
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.Property<int?>("rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PatientPreferences");
                });

            modelBuilder.Entity("MediLink.Entities.PatientSpokenLanguage", b =>
                {
                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("PatientDetailsId")
                        .HasColumnType("int");

                    b.HasKey("LanguageId", "PatientDetailsId");

                    b.HasIndex("PatientDetailsId");

                    b.ToTable("PatientSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.Practitioner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("IsAcceptingNewPatients")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValidated")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("PractitionerTypeId")
                        .HasColumnType("int");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("lastPatientAcceptedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("passwordReset")
                        .HasColumnType("bit");

                    b.Property<int?>("rating")
                        .HasColumnType("int");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PractitionerTypeId");

                    b.ToTable("Practitioners");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerOfficeAddress", b =>
                {
                    b.Property<int>("PractitionerId")
                        .HasColumnType("int");

                    b.Property<int>("OfficeAddressesId")
                        .HasColumnType("int");

                    b.HasKey("PractitionerId", "OfficeAddressesId");

                    b.HasIndex("OfficeAddressesId");

                    b.ToTable("PractitionerAddresses");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerSpokenLanguages", b =>
                {
                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("PractitionerId")
                        .HasColumnType("int");

                    b.HasKey("LanguageId", "PractitionerId");

                    b.HasIndex("PractitionerId");

                    b.ToTable("PractitionerSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(350)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.ToTable("PractitionerTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "A general health practitioner",
                            IsDeleted = false,
                            Name = "Family Doctor"
                        },
                        new
                        {
                            Id = 2,
                            Description = "A general health practitioner that specializes in child healthcare",
                            IsDeleted = false,
                            Name = "Pediatrician"
                        },
                        new
                        {
                            Id = 3,
                            Description = "",
                            IsDeleted = false,
                            Name = "Walk-in Clinic"
                        });
                });

            modelBuilder.Entity("MediLink.Entities.PreferedLanguage", b =>
                {
                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("PatientPreferenceId")
                        .HasColumnType("int");

                    b.HasKey("LanguageId", "PatientPreferenceId");

                    b.HasIndex("PatientPreferenceId");

                    b.ToTable("PreferedLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.WalkInClinic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClinicNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrentWaitTime")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<int?>("HistoricalWaitTimeMax")
                        .HasColumnType("int");

                    b.Property<int?>("HistoricalWaitTimeMin")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsValidated")
                        .HasColumnType("bit");

                    b.Property<int>("OfficeAddressId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("VARCHAR");

                    b.Property<bool>("passwordReset")
                        .HasColumnType("bit");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OfficeAddressId")
                        .IsUnique();

                    b.ToTable("WalkInClinics");
                });

            modelBuilder.Entity("MediLink.Entities.WalkInPractitionerSpokenLanguages", b =>
                {
                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("WalkInPractitionerId")
                        .HasColumnType("int");

                    b.HasKey("LanguageId", "WalkInPractitionerId");

                    b.HasIndex("WalkInPractitionerId");

                    b.ToTable("WalkInPractitionerSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeAddress", b =>
                {
                    b.HasOne("MediLink.Entities.OfficeType", "OfficeType")
                        .WithMany("OfficeAddresses")
                        .HasForeignKey("OfficeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficeType");
                });

            modelBuilder.Entity("MediLink.Entities.Patient", b =>
                {
                    b.HasOne("MediLink.Entities.PatientDetail", "PatientDetails")
                        .WithOne("Patients")
                        .HasForeignKey("MediLink.Entities.Patient", "PatientDetailsId");

                    b.HasOne("MediLink.Entities.PatientPreference", "PatientPreferences")
                        .WithOne("Patients")
                        .HasForeignKey("MediLink.Entities.Patient", "PatientPreferencesId");

                    b.Navigation("PatientDetails");

                    b.Navigation("PatientPreferences");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.HasOne("MediLink.Entities.PatientAddress", "PatientAddresses")
                        .WithOne("PatientDetails")
                        .HasForeignKey("MediLink.Entities.PatientDetail", "PatientAddressesId");

                    b.Navigation("PatientAddresses");
                });

            modelBuilder.Entity("MediLink.Entities.PatientOfficeType", b =>
                {
                    b.HasOne("MediLink.Entities.OfficeType", "OfficeType")
                        .WithMany("PatientOfficeTypes")
                        .HasForeignKey("OfficeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.PatientPreference", "PatientPreference")
                        .WithMany("PatientOfficeType")
                        .HasForeignKey("PatientPreferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficeType");

                    b.Navigation("PatientPreference");
                });

            modelBuilder.Entity("MediLink.Entities.PatientSpokenLanguage", b =>
                {
                    b.HasOne("MediLink.Entities.Languages", "Language")
                        .WithMany("PatientSpokenLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.PatientDetail", "PatientDetails")
                        .WithMany("PatientSpokenLanguages")
                        .HasForeignKey("PatientDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("PatientDetails");
                });

            modelBuilder.Entity("MediLink.Entities.Practitioner", b =>
                {
                    b.HasOne("MediLink.Entities.PractitionerType", "PractitionerType")
                        .WithMany("Practitioners")
                        .HasForeignKey("PractitionerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PractitionerType");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerOfficeAddress", b =>
                {
                    b.HasOne("MediLink.Entities.OfficeAddress", "OfficeAddresses")
                        .WithMany("PractitionerAddress")
                        .HasForeignKey("OfficeAddressesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.Practitioner", "Practitioner")
                        .WithMany("PractitionerAddress")
                        .HasForeignKey("PractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficeAddresses");

                    b.Navigation("Practitioner");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerSpokenLanguages", b =>
                {
                    b.HasOne("MediLink.Entities.Languages", "Language")
                        .WithMany("PractitionerSpokenLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.Practitioner", "Practitioner")
                        .WithMany("PractitionerSpokenLanguages")
                        .HasForeignKey("PractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Practitioner");
                });

            modelBuilder.Entity("MediLink.Entities.PreferedLanguage", b =>
                {
                    b.HasOne("MediLink.Entities.Languages", "Language")
                        .WithMany("PreferedLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.PatientPreference", "PatientPreference")
                        .WithMany("PreferedLanguages")
                        .HasForeignKey("PatientPreferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("PatientPreference");
                });

            modelBuilder.Entity("MediLink.Entities.WalkInClinic", b =>
                {
                    b.HasOne("MediLink.Entities.OfficeAddress", "OfficeAddress")
                        .WithOne("WalkInClinic")
                        .HasForeignKey("MediLink.Entities.WalkInClinic", "OfficeAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficeAddress");
                });

            modelBuilder.Entity("MediLink.Entities.WalkInPractitionerSpokenLanguages", b =>
                {
                    b.HasOne("MediLink.Entities.Languages", "Language")
                        .WithMany("WalkInPractitionerSpokenLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.WalkInClinic", "WalkInPractitioner")
                        .WithMany("WalkInPractitionerSpokenLanguages")
                        .HasForeignKey("WalkInPractitionerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("WalkInPractitioner");
                });

            modelBuilder.Entity("MediLink.Entities.Languages", b =>
                {
                    b.Navigation("PatientSpokenLanguages");

                    b.Navigation("PractitionerSpokenLanguages");

                    b.Navigation("PreferedLanguages");

                    b.Navigation("WalkInPractitionerSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeAddress", b =>
                {
                    b.Navigation("PractitionerAddress");

                    b.Navigation("WalkInClinic");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeType", b =>
                {
                    b.Navigation("OfficeAddresses");

                    b.Navigation("PatientOfficeTypes");
                });

            modelBuilder.Entity("MediLink.Entities.PatientAddress", b =>
                {
                    b.Navigation("PatientDetails");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.Navigation("PatientSpokenLanguages");

                    b.Navigation("Patients")
                        .IsRequired();
                });

            modelBuilder.Entity("MediLink.Entities.PatientPreference", b =>
                {
                    b.Navigation("PatientOfficeType");

                    b.Navigation("Patients")
                        .IsRequired();

                    b.Navigation("PreferedLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.Practitioner", b =>
                {
                    b.Navigation("PractitionerAddress");

                    b.Navigation("PractitionerSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerType", b =>
                {
                    b.Navigation("Practitioners");
                });

            modelBuilder.Entity("MediLink.Entities.WalkInClinic", b =>
                {
                    b.Navigation("WalkInPractitionerSpokenLanguages");
                });
#pragma warning restore 612, 618
        }
    }
}
