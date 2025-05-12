using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PatientHistoryAPI.Models;

public partial class PatientHistoryDbContext : DbContext
{
    public PatientHistoryDbContext()
    {
    }

    public PatientHistoryDbContext(DbContextOptions<PatientHistoryDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diagnosis> Diagnoses { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ELW5371;Database=PatientHistoryDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diagnosis>(entity =>
        {
            entity.HasKey(e => e.DiagnosisId).HasName("PK__Diagnose__0C54CC7320454587");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Diagnosis1)
                .HasMaxLength(255)
                .HasColumnName("Diagnosis");

            entity.HasOne(d => d.Visit).WithMany(p => p.Diagnoses)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("FK__Diagnoses__Visit__2B3F6F97");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.DoctorId).HasName("PK__Doctors__2DC00EBF9BBFA8CF");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Specialization).HasMaxLength(100);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId).HasName("PK__Patients__970EC366D3CD56FE");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(100);
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.HasKey(e => e.PrescriptionId).HasName("PK__Prescrip__4013083215B201B8");

            entity.Property(e => e.Dosage).HasMaxLength(100);
            entity.Property(e => e.Duration).HasMaxLength(100);
            entity.Property(e => e.Medication).HasMaxLength(255);

            entity.HasOne(d => d.Visit).WithMany(p => p.Prescriptions)
                .HasForeignKey(d => d.VisitId)
                .HasConstraintName("FK__Prescript__Visit__2E1BDC42");
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visits__4D3AA1DE480A0DE9");

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.VisitDate).HasColumnType("datetime");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Visits)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__Visits__DoctorId__286302EC");

            entity.HasOne(d => d.Patient).WithMany(p => p.Visits)
                .HasForeignKey(d => d.PatientId)
                .HasConstraintName("FK__Visits__PatientI__276EDEB3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
