using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class ChangesDBJQ3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientPreferences_PatientPreferencesId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientPreferencesId",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "PatientPreferencesId",
                table: "Patients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientPreferencesId",
                table: "Patients",
                column: "PatientPreferencesId",
                unique: true,
                filter: "[PatientPreferencesId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientPreferences_PatientPreferencesId",
                table: "Patients",
                column: "PatientPreferencesId",
                principalTable: "PatientPreferences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_PatientPreferences_PatientPreferencesId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_PatientPreferencesId",
                table: "Patients");

            migrationBuilder.AlterColumn<int>(
                name: "PatientPreferencesId",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientPreferencesId",
                table: "Patients",
                column: "PatientPreferencesId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_PatientPreferences_PatientPreferencesId",
                table: "Patients",
                column: "PatientPreferencesId",
                principalTable: "PatientPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
