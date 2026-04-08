namespace backend.Models
{
    public class AttachmentMethod
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "CORD_LOOP", "HOOKS", "TUNNEL"
        public int SortOrder { get; set; }

        // Navigation properties
        public ICollection<AttachmentMethodTranslation> Translations { get; set; } = new List<AttachmentMethodTranslation>();
        public ICollection<ProductAttachmentMethod> ProductAttachmentMethods { get; set; } = new List<ProductAttachmentMethod>();
    }

    public class AttachmentMethodTranslation
    {
        public int Id { get; set; }
        public int AttachmentMethodId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation properties
        public AttachmentMethod AttachmentMethod { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }

    // Junction table for Product and AttachmentMethod (N-to-M)
    public class ProductAttachmentMethod
    {
        public int ProductId { get; set; }
        public int AttachmentMethodId { get; set; }

        // Navigation properties
        public Product Product { get; set; } = null!;
        public AttachmentMethod AttachmentMethod { get; set; } = null!;
    }
}
