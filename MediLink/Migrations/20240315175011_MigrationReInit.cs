using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class MigrationReInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeTypeName = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: true),
                    Province = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    country = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: true),
                    PostalCode = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: true),
                    StreetAddress = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPreferences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(350)", maxLength: 350, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OfficeAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    Province = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    PostalCode = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    StreetAddress = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    zone = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeAddresses_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(17)", maxLength: 17, nullable: true),
                    creationDate = table.Column<DateTime>(name: "creation_Date", type: "datetime2", nullable: false),
                    DoB = table.Column<DateTime>(type: "date", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PatientAddressesId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDetails_PatientAddress_PatientAddressesId",
                        column: x => x.PatientAddressesId,
                        principalTable: "PatientAddress",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientOfficeTypes",
                columns: table => new
                {
                    PatientPreferenceId = table.Column<int>(type: "int", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientOfficeTypes", x => new { x.PatientPreferenceId, x.OfficeTypeId });
                    table.ForeignKey(
                        name: "FK_PatientOfficeTypes_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientOfficeTypes_PatientPreferences_PatientPreferenceId",
                        column: x => x.PatientPreferenceId,
                        principalTable: "PatientPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferedLanguages",
                columns: table => new
                {
                    PatientPreferenceId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferedLanguages", x => new { x.LanguageId, x.PatientPreferenceId });
                    table.ForeignKey(
                        name: "FK_PreferedLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferedLanguages_PatientPreferences_PatientPreferenceId",
                        column: x => x.PatientPreferenceId,
                        principalTable: "PatientPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Practitioners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    IsValidated = table.Column<bool>(type: "bit", nullable: false),
                    passwordReset = table.Column<bool>(type: "bit", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(12)", maxLength: 12, nullable: false),
                    lastPatientAcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAcceptingNewPatients = table.Column<bool>(type: "bit", nullable: false),
                    PractitionerTypeId = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practitioners_PractitionerTypes_PractitionerTypeId",
                        column: x => x.PractitionerTypeId,
                        principalTable: "PractitionerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkInClinics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    IsValidated = table.Column<bool>(type: "bit", nullable: false),
                    passwordReset = table.Column<bool>(type: "bit", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PhoneNumber = table.Column<string>(type: "VARCHAR(12)", maxLength: 12, nullable: false),
                    ClinicNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentWaitTime = table.Column<int>(type: "int", nullable: true),
                    HistoricalWaitTimeMin = table.Column<int>(type: "int", nullable: true),
                    HistoricalWaitTimeMax = table.Column<int>(type: "int", nullable: true),
                    OfficeAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInClinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInClinics_OfficeAddresses_OfficeAddressId",
                        column: x => x.OfficeAddressId,
                        principalTable: "OfficeAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    passwordReset = table.Column<bool>(type: "bit", nullable: false),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPreferencesId = table.Column<int>(type: "int", nullable: true),
                    PatientDetailsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_PatientDetails_PatientDetailsId",
                        column: x => x.PatientDetailsId,
                        principalTable: "PatientDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_PatientPreferences_PatientPreferencesId",
                        column: x => x.PatientPreferencesId,
                        principalTable: "PatientPreferences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientSpokenLanguages",
                columns: table => new
                {
                    PatientDetailsId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSpokenLanguages", x => new { x.LanguageId, x.PatientDetailsId });
                    table.ForeignKey(
                        name: "FK_PatientSpokenLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientSpokenLanguages_PatientDetails_PatientDetailsId",
                        column: x => x.PatientDetailsId,
                        principalTable: "PatientDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerAddresses",
                columns: table => new
                {
                    PractitionerId = table.Column<int>(type: "int", nullable: false),
                    OfficeAddressesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerAddresses", x => new { x.PractitionerId, x.OfficeAddressesId });
                    table.ForeignKey(
                        name: "FK_PractitionerAddresses_OfficeAddresses_OfficeAddressesId",
                        column: x => x.OfficeAddressesId,
                        principalTable: "OfficeAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerAddresses_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerSpokenLanguages",
                columns: table => new
                {
                    PractitionerId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerSpokenLanguages", x => new { x.LanguageId, x.PractitionerId });
                    table.ForeignKey(
                        name: "FK_PractitionerSpokenLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerSpokenLanguages_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkInClinicCheckedInPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientCheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalkInClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInClinicCheckedInPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInClinicCheckedInPatients_WalkInClinics_WalkInClinicId",
                        column: x => x.WalkInClinicId,
                        principalTable: "WalkInClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkInClinicHistoricalWaitTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfTheWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeOfDay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaitTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WalkInClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInClinicHistoricalWaitTimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInClinicHistoricalWaitTimes_WalkInClinics_WalkInClinicId",
                        column: x => x.WalkInClinicId,
                        principalTable: "WalkInClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkInClinicHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfTheWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpeningTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClosingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WalkInClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInClinicHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInClinicHours_WalkInClinics_WalkInClinicId",
                        column: x => x.WalkInClinicId,
                        principalTable: "WalkInClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WalkInPractitionerSpokenLanguages",
                columns: table => new
                {
                    WalkInPractitionerId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInPractitionerSpokenLanguages", x => new { x.LanguageId, x.WalkInPractitionerId });
                    table.ForeignKey(
                        name: "FK_WalkInPractitionerSpokenLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WalkInPractitionerSpokenLanguages_WalkInClinics_WalkInPractitionerId",
                        column: x => x.WalkInPractitionerId,
                        principalTable: "WalkInClinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewPatientRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PractitionerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PractitionerLastName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    PractitionerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientFirstName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    PatientLastName = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    PatientGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientPhoneNumber = table.Column<string>(type: "VARCHAR(17)", maxLength: 17, nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientDoB = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientStreetAddress = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    PatientPostalCode = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    PatientCity = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    PatientProvince = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    PatientCountry = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PractitionerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewPatientRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewPatientRequests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewPatientRequests_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerPatients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PractitionerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerPatients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PractitionerPatients_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerPatients_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPractitionerOnTime = table.Column<bool>(type: "bit", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PractitionerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PractitionerReviews_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerReviews_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaitLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    PractitionerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaitLists_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WaitLists_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "IsDeleted", "LanguageName" },
                values: new object[,]
                {
                    { 1, false, "English" },
                    { 2, false, "Spanish" },
                    { 3, false, "French" }
                });

            migrationBuilder.InsertData(
                table: "OfficeTypes",
                columns: new[] { "Id", "IsDeleted", "OfficeTypeName" },
                values: new object[,]
                {
                    { 1, false, "Community Center" },
                    { 2, false, "Walk In Clinic" },
                    { 3, false, "Medical Center" },
                    { 4, false, "Clinic" }
                });

            migrationBuilder.InsertData(
                table: "PractitionerTypes",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "A general health practitioner", false, "Family Doctor" },
                    { 2, "A general health practitioner that specializes in child healthcare", false, "Pediatrician" },
                    { 3, "", false, "Walk-in Clinic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewPatientRequests_PatientId",
                table: "NewPatientRequests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NewPatientRequests_PractitionerId",
                table: "NewPatientRequests",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeAddresses_OfficeTypeId",
                table: "OfficeAddresses",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientAddressesId",
                table: "PatientDetails",
                column: "PatientAddressesId",
                unique: true,
                filter: "[PatientAddressesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientOfficeTypes_OfficeTypeId",
                table: "PatientOfficeTypes",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId",
                unique: true,
                filter: "[PatientDetailsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientPreferencesId",
                table: "Patients",
                column: "PatientPreferencesId",
                unique: true,
                filter: "[PatientPreferencesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSpokenLanguages_PatientDetailsId",
                table: "PatientSpokenLanguages",
                column: "PatientDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerAddresses_OfficeAddressesId",
                table: "PractitionerAddresses",
                column: "OfficeAddressesId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerPatients_PatientId",
                table: "PractitionerPatients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerPatients_PractitionerId",
                table: "PractitionerPatients",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerReviews_PatientId",
                table: "PractitionerReviews",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerReviews_PractitionerId",
                table: "PractitionerReviews",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractitionerTypeId",
                table: "Practitioners",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerSpokenLanguages_PractitionerId",
                table: "PractitionerSpokenLanguages",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_PreferedLanguages_PatientPreferenceId",
                table: "PreferedLanguages",
                column: "PatientPreferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_PatientId",
                table: "WaitLists",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_WaitLists_PractitionerId",
                table: "WaitLists",
                column: "PractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInClinicCheckedInPatients_WalkInClinicId",
                table: "WalkInClinicCheckedInPatients",
                column: "WalkInClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInClinicHistoricalWaitTimes_WalkInClinicId",
                table: "WalkInClinicHistoricalWaitTimes",
                column: "WalkInClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInClinicHours_WalkInClinicId",
                table: "WalkInClinicHours",
                column: "WalkInClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInClinics_OfficeAddressId",
                table: "WalkInClinics",
                column: "OfficeAddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalkInPractitionerSpokenLanguages_WalkInPractitionerId",
                table: "WalkInPractitionerSpokenLanguages",
                column: "WalkInPractitionerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewPatientRequests");

            migrationBuilder.DropTable(
                name: "PatientOfficeTypes");

            migrationBuilder.DropTable(
                name: "PatientSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PractitionerAddresses");

            migrationBuilder.DropTable(
                name: "PractitionerPatients");

            migrationBuilder.DropTable(
                name: "PractitionerReviews");

            migrationBuilder.DropTable(
                name: "PractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PreferedLanguages");

            migrationBuilder.DropTable(
                name: "WaitLists");

            migrationBuilder.DropTable(
                name: "WalkInClinicCheckedInPatients");

            migrationBuilder.DropTable(
                name: "WalkInClinicHistoricalWaitTimes");

            migrationBuilder.DropTable(
                name: "WalkInClinicHours");

            migrationBuilder.DropTable(
                name: "WalkInPractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "WalkInClinics");

            migrationBuilder.DropTable(
                name: "PatientDetails");

            migrationBuilder.DropTable(
                name: "PatientPreferences");

            migrationBuilder.DropTable(
                name: "PractitionerTypes");

            migrationBuilder.DropTable(
                name: "OfficeAddresses");

            migrationBuilder.DropTable(
                name: "PatientAddress");

            migrationBuilder.DropTable(
                name: "OfficeTypes");
        }
    }
}
