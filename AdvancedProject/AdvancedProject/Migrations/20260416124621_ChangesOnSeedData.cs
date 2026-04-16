using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class ChangesOnSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 2,
                column: "Name",
                value: "Maintenance");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 3,
                column: "Name",
                value: "Payment");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                column: "NotificationTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                column: "NotificationTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 3,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 4,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 6,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 7,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 8,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 9,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 10,
                column: "NotificationTypeId",
                value: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 2,
                column: "Name",
                value: "LeaseApplication");

            migrationBuilder.UpdateData(
                table: "NotificationTypes",
                keyColumn: "NotificationTypeId",
                keyValue: 3,
                column: "Name",
                value: "Maintenance");

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "Name" },
                values: new object[] { 4, "Payment" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                column: "NotificationTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 3,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 4,
                column: "NotificationTypeId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 6,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 7,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 8,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 9,
                column: "NotificationTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 10,
                column: "NotificationTypeId",
                value: 3);
        }
    }
}
