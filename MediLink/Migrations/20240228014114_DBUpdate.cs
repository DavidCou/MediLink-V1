using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class DBUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypeId",
                table: "OfficeAddress");

            migrationBuilder.DropForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddressesId",
                table: "PractitionerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkInClinics_OfficeAddress_OfficeAddressId",
                table: "WalkInClinics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAddress",
                table: "OfficeAddress");

            migrationBuilder.RenameTable(
                name: "OfficeAddress",
                newName: "OfficeAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeAddress_OfficeTypeId",
                table: "OfficeAddresses",
                newName: "IX_OfficeAddresses_OfficeTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeTypeId",
                table: "OfficeAddresses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAddresses",
                table: "OfficeAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAddresses_OfficeTypes_OfficeTypeId",
                table: "OfficeAddresses",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddresses_OfficeAddressesId",
                table: "PractitionerAddresses",
                column: "OfficeAddressesId",
                principalTable: "OfficeAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalkInClinics_OfficeAddresses_OfficeAddressId",
                table: "WalkInClinics",
                column: "OfficeAddressId",
                principalTable: "OfficeAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfficeAddresses_OfficeTypes_OfficeTypeId",
                table: "OfficeAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddresses_OfficeAddressesId",
                table: "PractitionerAddresses");

            migrationBuilder.DropForeignKey(
                name: "FK_WalkInClinics_OfficeAddresses_OfficeAddressId",
                table: "WalkInClinics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OfficeAddresses",
                table: "OfficeAddresses");

            migrationBuilder.RenameTable(
                name: "OfficeAddresses",
                newName: "OfficeAddress");

            migrationBuilder.RenameIndex(
                name: "IX_OfficeAddresses_OfficeTypeId",
                table: "OfficeAddress",
                newName: "IX_OfficeAddress_OfficeTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "OfficeTypeId",
                table: "OfficeAddress",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OfficeAddress",
                table: "OfficeAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OfficeAddress_OfficeTypes_OfficeTypeId",
                table: "OfficeAddress",
                column: "OfficeTypeId",
                principalTable: "OfficeTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PractitionerAddresses_OfficeAddress_OfficeAddressesId",
                table: "PractitionerAddresses",
                column: "OfficeAddressesId",
                principalTable: "OfficeAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WalkInClinics_OfficeAddress_OfficeAddressId",
                table: "WalkInClinics",
                column: "OfficeAddressId",
                principalTable: "OfficeAddress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
