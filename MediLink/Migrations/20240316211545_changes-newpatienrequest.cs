using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class changesnewpatienrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientCity",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientCountry",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientEmail",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientFirstName",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientGender",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientLastName",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientPhoneNumber",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientPostalCode",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientProvince",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PatientStreetAddress",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PractitionerEmail",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PractitionerFirstName",
                table: "NewPatientRequests");

            migrationBuilder.DropColumn(
                name: "PractitionerLastName",
                table: "NewPatientRequests");

            migrationBuilder.RenameColumn(
                name: "PatientDoB",
                table: "NewPatientRequests",
                newName: "DateRequest");

            migrationBuilder.AddColumn<int>(
                name: "officePractitionerId",
                table: "NewPatientRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "officePractitionerId",
                table: "NewPatientRequests");

            migrationBuilder.RenameColumn(
                name: "DateRequest",
                table: "NewPatientRequests",
                newName: "PatientDoB");

            migrationBuilder.AddColumn<string>(
                name: "PatientCity",
                table: "NewPatientRequests",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientCountry",
                table: "NewPatientRequests",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientEmail",
                table: "NewPatientRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientFirstName",
                table: "NewPatientRequests",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientGender",
                table: "NewPatientRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientLastName",
                table: "NewPatientRequests",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientPhoneNumber",
                table: "NewPatientRequests",
                type: "VARCHAR(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientPostalCode",
                table: "NewPatientRequests",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientProvince",
                table: "NewPatientRequests",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientStreetAddress",
                table: "NewPatientRequests",
                type: "VARCHAR(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerEmail",
                table: "NewPatientRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerFirstName",
                table: "NewPatientRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerLastName",
                table: "NewPatientRequests",
                type: "VARCHAR(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
