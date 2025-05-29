using BloodDonationAPI.Entities;
using BloodDonationAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BloodDonationAPI.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        //MyStrong!Passw0rd
        //sa
        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Donation> Donations { get; set; }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Hospitals> Hospitals{get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.SurName).IsRequired();
                entity.Property(u => u.Mail).IsRequired();
                entity.Property(u => u.Password).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.BloodType).IsRequired();
                entity.Property(u => u.Tc).IsRequired();

                entity.HasMany(u => u.Requests)
                      .WithOne(r => r.User)
                      .HasForeignKey(r => r.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(u => u.Donations)
                      .WithOne(d => d.User)
                      .HasForeignKey(d => d.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Request
            modelBuilder.Entity<Request>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Description).IsRequired();
                entity.Property(r => r.BloodType).IsRequired();
                entity.Property(r => r.UrgencyLevel).IsRequired();

                entity.HasMany(r => r.Donations)
                      .WithOne(d => d.Request)
                      .HasForeignKey(d => d.RequestId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Donation
            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.HasIndex(d => new { d.UserId, d.RequestId }).IsUnique(); // Aynı user, aynı talebe iki kez bağış yapamasın
            });

            //Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.AdminUserName).IsRequired();
                entity.Property(a => a.Password).IsRequired();
            });

            //Places
            modelBuilder.Entity<Hospitals>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Address);
                entity.Property(p => p.IsMobile);
                entity.Property(p => p.Plate);
            });
        }
    }

}