namespace backend.Models
{
    public class WashingInstruction
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "MACHINE_30", "HAND_WASH"
        public string? IconUrl { get; set; }

        // Navigation properties
        public ICollection<WashingInstructionTranslation> Translations { get; set; } = new List<WashingInstructionTranslation>();
        public ICollection<MaterialWashingInstruction> MaterialWashingInstructions { get; set; } = new List<MaterialWashingInstruction>();
    }

    public class WashingInstructionTranslation
    {
        public int Id { get; set; }
        public int WashingInstructionId { get; set; }
        public int LanguageId { get; set; }
        public string Instruction { get; set; } = string.Empty;

        // Navigation properties
        public WashingInstruction WashingInstruction { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }

    // Junction table for MaterialType and WashingInstruction (N-to-M)
    public class MaterialWashingInstruction
    {
        public int MaterialTypeId { get; set; }
        public int WashingInstructionId { get; set; }
        public int SortOrder { get; set; }

        // Navigation properties
        public MaterialType MaterialType { get; set; } = null!;
        public WashingInstruction WashingInstruction { get; set; } = null!;
    }
}
