using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class DbCabServicesContext : DbContext
{
    public DbCabServicesContext(DbContextOptions<DbCabServicesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbBooking> TbBookings { get; set; }

    public virtual DbSet<TbCabDetail> TbCabDetails { get; set; }

    public virtual DbSet<TbCabType> TbCabTypes { get; set; }

    public virtual DbSet<TbTripDetail> TbTripDetails { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    public virtual DbSet<TbUserRole> TbUserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__tb_booki__C6D03BCDC2E2D9C7");

            entity.ToTable("tb_booking");

            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CabId).HasColumnName("cabId");
            entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.ScheduleDate)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ScheduleTime)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TripId).HasColumnName("tripId");
            entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Cab).WithMany(p => p.TbBookings)
                .HasForeignKey(d => d.CabId)
                .HasConstraintName("FK__tb_bookin__cabId__656C112C");

            entity.HasOne(d => d.Trip).WithMany(p => p.TbBookings)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("FK__tb_bookin__tripI__6754599E");

            entity.HasOne(d => d.User).WithMany(p => p.TbBookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tb_bookin__userI__66603565");
        });

        modelBuilder.Entity<TbCabDetail>(entity =>
        {
            entity.HasKey(e => e.Cabid).HasName("PK__tb_Cab_D__66AD3D0504F0A20C");

            entity.ToTable("tb_Cab_Details");

            entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.RegistrationNun)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");

            entity.HasOne(d => d.CabType).WithMany(p => p.TbCabDetails)
                .HasForeignKey(d => d.CabTypeId)
                .HasConstraintName("FK__tb_Cab_De__CabTy__4316F928");

            entity.HasOne(d => d.User).WithMany(p => p.TbCabDetails)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__tb_Cab_De__UserI__440B1D61");
        });

        modelBuilder.Entity<TbCabType>(entity =>
        {
            entity.HasKey(e => e.CabTypeId).HasName("PK__tb_CabTy__5E575436F925A58F");

            entity.ToTable("tb_CabType");

            entity.Property(e => e.CabTypeId).ValueGeneratedNever();
            entity.Property(e => e.CabName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbTripDetail>(entity =>
        {
            entity.HasKey(e => e.TripDetailId).HasName("PK__tb_trip___C7F5312A578CE740");

            entity.ToTable("tb_trip_Details");

            entity.Property(e => e.TripDetailId).HasColumnName("tripDetailId");
            entity.Property(e => e.DestinationAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SourceAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

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
            entity.Property(e => e.MobileNumber)
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
