using System;
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
                    Region = "Hong Kong Island",
                    District = "Chai Wan",
                    PropertyType = "Residential (HOS)",
                    SalePrice = 450000,
                    Address = "Unit 1, 7/F, Yuet Chui Court, 9 Yan Tsui Street, Chai Wan, Hong Kong",
                    GrossArea = "",
                    SaleableArea = "2,100 sq ft",
                    YearBuilt = "",
                    RefNo = "H6194",
                    Description = "Modern 2-bedroom apartment in the heart of downtown",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 85.5m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CSM2279.JPG"
                },
                new Property
                {
                    Id = 2,
                    Region = "New Territories",
                    District = "Fanling",
                    PropertyType = "Village",
                    SalePrice = 9950000,
                    Address = "\tDD 85, Lot No. 579, Section D, G/F, 1/F,2/F & Roof/F, Sub-section 1 of Section C of Lot No.583 In D D 85, 63 Lau Shui Heung, Fanling, New Territories",
                    GrossArea = "",
                    SaleableArea = "2,100 sq ft",
                    YearBuilt = "",
                    RefNo = "H6194",
                    Description = "Beautiful 3-bedroom house with garden",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 120.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CRU6194.JPG"
                },
                new Property
                {
                    Id = 3,
                    Region = "Kowloon",
                    District = "Sham Shui Po",
                    PropertyType = "Commercial",
                    SalePrice = 21500000,
                    Address = "Unit 32, 34 & 36, Lower G /F, Golden Building, 146-152 Fuk Wa Street , Sham Shui Po, Kowloon ",
                    GrossArea = "",
                    SaleableArea = "712 sq ft (Not Verified)",
                    YearBuilt = "1980",
                    RefNo = "H1102",
                    Description = "Cozy condo with river view",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Area = 65.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/1102LMU.JPG"
                },
                new Property
                {
                    Id = 4,
                    Region = "Kowloon",
                    District = "Mong Kok",
                    PropertyType = "Commercial",
                    SalePrice = 16000000,
                    Address = "Unit 2, G/F, Strong Good Building , 252 Tung Choi Street , Mong Kok, Kowloon ",
                    GrossArea = "",
                    SaleableArea = "623 sq ft (Not Verified)",
                    YearBuilt = "",
                    RefNo = "S2260",
                    Description = "Luxury villa with panoramic views",
                    Bedrooms = 5,
                    Bathrooms = 4,
                    Area = 250.0m,
                    ListedDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CSM2260.JPG"
                },
                new Property
                {
                    Id = 5,
                    Region = "Kowloon",
                    District = "Kwun Tong",
                    PropertyType = "Office",
                    SalePrice = 42000000,
                    Address = "Unit A, 31/F, T G Place, 10 Shing Yip Street, Kwun Tong, Kowloon ",
                    GrossArea = "3,813 sq ft (Not Verified)",
                    SaleableArea = "",
                    YearBuilt = "2014",
                    RefNo = "S1172",
                    Description = "Modern condo near shopping center",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 78.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CRM1172.JPG"
                    
                },
                new Property
                {
                    Id = 6,
                    Region = "New Territories",
                    District = "Tsuen Wan",
                    PropertyType = "Industrial",
                    SalePrice = 4200000,
                    Address = "Unit I, 7/F, Wah Lik Industrial Centre , 459-469 Castle Peak Road , Tsuen Wan, New Territories ",
                    GrossArea = "3,813 sq ft (Not Verified)",
                    SaleableArea = "",
                    YearBuilt = "2014",
                    RefNo = "S1172",
                    Description = "Charming historic townhouse",
                    Bedrooms = 3,
                    Bathrooms = 2,
                    Area = 110.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/1146LMU.JPG"
                },
                new Property
                {
                    Id = 7,
                    Region = "New Territories",
                    District = "Tin Shui Wai",
                    PropertyType = "Residential",
                    SalePrice = 4750000,
                    Address = "Unit A2, 7/F, Block 7, Wetland Seasons Park, Phase 3 of Wetland Lot No.34 Development , 9 Wetland Park Road, Yuen Long, New Territories ",
                    GrossArea = "",
                    SaleableArea = "446 sq ft",
                    YearBuilt = "",
                    RefNo = "S2240",
                    Description = "Student-friendly apartment near university",
                    Bedrooms = 1,
                    Bathrooms = 1,
                    Area = 55.0m,
                    ListedDate = DateTime.Now.AddDays(-12),
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CSM2240.JPG"
                },
                new Property
                {
                    Id = 8,
                    Region = "New Territories",
                    District = "Kwai Chung",
                    PropertyType = "Residential",
                    SalePrice = 750000,
                    Address = "Unit 8, 14/F, Block B, Shui King Building, 148-172 Wo Yi Hop Road, Kwai Chung, New Territories ",
                    GrossArea = "",
                    SaleableArea = "341 sq ft",
                    YearBuilt = "1974",
                    RefNo = "H6222",
                    Description = "Commercial warehouse space",
                    Bedrooms = 0,
                    Bathrooms = 2,
                    Area = 500.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CRU6222.JPG"
                },
                new Property
                {
                    Id = 9,
                    Region = "Kowloon",
                    District = "Tsim Sha Tsui",
                    PropertyType = "Commercial",
                    SalePrice = 18800000,
                    Address = "Shop No. 2, G/F, Pine Hill Mansion, 128 Austin Road, Kowloon ",
                    GrossArea = "",
                    SaleableArea = "400 sq ft (Not Verified)",
                    YearBuilt = "1965",
                    RefNo = "BG240315",
                    Description = "Executive apartment near business center",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 95.0m,
                    ListedDate = DateTime.Now.AddDays(-20),
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/BG240315.JPG"
                },
                new Property
                {
                    Id = 10,
                    Region = "New Territories",
                    District = "Yuen Long",
                    PropertyType = "Residential",
                    SalePrice = 3130000,
                    Address = "Unit E, 1/F, Yick Lee Mansion, 26 Kin Tak Street, Yuen Long, New Territories ",
                    GrossArea = "",
                    SaleableArea = "400 sq ft (Not Verified)",
                    YearBuilt = "1965",
                    RefNo = "BG240315",
                    Description = "Beachfront house with ocean views",
                    Bedrooms = 4,
                    Bathrooms = 3,
                    Area = 180.0m,
                    ListedDate = DateTime.Now,
                    ImageUrl = "https://ftp.knightfrank.com.hk/amis_pdf/photo/CSM2206.JPG"
                },
                new Property
                {
                    Id = 11,
                    Region = "North",
                    District = "Downtown",
                    PropertyType = "Apartment",
                    SalePrice = 450000,
                    Address = "123 Main St, Downtown",
                    GrossArea = "",
                    SaleableArea = "341 sq ft",
                    YearBuilt = "1974",
                    RefNo = "H6222",
                    Description = "Modern 2-bedroom apartment in the heart of downtown",
                    Bedrooms = 2,
                    Bathrooms = 2,
                    Area = 85.5m,
                    ListedDate = DateTime.Now.AddDays(-30),
                    ImageUrl = "https://via.placeholder.com/300x200"
                }
            );
        }
    }
}