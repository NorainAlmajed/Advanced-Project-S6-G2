using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMaintenanceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Tenants_TenantId",
                table: "MaintenanceRequests");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "MaintenanceRequests",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequests_TenantId",
                table: "MaintenanceRequests",
                newName: "IX_MaintenanceRequests_UserId");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "UserId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "UserId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "UserId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "UserId",
                value: 5);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                column: "UserId",
                value: 6);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Users_UserId",
                table: "MaintenanceRequests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_Users_UserId",
                table: "MaintenanceRequests");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MaintenanceRequests",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_MaintenanceRequests_UserId",
                table: "MaintenanceRequests",
                newName: "IX_MaintenanceRequests_TenantId");

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1,
                column: "TenantId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 2,
                column: "TenantId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 3,
                column: "TenantId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4,
                column: "TenantId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 5,
                column: "TenantId",
                value: 5);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_Tenants_TenantId",
                table: "MaintenanceRequests",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId");
        }
    }
}
