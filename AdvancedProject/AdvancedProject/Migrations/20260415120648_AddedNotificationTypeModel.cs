using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedNotificationTypeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "NotificationTypeId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Notifications",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.NotificationTypeId);
                });

            migrationBuilder.InsertData(
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Lease" },
                    { 2, "LeaseApplication" },
                    { 3, "Maintenance" },
                    { 4, "Payment" }
                });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "A new lease application has been submitted.", 2, "New Lease Application" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "Your lease application has been approved.", 2, "Application Approved" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 3,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "Your maintenance request status has been updated.", 3, "Maintenance Update" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 4,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "Your payment has been successfully received.", 4, "Payment Received" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 5,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "Your lease is now active.", 1, "Lease Activated" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 6,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "You have been assigned a new maintenance request.", 3, "New Assignment" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 7,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "Maintenance work is now in progress.", 3, "Work In Progress" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 8,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "A new HVAC issue has been reported.", 3, "Issue Reported" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 9,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "A carpentry issue has been reported.", 3, "Issue Reported" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 10,
                columns: new[] { "Message", "NotificationTypeId", "Title" },
                values: new object[] { "A painting issue has been reported.", 3, "Issue Reported" });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "NotificationTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Notifications");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Notifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1,
                columns: new[] { "Message", "Type" },
                values: new object[] { "New lease application received", "LeaseApplication" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Your application has been approved", "LeaseApplication" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 3,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Maintenance request updated", "MaintenanceRequest" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 4,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Payment received successfully", "Payment" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 5,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Lease activated for your unit", "Lease" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 6,
                columns: new[] { "Message", "Type" },
                values: new object[] { "New maintenance request assigned", "MaintenanceRequest" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 7,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Electrical repair marked as in progress", "MaintenanceRequest" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 8,
                columns: new[] { "Message", "Type" },
                values: new object[] { "HVAC issue reported in Unit A2", "MaintenanceRequest" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 9,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Carpentry issue reported in Unit B2", "MaintenanceRequest" });

            migrationBuilder.UpdateData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 10,
                columns: new[] { "Message", "Type" },
                values: new object[] { "Painting issue reported in Unit A3", "MaintenanceRequest" });
        }
    }
}
