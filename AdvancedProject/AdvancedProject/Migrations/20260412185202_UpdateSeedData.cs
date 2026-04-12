using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 1,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Building: 611, Road: 271, Block: 220", "A modern residential complex offering comfort and essential amenities." });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 2,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Building: 246, Road: 811, Block: 708", "A contemporary tower with modern facilities in a prime location." });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 3,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Building: 911, Road: 3062, Block: 461", "A residential property with spacious apartments for families." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 1,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Manama", "Nice" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 2,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Muharraq", "Modern building" });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 3,
                columns: new[] { "Address", "Description" },
                values: new object[] { "Riffa", "Family apartments" });
        }
    }
}
