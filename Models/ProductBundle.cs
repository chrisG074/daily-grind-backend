namespace backend.Models
{
    // Self-referencing N-to-M relationship for product bundles
    public class ProductBundle
    {
        public int MainProductId { get; set; }
        public int BundledProductId { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? DiscountPercentage { get; set; }

        // Navigation properties
        public Product MainProduct { get; set; } = null!;
        public Product BundledProduct { get; set; } = null!;
    }
}
