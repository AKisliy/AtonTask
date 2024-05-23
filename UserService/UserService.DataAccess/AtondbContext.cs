using Microsoft.EntityFrameworkCore;

namespace UserService.DataAccess;

public class AtondbContext : DbContext
{
    public AtondbContext()
    {
    }

    public AtondbContext(DbContextOptions<AtondbContext> options): base(options)
    {
    }

    public virtual DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birthday");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedOn)
                .HasColumnName("created_on");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("firstname");
            entity.Property(e => e.Gender)
                .HasDefaultValue(2)
                .HasColumnName("gender");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .HasColumnName("login");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(255)
                .HasColumnName("modified_by");
            entity.Property(e => e.ModifiedOn).HasColumnName("modified_on");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RevokedBy)
                .HasMaxLength(255)
                .HasColumnName("revoked_by");
            entity.Property(e => e.RevokedOn).HasColumnName("revoked_on");
            entity.Property(e => e.IsAdmin)
                .HasColumnName("isadmin")
                .HasDefaultValue(false);
        });
    }
}
