using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class I2DbUpadesDC : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientDetails_PatientAddress_PatientAddressesId",
                table: "PatientDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientDetails_PatientDetailsId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddressesId",
                table: "PractitionerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.DropTable(
                name: "PractitionerOfficeTypes");

            migrationBuilder.DropIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.DropIndex(
                name: "IX_PractitionerAddresses_OfficeAddressesId",
                table: "PractitionerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_PatientDetails_PatientAddressesId",
                table: "PatientDetails");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Practitioners");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Practitioners",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Practitioners",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<int>(
                name: "PractitionerTypeId",
                table: "Practitioners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfficeAddresseId",
                table: "PractitionerAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalkInPractitionerId",
                table: "PractitionerAddresses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientDetailsId",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "PatientPreferences",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "location",
                table: "PatientPreferences",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "PatientDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "PatientDetails",
                type: "VARCHAR(17)",
                maxLength: 17,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(17)",
                oldMaxLength: 17);

            migrationBuilder.AlterColumn<int>(
                name: "PatientAddressesId",
                table: "PatientDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoB",
                table: "PatientDetails",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "PatientAddress",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "PatientAddress",
                type: "VARCHAR(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "PatientAddress",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "PatientAddress",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(7)",
                oldMaxLength: 7);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "PatientAddress",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "OfficeTypeId",
                table: "OfficeAddress",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WalkInPractitioner",
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
                    PractitionerTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalkInPractitioner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalkInPractitioner_PractitionerTypes_PractitionerTypeId",
                        column: x => x.PractitionerTypeId,
                        principalTable: "PractitionerTypes",
                        principalColumn: "Id");
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
                        name: "FK_WalkInPractitionerSpokenLanguages_WalkInPractitioner_WalkInPractitionerId",
                        column: x => x.WalkInPractitionerId,
                        principalTable: "WalkInPractitioner",
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
                name: "IX_Practitioners_PractitionerTypeId",
                table: "Practitioners",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerAddresses_OfficeAddresseId",
                table: "PractitionerAddresses",
                column: "OfficeAddresseId");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerAddresses_WalkInPractitionerId",
                table: "PractitionerAddresses",
                column: "WalkInPractitionerId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId",
                unique: true,
                filter: "[PatientDetailsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientAddressesId",
                table: "PatientDetails",
                column: "PatientAddressesId",
                unique: true,
                filter: "[PatientAddressesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OfficeAddress_OfficeTypeId",
                table: "OfficeAddress",
                column: "OfficeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInPractitioner_PractitionerTypeId",
                table: "WalkInPractitioner",
                column: "PractitionerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WalkInPractitionerSpokenLanguages_WalkInPractitionerId",
                table: "WalkInPractitionerSpokenLanguages",
                column: "WalkInPractitionerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypeId",
                table: "OfficeAddress",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDetails_PatientAddress_PatientAddressesId",
                table: "PatientDetails",
                column: "PatientAddressesId",
                principalTable: "PatientAddress",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientDetails_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId",
                principalTable: "PatientDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddresseId",
                table: "PractitionerAddresses",
                column: "OfficeAddresseId",
                principalTable: "OfficeAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PractitionerAddresses_WalkInPractitioner_WalkInPractitionerId",
                table: "PractitionerAddresses",
                column: "WalkInPractitionerId",
                principalTable: "WalkInPractitioner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypeId",
                table: "Practitioners",
                column: "PractitionerTypeId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypeId",
                table: "OfficeAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientDetails_PatientAddress_PatientAddressesId",
                table: "PatientDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientDetails_PatientDetailsId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddresseId",
                table: "PractitionerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PractitionerAddresses_WalkInPractitioner_WalkInPractitionerId",
                table: "PractitionerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypeId",
                table: "Practitioners");

            migrationBuilder.DropTable(
                name: "WalkInPractitionerSpokenLanguages");

            migrationBuilder.DropTable(
                name: "WalkInPractitioner");

            migrationBuilder.DropIndex(
                name: "IX_Practitioners_PractitionerTypeId",
                table: "Practitioners");

            migrationBuilder.DropIndex(
                name: "IX_PractitionerAddresses_OfficeAddresseId",
                table: "PractitionerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_PractitionerAddresses_WalkInPractitionerId",
                table: "PractitionerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_PatientDetails_PatientAddressesId",
                table: "PatientDetails");

            migrationBuilder.DropIndex(
                name: "IX_OfficeAddress_OfficeTypeId",
                table: "OfficeAddress");

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Languages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OfficeTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OfficeTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OfficeTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OfficeTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "PractitionerTypeId",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "OfficeAddresseId",
                table: "PractitionerAddresses");

            migrationBuilder.DropColumn(
                name: "WalkInPractitionerId",
                table: "PractitionerAddresses");

            migrationBuilder.DropColumn(
                name: "OfficeTypeId",
                table: "OfficeAddress");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Practitioners",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Practitioners",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Practitioners",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "PatientDetailsId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "PatientPreferences",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "location",
                table: "PatientPreferences",
                type: "VARCHAR(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "PatientDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "PatientDetails",
                type: "VARCHAR(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(17)",
                oldMaxLength: 17,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PatientAddressesId",
                table: "PatientDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DoB",
                table: "PatientDetails",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "country",
                table: "PatientAddress",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(7)",
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StreetAddress",
                table: "PatientAddress",
                type: "VARCHAR(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Province",
                table: "PatientAddress",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "PatientAddress",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(7)",
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "PatientAddress",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PractitionerOfficeTypes",
                columns: table => new
                {
                    PractitionerId = table.Column<int>(type: "int", nullable: false),
                    OfficeTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PractitionerOfficeTypes", x => new { x.PractitionerId, x.OfficeTypesId });
                    table.ForeignKey(
                        name: "FK_PractitionerOfficeTypes_OfficeTypes_OfficeTypesId",
                        column: x => x.OfficeTypesId,
                        principalTable: "OfficeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PractitionerOfficeTypes_Practitioners_PractitionerId",
                        column: x => x.PractitionerId,
                        principalTable: "Practitioners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                unique: true,
                filter: "[PractitionerTypesId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerAddresses_OfficeAddressesId",
                table: "PractitionerAddresses",
                column: "OfficeAddressesId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientAddressesId",
                table: "PatientDetails",
                column: "PatientAddressesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PractitionerOfficeTypes_OfficeTypesId",
                table: "PractitionerOfficeTypes",
                column: "OfficeTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientDetails_PatientAddress_PatientAddressesId",
                table: "PatientDetails",
                column: "PatientAddressesId",
                principalTable: "PatientAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientDetails_PatientDetailsId",
                table: "Patients",
                column: "PatientDetailsId",
                principalTable: "PatientDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddressesId",
                table: "PractitionerAddresses",
                column: "OfficeAddressesId",
                principalTable: "OfficeAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id");
        }
    }
}
