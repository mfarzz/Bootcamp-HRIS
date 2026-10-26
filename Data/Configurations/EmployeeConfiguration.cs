using HRIS.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HRIS.API.Data.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.EmployeeNumber)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(e => e.FullName)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e => e.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(e=> e.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(EmployeeStatus.Active)
            .IsRequired();
        
        builder.HasIndex(e=>e.EmployeeNumber)
            .IsUnique();
        builder.HasIndex(e=>e.Email)
            .IsUnique();
        
        builder.HasOne(e=> e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e=> e.Position)
            .WithMany(p => p.Employees)
            .HasForeignKey(d => d.PositionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e=> e.Manager)
            .WithMany(m=> m.DirectReports)
            .HasForeignKey(d => d.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}