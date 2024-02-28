using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class DBTweak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfficeName",
                table: "OfficeTypes",
                newName: "OfficeTypeName");

            migrationBuilder.AddColumn<string>(
                name: "OfficeName",
                table: "OfficeAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OfficeName",
                table: "OfficeAddresses");

            migrationBuilder.RenameColumn(
                name: "OfficeTypeName",
                table: "OfficeTypes",
                newName: "OfficeName");
        }
    }
}
