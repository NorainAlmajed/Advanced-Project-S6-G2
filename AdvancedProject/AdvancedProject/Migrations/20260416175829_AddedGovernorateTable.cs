using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedGovernorateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "Properties",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Governorates",
                columns: table => new
                {
                    GovernorateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.GovernorateId);
                });

            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "GovernorateId", "Name" },
                values: new object[,]
                {
                    { 1, "Capital Governorate" },
                    { 2, "Muharraq Governorate" },
                    { 3, "Northern Governorate" },
                    { 4, "Southern Governorate" }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "CreatedAt", "Message", "NotificationTypeId", "Title", "UserId" },
                values: new object[] { 11, new DateTime(2026, 3, 11, 13, 44, 21, 0, DateTimeKind.Unspecified), "Your payment has been successfully received.", 3, "Payment Received", 2 });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 1,
                column: "GovernorateId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 2,
                column: "GovernorateId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 3,
                column: "GovernorateId",
                value: 4);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_GovernorateId",
                table: "Properties",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Governorates_GovernorateId",
                table: "Properties",
                column: "GovernorateId",
                principalTable: "Governorates",
                principalColumn: "GovernorateId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Governorates_GovernorateId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "Governorates");

            migrationBuilder.DropIndex(
                name: "IX_Properties_GovernorateId",
                table: "Properties");

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 11);

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "Properties");
        }
    }
}
