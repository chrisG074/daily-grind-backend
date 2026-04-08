namespace backend.Models
{
    public class MaterialType
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "POLY_110", "SPUNPOLYESTER"
        public string Specification { get; set; } = string.Empty; // e.g., "110g/m² Gloss Poly"
        public decimal PriceModifier { get; set; } = 1.0m;

        // Navigation properties
        public ICollection<MaterialTypeTranslation> Translations { get; set; } = new List<MaterialTypeTranslation>();
        public ICollection<MaterialWashingInstruction> MaterialWashingInstructions { get; set; } = new List<MaterialWashingInstruction>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
    }

    public class MaterialTypeTranslation
    {
        public int Id { get; set; }
        public int MaterialTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Navigation properties
        public MaterialType MaterialType { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }
}
