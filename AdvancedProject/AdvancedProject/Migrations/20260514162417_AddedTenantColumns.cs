using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class AddedTenantColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmploymentStatus",
                table: "Tenants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FinancialStability",
                table: "Tenants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "Tenants",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "Tenants",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { null, "Undetermined", null, null });

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
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { null, "Undetermined", null, null });

            migrationBuilder.UpdateData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 5,
                columns: new[] { "EmploymentStatus", "FinancialStability", "MaritalStatus", "Salary" },
                values: new object[] { null, "Undetermined", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "FinancialStability",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Tenants");
        }
    }
}
