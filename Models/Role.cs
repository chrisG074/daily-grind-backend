namespace backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // e.g., "ADMIN", "CONTENT_MANAGER", "INVENTORY_MANAGER", "CUSTOMER_SERVICE"
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Permissions { get; set; } = string.Empty; // JSON string of permissions

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

    // Junction table for User and Role (N-to-M for RBAC)
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; } = null!;
        public Role Role { get; set; } = null!;
    }
}
