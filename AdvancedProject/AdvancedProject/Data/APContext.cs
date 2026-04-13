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

    public virtual DbSet<Amenity> Amenities { get; set; }

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
                        j.HasIndex(new[] { "SkillId" }, "IX_MaintenanceStaffSkills_SkillId");
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
            entity.Property(e => e.Block).HasDefaultValue("");
            entity.Property(e => e.Building).HasDefaultValue("");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Road).HasDefaultValue("");
        });

        modelBuilder.Entity<PropertyManager>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.PropertyManagers).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasOne(d => d.User).WithMany(p => p.Tenants).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Property).WithMany(p => p.Units).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasMany(d => d.Amenities).WithMany(p => p.Units)
                .UsingEntity<Dictionary<string, object>>(
                    "UnitAmenity",
                    r => r.HasOne<Amenity>().WithMany()
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<Unit>().WithMany()
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UnitId", "AmenityId");
                        j.ToTable("UnitAmenities");
                        j.HasIndex(new[] { "AmenityId" }, "IX_UnitAmenities_AmenityId");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<User>().HasData(
      new User { UserId = 1, Username = "manager", Password = "Manager123", FullName = "System Manager", Email = "manager@mail.com", Phone = "33338876", Role = "Manager", IsActive = true, CreatedAt = new DateTime(2026, 1, 1) },

      new User { UserId = 2, Username = "zahraa.hubail", Password = "Zahraa.123", FullName = "Zahraa Hubail", Email = "zahraa.hubail8@gmail.com", Phone = "33735771", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 2, 12, 2, 3, 4) },
      new User { UserId = 3, Username = "raghad.aleskafi", Password = "Raghad.123", FullName = "Raghad Aleskafi", Email = "raghad@gmail.com", Phone = "39004266", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 15, 15, 12, 55) },
      new User { UserId = 4, Username = "fatima.alaiwi", Password = "Fatima.123", FullName = "Fatima Alaiwi", Email = "fatima@gmail.com", Phone = "36635578", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 20, 6, 11, 2) },
      new User { UserId = 5, Username = "norain.hassan", Password = "Norain.123", FullName = "Norain Hassan", Email = "norain@mail.com", Phone = "33744063", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 25, 5, 15, 27) },
      new User { UserId = 6, Username = "ahmed.ali", Password = "Ahmed.999", FullName = "Ahmed Ali", Email = "ahmed.ali@gmail.com", Phone = "33871125", Role = "Tenant", IsActive = true, CreatedAt = new DateTime(2026, 3, 28, 7, 17, 22) },

      new User { UserId = 7, Username = "ali.hassan", Password = "Ali.123", FullName = "Ali Hassan", Email = "alihassan@mail.com", Phone = "39207552", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 10, 9, 16, 34) },
      new User { UserId = 8, Username = "sara.mohamed", Password = "Sara.888", FullName = "Sara Mohamed", Email = "sara.mohamed@gmail.com", Phone = "33699152", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 11, 9, 10, 10) },
      new User { UserId = 9, Username = "abbas.hadi", Password = "Abbas.123", FullName = "Abbas Hadi", Email = "abbas@gmail.com", Phone = "33546672", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 12, 10, 2, 15) },
      new User { UserId = 10, Username = "laila.yaser", Password = "Laila.999", FullName = "Laila Yaser", Email = "laila@gmail.com", Phone = "39126632", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 13, 6, 21, 41) },
      new User { UserId = 11, Username = "mohammed.karim", Password = "mohammed.123", FullName = "Mohammed Karim", Email = "mohammed@gmail.com", Phone = "33921092", Role = "Staff", IsActive = true, CreatedAt = new DateTime(2026, 3, 14, 8, 13, 44) }
      );


        modelBuilder.Entity<PropertyManager>().HasData(
        new PropertyManager { ManagerId = 1, UserId = 1, HireDate = new DateTime(2025, 1, 1) }
        );

        modelBuilder.Entity<Tenant>().HasData(
       new Tenant { TenantId = 1, UserId = 2, Dob = new DateOnly(2004, 10, 18), NationalId = "041081254" },
       new Tenant { TenantId = 2, UserId = 3, Dob = new DateOnly(1995, 3, 26), NationalId = "950306321" },
       new Tenant { TenantId = 3, UserId = 4, Dob = new DateOnly(1977, 9, 9), NationalId = "770907721" },
       new Tenant { TenantId = 4, UserId = 5, Dob = new DateOnly(1989, 11, 25), NationalId = "891106213" },
       new Tenant { TenantId = 5, UserId = 6, Dob = new DateOnly(1982, 7, 18), NationalId = "820752231" }
   );

        modelBuilder.Entity<MaintenanceStaff>().HasData(
           new MaintenanceStaff { StaffId = 1, UserId = 7, AvailabilityStatus = "Available" },
           new MaintenanceStaff { StaffId = 2, UserId = 8, AvailabilityStatus = "Busy" },
           new MaintenanceStaff { StaffId = 3, UserId = 9, AvailabilityStatus = "Available" },
           new MaintenanceStaff { StaffId = 4, UserId = 10, AvailabilityStatus = "Available" },
           new MaintenanceStaff { StaffId = 5, UserId = 11, AvailabilityStatus = "Busy" }
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
         new Property { PropertyId = 1, Name = "Abraj Al Lulu", Building = "611", Road = "271", Block = "220", City = "Manama", Description = "A modern residential complex offering comfort and essential amenities.", CreatedAt = new DateTime(2026, 1, 1, 12, 55, 21) },
         new Property { PropertyId = 2, Name = "Almoayyed Tower", Building = "246", Road = "811", Block = "708", City = "Muharraq", Description = "A contemporary tower with modern facilities in a prime location.", CreatedAt = new DateTime(2026, 1, 5, 15, 22, 29) },
         new Property { PropertyId = 3, Name = "United Tower", Building = "922", Road = "3062", Block = "461", City = "Riffa", Description = "A residential property with spacious apartments for families.", CreatedAt = new DateTime(2026, 1, 10, 3, 31, 43) }
     );


        modelBuilder.Entity<Unit>().HasData(
       new Unit { UnitId = 1, PropertyId = 1, UnitNumber = "A1", Type = "Apartment", SizeSqFt = 100, RentAmount = 300, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 1, 4, 12, 55) },
       new Unit { UnitId = 2, PropertyId = 1, UnitNumber = "A2", Type = "Apartment", SizeSqFt = 120, RentAmount = 350, AvailabilityStatus = "Occupied", CreatedAt = new DateTime(2026, 1, 2, 23, 16, 33) },
       new Unit { UnitId = 3, PropertyId = 2, UnitNumber = "B1", Type = "Office", SizeSqFt = 200, RentAmount = 500, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 3, 9, 11, 7) },
       new Unit { UnitId = 4, PropertyId = 2, UnitNumber = "B2", Type = "Office", SizeSqFt = 250, RentAmount = 550, AvailabilityStatus = "Occupied", CreatedAt = new DateTime(2026, 1, 4, 7, 16, 22) },
       new Unit { UnitId = 5, PropertyId = 1, UnitNumber = "A3", Type = "Studio", SizeSqFt = 80, RentAmount = 250, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 5, 10, 10, 12) },
       new Unit { UnitId = 6, PropertyId = 3, UnitNumber = "C1", Type = "Apartment", SizeSqFt = 110, RentAmount = 320, AvailabilityStatus = "Available", CreatedAt = new DateTime(2026, 1, 6, 4, 15, 45) }
   );


        modelBuilder.Entity<MaintenanceRequest>().HasData(
       new MaintenanceRequest { RequestId = 1, UnitId = 2, TenantId = 1, SkillId = 1, Priority = "High", Status = "Submitted", AssignedStaffId = 1, Notes = "Water leaking from bathroom pipe", RequestDate = new DateTime(2026, 3, 1, 13, 12, 3) },
       new MaintenanceRequest { RequestId = 2, UnitId = 3, TenantId = 2, SkillId = 2, Priority = "Medium", Status = "In Progress", AssignedStaffId = 2, Notes = "Living room light not working", RequestDate = new DateTime(2026, 3, 2, 23, 12, 42) },
       new MaintenanceRequest { RequestId = 3, UnitId = 1, TenantId = 3, SkillId = 3, Priority = "Low", Status = "Resolved", AssignedStaffId = 3, Notes = "AC cooling is weak", RequestDate = new DateTime(2026, 3, 3, 20, 20, 4) },
       new MaintenanceRequest { RequestId = 4, UnitId = 4, TenantId = 4, SkillId = 4, Priority = "High", Status = "Submitted", AssignedStaffId = 4, Notes = "Front door lock is broken", RequestDate = new DateTime(2026, 3, 4, 2, 44, 11) },
       new MaintenanceRequest { RequestId = 5, UnitId = 5, TenantId = 5, SkillId = 5, Priority = "Low", Status = "Closed", AssignedStaffId = 5, Notes = "Wall paint is fading and peeling", RequestDate = new DateTime(2026, 3, 5, 11, 32, 0) }
   );

        modelBuilder.Entity<LeaseApplication>().HasData(
      new LeaseApplication { ApplicationId = 1, TenantId = 1, UnitId = 1, ApplicationDate = new DateTime(2026, 2, 1, 20, 30, 33), Status = "Approved" , StartDate = new DateTime(2026, 6, 1, 8, 0, 0), Duration = 6, ApproveTime = new DateTime(2026, 2, 2, 13, 22, 17) },
      new LeaseApplication { ApplicationId = 2, TenantId = 2, UnitId = 2, ApplicationDate = new DateTime(2026, 2, 3, 3, 7, 12), Status = "Pending", StartDate = new DateTime(2026, 5, 5, 18, 9, 24), Duration = 24 },
      new LeaseApplication { ApplicationId = 3, TenantId = 3, UnitId = 3, ApplicationDate = new DateTime(2026, 2, 5, 5, 24, 13), Status = "Rejected", StartDate = new DateTime(2027, 1, 1, 9, 10, 22), Duration = 6, RejectTime = new DateTime(2026, 2, 22, 3, 17, 29) },
      new LeaseApplication { ApplicationId = 4, TenantId = 4, UnitId = 4, ApplicationDate = new DateTime(2026, 2, 2, 22, 55, 2), Status = "Approved", StartDate = new DateTime(2026, 3, 15, 8, 0, 0), Duration = 12, ApproveTime = new DateTime(2026, 2, 5, 14, 30, 0) },
      new LeaseApplication { ApplicationId = 5, TenantId = 5, UnitId = 5, ApplicationDate = new DateTime(2026, 2, 9, 9, 33, 11), Status = "Pending", StartDate = new DateTime(2026, 4, 20, 14, 22, 5), Duration = 12 },
      new LeaseApplication { ApplicationId = 6, TenantId = 2, UnitId = 3, ApplicationDate = new DateTime(2026, 3, 3, 10, 21, 10), Status = "Approved", StartDate = new DateTime(2026, 7, 1, 12, 6, 12), Duration = 6, ApproveTime = new DateTime(2026, 3, 5, 6, 44, 3) },
      new LeaseApplication { ApplicationId = 7, TenantId = 3, UnitId = 1, ApplicationDate = new DateTime(2026, 1, 12, 6, 10, 9), Status = "Approved", StartDate = new DateTime(2026, 3, 10, 11, 1, 22), Duration = 24, ApproveTime = new DateTime(2026, 3, 11, 7, 52, 33) },
      new LeaseApplication { ApplicationId = 8, TenantId = 5, UnitId = 5, ApplicationDate = new DateTime(2026, 1, 12, 14, 32, 17), Status = "Approved", StartDate = new DateTime(2026, 2, 1, 8, 0, 0), Duration = 24, ApproveTime = new DateTime(2026, 1, 15, 4, 32, 29) }



  );

        modelBuilder.Entity<Lease>().HasData(
      new Lease { LeaseId = 1, TenantId = 1, UnitId = 1, StartDate = new DateTime(2026, 6, 1, 8, 0, 0), EndDate = new DateTime(2026, 12, 1, 8, 0, 0), MonthlyRent = 300, Status = "Active", CreatedAt = new DateTime(2026, 2, 2, 13, 22, 17), Duration = 6 },
      new Lease { LeaseId = 2, TenantId = 2, UnitId = 3, StartDate = new DateTime(2026, 7, 1, 12, 6, 12), EndDate = new DateTime(2026, 6, 30, 8, 0 ,0), MonthlyRent = 500, Status = "Active", CreatedAt = new DateTime(2026, 3, 5, 6, 44, 3) , Duration = 6}, 
      new Lease { LeaseId = 3, TenantId = 3, UnitId = 1, StartDate = new DateTime(2026, 3, 10, 11, 1, 22), EndDate = new DateTime(2027, 3, 10, 8, 0, 0), MonthlyRent = 350, Status = "Terminated", CreatedAt = new DateTime(2026, 3, 10, 7, 52, 33) , Duration = 24}, 
      new Lease { LeaseId = 4, TenantId = 4, UnitId = 4, StartDate = new DateTime(2026, 3, 15, 8, 0, 0), EndDate = new DateTime(2027, 3, 14, 8, 0, 0), MonthlyRent = 550, Status = "Active", CreatedAt = new DateTime(2026, 2, 5, 14, 30, 0), Duration = 12 },
      new Lease { LeaseId = 5, TenantId = 5, UnitId = 5, StartDate = new DateTime(2026, 2, 1, 8, 0, 0), EndDate = new DateTime(2028, 1, 31, 8, 0, 0), MonthlyRent = 250, Status = "Active", CreatedAt = new DateTime(2026, 1, 15, 4, 32, 29) , Duration = 24} 
  );

        modelBuilder.Entity<Payment>().HasData(
       new Payment { PaymentId = 1, LeaseId = 1, Amount = 300, PaymentDate = new DateTime(2026, 3, 1, 12, 11, 5), Status = "Paid" },
       new Payment { PaymentId = 2, LeaseId = 2, Amount = 500, PaymentDate = new DateTime(2026, 3, 5, 9, 33, 12), Status = "Paid" },
       new Payment { PaymentId = 3, LeaseId = 3, Amount = 350, PaymentDate = new DateTime(2026, 3, 10, 22, 11, 9), Status = "Late" },
       new Payment { PaymentId = 4, LeaseId = 4, Amount = 550, PaymentDate = new DateTime(2026, 3, 12, 7, 15, 22), Status = "Paid" },
       new Payment { PaymentId = 5, LeaseId = 5, Amount = 250, PaymentDate = new DateTime(2026, 3, 15, 8, 19, 27), Status = "Pending" }
   );

        modelBuilder.Entity<Notification>().HasData(
       new Notification { NotificationId = 1, UserId = 1, Message = "New lease application received", CreatedAt = new DateTime(2026, 2, 1, 10, 33, 21), Type = "LeaseApplication" },
       new Notification { NotificationId = 2, UserId = 2, Message = "Your application has been approved", CreatedAt = new DateTime(2026, 2, 3, 9, 18, 55), Type = "LeaseApplication" },
       new Notification { NotificationId = 3, UserId = 3, Message = "Maintenance request updated", CreatedAt = new DateTime(2026, 3, 2, 14, 25, 14), Type = "MaintenanceRequest" },
       new Notification { NotificationId = 4, UserId = 4, Message = "Payment received successfully", CreatedAt = new DateTime(2026, 3, 12, 13, 44, 21), Type = "Payment" },
       new Notification { NotificationId = 5, UserId = 5, Message = "Lease activated for your unit", CreatedAt = new DateTime(2026, 3, 15, 21, 4, 17), Type = "Lease" },
       new Notification { NotificationId = 6, UserId = 7, Message = "New maintenance request assigned", CreatedAt = new DateTime(2026, 3, 1, 12, 6, 33), Type = "MaintenanceRequest" },
       new Notification { NotificationId = 7, UserId = 8, Message = "Electrical repair marked as in progress", CreatedAt = new DateTime(2026, 3, 2, 8, 15, 32), Type = "MaintenanceRequest" },
       new Notification { NotificationId = 8, UserId = 9, Message = "HVAC issue reported in Unit A2", CreatedAt = new DateTime(2026, 3, 3, 21, 17, 28), Type = "MaintenanceRequest" },
       new Notification { NotificationId = 9, UserId = 10, Message = "Carpentry issue reported in Unit B2", CreatedAt = new DateTime(2026, 3, 4, 17, 5, 43), Type = "MaintenanceRequest" },
       new Notification { NotificationId = 10, UserId = 11, Message = "Painting issue reported in Unit A3", CreatedAt = new DateTime(2026, 3, 5, 23, 14, 10), Type = "MaintenanceRequest" }
    );

        modelBuilder.Entity<Amenity>().HasData(
        new Amenity { AmenityId = 1, Name = "Parking" },
        new Amenity { AmenityId = 2, Name = "Swimming Pool" },
        new Amenity { AmenityId = 3, Name = "Gym" },
        new Amenity { AmenityId = 4, Name = "Security" },
        new Amenity { AmenityId = 5, Name = "Elevator" },
        new Amenity { AmenityId = 6, Name = "Central AC" },
        new Amenity { AmenityId = 7, Name = "WiFi" }
    );

        modelBuilder.Entity<Unit>()
         .HasMany(u => u.Amenities)
         .WithMany(a => a.Units)
         .UsingEntity(j => j.HasData(
             new { UnitId = 1, AmenityId = 1 }, // Parking
             new { UnitId = 1, AmenityId = 2 }, // Swimming Pool
             new { UnitId = 1, AmenityId = 4 }, // Security

             new { UnitId = 2, AmenityId = 1 }, // Parking
             new { UnitId = 2, AmenityId = 5 }, // Elevator

             new { UnitId = 3, AmenityId = 3 }, // Gym
             new { UnitId = 3, AmenityId = 4 }, // Security
             new { UnitId = 3, AmenityId = 6 }, // Central AC

             new { UnitId = 4, AmenityId = 4 }, // Security
             new { UnitId = 4, AmenityId = 5 }, // Elevator

             new { UnitId = 5, AmenityId = 1 }, // Parking
             new { UnitId = 5, AmenityId = 7 }, // WiFi

             new { UnitId = 6, AmenityId = 2 }, // Swimming Pool
             new { UnitId = 6, AmenityId = 3 }, // Gym
             new { UnitId = 6, AmenityId = 4 }  // Security
         ));

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
