using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedDurationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Leases",
                newName: "DurationId");

            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "LeaseApplications",
                newName: "DurationId");

            migrationBuilder.CreateTable(
                name: "Durations",
                columns: table => new
                {
                    DurationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Months = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Durations", x => x.DurationId);
                });

            migrationBuilder.InsertData(
                table: "Durations",
                columns: new[] { "DurationId", "Months" },
                values: new object[,]
                {
                    { 1, 6 },
                    { 2, 12 },
                    { 3, 24 }
                });

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 1,
                column: "DurationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 2,
                column: "DurationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 3,
                column: "DurationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 4,
                column: "DurationId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 5,
                column: "DurationId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 6,
                column: "DurationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 7,
                column: "DurationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 8,
                column: "DurationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 1,
                column: "DurationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 2,
                column: "DurationId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 3,
                column: "DurationId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 4,
                column: "DurationId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 5,
                column: "DurationId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Leases_DurationId",
                table: "Leases",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplications_DurationId",
                table: "LeaseApplications",
                column: "DurationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaseApplications_Durations_DurationId",
                table: "LeaseApplications",
                column: "DurationId",
                principalTable: "Durations",
                principalColumn: "DurationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Leases_Durations_DurationId",
                table: "Leases",
                column: "DurationId",
                principalTable: "Durations",
                principalColumn: "DurationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaseApplications_Durations_DurationId",
                table: "LeaseApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Leases_Durations_DurationId",
                table: "Leases");

            migrationBuilder.DropTable(
                name: "Durations");

            migrationBuilder.DropIndex(
                name: "IX_Leases_DurationId",
                table: "Leases");

            migrationBuilder.DropIndex(
                name: "IX_LeaseApplications_DurationId",
                table: "LeaseApplications");

            migrationBuilder.RenameColumn(
                name: "DurationId",
                table: "Leases",
                newName: "Duration");

            migrationBuilder.RenameColumn(
                name: "DurationId",
                table: "LeaseApplications",
                newName: "Duration");

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 1,
                column: "Duration",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 2,
                column: "Duration",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 3,
                column: "Duration",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 4,
                column: "Duration",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 5,
                column: "Duration",
                value: 12);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 6,
                column: "Duration",
                value: 6);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 7,
                column: "Duration",
                value: 24);

            migrationBuilder.UpdateData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 8,
                column: "Duration",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 1,
                column: "Duration",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 2,
                column: "Duration",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 3,
                column: "Duration",
                value: 24);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 4,
                column: "Duration",
                value: 12);

            migrationBuilder.UpdateData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 5,
                column: "Duration",
                value: 24);
        }
    }
}
