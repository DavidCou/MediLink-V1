using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class ChangesDBJQ7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.DropIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.AlterColumn<int>(
                name: "PractitionerTypesId",
                table: "Practitioners",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                unique: true,
                filter: "[PractitionerTypesId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.DropIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners");

            migrationBuilder.AlterColumn<int>(
                name: "PractitionerTypesId",
                table: "Practitioners",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Practitioners_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Practitioners_PractitionerTypes_PractitionerTypesId",
                table: "Practitioners",
                column: "PractitionerTypesId",
                principalTable: "PractitionerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
