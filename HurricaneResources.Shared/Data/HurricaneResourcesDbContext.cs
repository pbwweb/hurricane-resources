using Microsoft.EntityFrameworkCore;
using HurricaneResources.Shared.Models;

namespace HurricaneResources.Shared.Data;

public class HurricaneResourcesDbContext : DbContext
{
    public HurricaneResourcesDbContext(DbContextOptions<HurricaneResourcesDbContext> options)
        : base(options)
    {
    }

    public DbSet<HurricaneResource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<HurricaneResource>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Category).HasConversion<string>().IsRequired();
            entity.Property(e => e.IconUrl).IsRequired().HasMaxLength(500);
            entity.Property(e => e.ResourceLink).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(200);

            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.IsActive);
            entity.HasIndex(e => e.FileName);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        const string defaultIconUrl = "https://pbwblobs.blob.core.windows.net/icons/emergency-20250916003531.jpg";

        var resources = new List<HurricaneResource>
        {
            new()
            {
                Id = 1,
                Category = ResourceCategory.Emergency,
                Name = "Broward County Emergency Management",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/Emergency/Pages/default.aspx",
                FileName = "emergency-call-svgrepo-com.jpg",
                IsActive = true
            },
            new()
            {
                Id = 2,
                Category = ResourceCategory.Emergency,
                Name = "Florida Division of Emergency Management",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.floridadisaster.org/",
                FileName = "call-center-telephone-svgrepo-com.jpg",
                IsActive = true
            },
            new()
            {
                Id = 3,
                Category = ResourceCategory.Emergency,
                Name = "Emergency Operations Center",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/Emergency/Pages/default.aspx",
                FileName = "emergency-operations.jpg",
                IsActive = true
            },
            new()
            {
                Id = 4,
                Category = ResourceCategory.Shelter,
                Name = "Hurricane Shelters",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/Hurricane/Pages/Shelters.aspx",
                FileName = "hurricane-shelters.jpg",
                IsActive = true
            },
            new()
            {
                Id = 5,
                Category = ResourceCategory.Shelter,
                Name = "Red Cross Shelters",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.redcross.org/local/florida/south-florida",
                FileName = "red-cross-shelter.jpg",
                IsActive = true
            },
            new()
            {
                Id = 6,
                Category = ResourceCategory.Shelter,
                Name = "Special Needs Shelter",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/AtRisk/Pages/SpecialNeeds.aspx",
                FileName = "special-needs-shelter.jpg",
                IsActive = true
            },
            new()
            {
                Id = 7,
                Category = ResourceCategory.Medical,
                Name = "Broward Health Emergency",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.browardhealth.org/pages/emergency-services",
                FileName = "broward-health.jpg",
                IsActive = true
            },
            new()
            {
                Id = 8,
                Category = ResourceCategory.Medical,
                Name = "Memorial Healthcare",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.mhs.net/services/emergency",
                FileName = "memorial-healthcare.jpg",
                IsActive = true
            },
            new()
            {
                Id = 9,
                Category = ResourceCategory.Food,
                Name = "Feeding South Florida",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://feedingsouthflorida.org/",
                FileName = "feeding-south-florida.jpg",
                IsActive = true
            },
            new()
            {
                Id = 10,
                Category = ResourceCategory.Supplies,
                Name = "Salvation Army",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://salvationarmyflorida.org/fortlauderdale/",
                FileName = "salvation-army.jpg",
                IsActive = true
            },
            new()
            {
                Id = 11,
                Category = ResourceCategory.Supplies,
                Name = "Emergency Supply Kit",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.ready.gov/kit",
                FileName = "emergency-kit.jpg",
                IsActive = true
            },
            new()
            {
                Id = 12,
                Category = ResourceCategory.Transportation,
                Name = "Evacuation Routes",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/Hurricane/Pages/EvacuationZones.aspx",
                FileName = "evacuation-routes.jpg",
                IsActive = true
            },
            new()
            {
                Id = 13,
                Category = ResourceCategory.Transportation,
                Name = "BCT Emergency Transportation",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.broward.org/BCT/Pages/default.aspx",
                FileName = "bct-emergency.jpg",
                IsActive = true
            },
            new()
            {
                Id = 14,
                Category = ResourceCategory.Emergency,
                Name = "FEMA Assistance",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.fema.gov/assistance/individual",
                FileName = "fema-assistance.jpg",
                IsActive = true
            },
            new()
            {
                Id = 15,
                Category = ResourceCategory.Emergency,
                Name = "National Weather Service",
                IconUrl = defaultIconUrl,
                ResourceLink = "https://www.weather.gov/mfl/",
                FileName = "weather-service.jpg",
                IsActive = true
            }
        };

        modelBuilder.Entity<HurricaneResource>().HasData(resources);
    }
}
