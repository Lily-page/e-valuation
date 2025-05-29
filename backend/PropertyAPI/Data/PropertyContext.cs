using Microsoft.EntityFrameworkCore;
using PropertyAPI.Models;

namespace PropertyAPI.Data
{
    public class PropertyContext : DbContext
    {
        public PropertyContext(DbContextOptions<PropertyContext> options) : base(options)
        {
        }

        public DbSet<Property> Properties { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SalePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Area).HasColumnType("decimal(10,2)");
            });

            // Seed data
            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    Id = 1,
                    Region = "North",
                    District = "Downtown",
                    PropertyType = "Apartment",
                    SalePrice = 450000,
                    Address = "123 Main St, Downtown",
                    Description = "Modern 2-bedroom apartment in the heart of downtown",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 85.5m,
                    ListedDate = DateTime.Now.AddDays(-30),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 2,
                    Region = "South",
                    District = "Suburbs",
                    PropertyType = "House",
                    SalePrice = 650000,
                    Address = "456 Oak Ave, Suburbs",
                    Description = "Beautiful 3-bedroom house with garden",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 120.0m,
                    ListedDate = DateTime.Now.AddDays(-15),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 3,
                    Region = "East",
                    District = "Riverside",
                    PropertyType = "Condo",
                    SalePrice = 380000,
                    Address = "789 River Rd, Riverside",
                    Description = "Cozy condo with river view",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Area = 65.0m,
                    ListedDate = DateTime.Now.AddDays(-10),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 4,
                    Region = "West",
                    District = "Hills",
                    PropertyType = "Villa",
                    SalePrice = 1200000,
                    Address = "321 Hill Top Dr, Hills",
                    Description = "Luxury villa with panoramic views",
                    Bedrooms = 5,
                    Bathrooms = 4,
                    Area = 250.0m,
                    ListedDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 5,
                    Region = "North",
                    District = "Business",
                    PropertyType = "Apartment",
                    SalePrice = 520000,
                    Address = "555 Business Blvd, Business District",
                    Description = "Executive apartment near business center",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 95.0m,
                    ListedDate = DateTime.Now.AddDays(-20),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 6,
                    Region = "South",
                    District = "Beachside",
                    PropertyType = "House",
                    SalePrice = 850000,
                    Address = "777 Beach Ave, Beachside",
                    Description = "Beachfront house with ocean views",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    Area = 180.0m,
                    ListedDate = DateTime.Now.AddDays(-8),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 7,
                    Region = "East",
                    District = "University",
                    PropertyType = "Apartment",
                    SalePrice = 320000,
                    Address = "999 Campus St, University",
                    Description = "Student-friendly apartment near university",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Area = 55.0m,
                    ListedDate = DateTime.Now.AddDays(-12),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 8,
                    Region = "West",
                    District = "Industrial",
                    PropertyType = "Warehouse",
                    SalePrice = 750000,
                    Address = "111 Factory Rd, Industrial",
                    Description = "Commercial warehouse space",
                    Bedrooms = 0,
                    Bathrooms = 2,
                    Area = 500.0m,
                    ListedDate = DateTime.Now.AddDays(-25),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 9,
                    Region = "North",
                    District = "Shopping",
                    PropertyType = "Condo",
                    SalePrice = 420000,
                    Address = "222 Mall St, Shopping District",
                    Description = "Modern condo near shopping center",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 78.0m,
                    ListedDate = DateTime.Now.AddDays(-18),
                    ImageUrl = "https://via.placeholder.com/300x200"
                },
                new Property
                {
                    Id = 10,
                    Region = "South",
                    District = "Historic",
                    PropertyType = "Townhouse",
                    SalePrice = 580000,
                    Address = "333 Heritage Ln, Historic District",
                    Description = "Charming historic townhouse",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 110.0m,
                    ListedDate = DateTime.Now.AddDays(-7),
                    ImageUrl = "https://via.placeholder.com/300x200"
                }
            );
        }
    }
}