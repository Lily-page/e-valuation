using Microsoft.EntityFrameworkCore;
using Knightfrank.eValuation.WebApi.Models;
using System;

namespace Knightfrank.eValuation.WebApi.Data;

public class EValuationContext : DbContext
{
    public EValuationContext(DbContextOptions<EValuationContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public DbSet<Valuation> Valuations { get; set; }
    public DbSet<AnonymousToken> AnonymousTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Property entity
        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Zone).IsRequired().HasMaxLength(100);
            entity.Property(e => e.District).IsRequired().HasMaxLength(100);
            entity.Property(e => e.EstateName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Block).HasMaxLength(50);
            entity.Property(e => e.Floor).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(50);
            entity.Property(e => e.GrossFloorArea).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SaleableFloorArea).HasColumnType("decimal(18,2)");
        });

        // Configure Valuation entity
        modelBuilder.Entity<Valuation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EstimatedValue).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(e => e.GfaUnitRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.SfaUnitRate).HasColumnType("decimal(18,2)");
            entity.Property(e => e.TokenUsed).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);

            entity.HasOne(e => e.Property)
                  .WithMany(p => p.Valuations)
                  .HasForeignKey(e => e.PropertyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure AnonymousToken entity
        modelBuilder.Entity<AnonymousToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Token).IsRequired().HasMaxLength(500);
            entity.Property(e => e.SessionId).HasMaxLength(100);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.SessionId);
        });

        // Seed sample data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Properties
        modelBuilder.Entity<Property>().HasData(
            new Property
            {
                Id = 1,
                Zone = "Hong Kong",
                District = "Central",
                EstateName = "The Center",
                Block = "A",
                Floor = "15",
                Unit = "1501",
                GrossFloorArea = 800.50m,
                SaleableFloorArea = 650.25m,
                BuiltYear = new DateTime(2010, 1, 1)
            },
            new Property
            {
                Id = 2,
                Zone = "Hong Kong",
                District = "Admiralty",
                EstateName = "Pacific Place",
                Block = "B",
                Floor = "20",
                Unit = "2001",
                GrossFloorArea = 1200.75m,
                SaleableFloorArea = 980.50m,
                BuiltYear = new DateTime(2015, 6, 15)
            },
            new Property
            {
                Id = 3,
                Zone = "Hong Kong",
                District = "Wan Chai",
                EstateName = "Times Square",
                Block = "C",
                Floor = "25",
                Unit = "2505",
                GrossFloorArea = 950.25m,
                SaleableFloorArea = 780.75m,
                BuiltYear = new DateTime(2012, 3, 10)
            }
        );

        // Seed Valuations
        modelBuilder.Entity<Valuation>().HasData(
            new Valuation
            {
                Id = 1,
                PropertyId = 1,
                EstimatedValue = 12500000m,
                GfaUnitRate = 15625m,
                SfaUnitRate = 19230m,
                ValuationDate = DateTime.UtcNow.AddDays(-30),
                CreatedAt = DateTime.UtcNow.AddDays(-30),
                Notes = "Prime location in Central district"
            },
            new Valuation
            {
                Id = 2,
                PropertyId = 2,
                EstimatedValue = 18750000m,
                GfaUnitRate = 15625m,
                SfaUnitRate = 19130m,
                ValuationDate = DateTime.UtcNow.AddDays(-15),
                CreatedAt = DateTime.UtcNow.AddDays(-15),
                Notes = "Luxury development in Admiralty"
            },
            new Valuation
            {
                Id = 3,
                PropertyId = 3,
                EstimatedValue = 14850000m,
                GfaUnitRate = 15631m,
                SfaUnitRate = 19025m,
                ValuationDate = DateTime.UtcNow.AddDays(-7),
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                Notes = "Commercial hub location"
            }
        );
    }
}