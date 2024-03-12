using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class I3DbUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    PractitionerFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PractitionerLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PractitionerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    PatientFirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientLastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_NewPatientRequests_PatientId",
                table: "NewPatientRequests",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_NewPatientRequests_PractitionerId",
                table: "NewPatientRequests",
                column: "PractitionerId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewPatientRequests");

            migrationBuilder.DropTable(
                name: "PractitionerPatients");

            migrationBuilder.DropTable(
                name: "PractitionerReviews");

            migrationBuilder.DropTable(
                name: "WaitLists");

            migrationBuilder.DropTable(
                name: "WalkInClinicCheckedInPatients");

            migrationBuilder.DropTable(
                name: "WalkInClinicHistoricalWaitTimes");

            migrationBuilder.DropTable(
                name: "WalkInClinicHours");
        }
    }
}
