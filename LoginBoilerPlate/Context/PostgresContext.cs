using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LoginBoilerPlate;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ResetMotDePasse> ResetMotDePasses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }
    readonly string? _connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ResetMotDePasse>(entity =>
        {
            entity.HasKey(e => e.IdReset).HasName("reset_mot_de_passe_pk");

            entity.ToTable("reset_mot_de_passe");

            entity.Property(e => e.IdReset).HasColumnName("id_reset");
            entity.Property(e => e.DateCreation).HasColumnName("date_creation");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.ResetMotDePasse1)
                .HasMaxLength(50)
                .HasColumnName("reset_mot_de_passe");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ResetMotDePasses)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reset_mot_de_passe_user_fk");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("role_pk");

            entity.ToTable("role");

            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.RoleAdmin).HasColumnName("role_admin");
            entity.Property(e => e.RoleUser).HasColumnName("role_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("user_pk");

            entity.ToTable("user");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Mail)
                .HasMaxLength(50)
                .HasColumnName("mail");
            entity.Property(e => e.MotDePasse)
                .HasMaxLength(50)
                .HasColumnName("mot_de_passe");
            entity.Property(e => e.Nom)
                .HasMaxLength(50)
                .HasColumnName("nom");
            entity.Property(e => e.PhotoProfile).HasColumnName("photo_profile");
            entity.Property(e => e.Prenom)
                .HasMaxLength(50)
                .HasColumnName("prenom");
            entity.Property(e => e.Valide).HasColumnName("valide");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_role_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
