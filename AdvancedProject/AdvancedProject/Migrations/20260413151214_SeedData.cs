using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AdvancedProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Properties",
                columns: new[] { "PropertyId", "Block", "Building", "City", "CreatedAt", "Description", "Name", "Road" },
                values: new object[,]
                {
                    { 1, "220", "611", "Manama", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A modern residential complex offering comfort and essential amenities.", "Abraj Al Lulu", "271" },
                    { 2, "708", "246", "Muharraq", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "A contemporary tower with modern facilities in a prime location.", "Almoayyed Tower", "811" },
                    { 3, "461", "922", "Riffa", new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "A residential property with spacious apartments for families.", "United Tower", "3062" }
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
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "FullName", "IsActive", "Password", "Phone", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@mail.com", "System Admin", true, "Admin123", "33338876", "Manager", "admin" },
                    { 2, new DateTime(2026, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "zahraa.hubail8@gmail.com", "Zahraa Hubail", true, "Zahraa.123", "33735771", "Tenant", "zahraa.hubail" },
                    { 3, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "raghad@gmail.com", "Raghad Aleskafi", true, "Raghad.123", "39004266", "Tenant", "raghad.aleskafi" },
                    { 4, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "fatima@gmail.com", "Fatima Alaiwi", true, "Fatima.123", "36635578", "Tenant", "fatima.alaiwi" },
                    { 5, new DateTime(2026, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "norain@mail.com", "Norain Hassan", true, "Norain.123", "33744063", "Tenant", "norain.hassan" },
                    { 6, new DateTime(2026, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "ahmed.ali@gmail.com", "Ahmed Ali", true, "Ahmed.999", "33871125", "Tenant", "ahmed.ali" },
                    { 7, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "alihassan@mail.com", "Ali Hassan", true, "Ali.123", "39207552", "Staff", "ali.hassan" },
                    { 8, new DateTime(2026, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "sara.mohamed@gmail.com", "Sara Mohamed", true, "Sara.888", "33699152", "Staff", "sara.mohamed" },
                    { 9, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "abbas@gmail.com", "Abbas Hadi", true, "Abbas.123", "33546672", "Staff", "abbas.hadi" },
                    { 10, new DateTime(2026, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "laila@gmail.com", "Laila Yaser", true, "Laila.999", "39126632", "Staff", "laila.yaser" },
                    { 11, new DateTime(2026, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "mohammed@gmail.com", "Mohammed Karim", true, "mohammed.123", "33921092", "Staff", "mohammed.karim" }
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
                columns: new[] { "NotificationId", "CreatedAt", "Message", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New lease application received", "LeaseApplication", 1 },
                    { 2, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Your application has been approved", "LeaseApplication", 2 },
                    { 3, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Maintenance request updated", "MaintenanceRequest", 3 },
                    { 4, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Payment received successfully", "Payment", 4 },
                    { 5, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lease activated for your unit", "Lease", 5 },
                    { 6, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New maintenance request assigned", "MaintenanceRequest", 7 },
                    { 7, new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electrical repair marked as in progress", "MaintenanceRequest", 8 },
                    { 8, new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "HVAC issue reported in Unit A2", "MaintenanceRequest", 9 },
                    { 9, new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carpentry issue reported in Unit B2", "MaintenanceRequest", 10 },
                    { 10, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Painting issue reported in Unit A3", "MaintenanceRequest", 11 }
                });

            migrationBuilder.InsertData(
                table: "PropertyManagers",
                columns: new[] { "ManagerId", "HireDate", "UserId" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 });

            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "TenantId", "DOB", "NationalId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2004, 10, 18), "041081254", 2 },
                    { 2, new DateOnly(1995, 3, 26), "950306321", 3 },
                    { 3, new DateOnly(1977, 9, 9), "770907721", 4 },
                    { 4, new DateOnly(1989, 11, 25), "891106213", 5 },
                    { 5, new DateOnly(1982, 7, 18), "820752231", 6 }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "UnitId", "AvailabilityStatus", "CreatedAt", "PropertyId", "RentAmount", "SizeSqFt", "Type", "UnitNumber" },
                values: new object[,]
                {
                    { 1, "Available", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 300m, 100m, "Apartment", "A1" },
                    { 2, "Occupied", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 350m, 120m, "Apartment", "A2" },
                    { 3, "Available", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 500m, 200m, "Office", "B1" },
                    { 4, "Occupied", new DateTime(2026, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 550m, 250m, "Office", "B2" },
                    { 5, "Available", new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 250m, 80m, "Studio", "A3" },
                    { 6, "Available", new DateTime(2026, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 320m, 110m, "Apartment", "C1" }
                });

            migrationBuilder.InsertData(
                table: "LeaseApplications",
                columns: new[] { "ApplicationId", "ApplicationDate", "ApproveTime", "RejectTime", "Status", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Approved", 1, 1 },
                    { 2, new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pending", 2, 2 },
                    { 3, new DateTime(2026, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Rejected", 3, 3 },
                    { 4, new DateTime(2026, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Approved", 4, 4 },
                    { 5, new DateTime(2026, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Pending", 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Leases",
                columns: new[] { "LeaseId", "CreatedAt", "EndDate", "MonthlyRent", "StartDate", "Status", "TenantId", "TerminationDate", "UnitId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 300m, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 1, null, 2 },
                    { 2, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 500m, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 2, null, 3 },
                    { 3, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 350m, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Terminated", 3, null, 1 },
                    { 4, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 550m, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 4, null, 4 },
                    { 5, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 250m, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Active", 5, null, 5 }
                });

            migrationBuilder.InsertData(
                table: "MaintenanceRequests",
                columns: new[] { "RequestId", "AssignedStaffId", "AssignedTime", "ClosedTime", "CompletedDate", "InProgressTime", "Notes", "Priority", "RequestDate", "ResolvedTime", "SkillId", "Status", "TenantId", "UnitId" },
                values: new object[,]
                {
                    { 1, 1, null, null, null, null, "Water leaking from bathroom pipe", "High", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, "Submitted", 1, 2 },
                    { 2, 2, null, null, null, null, "Living room light not working", "Medium", new DateTime(2026, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, "In Progress", 2, 3 },
                    { 3, 3, null, null, null, null, "AC cooling is weak", "Low", new DateTime(2026, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, "Resolved", 3, 1 },
                    { 4, 4, null, null, null, null, "Front door lock is broken", "High", new DateTime(2026, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, "Submitted", 4, 4 },
                    { 5, 5, null, null, null, null, "Wall paint is fading and peeling", "Low", new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 5, "Closed", 5, 5 }
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
                columns: new[] { "PaymentId", "Amount", "LeaseId", "PaymentDate", "Status" },
                values: new object[,]
                {
                    { 1, 300m, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid" },
                    { 2, 500m, 2, new DateTime(2026, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid" },
                    { 3, 350m, 3, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Late" },
                    { 4, 550m, 4, new DateTime(2026, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Paid" },
                    { 5, 250m, 5, new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaseApplications",
                keyColumn: "ApplicationId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaintenanceRequests",
                keyColumn: "RequestId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "MaintenanceStaffSkills",
                keyColumns: new[] { "SkillId", "StaffId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Notifications",
                keyColumn: "NotificationId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Payments",
                keyColumn: "PaymentId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PropertyManagers",
                keyColumn: "ManagerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "UnitAmenities",
                keyColumns: new[] { "AmenityId", "UnitId" },
                keyValues: new object[] { 4, 6 });

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Amenities",
                keyColumn: "AmenityId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Leases",
                keyColumn: "LeaseId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaff",
                keyColumn: "StaffId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaff",
                keyColumn: "StaffId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaff",
                keyColumn: "StaffId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaff",
                keyColumn: "StaffId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MaintenanceStaff",
                keyColumn: "StaffId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "SkillId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "TenantId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Units",
                keyColumn: "UnitId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "PropertyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 6);
        }
    }
}
