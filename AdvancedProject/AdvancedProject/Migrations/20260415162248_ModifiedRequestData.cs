using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedRequestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "Status",
                value: "Pending");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "Status",
                value: "Assigned");
        }
    }
}
