using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class ChangesDBJQ8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientSpokenLanguages_PatientPreferences_PatientPreferenceId",
                table: "PatientSpokenLanguages");

            migrationBuilder.RenameColumn(
                name: "PatientPreferenceId",
                table: "PatientSpokenLanguages",
                newName: "PatientDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientSpokenLanguages_PatientPreferenceId",
                table: "PatientSpokenLanguages",
                newName: "IX_PatientSpokenLanguages_PatientDetailsId");

            migrationBuilder.CreateTable(
                name: "PreferedLanguages",
                columns: table => new
                {
                    PatientPreferenceId = table.Column<int>(type: "int", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferedLanguages", x => new { x.LanguageId, x.PatientPreferenceId });
                    table.ForeignKey(
                        name: "FK_PreferedLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferedLanguages_PatientPreferences_PatientPreferenceId",
                        column: x => x.PatientPreferenceId,
                        principalTable: "PatientPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreferedLanguages_PatientPreferenceId",
                table: "PreferedLanguages",
                column: "PatientPreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientSpokenLanguages_PatientDetails_PatientDetailsId",
                table: "PatientSpokenLanguages",
                column: "PatientDetailsId",
                principalTable: "PatientDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientSpokenLanguages_PatientDetails_PatientDetailsId",
                table: "PatientSpokenLanguages");

            migrationBuilder.DropTable(
                name: "PreferedLanguages");

            migrationBuilder.RenameColumn(
                name: "PatientDetailsId",
                table: "PatientSpokenLanguages",
                newName: "PatientPreferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_PatientSpokenLanguages_PatientDetailsId",
                table: "PatientSpokenLanguages",
                newName: "IX_PatientSpokenLanguages_PatientPreferenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientSpokenLanguages_PatientPreferences_PatientPreferenceId",
                table: "PatientSpokenLanguages",
                column: "PatientPreferenceId",
                principalTable: "PatientPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
