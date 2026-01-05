using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace auditdemonet.Models;

public partial class AuditDemoDbContext : DbContext
{
    public AuditDemoDbContext()
    {
    }

    public AuditDemoDbContext(DbContextOptions<AuditDemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditEvent> AuditEvents { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeTransfer> EmployeeTransfers { get; set; }

 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditEvent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__AuditEve__7944C81097BF8049");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC072690E4AE");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07BE718984");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Departments");
        });

        modelBuilder.Entity<EmployeeTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC079C10D623");

            entity.Property(e => e.TransferDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeTransfers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_Employee");

            entity.HasOne(d => d.FromDepartment).WithMany(p => p.EmployeeTransferFromDepartments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_FromDept");

            entity.HasOne(d => d.ToDepartment).WithMany(p => p.EmployeeTransferToDepartments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transfer_ToDept");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
