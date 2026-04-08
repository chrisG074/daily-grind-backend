namespace backend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public int FlagTypeId { get; set; }
        public int? MaterialTypeId { get; set; }
        public decimal BasePrice { get; set; }
        public int StockQuantity { get; set; }
        public string? Image { get; set; }
        public double? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public string? Badge { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public FlagType FlagType { get; set; } = null!;
        public MaterialType? MaterialType { get; set; }
        public ICollection<ProductTranslation> Translations { get; set; } = new List<ProductTranslation>();
        public ICollection<ProductAttachmentMethod> ProductAttachmentMethods { get; set; } = new List<ProductAttachmentMethod>();
        public ICollection<ProductBundle> BundleProducts { get; set; } = new List<ProductBundle>();
        public ICollection<ProductBundle> BundledInProducts { get; set; } = new List<ProductBundle>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
