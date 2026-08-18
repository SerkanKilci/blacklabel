using Blacklabel.Domain.Entities;
using Blacklabel.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Blacklabel.Infrastructure.Persistence;

public class BlacklabelDbContext : DbContext
{
    public BlacklabelDbContext(DbContextOptions<BlacklabelDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Additive> Additives => Set<Additive>();
    public DbSet<ProductAdditive> ProductAdditives => Set<ProductAdditive>();
    public DbSet<Allergen> Allergens => Set<Allergen>();
    public DbSet<ProductAllergen> ProductAllergens => Set<ProductAllergen>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<HouseholdProfile> HouseholdProfiles => Set<HouseholdProfile>();
    public DbSet<Scan> Scans => Set<Scan>();
    public DbSet<Contribution> Contributions => Set<Contribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Barcode).HasMaxLength(14).IsRequired();
            entity.HasIndex(p => p.Barcode).IsUnique();
            entity.Property(p => p.Name).HasMaxLength(300).IsRequired();
            entity.Property(p => p.Brand).HasMaxLength(200);
            entity.Property(p => p.Quantity).HasMaxLength(100);
            entity.Property(p => p.NutriScore).HasMaxLength(1);
            entity.Property(p => p.ImageUrl).HasMaxLength(500);
            entity.Property(p => p.Categories).HasMaxLength(1000).IsRequired();
        });

        modelBuilder.Entity<Additive>(entity =>
        {
            entity.HasKey(a => a.Code);
            entity.Property(a => a.Code).HasMaxLength(10);
            entity.Property(a => a.NameTr).HasMaxLength(200).IsRequired();
            entity.Property(a => a.NameEn).HasMaxLength(200).IsRequired();
            entity.Property(a => a.DescriptionTr).HasMaxLength(1000).IsRequired();
            entity.Property(a => a.DescriptionEn).HasMaxLength(1000).IsRequired();
            entity.Property(a => a.SourceNote).HasMaxLength(500);
            entity.Property(a => a.Synonyms).HasMaxLength(500).IsRequired();
            entity.HasData(AdditiveSeedData.Get());
        });

        modelBuilder.Entity<ProductAdditive>(entity =>
        {
            entity.HasKey(pa => new { pa.ProductId, pa.AdditiveCode });
            entity.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAdditives)
                .HasForeignKey(pa => pa.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pa => pa.Additive)
                .WithMany(a => a.ProductAdditives)
                .HasForeignKey(pa => pa.AdditiveCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Allergen>(entity =>
        {
            entity.HasKey(a => a.Code);
            entity.Property(a => a.NameTr).HasMaxLength(200).IsRequired();
            entity.Property(a => a.NameEn).HasMaxLength(200).IsRequired();
            entity.HasData(AllergenSeedData.Get());
        });

        modelBuilder.Entity<ProductAllergen>(entity =>
        {
            entity.HasKey(pa => new { pa.ProductId, pa.AllergenCode });
            entity.HasOne(pa => pa.Product)
                .WithMany(p => p.ProductAllergens)
                .HasForeignKey(pa => pa.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(pa => pa.Allergen)
                .WithMany(a => a.ProductAllergens)
                .HasForeignKey(pa => pa.AllergenCode)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.DeviceId).IsRequired();
        });

        modelBuilder.Entity<HouseholdProfile>(entity =>
        {
            entity.HasKey(hp => hp.Id);
            entity.Property(hp => hp.Name).HasMaxLength(50).IsRequired();
            entity.HasOne(hp => hp.User)
                .WithMany(u => u.HouseholdProfiles)
                .HasForeignKey(hp => hp.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(hp => hp.UserId);
        });

        modelBuilder.Entity<Scan>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.HasOne(s => s.User)
                .WithMany(u => u.Scans)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(s => new { s.UserId, s.ScannedAt }).IsDescending(false, true);
        });

        modelBuilder.Entity<Contribution>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasOne(c => c.User)
                .WithMany(u => u.Contributions)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(c => c.Status);
        });
    }
}
