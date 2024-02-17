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
    [Migration("20240213021057_ChangesDB_JQ")]
    partial class ChangesDBJQ
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

                    b.Property<int>("OfficeTypesId")
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

                    b.HasKey("Id");

                    b.HasIndex("OfficeTypesId")
                        .IsUnique();

                    b.ToTable("OfficeAddress");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.HasKey("Id");

                    b.ToTable("OfficeTypes");
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

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("PatientDetailsId")
                        .HasColumnType("int");

                    b.Property<int>("PatientPreferencesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PatientDetailsId")
                        .IsUnique();

                    b.HasIndex("PatientPreferencesId")
                        .IsUnique();

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("MediLink.Entities.PatientAddress", b =>
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

                    b.HasKey("Id");

                    b.ToTable("PatientAddress");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DoB")
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

                    b.Property<int>("PatientAddressesId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(17)
                        .HasColumnType("VARCHAR");

                    b.Property<DateTime>("creation_Date")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PatientAddressesId")
                        .IsUnique();

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
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("rating")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PatientPreferences");
                });

            modelBuilder.Entity("MediLink.Entities.PatientSpokenLanguages", b =>
                {
                    b.Property<int>("LanguageId")
                        .HasColumnType("int");

                    b.Property<int>("PatientPreferenceId")
                        .HasColumnType("int");

                    b.HasKey("LanguageId", "PatientPreferenceId");

                    b.HasIndex("PatientPreferenceId");

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
                        .HasMaxLength(200)
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
                        .HasMaxLength(150)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("VARCHAR");

                    b.Property<int>("PractitionerTypesId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("VARCHAR");

                    b.Property<string>("gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("lastPatientAcceptedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PractitionerTypesId")
                        .IsUnique();

                    b.ToTable("Practitioners");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerAddress", b =>
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
                });

            modelBuilder.Entity("MediLink.Entities.OfficeAddress", b =>
                {
                    b.HasOne("MediLink.Entities.OfficeType", "OfficeTypes")
                        .WithOne("OfficeAddressed")
                        .HasForeignKey("MediLink.Entities.OfficeAddress", "OfficeTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OfficeTypes");
                });

            modelBuilder.Entity("MediLink.Entities.Patient", b =>
                {
                    b.HasOne("MediLink.Entities.PatientDetail", "PatientDetails")
                        .WithOne("Patients")
                        .HasForeignKey("MediLink.Entities.Patient", "PatientDetailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.PatientPreference", "PatientPreferences")
                        .WithOne("Patients")
                        .HasForeignKey("MediLink.Entities.Patient", "PatientPreferencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PatientDetails");

                    b.Navigation("PatientPreferences");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.HasOne("MediLink.Entities.PatientAddress", "PatientAddresses")
                        .WithOne("PatientDetails")
                        .HasForeignKey("MediLink.Entities.PatientDetail", "PatientAddressesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("MediLink.Entities.PatientSpokenLanguages", b =>
                {
                    b.HasOne("MediLink.Entities.Languages", "Language")
                        .WithMany("PatientSpokenLanguages")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MediLink.Entities.PatientPreference", "PatientPreference")
                        .WithMany("PatientSpokenLanguages")
                        .HasForeignKey("PatientPreferenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("PatientPreference");
                });

            modelBuilder.Entity("MediLink.Entities.Practitioner", b =>
                {
                    b.HasOne("MediLink.Entities.PractitionerType", "PractitionerTypes")
                        .WithOne("Practitioners")
                        .HasForeignKey("MediLink.Entities.Practitioner", "PractitionerTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PractitionerTypes");
                });

            modelBuilder.Entity("MediLink.Entities.PractitionerAddress", b =>
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

            modelBuilder.Entity("MediLink.Entities.Languages", b =>
                {
                    b.Navigation("PatientSpokenLanguages");

                    b.Navigation("PractitionerSpokenLanguages");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeAddress", b =>
                {
                    b.Navigation("PractitionerAddress");
                });

            modelBuilder.Entity("MediLink.Entities.OfficeType", b =>
                {
                    b.Navigation("OfficeAddressed");

                    b.Navigation("PatientOfficeTypes");
                });

            modelBuilder.Entity("MediLink.Entities.PatientAddress", b =>
                {
                    b.Navigation("PatientDetails");
                });

            modelBuilder.Entity("MediLink.Entities.PatientDetail", b =>
                {
                    b.Navigation("Patients")
                        .IsRequired();
                });

            modelBuilder.Entity("MediLink.Entities.PatientPreference", b =>
                {
                    b.Navigation("PatientOfficeType");

                    b.Navigation("PatientSpokenLanguages");

                    b.Navigation("Patients")
                        .IsRequired();
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
#pragma warning restore 612, 618
        }
    }
}
