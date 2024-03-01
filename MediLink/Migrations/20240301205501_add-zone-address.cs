using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class addzoneaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "zone",
                table: "OfficeAddresses",
                type: "VARCHAR(250)",
                maxLength: 250,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zone",
                table: "OfficeAddresses");
        }
    }
}
