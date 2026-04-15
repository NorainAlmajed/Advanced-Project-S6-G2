using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "Status",
                value: "Pending");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "Status",
                value: "Assigned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "Status",
                value: "Submitted");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "Status",
                value: "Submitted");
        }
    }
}
