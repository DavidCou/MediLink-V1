using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class DbAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientEmail",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "PatientFirstName",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "PatientLastName",
                table: "WaitLists");

            migrationBuilder.DropColumn(
                name: "PatientFirstName",
                table: "PractitionerReviews");

            migrationBuilder.DropColumn(
                name: "PractitionerEmail",
                table: "PractitionerReviews");

            migrationBuilder.DropColumn(
                name: "PractitionerFirstName",
                table: "PractitionerReviews");

            migrationBuilder.DropColumn(
                name: "PractitionerLastName",
                table: "PractitionerReviews");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "WaitLists",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "WaitLists");

            migrationBuilder.AddColumn<string>(
                name: "PatientEmail",
                table: "WaitLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientFirstName",
                table: "WaitLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientLastName",
                table: "WaitLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientFirstName",
                table: "PractitionerReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerEmail",
                table: "PractitionerReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerFirstName",
                table: "PractitionerReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PractitionerLastName",
                table: "PractitionerReviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
