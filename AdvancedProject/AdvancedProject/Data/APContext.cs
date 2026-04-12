using System;
using System.Collections.Generic;
using AdvancedProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvancedProject.Data;

public partial class APContext : DbContext
{
    public APContext()
    {
    }

    public APContext(DbContextOptions<APContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lease> Leases { get; set; }

    public virtual DbSet<LeaseApplication> LeaseApplications { get; set; }

    public virtual DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }

    public virtual DbSet<MaintenanceStaff> MaintenanceStaffs { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Property> Properties { get; set; }

    public virtual DbSet<PropertyManager> PropertyManagers { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AdvancedDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lease>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Leases).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Unit).WithMany(p => p.Leases).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LeaseApplication>(entity =>
        {
            entity.Property(e => e.ApplicationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Tenant).WithMany(p => p.LeaseApplications).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Unit).WithMany(p => p.LeaseApplications).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Skill).WithMany(p => p.MaintenanceRequests).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Tenant).WithMany(p => p.MaintenanceRequests).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Unit).WithMany(p => p.MaintenanceRequests).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MaintenanceStaff>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.MaintenanceStaffs).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Skills).WithMany(p => p.Staff)
                .UsingEntity<Dictionary<string, object>>(
                    "MaintenanceStaffSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<MaintenanceStaff>().WithMany()
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("StaffId", "SkillId");
                        j.ToTable("MaintenanceStaffSkills");
                        j.IndexerProperty<int>("StaffId").ValueGeneratedOnAdd();
                    });
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Lease).WithMany(p => p.Payments).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PropertyManager>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.PropertyManagers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.Tenants).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Property).WithMany(p => p.Units).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<User>().HasData(
new User { UserId = 1, Username = "admin", Password = "Admin123", FullName = "System Admin", Email = "admin@mail.com", Phone = "33338876", Role = "Manager", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) },

new User { UserId = 2, Username = "zahraa.hubail", Password = "Zahraa.123", FullName = "Zahraa Hubail", Email = "zahraa.hubail8@gmail.com", Phone = "33735771", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 2, 12) },
new User { UserId = 3, Username = "raghad.aleskafi", Password = "Raghad.123", FullName = "Raghad Aleskafi", Email = "raghad@gmail.com", Phone = "39004266", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 15) },
new User { UserId = 4, Username = "fatima.alaiwi", Password = "Fatima.123", FullName = "Fatima Alaiwi", Email = "fatima@gmail.com", Phone = "36635578", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 20) },
new User { UserId = 5, Username = "norain.hassan", Password = "Norain.123", FullName = "Norain Hassan", Email = "norain@mail.com", Phone = "33744063", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 25) },
new User { UserId = 6, Username = "ahmed.ali", Password = "Ahmed.999", FullName = "Ahmed Ali", Email = "ahmed.ali@gmail.com", Phone = "33871125", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 28) },

new User { UserId = 7, Username = "ali.hassan", Password = "Ali.123", FullName = "Ali Hassan", Email = "alihassan@mail.com", Phone = "39207552", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 10) },
new User { UserId = 8, Username = "sara.mohamed", Password = "Sara.888", FullName = "Sara Mohamed", Email = "sara.mohamed@gmail.com", Phone = "33699152", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 11) },
new User { UserId = 9, Username = "abbas.hadi", Password = "Abbas.123", FullName = "Abbas Hadi", Email = "abbas@gmail.com", Phone = "33546672", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 12) },
new User { UserId = 10, Username = "laila.yaser", Password = "Laila.999", FullName = "Laila Yaser", Email = "laila@gmail.com", Phone = "39126632", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 13) },
new User { UserId = 11, Username = "mohammed.karim", Password = "mohammed.123", FullName = "Mohammed Karim", Email = "mohammed@gmail.com", Phone = "33921092", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 14) }
);


        modelBuilder.Entity<PropertyManager>().HasData(
        new PropertyManager { ManagerId = 1, UserId = 1, HireDate = new DateOnly(2025, 1, 1) }
    );


        modelBuilder.Entity<Tenant>().HasData(
        new Tenant { TenantId = 1, UserId = 2, Dob = new DateOnly(2004, 10, 18), NationalId = "041081254", EmergencyContact = "33111111", CreatedAt = new DateTime(2026, 2, 12) },
        new Tenant { TenantId = 2, UserId = 3, Dob = new DateOnly(1995, 3, 26), NationalId = "950306321", EmergencyContact = "33222222", CreatedAt = new DateTime(2026, 3, 15) },
        new Tenant { TenantId = 3, UserId = 4, Dob = new DateOnly(1977, 9, 9), NationalId = "770907721", EmergencyContact = "33333333", CreatedAt = new DateTime(2026, 3, 20) },
        new Tenant { TenantId = 4, UserId = 5, Dob = new DateOnly(1989, 11, 25), NationalId = "891106213", EmergencyContact = "33444444", CreatedAt = new DateTime(2026, 3, 25) },
        new Tenant { TenantId = 5, UserId = 6, Dob = new DateOnly(1982, 7, 18), NationalId = "820752231", EmergencyContact = "33555555", CreatedAt = new DateTime(2026, 3, 28) }
    );

        modelBuilder.Entity<MaintenanceStaff>().HasData(
        new MaintenanceStaff { StaffId = 1, UserId = 7, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 3, 10) },
        new MaintenanceStaff { StaffId = 2, UserId = 8, AvailabilityStatus = "Busy", CreatedAt = new DateTime(2026, 3, 11) },
        new MaintenanceStaff { StaffId = 3, UserId = 9, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 3, 12) },
        new MaintenanceStaff { StaffId = 4, UserId = 10, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 3, 13) },
        new MaintenanceStaff { StaffId = 5, UserId = 11, AvailabilityStatus = "Busy", CreatedAt = new DateTime(2026, 3, 14) }
    );

        modelBuilder.Entity<Skill>().HasData(
        new Skill { SkillId = 1, Name = "Plumbing" },
        new Skill { SkillId = 2, Name = "Electrical" },
        new Skill { SkillId = 3, Name = "HVAC" },
        new Skill { SkillId = 4, Name = "Carpentry" },
        new Skill { SkillId = 5, Name = "Painting" }
    );

        modelBuilder.Entity<MaintenanceStaff>()
    .HasMany(s => s.Skills)
    .WithMany(s => s.Staff)
    .UsingEntity(j => j.HasData(
        new { StaffId = 1, SkillId = 1 },
        new { StaffId = 1, SkillId = 2 },

        new { StaffId = 2, SkillId = 2 },
        new { StaffId = 2, SkillId = 3 },

        new { StaffId = 3, SkillId = 1 },
        new { StaffId = 3, SkillId = 4 },

        new { StaffId = 4, SkillId = 5 },

        new { StaffId = 5, SkillId = 3 }
    ));

        modelBuilder.Entity<Property>().HasData(
        new Property { PropertyId = 1, Name = "Abraj Al Lulu", Address = "Manama", City = "Manama", Description = "Nice", CreatedAt = new DateTime(2026, 1, 1) },
        new Property { PropertyId = 2, Name = "Almoayyed Tower", Address = "Muharraq", City = "Muharraq", Description = "Modern building", CreatedAt = new DateTime(2026, 1, 5) },
        new Property { PropertyId = 3, Name = "United Tower", Address = "Riffa", City = "Riffa", Description = "Family apartments", CreatedAt = new DateTime(2026, 1, 10) }
    );

        modelBuilder.Entity<Unit>().HasData(
        new Unit { UnitId = 1, PropertyId = 1, UnitNumber = "A1", Type = "Apartment", SizeSqFt = 100, Amenities = "AC", RentAmount = 300, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 1) },
        new Unit { UnitId = 2, PropertyId = 1, UnitNumber = "A2", Type = "Apartment", SizeSqFt = 120, Amenities = "AC", RentAmount = 350, AvailabilityStatus = "Occupied", CreatedAt = new DateTime(2026, 1, 2) },
        new Unit { UnitId = 3, PropertyId = 2, UnitNumber = "B1", Type = "Office", SizeSqFt = 200, Amenities = "Parking", RentAmount = 500, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 3) },
        new Unit { UnitId = 4, PropertyId = 2, UnitNumber = "B2", Type = "Office", SizeSqFt = 250, Amenities = "Parking", RentAmount = 550, AvailabilityStatus = "Occupied", CreatedAt = new DateTime(2026, 1, 4) },
        new Unit { UnitId = 5, PropertyId = 1, UnitNumber = "A3", Type = "Studio", SizeSqFt = 80, Amenities = "WiFi", RentAmount = 250, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 5) },
        new Unit { UnitId = 6, PropertyId = 3, UnitNumber = "C1", Type = "Apartment", SizeSqFt = 110, Amenities = "AC, Parking", RentAmount = 320, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 6) }
    );

        modelBuilder.Entity<MaintenanceRequest>().HasData(
        new MaintenanceRequest { RequestId = 1, UnitId = 2, TenantId = 1, SkillId = 1, Priority = "High", Status = "Submitted", AssignedStaffId = 1, Notes = "Water leaking from bathroom pipe", RequestDate = new DateTime(2026, 3, 1) },
        new MaintenanceRequest { RequestId = 2, UnitId = 3, TenantId = 2, SkillId = 2, Priority = "Medium", Status = "In Progress", AssignedStaffId = 2, Notes = "Living room light not working", RequestDate = new DateTime(2026, 3, 2) },
        new MaintenanceRequest { RequestId = 3, UnitId = 1, TenantId = 3, SkillId = 3, Priority = "Low", Status = "Resolved", AssignedStaffId = 3, Notes = "AC cooling is weak", RequestDate = new DateTime(2026, 3, 3) },
        new MaintenanceRequest { RequestId = 4, UnitId = 4, TenantId = 4, SkillId = 4, Priority = "High", Status = "Submitted", AssignedStaffId = 4, Notes = "Front door lock is broken", RequestDate = new DateTime(2026, 3, 4) },
        new MaintenanceRequest { RequestId = 5, UnitId = 5, TenantId = 5, SkillId = 5, Priority = "Low", Status = "Closed", AssignedStaffId = 5, Notes = "Wall paint is fading and peeling", RequestDate = new DateTime(2026, 3, 5) }
    );

        modelBuilder.Entity<LeaseApplication>().HasData(
        new LeaseApplication { ApplicationId = 1, TenantId = 1, UnitId = 1, ApplicationDate = new DateTime(2026, 2, 1), Status = "Approved", Notes = "Interested in unit A1" },
        new LeaseApplication { ApplicationId = 2, TenantId = 2, UnitId = 2, ApplicationDate = new DateTime(2026, 2, 3), Status = "Pending" },
        new LeaseApplication { ApplicationId = 3, TenantId = 3, UnitId = 3, ApplicationDate = new DateTime(2026, 2, 5), Status = "Rejected" },
        new LeaseApplication { ApplicationId = 4, TenantId = 4, UnitId = 4, ApplicationDate = new DateTime(2026, 2, 7), Status = "Approved", Notes = "Ready to move in soon" },
        new LeaseApplication { ApplicationId = 5, TenantId = 5, UnitId = 5, ApplicationDate = new DateTime(2026, 2, 9), Status = "Pending" }
    );

        modelBuilder.Entity<Lease>().HasData(
        new Lease { LeaseId = 1, TenantId = 1, UnitId = 2, StartDate = new DateOnly(2026, 3, 1), EndDate = new DateOnly(2027, 3, 1), MonthlyRent = 300, Status = "Active", CreatedAt = new DateTime(2026, 3, 1) },
        new Lease { LeaseId = 2, TenantId = 2, UnitId = 3, StartDate = new DateOnly(2026, 3, 5), EndDate = new DateOnly(2027, 3, 5), MonthlyRent = 500, Status = "Active", CreatedAt = new DateTime(2026, 3, 5) },
        new Lease { LeaseId = 3, TenantId = 3, UnitId = 1, StartDate = new DateOnly(2026, 3, 10), EndDate = new DateOnly(2027, 3, 10), MonthlyRent = 350, Status = "Terminated", CreatedAt = new DateTime(2026, 3, 10) },
        new Lease { LeaseId = 4, TenantId = 4, UnitId = 4, StartDate = new DateOnly(2026, 3, 12), EndDate = new DateOnly(2027, 3, 12), MonthlyRent = 550, Status = "Active", CreatedAt = new DateTime(2026, 3, 12) },
        new Lease { LeaseId = 5, TenantId = 5, UnitId = 5, StartDate = new DateOnly(2026, 3, 15), EndDate = new DateOnly(2027, 3, 15), MonthlyRent = 250, Status = "Active", CreatedAt = new DateTime(2026, 3, 15) }
    );

        modelBuilder.Entity<Payment>().HasData(
        new Payment { PaymentId = 1, LeaseId = 1, Amount = 300, PaymentDate = new DateTime(2026, 3, 1), Status = "Paid" },
        new Payment { PaymentId = 2, LeaseId = 2, Amount = 500, PaymentDate = new DateTime(2026, 3, 5), Status = "Paid", Notes = "On time payment" },
        new Payment { PaymentId = 3, LeaseId = 3, Amount = 350, PaymentDate = new DateTime(2026, 3, 10), Status = "Late" },
        new Payment { PaymentId = 4, LeaseId = 4, Amount = 550, PaymentDate = new DateTime(2026, 3, 12), Status = "Paid", Notes = "Full payment received" },
        new Payment { PaymentId = 5, LeaseId = 5, Amount = 250, PaymentDate = new DateTime(2026, 3, 15), Status = "Pending" }
    );

        modelBuilder.Entity<Notification>().HasData(
         new Notification { NotificationId = 1, UserId = 1, Message = "New lease application received", CreatedAt = new DateTime(2026, 2, 1), IsRead = false, RelatedEntityType = "LeaseApplication", RelatedEntityId = 1 },
         new Notification { NotificationId = 2, UserId = 2, Message = "Your application has been approved", CreatedAt = new DateTime(2026, 2, 3), IsRead = true, RelatedEntityType = "LeaseApplication", RelatedEntityId = 2 },
         new Notification { NotificationId = 3, UserId = 3, Message = "Maintenance request updated", CreatedAt = new DateTime(2026, 3, 2), IsRead = false, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 2 },
         new Notification { NotificationId = 4, UserId = 4, Message = "Payment received successfully", CreatedAt = new DateTime(2026, 3, 12), IsRead = true, RelatedEntityType = "Payment", RelatedEntityId = 4 },
         new Notification { NotificationId = 5, UserId = 5, Message = "Lease activated for your unit", CreatedAt = new DateTime(2026, 3, 15), IsRead = false, RelatedEntityType = "Lease", RelatedEntityId = 5 },
         new Notification { NotificationId = 6, UserId = 7, Message = "New maintenance request assigned", CreatedAt = new DateTime(2026, 3, 1), IsRead = false, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 1 },
         new Notification { NotificationId = 7, UserId = 8, Message = "Electrical repair marked as in progress", CreatedAt = new DateTime(2026, 3, 2), IsRead = true, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 2 },
         new Notification { NotificationId = 8, UserId = 9, Message = "HVAC issue reported in Unit A2", CreatedAt = new DateTime(2026, 3, 3), IsRead = false, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 3 },
         new Notification { NotificationId = 9, UserId = 10, Message = "Carpentry issue reported in Unit B2", CreatedAt = new DateTime(2026, 3, 4), IsRead = false, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 4 },
         new Notification { NotificationId = 10, UserId = 11, Message = "Painting issue reported in Unit A3", CreatedAt = new DateTime(2026, 3, 5), IsRead = true, RelatedEntityType = "MaintenanceRequest", RelatedEntityId = 5 }
     );


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
