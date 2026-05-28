using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesOnSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                column: "DOB",
                value: new DateOnly(2004, 10, 18));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                column: "DOB",
                value: new DateOnly(2005, 10, 18));
        }
    }
}
