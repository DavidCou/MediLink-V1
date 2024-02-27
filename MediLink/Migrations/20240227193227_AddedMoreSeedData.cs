using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PractitionerTypes",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "A general health practitioner", false, "Family Doctor" },
                    { 2, "A general health practitioner that specializes in child healthcare", false, "Pediatrician" },
                    { 3, "", false, "Walk-in Clinic" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PractitionerTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PractitionerTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PractitionerTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
