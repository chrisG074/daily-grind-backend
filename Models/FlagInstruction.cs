namespace backend.Models
{
    public class FlagInstruction
    {
        public int Id { get; set; }
        public string CountryCode { get; set; } = string.Empty; // ISO 3166-1 alpha-2 code (e.g., "NL", "BE")
        public int LanguageId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public Language Language { get; set; } = null!;
    }
}
