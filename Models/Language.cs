namespace backend.Models
{
    public class Language
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "nl", "en", "de", "fr"
        public string Name { get; set; } = string.Empty; // e.g., "Nederlands", "English"
        public bool IsDefault { get; set; } = false;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<ProductTranslation> ProductTranslations { get; set; } = new List<ProductTranslation>();
        public ICollection<FlagTypeTranslation> FlagTypeTranslations { get; set; } = new List<FlagTypeTranslation>();
        public ICollection<AttachmentMethodTranslation> AttachmentMethodTranslations { get; set; } = new List<AttachmentMethodTranslation>();
        public ICollection<MaterialTypeTranslation> MaterialTypeTranslations { get; set; } = new List<MaterialTypeTranslation>();
        public ICollection<WashingInstructionTranslation> WashingInstructionTranslations { get; set; } = new List<WashingInstructionTranslation>();
        public ICollection<FlagInstruction> FlagInstructions { get; set; } = new List<FlagInstruction>();
    }
}
