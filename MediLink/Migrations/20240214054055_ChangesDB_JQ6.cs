using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class ChangesDBJQ6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypesId",
                table: "OfficeAddress");

            migrationBuilder.DropIndex(
                name: "IX_OfficeAddress_OfficeTypesId",
                table: "OfficeAddress");

            migrationBuilder.DropColumn(
                name: "OfficeTypesId",
                table: "OfficeAddress");

            migrationBuilder.AddColumn<bool>(
                name: "passwordReset",
                table: "Practitioners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "Practitioners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                name: "IX_PractitionerOfficeTypes_OfficeTypesId",
                table: "PractitionerOfficeTypes",
                column: "OfficeTypesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PractitionerOfficeTypes");

            migrationBuilder.DropColumn(
                name: "passwordReset",
                table: "Practitioners");

            migrationBuilder.DropColumn(
                name: "token",
                table: "Practitioners");

            migrationBuilder.AddColumn<int>(
                name: "OfficeTypesId",
                table: "OfficeAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OfficeAddress_OfficeTypesId",
                table: "OfficeAddress",
                column: "OfficeTypesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypesId",
                table: "OfficeAddress",
                column: "OfficeTypesId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
