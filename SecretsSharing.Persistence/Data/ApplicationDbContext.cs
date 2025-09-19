using Microsoft.EntityFrameworkCore;
using SecretsSharing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharing.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SecretFile> SecretFiles { get; set; }
        public DbSet<SecretText> SecretTexts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.PasswordHash).IsRequired();
            });

            // Configure SecretFile entity
            modelBuilder.Entity<SecretFile>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.FileName).IsRequired();
                entity.Property(f => f.StoragePath).IsRequired();
                entity.Property(f => f.AccessToken).IsRequired();

                entity.HasOne(f => f.User)
                    .WithMany(u => u.Files)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure SecretText entity
            modelBuilder.Entity<SecretText>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Content).IsRequired();
                entity.Property(t => t.AccessToken).IsRequired();

                entity.HasOne(t => t.User)
                    .WithMany(u => u.Texts)
                    .HasForeignKey(t => t.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
