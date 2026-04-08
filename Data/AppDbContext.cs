using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Product related
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<ProductBundle> ProductBundles { get; set; }
        public DbSet<FlagType> FlagTypes { get; set; }
        public DbSet<FlagTypeTranslation> FlagTypeTranslations { get; set; }
        
        // Attachment Methods
        public DbSet<AttachmentMethod> AttachmentMethods { get; set; }
        public DbSet<AttachmentMethodTranslation> AttachmentMethodTranslations { get; set; }
        public DbSet<ProductAttachmentMethod> ProductAttachmentMethods { get; set; }
        
        // Materials & Washing
        public DbSet<MaterialType> MaterialTypes { get; set; }
        public DbSet<MaterialTypeTranslation> MaterialTypeTranslations { get; set; }
        public DbSet<WashingInstruction> WashingInstructions { get; set; }
        public DbSet<WashingInstructionTranslation> WashingInstructionTranslations { get; set; }
        public DbSet<MaterialWashingInstruction> MaterialWashingInstructions { get; set; }
        
        // Languages & SEO
        public DbSet<Language> Languages { get; set; }
        public DbSet<FlagInstruction> FlagInstructions { get; set; }
        
        // Users & Security
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        
        // Orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<OrderStatusTranslation> OrderStatusTranslations { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        
        // Inventory
        public DbSet<RawMaterial> RawMaterials { get; set; }
        public DbSet<RawMaterialAlert> RawMaterialAlerts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== PRODUCT CONFIGURATION =====
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Sku).IsRequired().HasMaxLength(50);
                entity.Property(p => p.BasePrice).HasPrecision(18, 2);
                entity.HasIndex(p => p.Sku).IsUnique();
                
                entity.HasOne(p => p.FlagType)
                    .WithMany(ft => ft.Products)
                    .HasForeignKey(p => p.FlagTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(p => p.MaterialType)
                    .WithMany(mt => mt.Products)
                    .HasForeignKey(p => p.MaterialTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== PRODUCT TRANSLATIONS =====
            modelBuilder.Entity<ProductTranslation>(entity =>
            {
                entity.HasKey(pt => pt.Id);
                entity.Property(pt => pt.Name).IsRequired().HasMaxLength(200);
                entity.Property(pt => pt.Slug).HasMaxLength(250);
                
                entity.HasIndex(pt => new { pt.ProductId, pt.LanguageId }).IsUnique();
                entity.HasIndex(pt => new { pt.LanguageId, pt.Slug }).IsUnique();
                
                entity.HasOne(pt => pt.Product)
                    .WithMany(p => p.Translations)
                    .HasForeignKey(pt => pt.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(pt => pt.Language)
                    .WithMany(l => l.ProductTranslations)
                    .HasForeignKey(pt => pt.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== PRODUCT BUNDLES (Self-referencing N-to-M) =====
            modelBuilder.Entity<ProductBundle>(entity =>
            {
                entity.HasKey(pb => new { pb.MainProductId, pb.BundledProductId });
                entity.Property(pb => pb.DiscountPercentage).HasPrecision(5, 2);
                
                entity.HasOne(pb => pb.MainProduct)
                    .WithMany(p => p.BundleProducts)
                    .HasForeignKey(pb => pb.MainProductId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(pb => pb.BundledProduct)
                    .WithMany(p => p.BundledInProducts)
                    .HasForeignKey(pb => pb.BundledProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== FLAG TYPE =====
            modelBuilder.Entity<FlagType>(entity =>
            {
                entity.HasKey(ft => ft.Id);
                entity.Property(ft => ft.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(ft => ft.Code).IsUnique();
            });

            modelBuilder.Entity<FlagTypeTranslation>(entity =>
            {
                entity.HasKey(ftt => ftt.Id);
                entity.HasIndex(ftt => new { ftt.FlagTypeId, ftt.LanguageId }).IsUnique();
                
                entity.HasOne(ftt => ftt.FlagType)
                    .WithMany(ft => ft.Translations)
                    .HasForeignKey(ftt => ftt.FlagTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ftt => ftt.Language)
                    .WithMany(l => l.FlagTypeTranslations)
                    .HasForeignKey(ftt => ftt.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== ATTACHMENT METHODS =====
            modelBuilder.Entity<AttachmentMethod>(entity =>
            {
                entity.HasKey(am => am.Id);
                entity.Property(am => am.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(am => am.Code).IsUnique();
            });

            modelBuilder.Entity<AttachmentMethodTranslation>(entity =>
            {
                entity.HasKey(amt => amt.Id);
                entity.HasIndex(amt => new { amt.AttachmentMethodId, amt.LanguageId }).IsUnique();
                
                entity.HasOne(amt => amt.AttachmentMethod)
                    .WithMany(am => am.Translations)
                    .HasForeignKey(amt => amt.AttachmentMethodId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(amt => amt.Language)
                    .WithMany(l => l.AttachmentMethodTranslations)
                    .HasForeignKey(amt => amt.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== PRODUCT ATTACHMENT METHODS (N-to-M) =====
            modelBuilder.Entity<ProductAttachmentMethod>(entity =>
            {
                entity.HasKey(pam => new { pam.ProductId, pam.AttachmentMethodId });
                
                entity.HasOne(pam => pam.Product)
                    .WithMany(p => p.ProductAttachmentMethods)
                    .HasForeignKey(pam => pam.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(pam => pam.AttachmentMethod)
                    .WithMany(am => am.ProductAttachmentMethods)
                    .HasForeignKey(pam => pam.AttachmentMethodId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== MATERIAL TYPES =====
            modelBuilder.Entity<MaterialType>(entity =>
            {
                entity.HasKey(mt => mt.Id);
                entity.Property(mt => mt.Code).IsRequired().HasMaxLength(50);
                entity.Property(mt => mt.Specification).IsRequired().HasMaxLength(200);
                entity.Property(mt => mt.PriceModifier).HasPrecision(18, 2);
                entity.HasIndex(mt => mt.Code).IsUnique();
            });

            modelBuilder.Entity<MaterialTypeTranslation>(entity =>
            {
                entity.HasKey(mtt => mtt.Id);
                entity.HasIndex(mtt => new { mtt.MaterialTypeId, mtt.LanguageId }).IsUnique();
                
                entity.HasOne(mtt => mtt.MaterialType)
                    .WithMany(mt => mt.Translations)
                    .HasForeignKey(mtt => mtt.MaterialTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(mtt => mtt.Language)
                    .WithMany(l => l.MaterialTypeTranslations)
                    .HasForeignKey(mtt => mtt.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== WASHING INSTRUCTIONS =====
            modelBuilder.Entity<WashingInstruction>(entity =>
            {
                entity.HasKey(wi => wi.Id);
                entity.Property(wi => wi.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(wi => wi.Code).IsUnique();
            });

            modelBuilder.Entity<WashingInstructionTranslation>(entity =>
            {
                entity.HasKey(wit => wit.Id);
                entity.HasIndex(wit => new { wit.WashingInstructionId, wit.LanguageId }).IsUnique();
                
                entity.HasOne(wit => wit.WashingInstruction)
                    .WithMany(wi => wi.Translations)
                    .HasForeignKey(wit => wit.WashingInstructionId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(wit => wit.Language)
                    .WithMany(l => l.WashingInstructionTranslations)
                    .HasForeignKey(wit => wit.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== MATERIAL WASHING INSTRUCTIONS (N-to-M) =====
            modelBuilder.Entity<MaterialWashingInstruction>(entity =>
            {
                entity.HasKey(mwi => new { mwi.MaterialTypeId, mwi.WashingInstructionId });
                
                entity.HasOne(mwi => mwi.MaterialType)
                    .WithMany(mt => mt.MaterialWashingInstructions)
                    .HasForeignKey(mwi => mwi.MaterialTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(mwi => mwi.WashingInstruction)
                    .WithMany(wi => wi.MaterialWashingInstructions)
                    .HasForeignKey(mwi => mwi.WashingInstructionId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== LANGUAGES =====
            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.Code).IsRequired().HasMaxLength(10);
                entity.Property(l => l.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(l => l.Code).IsUnique();
            });

            // ===== FLAG INSTRUCTIONS =====
            modelBuilder.Entity<FlagInstruction>(entity =>
            {
                entity.HasKey(fi => fi.Id);
                entity.Property(fi => fi.CountryCode).IsRequired().HasMaxLength(2);
                entity.HasIndex(fi => new { fi.CountryCode, fi.LanguageId }).IsUnique();
                
                entity.HasOne(fi => fi.Language)
                    .WithMany(l => l.FlagInstructions)
                    .HasForeignKey(fi => fi.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== USERS =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // ===== ROLES =====
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Code).IsRequired().HasMaxLength(50);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(r => r.Code).IsUnique();
            });

            // ===== USER ROLES (N-to-M for RBAC) =====
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                
                entity.HasOne(ur => ur.User)
                    .WithMany(u => u.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== ORDERS =====
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
                entity.Property(o => o.TotalAmount).HasPrecision(18, 2);
                entity.HasIndex(o => o.OrderNumber).IsUnique();
                
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
                
                entity.HasOne(o => o.Language)
                    .WithMany()
                    .HasForeignKey(o => o.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== ORDER ITEMS =====
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.UnitPrice).HasPrecision(18, 2);
                entity.Property(oi => oi.Subtotal).HasPrecision(18, 2);
                
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(oi => oi.Product)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(oi => oi.ProductId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== ORDER STATUS =====
            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(os => os.Id);
                entity.Property(os => os.Code).IsRequired().HasMaxLength(50);
                entity.HasIndex(os => os.Code).IsUnique();
            });

            modelBuilder.Entity<OrderStatusTranslation>(entity =>
            {
                entity.HasKey(ost => ost.Id);
                entity.HasIndex(ost => new { ost.OrderStatusId, ost.LanguageId }).IsUnique();
                
                entity.HasOne(ost => ost.OrderStatus)
                    .WithMany(os => os.Translations)
                    .HasForeignKey(ost => ost.OrderStatusId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(ost => ost.Language)
                    .WithMany()
                    .HasForeignKey(ost => ost.LanguageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== ORDER STATUS HISTORY =====
            modelBuilder.Entity<OrderStatusHistory>(entity =>
            {
                entity.HasKey(osh => osh.Id);
                
                entity.HasOne(osh => osh.Order)
                    .WithMany(o => o.StatusHistory)
                    .HasForeignKey(osh => osh.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasOne(osh => osh.OrderStatus)
                    .WithMany(os => os.StatusHistories)
                    .HasForeignKey(osh => osh.OrderStatusId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== RAW MATERIALS =====
            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.HasKey(rm => rm.Id);
                entity.Property(rm => rm.Name).IsRequired().HasMaxLength(200);
                entity.Property(rm => rm.CurrentStock).HasPrecision(18, 2);
                entity.Property(rm => rm.ThresholdLevel).HasPrecision(18, 2);
                entity.Property(rm => rm.CostPerUnit).HasPrecision(18, 2);
                entity.Property(rm => rm.Unit).HasMaxLength(20);
                
                entity.HasOne(rm => rm.MaterialType)
                    .WithMany(mt => mt.RawMaterials)
                    .HasForeignKey(rm => rm.MaterialTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== RAW MATERIAL ALERTS =====
            modelBuilder.Entity<RawMaterialAlert>(entity =>
            {
                entity.HasKey(rma => rma.Id);
                entity.Property(rma => rma.AlertType).IsRequired().HasMaxLength(50);
                
                entity.HasOne(rma => rma.RawMaterial)
                    .WithMany(rm => rm.Alerts)
                    .HasForeignKey(rma => rma.RawMaterialId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}