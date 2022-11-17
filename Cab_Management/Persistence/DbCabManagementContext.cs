using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class DbCabManagementContext : DbContext
{
    public DbCabManagementContext()
    {
    }

    public DbCabManagementContext(DbContextOptions<DbCabManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TabAddress> TabAddresses { get; set; }

    public virtual DbSet<TabCabDetail> TabCabDetails { get; set; }

    public virtual DbSet<TabCabType> TabCabTypes { get; set; }

    public virtual DbSet<TabDriverDetail> TabDriverDetails { get; set; }

    public virtual DbSet<TabPersonalDetail> TabPersonalDetails { get; set; }

    public virtual DbSet<TabUsersDetail> TabUsersDetails { get; set; }

    public virtual DbSet<TbUserType> TbUserTypes { get; set; }
    public IEnumerable<object> Login { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = 65.0.181.176;Database=db_CabManagement;User Id = admin;Password = Asdf1234*;TrustServerCertificate=True;Connection Timeout=300;command timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TabAddress>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__tab_Addr__091C2AFBC0E43AB8");

            entity.ToTable("tab_Address");

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Locality)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Street)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("street");
        });

        modelBuilder.Entity<TabCabDetail>(entity =>
        {
            entity.HasKey(e => e.Cabid).HasName("PK__tab_Cab___66AD3D0559DB5D1F");

            entity.ToTable("tab_Cab_Details");

            entity.Property(e => e.RegistrationNun)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.CabType).WithMany(p => p.TabCabDetails)
                .HasForeignKey(d => d.CabTypeId)
                .HasConstraintName("FK__tab_Cab_D__CabTy__398D8EEE");
        });

        modelBuilder.Entity<TabCabType>(entity =>
        {
            entity.HasKey(e => e.CabTypeId).HasName("PK__tab_CabT__5E5754364A17F378");

            entity.ToTable("tab_CabType");

            entity.Property(e => e.CabName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TabDriverDetail>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__tab_Driv__7ED91ACF8B13E994");

            entity.ToTable("tab_Driver_Details");

            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Dlnumber)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DLNumber");
            entity.Property(e => e.DriverId)
                .ValueGeneratedOnAdd()
                .HasColumnName("driverId");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsValidLicense).HasColumnName("isValidLicense");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressDetailsNavigation).WithMany(p => p.TabDriverDetails)
                .HasForeignKey(d => d.AddressDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_Drive__Addre__403A8C7D");

            entity.HasOne(d => d.Cab).WithMany(p => p.TabDriverDetails)
                .HasForeignKey(d => d.CabId)
                .HasConstraintName("FK__tab_Drive__CabId__412EB0B6");
        });

        modelBuilder.Entity<TabPersonalDetail>(entity =>
        {
            entity.HasKey(e => e.PersonalId).HasName("PK__tab_Pers__283437F3196D030E");

            entity.ToTable("tab_Personal_Details");

            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.AddressDetailsNavigation).WithMany(p => p.TabPersonalDetails)
                .HasForeignKey(d => d.AddressDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_Perso__Addre__31EC6D26");

            entity.HasOne(d => d.Email).WithMany(p => p.TabPersonalDetails)
                .HasForeignKey(d => d.EmailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_Perso__Email__30F848ED");
        });

        modelBuilder.Entity<TabUsersDetail>(entity =>
        {
            entity.HasKey(e => e.EmailId).HasName("PK__tab_User__7ED91ACF90A351C2");

            entity.ToTable("tab_Users_Details");

            entity.Property(e => e.EmailId)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("smalldatetime");
            entity.Property(e => e.UserId).ValueGeneratedOnAdd();
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.UserTypeNavigation).WithMany(p => p.TabUsersDetails)
                .HasForeignKey(d => d.UserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tab_Users__UserT__29572725");
        });

        modelBuilder.Entity<TbUserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__tb_UserT__40D2D81600C3446E");

            entity.ToTable("tb_UserType");

            entity.Property(e => e.UserTypeName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
