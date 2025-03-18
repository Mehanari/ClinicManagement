using System;
using System.Collections.Generic;
using ClinicManagement.Models;
using ClinicManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace ClinicManagement.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentResult> AppointmentResults { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorSpeciality> DoctorSpecialities { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Identification> Identifications { get; set; }

    public virtual DbSet<IdentificationType> IdentificationTypes { get; set; }

    public virtual DbSet<MedicalCard> MedicalCards { get; set; }

    public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

    public virtual DbSet<Role?> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;uid=root;pwd=root;database=mydb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("appointment");

            entity.HasIndex(e => e.DoctorSpecialityId, "fk_appointment_doctor_speciality1_idx");

            entity.HasIndex(e => e.CardNumber, "fk_appointment_medical_card1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CardNumber).HasColumnName("card_number");
            entity.Property(e => e.DoctorSpecialityId).HasColumnName("doctor_speciality_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");

            entity.HasOne(d => d.CardNumberNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.CardNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_medical_card1");

            entity.HasOne(d => d.DoctorSpeciality).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorSpecialityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_doctor_speciality1");
        });

        modelBuilder.Entity<AppointmentResult>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("appointment_result");

            entity.HasIndex(e => e.AppointmentId, "fk_appointment_result_appointment1_idx");

            entity.HasIndex(e => e.DoctorId, "fk_appointment_result_doctor1_idx");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actions)
                .HasMaxLength(255)
                .HasColumnName("actions");
            entity.Property(e => e.Anamnesis)
                .HasMaxLength(255)
                .HasColumnName("anamnesis");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Conclusion)
                .HasMaxLength(255)
                .HasColumnName("conclusion");
            entity.Property(e => e.Diagnosis)
                .HasMaxLength(255)
                .HasColumnName("diagnosis");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Objectively)
                .HasMaxLength(255)
                .HasColumnName("objectively");
            entity.Property(e => e.Prescription)
                .HasMaxLength(255)
                .HasColumnName("prescription");
            entity.Property(e => e.RadiationDose)
                .HasPrecision(10)
                .HasColumnName("radiationDose");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.Recommendations)
                .HasMaxLength(255)
                .HasColumnName("recommendations");

            entity.HasOne(d => d.Appointment).WithMany(p => p.AppointmentResults)
                .HasForeignKey(d => d.AppointmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_result_appointment1");

            entity.HasOne(d => d.Doctor).WithMany(p => p.AppointmentResults)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_appointment_result_doctor1");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("doctor");

            entity.HasIndex(e => e.SpecialityId, "fk_doctor_doctor_speciality1_idx");

            entity.HasIndex(e => e.Id, "fk_doctor_user1_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.SpecialityId).HasColumnName("speciality_id");

            entity.HasOne(d => d.Speciality).WithMany(p => p.Doctors)
                .HasForeignKey(d => d.SpecialityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_doctor_doctor_speciality1");
        });

        modelBuilder.Entity<DoctorSpeciality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("doctor_speciality");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("gender");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Identification>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.IdentificationTypeId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("identification");

            entity.HasIndex(e => e.IdentificationTypeId, "fk_identification_identification_type1_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdentificationTypeId).HasColumnName("identification_type_id");
            entity.Property(e => e.Identifier)
                .HasMaxLength(45)
                .HasColumnName("identifier");

            entity.HasOne(d => d.IdentificationType).WithMany(p => p.Identifications)
                .HasForeignKey(d => d.IdentificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_identification_identification_type1");
        });

        modelBuilder.Entity<IdentificationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("identification_type");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<MedicalCard>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PRIMARY");

            entity.ToTable("medical_card");

            entity.HasIndex(e => e.IdentificationId, "fk_medical_card_identification1_idx");

            entity.HasIndex(e => e.PersonalInfoId, "fk_medical_card_personal_info1_idx");

            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.IdentificationId).HasColumnName("identification_id");
            entity.Property(e => e.LastDiagnosis)
                .HasMaxLength(255)
                .HasColumnName("last_diagnosis");
            entity.Property(e => e.PersonalInfoId).HasColumnName("personal_info_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(45)
                .HasColumnName("phone");
            entity.Property(e => e.Workplace)
                .HasMaxLength(255)
                .HasColumnName("workplace");
        });

        modelBuilder.Entity<PersonalInfo>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.GenderId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("personal_info");

            entity.HasIndex(e => e.GenderId, "fk_personal_info_gender_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.GenderId).HasColumnName("gender_id");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Surname)
                .HasMaxLength(45)
                .HasColumnName("surname");

            entity.HasOne(d => d.Gender).WithMany(p => p.PersonalInfos)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_personal_info_gender");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.RoleId, e.PersonalInfoId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("user");

            entity.HasIndex(e => e.PersonalInfoId, "fk_user_personal_info1_idx");

            entity.HasIndex(e => e.RoleId, "fk_user_rold1_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PersonalInfoId).HasColumnName("personal_info_id");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_rold1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
