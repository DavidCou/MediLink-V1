using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class DupFieldFix : Migration
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
                    OfficeName = table.Column<string>(type: "VARCHAR(200)", maxLength: 200, nullable: false),
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
                name: "OfficeAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false),
                    Province = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    PostalCode = table.Column<string>(type: "VARCHAR(7)", maxLength: 7, nullable: false),
                    StreetAddress = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OfficeAddress_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id");
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
                    lastPatientAcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAcceptingNewPatients = table.Column<bool>(type: "bit", nullable: false),
                    PractitionerTypeId = table.Column<int>(type: "int", nullable: false)
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
                    CurrentWaitTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HistoricalWaitTimeMin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HistoricalWaitTimeMax = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OfficeAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInClinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInClinics_OfficeAddress_OfficeAddressId",
                        column: x => x.OfficeAddressId,
                        principalTable: "OfficeAddress",
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
                        name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddressesId",
                        column: x => x.OfficeAddressesId,
                        principalTable: "OfficeAddress",
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
                columns: new[] { "Id", "IsDeleted", "OfficeName" },
                values: new object[,]
                {
                    { 1, false, "Community Center" },
                    { 2, false, "Walk In Clinic" },
                    { 3, false, "Medical Center" },
                    { 4, false, "Clinic" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficeAddress_OfficeTypeId",
                table: "OfficeAddress",
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
                name: "PatientOfficeTypes");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PractitionerAddresses");

            migrationBuilder.DropTable(
                name: "PractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PreferedLanguages");

            migrationBuilder.DropTable(
                name: "WalkInPractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PatientDetails");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "PatientPreferences");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "WalkInClinics");

            migrationBuilder.DropTable(
                name: "PatientAddress");

            migrationBuilder.DropTable(
                name: "PractitionerTypes");

            migrationBuilder.DropTable(
                name: "OfficeAddress");

            migrationBuilder.DropTable(
                name: "OfficeTypes");
        }
    }
}
