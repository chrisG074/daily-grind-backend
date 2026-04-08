namespace backend.Models
{
    public class FlagType
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "COUNTRY", "SIGNAL", "COMPANY"
        public int SortOrder { get; set; }

        // Navigation properties
        public ICollection<FlagTypeTranslation> Translations { get; set; } = new List<FlagTypeTranslation>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public class FlagTypeTranslation
    {
        public int Id { get; set; }
        public int FlagTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation properties
        public FlagType FlagType { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }
}
