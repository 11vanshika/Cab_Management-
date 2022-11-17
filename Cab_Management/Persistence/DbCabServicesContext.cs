using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class DbCabServicesContext : DbContext
{
    public DbCabServicesContext()
    {
    }

    public DbCabServicesContext(DbContextOptions<DbCabServicesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    public virtual DbSet<TbUserRole> TbUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = 65.0.181.176;Database=db_CabServices;User Id = admin;Password = Asdf1234*;TrustServerCertificate=True;Connection Timeout=300;command timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tb_Users__1788CC4CA911AC87");

            entity.ToTable("tb_Users");

            entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.UserRole).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__tb_Users__UserRo__29572725");
        });

        modelBuilder.Entity<TbUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__tb_UserR__3D978A35F6D4CA90");

            entity.ToTable("tb_UserRole");

            entity.Property(e => e.UserRoleId).ValueGeneratedNever();
            entity.Property(e => e.UserRoleName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
