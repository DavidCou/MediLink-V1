using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PractionerAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractionerAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    IsAcceptingNePatients = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfficeTypeID = table.Column<int>(name: "OfficeType_ID", type: "int", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientPreferences_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractionerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastPatientAcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAcceptingNewPatients = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    OfficeTypeID = table.Column<int>(name: "OfficeType_ID", type: "int", nullable: false),
                    PractionerAddressID = table.Column<int>(name: "PractionerAddress_ID", type: "int", nullable: false),
                    OfficeTypeId = table.Column<int>(type: "int", nullable: false),
                    PractionerAddressId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractionerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PractionerDetails_OfficeTypes_OfficeTypeId",
                        column: x => x.OfficeTypeId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractionerDetails_PractionerAddresses_PractionerAddressId",
                        column: x => x.PractionerAddressId,
                        principalTable: "PractionerAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PatientAddressID = table.Column<int>(name: "PatientAddress_ID", type: "int", nullable: false),
                    PatientPreferencesID = table.Column<int>(name: "PatientPreferences_ID", type: "int", nullable: false),
                    PatientAddressId = table.Column<int>(type: "int", nullable: false),
                    PatientPreferencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDetails_PatientAddresses_PatientAddressId",
                        column: x => x.PatientAddressId,
                        principalTable: "PatientAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDetails_PatientPreferences_PatientPreferencesId",
                        column: x => x.PatientPreferencesId,
                        principalTable: "PatientPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferedSpokenLanguages",
                columns: table => new
                {
                    PatientPreferencesID = table.Column<int>(name: "PatientPreferences_ID", type: "int", nullable: false),
                    LanguageID = table.Column<int>(name: "Language_ID", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferedSpokenLanguages", x => new { x.LanguageID, x.PatientPreferencesID });
                    table.ForeignKey(
                        name: "FK_PreferedSpokenLanguages_Languages_Language_ID",
                        column: x => x.LanguageID,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferedSpokenLanguages_PatientPreferences_PatientPreferences_ID",
                        column: x => x.PatientPreferencesID,
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
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsValidated = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PractitionerTypeID = table.Column<int>(name: "PractitionerType_ID", type: "int", nullable: false),
                    PractionerDetailsID = table.Column<int>(name: "PractionerDetails_ID", type: "int", nullable: false),
                    PractionerDetailsId = table.Column<int>(type: "int", nullable: false),
                    PractitionerTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Practitioners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Practitioners_PractionerDetails_PractionerDetailsId",
                        column: x => x.PractionerDetailsId,
                        principalTable: "PractionerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Practitioners_PractitionerTypes_PractitionerTypeId",
                        column: x => x.PractitionerTypeId,
                        principalTable: "PractitionerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PractitionerSpokenLanguages",
                columns: table => new
                {
                    PractionerDetailsID = table.Column<int>(name: "PractionerDetails_ID", type: "int", nullable: false),
                    LaguageID = table.Column<int>(name: "Laguage_ID", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerSpokenLanguages", x => new { x.LaguageID, x.PractionerDetailsID });
                    table.ForeignKey(
                        name: "FK_PractitionerSpokenLanguages_Languages_Laguage_ID",
                        column: x => x.LaguageID,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerSpokenLanguages_PractionerDetails_PractionerDetails_ID",
                        column: x => x.PractionerDetailsID,
                        principalTable: "PractionerDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    PatietDetailsID = table.Column<int>(name: "PatietDetails_ID", type: "int", nullable: false),
                    PatientDetailsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_PatientDetails_PatientDetailsId",
                        column: x => x.PatientDetailsId,
                        principalTable: "PatientDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientSpokenLanguages",
                columns: table => new
                {
                    PatientDetailsID = table.Column<int>(name: "PatientDetails_ID", type: "int", nullable: false),
                    LaguageID = table.Column<int>(name: "Laguage_ID", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientSpokenLanguages", x => new { x.LaguageID, x.PatientDetailsID });
                    table.ForeignKey(
                        name: "FK_PatientSpokenLanguages_Languages_Laguage_ID",
                        column: x => x.LaguageID,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientSpokenLanguages_PatientDetails_PatientDetails_ID",
                        column: x => x.PatientDetailsID,
                        principalTable: "PatientDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientAddressId",
                table: "PatientDetails",
                column: "PatientAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientPreferencesId",
                table: "PatientDetails",
                column: "PatientPreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientPreferences_OfficeTypeId",
                table: "PatientPreferences",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientSpokenLanguages_PatientDetails_ID",
                table: "PatientSpokenLanguages",
                column: "PatientDetails_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PractionerDetails_OfficeTypeId",
                table: "PractionerDetails",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PractionerDetails_PractionerAddressId",
                table: "PractionerDetails",
                column: "PractionerAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractionerDetailsId",
                table: "Practitioners",
                column: "PractionerDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractitionerTypeId",
                table: "Practitioners",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerSpokenLanguages_PractionerDetails_ID",
                table: "PractitionerSpokenLanguages",
                column: "PractionerDetails_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PreferedSpokenLanguages_PatientPreferences_ID",
                table: "PreferedSpokenLanguages",
                column: "PatientPreferences_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "PatientSpokenLanguages");

            migrationBuilder.DropTable(
                name: "Practitioners");

            migrationBuilder.DropTable(
                name: "PractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PreferedSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PatientDetails");

            migrationBuilder.DropTable(
                name: "PractitionerTypes");

            migrationBuilder.DropTable(
                name: "PractionerDetails");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "PatientAddresses");

            migrationBuilder.DropTable(
                name: "PatientPreferences");

            migrationBuilder.DropTable(
                name: "PractionerAddresses");

            migrationBuilder.DropTable(
                name: "OfficeTypes");
        }
    }
}
