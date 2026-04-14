using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnitTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Units");

            migrationBuilder.AddColumn<int>(
                name: "UnitTypeId",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UnitTypes",
                columns: table => new
                {
                    UnitTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitTypes", x => x.UnitTypeId);
                });

            migrationBuilder.InsertData(
                table: "UnitTypes",
                columns: new[] { "UnitTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Apartment" },
                    { 2, "Office" },
                    { 3, "Studio" }
                });

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 1,
                column: "UnitTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 2,
                column: "UnitTypeId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 3,
                column: "UnitTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 4,
                column: "UnitTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 5,
                column: "UnitTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 6,
                column: "UnitTypeId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitTypeId",
                table: "Units",
                column: "UnitTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Units_UnitTypes_UnitTypeId",
                table: "Units",
                column: "UnitTypeId",
                principalTable: "UnitTypes",
                principalColumn: "UnitTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Units_UnitTypes_UnitTypeId",
                table: "Units");

            migrationBuilder.DropTable(
                name: "UnitTypes");

            migrationBuilder.DropIndex(
                name: "IX_Units_UnitTypeId",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "UnitTypeId",
                table: "Units");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Units",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 1,
                column: "Type",
                value: "Apartment");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 2,
                column: "Type",
                value: "Apartment");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 3,
                column: "Type",
                value: "Office");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 4,
                column: "Type",
                value: "Office");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 5,
                column: "Type",
                value: "Studio");

            migrationBuilder.UpdateData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 6,
                column: "Type",
                value: "Apartment");
        }
    }
}
