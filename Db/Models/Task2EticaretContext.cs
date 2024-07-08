using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Db.Models;

public partial class Task2EticaretContext : DbContext
{
    public Task2EticaretContext()
    {
    }

    public Task2EticaretContext(DbContextOptions<Task2EticaretContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<Satislar> Satislars { get; set; }

    public virtual DbSet<Urunler> Urunlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-CGDTBSJ\\MSSQLSERVER5;Database=task2_Eticaret;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__Admin__43AA4141ECDEB1E4");

            entity.ToTable("Admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.Ad)
                .HasMaxLength(50)
                .HasColumnName("ad");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .HasColumnName("mail");
            entity.Property(e => e.MailSifre)
                .HasMaxLength(50)
                .HasColumnName("mailSifre");
            entity.Property(e => e.Sifre)
                .HasMaxLength(20)
                .HasColumnName("sifre");
        });

        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.KategoriId).HasName("PK__Kategori__AFB6FE700CDFED7F");

            entity.ToTable("Kategoriler");

            entity.Property(e => e.KategoriId).HasColumnName("kategori_id");
            entity.Property(e => e.Aktiflik).HasColumnName("aktiflik");
            entity.Property(e => e.KategoriAd)
                .HasMaxLength(50)
                .HasColumnName("kategori_ad");
        });

        modelBuilder.Entity<Satislar>(entity =>
        {
            entity.HasKey(e => e.SatisId).HasName("PK__Satislar__BD37A3E897389466");

            entity.ToTable("Satislar");

            entity.Property(e => e.SatisId).HasColumnName("satis_id");
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.AliciAd).HasMaxLength(100);
            entity.Property(e => e.Cvv)
                .HasMaxLength(5)
                .HasColumnName("CVV");
            entity.Property(e => e.GecerlilikTarihi).HasMaxLength(5);
            entity.Property(e => e.KartNumarasi).HasMaxLength(30);
            entity.Property(e => e.Mail).HasMaxLength(40);
            entity.Property(e => e.Onay).HasColumnName("onay");
            entity.Property(e => e.Tarih).HasColumnType("datetime");
            entity.Property(e => e.Tel).HasMaxLength(15);
            entity.Property(e => e.UrunId).HasColumnName("urun_id");

            entity.HasOne(d => d.Urun).WithMany(p => p.Satislars)
                .HasForeignKey(d => d.UrunId)
                .HasConstraintName("FK__Satislar__urun_i__2B3F6F97");
        });

        modelBuilder.Entity<Urunler>(entity =>
        {
            entity.HasKey(e => e.UrunId).HasName("PK__Urunler__933C200A76A24284");

            entity.ToTable("Urunler");

            entity.Property(e => e.UrunId).HasColumnName("urun_id");
            entity.Property(e => e.Akitflik).HasColumnName("akitflik");
            entity.Property(e => e.KategoriId).HasColumnName("kategori_id");
            entity.Property(e => e.Miktar).HasColumnName("miktar");
            entity.Property(e => e.UrunAd)
                .HasMaxLength(100)
                .HasColumnName("urunAd");

            entity.HasOne(d => d.Kategori).WithMany(p => p.Urunlers)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK__Urunler__kategor__286302EC");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
