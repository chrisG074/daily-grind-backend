namespace backend.Models
{
    public class RawMaterial
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MaterialTypeId { get; set; }
        public string? Sku { get; set; }
        public decimal CurrentStock { get; set; } // Current quantity in stock
        public decimal ThresholdLevel { get; set; } // Minimum level before alert
        public string Unit { get; set; } = "m²"; // e.g., "m²", "kg", "rolls"
        public decimal? CostPerUnit { get; set; }
        public DateTime? LastRestocked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public MaterialType MaterialType { get; set; } = null!;
        public ICollection<RawMaterialAlert> Alerts { get; set; } = new List<RawMaterialAlert>();
    }

    public class RawMaterialAlert
    {
        public int Id { get; set; }
        public int RawMaterialId { get; set; }
        public string AlertType { get; set; } = string.Empty; // e.g., "LOW_STOCK", "OUT_OF_STOCK"
        public bool IsResolved { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }

        // Navigation properties
        public RawMaterial RawMaterial { get; set; } = null!;
    }
}
