using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTenantSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                columns: new[] { "DOB", "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { new DateOnly(2005, 10, 18), "Employed", "Stable", "Single", 2000m });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 2,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { "Self-Employed", "Moderately Stable", "Married", 1500m });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 3,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { "Unemployed", "Unstable", "Divorced", 800m });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 4,
                columns: new[] { "EmploymentStatus", "MaritalStatus", "Salary" },
                values: new object[] { "Employed", "Married", 3200m });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 5,
                columns: new[] { "EmploymentStatus", "MaritalStatus", "Salary" },
                values: new object[] { "Retired", "Widowed", 1100m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                columns: new[] { "DOB", "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { new DateOnly(2004, 10, 18), null, "Undetermined", null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 2,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { null, "Undetermined", null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 3,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { null, "Undetermined", null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 4,
                columns: new[] { "EmploymentStatus", "MaritalStatus", "Salary" },
                values: new object[] { null, null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 5,
                columns: new[] { "EmploymentStatus", "MaritalStatus", "Salary" },
                values: new object[] { null, null, null });
        }
    }
}
