using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProjectAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amenities", x => x.AmenityId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "PaymentFrequencies",
                columns: table => new
                {
                    PaymentFrequencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentFrequencies", x => x.PaymentFrequencyId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                });

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

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    PropertyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Block = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Building = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Road = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    GovernorateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.PropertyId);
                    table.ForeignKey(
                        name: "FK_Properties_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalTable: "Governorates",
                        principalColumn: "GovernorateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceStaff",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStaff", x => x.StaffId);
                    table.ForeignKey(
                        name: "FK_MaintenanceStaff_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "NotificationTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "PropertyManagers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyManagers", x => x.ManagerId);
                    table.ForeignKey(
                        name: "FK_PropertyManagers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinancialStability = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    SizeSqFt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AvailabilityStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.UnitId);
                    table.ForeignKey(
                        name: "FK_Units_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "PropertyId");
                    table.ForeignKey(
                        name: "FK_Units_UnitTypes_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "UnitTypes",
                        principalColumn: "UnitTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceStaffSkills",
                columns: table => new
                {
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceStaffSkills", x => new { x.StaffId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_MaintenanceStaffSkills_MaintenanceStaff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "MaintenanceStaff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_MaintenanceStaffSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "LeaseApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ApproveTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseApplications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_LeaseApplications_Durations_DurationId",
                        column: x => x.DurationId,
                        principalTable: "Durations",
                        principalColumn: "DurationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseApplications_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_LeaseApplications_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "Leases",
                columns: table => new
                {
                    LeaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthlyRent = table.Column<decimal>(type: "decimal(10,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DurationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leases", x => x.LeaseId);
                    table.ForeignKey(
                        name: "FK_Leases_Durations_DurationId",
                        column: x => x.DurationId,
                        principalTable: "Durations",
                        principalColumn: "DurationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leases_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_Leases_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getdate())"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AssignedStaffId = table.Column<int>(type: "int", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResolvedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InProgressTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_MaintenanceStaff_AssignedStaffId",
                        column: x => x.AssignedStaffId,
                        principalTable: "MaintenanceStaff",
                        principalColumn: "StaffId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                    table.ForeignKey(
                        name: "FK_MaintenanceRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UnitAmenities",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    AmenityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitAmenities", x => new { x.UnitId, x.AmenityId });
                    table.ForeignKey(
                        name: "FK_UnitAmenities_Amenities_AmenityId",
                        column: x => x.AmenityId,
                        principalTable: "Amenities",
                        principalColumn: "AmenityId");
                    table.ForeignKey(
                        name: "FK_UnitAmenities_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "UnitId");
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaseId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    PaymentFrequencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Leases_LeaseId",
                        column: x => x.LeaseId,
                        principalTable: "Leases",
                        principalColumn: "LeaseId");
                    table.ForeignKey(
                        name: "FK_Payments_PaymentFrequencies_PaymentFrequencyId",
                        column: x => x.PaymentFrequencyId,
                        principalTable: "PaymentFrequencies",
                        principalColumn: "PaymentFrequencyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "AmenityId", "Name" },
                values: new object[,]
                {
                    { 1, "Parking" },
                    { 2, "Swimming Pool" },
                    { 3, "Gym" },
                    { 4, "Security" },
                    { 5, "Elevator" },
                    { 6, "Central AC" },
                    { 7, "WiFi" }
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
                table: "NotificationTypes",
                columns: new[] { "NotificationTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "Lease" },
                    { 2, "Maintenance" },
                    { 3, "Payment" }
                });

            migrationBuilder.InsertData(
                table: "PaymentFrequencies",
                columns: new[] { "PaymentFrequencyId", "Frequency", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Monthly" },
                    { 2, 3, "Quarterly (every 3 months)" },
                    { 3, 6, "Semi-Annual (every 6 months)" },
                    { 4, 12, "Yearly" }
                });

            migrationBuilder.InsertData(
                table: "PaymentMethods",
                columns: new[] { "PaymentMethodId", "Name" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "Bank Transfer" },
                    { 3, "Credit Card" },
                    { 4, "Debit Card" },
                    { 5, "BenefitPay" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "SkillId", "Name" },
                values: new object[,]
                {
                    { 1, "Plumbing" },
                    { 2, "Electrical" },
                    { 3, "HVAC" },
                    { 4, "Carpentry" },
                    { 5, "Painting" }
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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "Gender", "IsActive", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager@mail.com", "System Manager", "M", true, "Manager123", "33338876", "Manager", "manager" },
                    { 2, new DateTime(2026, 2, 12, 2, 3, 4, 0, DateTimeKind.Unspecified), "zahraa.hubail8@gmail.com", "Zahraa Hubail", "F", true, "Zahraa.123", "33735771", "Tenant", "zahraa.hubail" },
                    { 3, new DateTime(2026, 3, 15, 15, 12, 55, 0, DateTimeKind.Unspecified), "raghad@gmail.com", "Raghad Aleskafi", "F", true, "Raghad.123", "39004266", "Tenant", "raghad.aleskafi" },
                    { 4, new DateTime(2026, 3, 20, 6, 11, 2, 0, DateTimeKind.Unspecified), "fatima@gmail.com", "Fatima Alaiwi", "F", true, "Fatima.123", "36635578", "Tenant", "fatima.alaiwi" },
                    { 5, new DateTime(2026, 3, 25, 5, 15, 27, 0, DateTimeKind.Unspecified), "norain@mail.com", "Norain Almajed", "F", true, "Norain.123", "33744063", "Tenant", "norain.almajed" },
                    { 6, new DateTime(2026, 3, 28, 7, 17, 22, 0, DateTimeKind.Unspecified), "ahmed.ali@gmail.com", "Ahmed Ali", "M", true, "Ahmed.999", "33871125", "Tenant", "ahmed.ali" },
                    { 7, new DateTime(2026, 3, 10, 9, 16, 34, 0, DateTimeKind.Unspecified), "alihassan@mail.com", "Ali Hassan", "M", true, "Ali.123", "39207552", "Staff", "ali.hassan" },
                    { 8, new DateTime(2026, 3, 11, 9, 10, 10, 0, DateTimeKind.Unspecified), "sara.mohamed@gmail.com", "Sara Mohamed", "F", true, "Sara.888", "33699152", "Staff", "sara.mohamed" },
                    { 9, new DateTime(2026, 3, 12, 10, 2, 15, 0, DateTimeKind.Unspecified), "abbas@gmail.com", "Abbas Hadi", "M", true, "Abbas.123", "33546672", "Staff", "abbas.hadi" },
                    { 10, new DateTime(2026, 3, 13, 6, 21, 41, 0, DateTimeKind.Unspecified), "layla@gmail.com", "Layla Yaser", "F", true, "Layla.999", "39126632", "Staff", "layla.yaser" },
                    { 11, new DateTime(2026, 3, 14, 8, 13, 44, 0, DateTimeKind.Unspecified), "mohammed@gmail.com", "Mohammed Karim", "M", true, "mohammed.123", "33921092", "Staff", "mohammed.karim" }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceStaff",
                columns: new[] { "StaffId", "AvailabilityStatus", "UserId" },
                values: new object[,]
                {
                    { 1, "Available", 7 },
                    { 2, "Busy", 8 },
                    { 3, "Available", 9 },
                    { 4, "Available", 10 },
                    { 5, "Busy", 11 }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "NotificationId", "CreatedAt", "Message", "NotificationTypeId", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 10, 33, 21, 0, DateTimeKind.Unspecified), "A new lease application has been submitted.", 1, "New Lease Application", 1 },
                    { 2, new DateTime(2026, 2, 3, 9, 18, 55, 0, DateTimeKind.Unspecified), "Your lease application has been approved.", 1, "Application Approved", 2 },
                    { 3, new DateTime(2026, 3, 2, 14, 25, 14, 0, DateTimeKind.Unspecified), "Your maintenance request status has been updated.", 2, "Maintenance Update", 3 },
                    { 4, new DateTime(2026, 3, 12, 13, 44, 21, 0, DateTimeKind.Unspecified), "Your payment has been successfully received.", 3, "Payment Received", 4 },
                    { 5, new DateTime(2026, 3, 15, 21, 4, 17, 0, DateTimeKind.Unspecified), "Your lease is now active.", 1, "Lease Activated", 5 },
                    { 6, new DateTime(2026, 3, 1, 12, 6, 33, 0, DateTimeKind.Unspecified), "You have been assigned a new maintenance request.", 2, "New Assignment", 7 },
                    { 7, new DateTime(2026, 3, 2, 8, 15, 32, 0, DateTimeKind.Unspecified), "Maintenance work is now in progress.", 2, "Work In Progress", 8 },
                    { 8, new DateTime(2026, 3, 3, 21, 17, 28, 0, DateTimeKind.Unspecified), "A new HVAC issue has been reported.", 2, "Issue Reported", 9 },
                    { 9, new DateTime(2026, 3, 4, 17, 5, 43, 0, DateTimeKind.Unspecified), "A carpentry issue has been reported.", 2, "Issue Reported", 10 },
                    { 10, new DateTime(2026, 3, 5, 23, 14, 10, 0, DateTimeKind.Unspecified), "A painting issue has been reported.", 2, "Issue Reported", 11 },
                    { 11, new DateTime(2026, 3, 11, 13, 44, 21, 0, DateTimeKind.Unspecified), "Your payment has been successfully received.", 3, "Payment Received", 2 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "PropertyId", "Block", "Building", "City", "CreatedAt", "Description", "GovernorateId", "IsActive", "Name", "Road" },
                values: new object[,]
                {
                    { 1, "220", "611", "Manama", new DateTime(2026, 1, 1, 12, 55, 21, 0, DateTimeKind.Unspecified), "A modern residential complex offering comfort and essential amenities.", 1, true, "Abraj Al Lulu", "271" },
                    { 2, "708", "246", "Muharraq", new DateTime(2026, 1, 5, 15, 22, 29, 0, DateTimeKind.Unspecified), "A contemporary tower with modern facilities in a prime location.", 2, true, "Almoayyed Tower", "811" },
                    { 3, "461", "922", "Riffa", new DateTime(2026, 1, 10, 3, 31, 43, 0, DateTimeKind.Unspecified), "A residential property with spacious apartments for families.", 4, true, "United Tower", "3062" }
                });

            migrationBuilder.InsertData(
                table: "PropertyManagers",
                columns: new[] { "ManagerId", "HireDate", "UserId" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "DOB", "EmploymentStatus", "FinancialStability", "MaritalStatus", "NationalId", "Salary", "UserId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2005, 10, 18), "Employed", "Stable", "Single", "041081254", 2000m, 2 },
                    { 2, new DateOnly(1995, 3, 26), "Self-Employed", "Moderately Stable", "Married", "950306321", 1500m, 3 },
                    { 3, new DateOnly(1977, 9, 9), "Unemployed", "Unstable", "Divorced", "770907721", 800m, 4 },
                    { 4, new DateOnly(1989, 11, 25), "Employed", "Undetermined", "Married", "891106213", 3200m, 5 },
                    { 5, new DateOnly(1982, 7, 18), "Retired", "Undetermined", "Widowed", "820752231", 1100m, 6 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceStaffSkills",
                columns: new[] { "SkillId", "StaffId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 2 },
                    { 1, 3 },
                    { 4, 3 },
                    { 5, 4 },
                    { 3, 5 }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "UnitId", "AvailabilityStatus", "CreatedAt", "IsActive", "PropertyId", "RentAmount", "SizeSqFt", "UnitNumber", "UnitTypeId" },
                values: new object[,]
                {
                    { 1, "Available", new DateTime(2026, 1, 1, 4, 12, 55, 0, DateTimeKind.Unspecified), true, 1, 300m, 100m, "A1", 1 },
                    { 2, "Occupied", new DateTime(2026, 1, 2, 23, 16, 33, 0, DateTimeKind.Unspecified), true, 1, 350m, 120m, "A2", 1 },
                    { 3, "Available", new DateTime(2026, 1, 3, 9, 11, 7, 0, DateTimeKind.Unspecified), true, 2, 500m, 200m, "B1", 2 },
                    { 4, "Occupied", new DateTime(2026, 1, 4, 7, 16, 22, 0, DateTimeKind.Unspecified), true, 2, 550m, 250m, "B2", 2 },
                    { 5, "Available", new DateTime(2026, 1, 5, 10, 10, 12, 0, DateTimeKind.Unspecified), true, 1, 250m, 80m, "A3", 3 },
                    { 6, "Available", new DateTime(2026, 1, 6, 4, 15, 45, 0, DateTimeKind.Unspecified), true, 3, 320m, 110m, "C1", 1 }
                });

            migrationBuilder.InsertData(
                table: "LeaseApplications",
                columns: new[] { "ApplicationId", "ApplicationDate", "ApproveTime", "DurationId", "RejectTime", "StartDate", "Status", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 20, 30, 33, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 2, 13, 22, 17, 0, DateTimeKind.Unspecified), 1, null, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Approved", 1, 1 },
                    { 2, new DateTime(2026, 2, 3, 3, 7, 12, 0, DateTimeKind.Unspecified), null, 3, null, new DateTime(2026, 5, 5, 18, 9, 24, 0, DateTimeKind.Unspecified), "Pending", 2, 2 },
                    { 3, new DateTime(2026, 2, 5, 5, 24, 13, 0, DateTimeKind.Unspecified), null, 1, new DateTime(2026, 2, 22, 3, 17, 29, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 1, 9, 10, 22, 0, DateTimeKind.Unspecified), "Rejected", 3, 3 },
                    { 4, new DateTime(2026, 2, 2, 22, 55, 2, 0, DateTimeKind.Unspecified), new DateTime(2026, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, null, new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "Approved", 4, 4 },
                    { 5, new DateTime(2026, 2, 9, 9, 33, 11, 0, DateTimeKind.Unspecified), null, 2, null, new DateTime(2026, 4, 20, 14, 22, 5, 0, DateTimeKind.Unspecified), "Pending", 5, 5 },
                    { 6, new DateTime(2026, 3, 3, 10, 21, 10, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 5, 6, 44, 3, 0, DateTimeKind.Unspecified), 1, null, new DateTime(2026, 7, 1, 12, 6, 12, 0, DateTimeKind.Unspecified), "Approved", 2, 3 },
                    { 7, new DateTime(2026, 1, 12, 6, 10, 9, 0, DateTimeKind.Unspecified), new DateTime(2026, 3, 11, 7, 52, 33, 0, DateTimeKind.Unspecified), 3, null, new DateTime(2026, 3, 10, 11, 1, 22, 0, DateTimeKind.Unspecified), "Approved", 3, 1 },
                    { 8, new DateTime(2026, 1, 12, 14, 32, 17, 0, DateTimeKind.Unspecified), new DateTime(2026, 1, 15, 4, 32, 29, 0, DateTimeKind.Unspecified), 3, null, new DateTime(2026, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Approved", 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Leases",
                columns: new[] { "LeaseId", "CreatedAt", "DurationId", "EndDate", "MonthlyRent", "StartDate", "Status", "TenantId", "TerminationDate", "UnitId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 2, 13, 22, 17, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 12, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), 300m, new DateTime(2026, 6, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1, null, 1 },
                    { 2, new DateTime(2026, 3, 5, 6, 44, 3, 0, DateTimeKind.Unspecified), 1, new DateTime(2026, 11, 5, 6, 44, 3, 0, DateTimeKind.Unspecified), 500m, new DateTime(2026, 3, 5, 6, 44, 3, 0, DateTimeKind.Unspecified), "Active", 2, null, 3 },
                    { 3, new DateTime(2026, 3, 10, 7, 52, 33, 0, DateTimeKind.Unspecified), 3, new DateTime(2027, 3, 10, 8, 0, 0, 0, DateTimeKind.Unspecified), 350m, new DateTime(2026, 3, 10, 11, 1, 22, 0, DateTimeKind.Unspecified), "Terminated", 3, null, 1 },
                    { 4, new DateTime(2026, 2, 5, 14, 30, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2027, 3, 14, 8, 0, 0, 0, DateTimeKind.Unspecified), 550m, new DateTime(2026, 3, 15, 8, 0, 0, 0, DateTimeKind.Unspecified), "Active", 4, null, 4 },
                    { 5, new DateTime(2026, 1, 15, 4, 32, 29, 0, DateTimeKind.Unspecified), 3, new DateTime(2028, 1, 31, 8, 0, 0, 0, DateTimeKind.Unspecified), 250m, new DateTime(2026, 2, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), "Active", 5, null, 5 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequests",
                columns: new[] { "RequestId", "AssignedStaffId", "AssignedTime", "ClosedTime", "CompletedDate", "InProgressTime", "Notes", "Priority", "RequestDate", "ResolvedTime", "SkillId", "Status", "UnitId", "UserId" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, null, "Water leaking from bathroom pipe", "High", new DateTime(2026, 3, 1, 13, 12, 3, 0, DateTimeKind.Unspecified), null, 1, "Pending", 2, 2 },
                    { 2, 2, null, null, null, null, "Living room light not working", "Medium", new DateTime(2026, 3, 2, 23, 12, 42, 0, DateTimeKind.Unspecified), null, 2, "In Progress", 3, 3 },
                    { 3, 3, null, null, null, null, "AC cooling is weak", "Low", new DateTime(2026, 3, 3, 20, 20, 4, 0, DateTimeKind.Unspecified), null, 3, "Resolved", 1, 4 },
                    { 4, 4, null, null, null, null, "Front door lock is broken", "High", new DateTime(2026, 3, 4, 2, 44, 11, 0, DateTimeKind.Unspecified), null, 4, "Pending", 4, 5 },
                    { 5, 5, null, null, null, null, "Wall paint is fading and peeling", "Low", new DateTime(2026, 3, 5, 11, 32, 0, 0, DateTimeKind.Unspecified), null, 5, "Closed", 5, 6 }
                });

            migrationBuilder.InsertData(
                table: "UnitAmenities",
                columns: new[] { "AmenityId", "UnitId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 4, 1 },
                    { 1, 2 },
                    { 5, 2 },
                    { 3, 3 },
                    { 4, 3 },
                    { 6, 3 },
                    { 4, 4 },
                    { 5, 4 },
                    { 1, 5 },
                    { 7, 5 },
                    { 2, 6 },
                    { 3, 6 },
                    { 4, 6 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "PaymentId", "Amount", "EndDate", "LeaseId", "PaymentFrequencyId", "PaymentMethodId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 300m, new DateTime(2026, 3, 8, 12, 11, 5, 0, DateTimeKind.Unspecified), 1, 1, 1, new DateTime(2026, 3, 1, 12, 11, 5, 0, DateTimeKind.Unspecified), "Paid" },
                    { 2, 6000m, new DateTime(2026, 3, 12, 9, 33, 12, 0, DateTimeKind.Unspecified), 2, 4, 4, new DateTime(2026, 3, 5, 9, 33, 12, 0, DateTimeKind.Unspecified), "Paid" },
                    { 3, 2100m, new DateTime(2026, 3, 17, 22, 11, 9, 0, DateTimeKind.Unspecified), 3, 3, 2, new DateTime(2026, 3, 10, 22, 11, 9, 0, DateTimeKind.Unspecified), "Late" },
                    { 4, 1650m, new DateTime(2026, 3, 19, 7, 15, 22, 0, DateTimeKind.Unspecified), 4, 2, 5, new DateTime(2026, 3, 12, 7, 15, 22, 0, DateTimeKind.Unspecified), "Paid" },
                    { 5, 250m, new DateTime(2026, 3, 22, 8, 19, 27, 0, DateTimeKind.Unspecified), 5, 1, 3, new DateTime(2026, 3, 15, 8, 19, 27, 0, DateTimeKind.Unspecified), "Pending" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplications_DurationId",
                table: "LeaseApplications",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplications_TenantId",
                table: "LeaseApplications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseApplications_UnitId",
                table: "LeaseApplications",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_DurationId",
                table: "Leases",
                column: "DurationId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_TenantId",
                table: "Leases",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Leases_UnitId",
                table: "Leases",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_AssignedStaffId",
                table: "MaintenanceRequests",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_SkillId",
                table: "MaintenanceRequests",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_UnitId",
                table: "MaintenanceRequests",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceRequests_UserId",
                table: "MaintenanceRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStaff_UserId",
                table: "MaintenanceStaff",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceStaffSkills_SkillId",
                table: "MaintenanceStaffSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_LeaseId",
                table: "Payments",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentFrequencyId",
                table: "Payments",
                column: "PaymentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethodId",
                table: "Payments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_GovernorateId",
                table: "Properties",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyManagers_UserId",
                table: "PropertyManagers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitAmenities_AmenityId",
                table: "UnitAmenities",
                column: "AmenityId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_PropertyId",
                table: "Units",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_UnitTypeId",
                table: "Units",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "Unique_Email_Users",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_Phone_Users",
                table: "Users",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Unique_Username_Users",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "LeaseApplications");

            migrationBuilder.DropTable(
                name: "MaintenanceRequests");

            migrationBuilder.DropTable(
                name: "MaintenanceStaffSkills");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PropertyManagers");

            migrationBuilder.DropTable(
                name: "UnitAmenities");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MaintenanceStaff");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "Leases");

            migrationBuilder.DropTable(
                name: "PaymentFrequencies");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.DropTable(
                name: "Durations");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "UnitTypes");

            migrationBuilder.DropTable(
                name: "Governorates");
        }
    }
}
