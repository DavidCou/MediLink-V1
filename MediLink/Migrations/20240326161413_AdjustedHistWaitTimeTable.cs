using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediLink.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedHistWaitTimeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOfDay",
                table: "WalkInClinicHistoricalWaitTimes");

            migrationBuilder.DropColumn(
                name: "WaitTime",
                table: "WalkInClinicHistoricalWaitTimes");

            migrationBuilder.AddColumn<int>(
                name: "WaitTimeInSeconds",
                table: "WalkInClinicHistoricalWaitTimes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<double>(
                name: "rating",
                table: "Practitioners",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WaitTimeInSeconds",
                table: "WalkInClinicHistoricalWaitTimes");

            migrationBuilder.AddColumn<string>(
                name: "TimeOfDay",
                table: "WalkInClinicHistoricalWaitTimes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WaitTime",
                table: "WalkInClinicHistoricalWaitTimes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "Practitioners",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
