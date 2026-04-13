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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
