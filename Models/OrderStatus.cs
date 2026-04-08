namespace backend.Models
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "PENDING", "PROCESSING", "SHIPPED", "DELIVERED"
        public int SortOrder { get; set; }

        // Navigation properties
        public ICollection<OrderStatusTranslation> Translations { get; set; } = new List<OrderStatusTranslation>();
        public ICollection<OrderStatusHistory> StatusHistories { get; set; } = new List<OrderStatusHistory>();
    }

    public class OrderStatusTranslation
    {
        public int Id { get; set; }
        public int OrderStatusId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? CustomerMessage { get; set; } // Message to display to customer in their language

        // Navigation properties
        public OrderStatus OrderStatus { get; set; } = null!;
        public Language Language { get; set; } = null!;
    }

    public class OrderStatusHistory
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public string? Notes { get; set; }
        public DateTime StatusDate { get; set; } = DateTime.UtcNow;
        public string? TrackingNumber { get; set; }

        // Navigation properties
        public Order Order { get; set; } = null!;
        public OrderStatus OrderStatus { get; set; } = null!;
    }
}
