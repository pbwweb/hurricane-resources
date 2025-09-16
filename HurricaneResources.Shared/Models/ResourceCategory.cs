using System.ComponentModel.DataAnnotations;

namespace HurricaneResources.Shared.Models;

public enum ResourceCategory
{
    Shelter,
    Food,
    Medical,
    Emergency,
    Transportation,
    Communication,
    Relief,
    Supplies
}

public static class ResourceCategoryExtensions
{
    public static string GetDisplayName(this ResourceCategory category)
    {
        return category switch
        {
            ResourceCategory.Shelter => "Shelters",
            ResourceCategory.Food => "Food Banks",
            ResourceCategory.Medical => "Medical Facilities",
            ResourceCategory.Emergency => "Emergency Services",
            ResourceCategory.Transportation => "Transportation",
            ResourceCategory.Communication => "Communication",
            ResourceCategory.Relief => "Relief Centers",
            ResourceCategory.Supplies => "Supply Distribution",
            _ => category.ToString()
        };
    }

    public static string GetIcon(this ResourceCategory category)
    {
        return category switch
        {
            ResourceCategory.Shelter => "🏠",
            ResourceCategory.Food => "🍞",
            ResourceCategory.Medical => "🏥",
            ResourceCategory.Emergency => "🚨",
            ResourceCategory.Transportation => "🚐",
            ResourceCategory.Communication => "📞",
            ResourceCategory.Relief => "🆘",
            ResourceCategory.Supplies => "📦",
            _ => "📍"
        };
    }

    public static string GetBootstrapIcon(this ResourceCategory category)
    {
        return category switch
        {
            ResourceCategory.Shelter => "bi-house-door",
            ResourceCategory.Food => "bi-cart",
            ResourceCategory.Medical => "bi-hospital",
            ResourceCategory.Emergency => "bi-exclamation-triangle",
            ResourceCategory.Transportation => "bi-truck",
            ResourceCategory.Communication => "bi-telephone",
            ResourceCategory.Relief => "bi-heart",
            ResourceCategory.Supplies => "bi-box",
            _ => "bi-geo-alt"
        };
    }
}