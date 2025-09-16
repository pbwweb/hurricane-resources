using System.ComponentModel.DataAnnotations;

namespace HurricaneResources.Shared.Models;

public class HurricaneResource
{
    public int Id { get; set; }

    [Required]
    public ResourceCategory Category { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Resource Name")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Url]
    [StringLength(500)]
    public string IconUrl { get; set; } = string.Empty;

    [Required]
    [Url]
    [StringLength(500)]
    public string ResourceLink { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    [Display(Name = "Icon File Name")]
    public string FileName { get; set; } = string.Empty; // Stores the icon filename (e.g., "emergency-icon.jpg")

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Computed property for display
    public string CategoryDisplayName => Category.GetDisplayName();

    public string CategoryBootstrapIcon => Category.GetBootstrapIcon();
}