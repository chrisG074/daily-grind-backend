using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Seed Languages
            if (!await context.Languages.AnyAsync())
            {
                var languages = new[]
                {
                    new Language { Code = "nl", Name = "Nederlands", IsDefault = true, IsActive = true },
                    new Language { Code = "en", Name = "English", IsDefault = false, IsActive = true },
                    new Language { Code = "de", Name = "Deutsch", IsDefault = false, IsActive = true },
                    new Language { Code = "fr", Name = "Français", IsDefault = false, IsActive = true }
                };
                await context.Languages.AddRangeAsync(languages);
                await context.SaveChangesAsync();
            }

            var nlLang = await context.Languages.FirstAsync(l => l.Code == "nl");
            var enLang = await context.Languages.FirstAsync(l => l.Code == "en");

            // Seed Roles
            if (!await context.Roles.AnyAsync())
            {
                var roles = new[]
                {
                    new Role { Code = "ADMIN", Name = "Administrator", Description = "Full system access", Permissions = "{\"all\":true}" },
                    new Role { Code = "CONTENT_MANAGER", Name = "Content Manager", Description = "Manage products and content", Permissions = "{\"products\":true,\"content\":true}" },
                    new Role { Code = "INVENTORY_MANAGER", Name = "Inventory Manager", Description = "Manage stock and raw materials", Permissions = "{\"inventory\":true}" },
                    new Role { Code = "CUSTOMER_SERVICE", Name = "Customer Service", Description = "Manage orders and customers", Permissions = "{\"orders\":true,\"customers\":true}" }
                };
                await context.Roles.AddRangeAsync(roles);
                await context.SaveChangesAsync();
            }

            // Seed Flag Types
            if (!await context.FlagTypes.AnyAsync())
            {
                var flagTypes = new[]
                {
                    new FlagType { Code = "COUNTRY", SortOrder = 1 },
                    new FlagType { Code = "SIGNAL", SortOrder = 2 },
                    new FlagType { Code = "COMPANY", SortOrder = 3 }
                };
                await context.FlagTypes.AddRangeAsync(flagTypes);
                await context.SaveChangesAsync();

                // Add translations
                var countryType = await context.FlagTypes.FirstAsync(ft => ft.Code == "COUNTRY");
                var signalType = await context.FlagTypes.FirstAsync(ft => ft.Code == "SIGNAL");
                var companyType = await context.FlagTypes.FirstAsync(ft => ft.Code == "COMPANY");

                var flagTypeTranslations = new[]
                {
                    new FlagTypeTranslation { FlagTypeId = countryType.Id, LanguageId = nlLang.Id, Name = "Landenvlaggen", Description = "Vlaggen van landen wereldwijd" },
                    new FlagTypeTranslation { FlagTypeId = countryType.Id, LanguageId = enLang.Id, Name = "Country Flags", Description = "Flags of countries worldwide" },
                    new FlagTypeTranslation { FlagTypeId = signalType.Id, LanguageId = nlLang.Id, Name = "Seinvlaggen", Description = "Maritieme seinvlaggen" },
                    new FlagTypeTranslation { FlagTypeId = signalType.Id, LanguageId = enLang.Id, Name = "Signal Flags", Description = "Maritime signal flags" },
                    new FlagTypeTranslation { FlagTypeId = companyType.Id, LanguageId = nlLang.Id, Name = "Bedrijfsvlaggen", Description = "Vlaggen voor bedrijven" },
                    new FlagTypeTranslation { FlagTypeId = companyType.Id, LanguageId = enLang.Id, Name = "Company Flags", Description = "Flags for businesses" }
                };
                await context.FlagTypeTranslations.AddRangeAsync(flagTypeTranslations);
                await context.SaveChangesAsync();
            }

            // Seed Attachment Methods
            if (!await context.AttachmentMethods.AnyAsync())
            {
                var attachmentMethods = new[]
                {
                    new AttachmentMethod { Code = "CORD_LOOP", SortOrder = 1 },
                    new AttachmentMethod { Code = "HOOKS", SortOrder = 2 },
                    new AttachmentMethod { Code = "TUNNEL", SortOrder = 3 }
                };
                await context.AttachmentMethods.AddRangeAsync(attachmentMethods);
                await context.SaveChangesAsync();

                // Add translations
                var cordLoop = await context.AttachmentMethods.FirstAsync(am => am.Code == "CORD_LOOP");
                var hooks = await context.AttachmentMethods.FirstAsync(am => am.Code == "HOOKS");
                var tunnel = await context.AttachmentMethods.FirstAsync(am => am.Code == "TUNNEL");

                var attachmentTranslations = new[]
                {
                    new AttachmentMethodTranslation { AttachmentMethodId = cordLoop.Id, LanguageId = nlLang.Id, Name = "Koord/Lus", Description = "Bevestiging met koord en lus" },
                    new AttachmentMethodTranslation { AttachmentMethodId = cordLoop.Id, LanguageId = enLang.Id, Name = "Cord/Loop", Description = "Attachment with cord and loop" },
                    new AttachmentMethodTranslation { AttachmentMethodId = hooks.Id, LanguageId = nlLang.Id, Name = "Haken", Description = "Bevestiging met haken" },
                    new AttachmentMethodTranslation { AttachmentMethodId = hooks.Id, LanguageId = enLang.Id, Name = "Hooks", Description = "Attachment with hooks" },
                    new AttachmentMethodTranslation { AttachmentMethodId = tunnel.Id, LanguageId = nlLang.Id, Name = "Tunnel", Description = "Bevestiging met tunnel" },
                    new AttachmentMethodTranslation { AttachmentMethodId = tunnel.Id, LanguageId = enLang.Id, Name = "Tunnel", Description = "Attachment with tunnel" }
                };
                await context.AttachmentMethodTranslations.AddRangeAsync(attachmentTranslations);
                await context.SaveChangesAsync();
            }

            // Seed Material Types
            if (!await context.MaterialTypes.AnyAsync())
            {
                var materialTypes = new[]
                {
                    new MaterialType { Code = "POLY_110", Specification = "110g/m² Gloss Poly", PriceModifier = 1.0m },
                    new MaterialType { Code = "SPUNPOLYESTER", Specification = "Spunpolyester", PriceModifier = 1.2m },
                    new MaterialType { Code = "KNITTED_POLY", Specification = "Knitted Polyester", PriceModifier = 1.5m }
                };
                await context.MaterialTypes.AddRangeAsync(materialTypes);
                await context.SaveChangesAsync();

                // Add translations
                var poly110 = await context.MaterialTypes.FirstAsync(mt => mt.Code == "POLY_110");
                var spunpoly = await context.MaterialTypes.FirstAsync(mt => mt.Code == "SPUNPOLYESTER");
                var knitted = await context.MaterialTypes.FirstAsync(mt => mt.Code == "KNITTED_POLY");

                var materialTranslations = new[]
                {
                    new MaterialTypeTranslation { MaterialTypeId = poly110.Id, LanguageId = nlLang.Id, Name = "Glans Polyester 110g/m²", Description = "Lichtgewicht glanzend polyester" },
                    new MaterialTypeTranslation { MaterialTypeId = poly110.Id, LanguageId = enLang.Id, Name = "Gloss Polyester 110g/m²", Description = "Lightweight glossy polyester" },
                    new MaterialTypeTranslation { MaterialTypeId = spunpoly.Id, LanguageId = nlLang.Id, Name = "Spunpolyester", Description = "Duurzaam en UV-bestendig materiaal" },
                    new MaterialTypeTranslation { MaterialTypeId = spunpoly.Id, LanguageId = enLang.Id, Name = "Spunpolyester", Description = "Durable and UV-resistant material" },
                    new MaterialTypeTranslation { MaterialTypeId = knitted.Id, LanguageId = nlLang.Id, Name = "Gebreide Polyester", Description = "Hoogwaardige gebreide kwaliteit" },
                    new MaterialTypeTranslation { MaterialTypeId = knitted.Id, LanguageId = enLang.Id, Name = "Knitted Polyester", Description = "High-quality knitted fabric" }
                };
                await context.MaterialTypeTranslations.AddRangeAsync(materialTranslations);
                await context.SaveChangesAsync();
            }

            // Seed Washing Instructions
            if (!await context.WashingInstructions.AnyAsync())
            {
                var washingInstructions = new[]
                {
                    new WashingInstruction { Code = "MACHINE_30", IconUrl = "/icons/wash-30.svg" },
                    new WashingInstruction { Code = "HAND_WASH", IconUrl = "/icons/hand-wash.svg" },
                    new WashingInstruction { Code = "NO_BLEACH", IconUrl = "/icons/no-bleach.svg" },
                    new WashingInstruction { Code = "AIR_DRY", IconUrl = "/icons/air-dry.svg" }
                };
                await context.WashingInstructions.AddRangeAsync(washingInstructions);
                await context.SaveChangesAsync();

                // Add translations
                var machine30 = await context.WashingInstructions.FirstAsync(wi => wi.Code == "MACHINE_30");
                var handWash = await context.WashingInstructions.FirstAsync(wi => wi.Code == "HAND_WASH");
                var noBleach = await context.WashingInstructions.FirstAsync(wi => wi.Code == "NO_BLEACH");
                var airDry = await context.WashingInstructions.FirstAsync(wi => wi.Code == "AIR_DRY");

                var washingTranslations = new[]
                {
                    new WashingInstructionTranslation { WashingInstructionId = machine30.Id, LanguageId = nlLang.Id, Instruction = "Machinewasbaar op 30°C" },
                    new WashingInstructionTranslation { WashingInstructionId = machine30.Id, LanguageId = enLang.Id, Instruction = "Machine washable at 30°C" },
                    new WashingInstructionTranslation { WashingInstructionId = handWash.Id, LanguageId = nlLang.Id, Instruction = "Handwas aanbevolen" },
                    new WashingInstructionTranslation { WashingInstructionId = handWash.Id, LanguageId = enLang.Id, Instruction = "Hand wash recommended" },
                    new WashingInstructionTranslation { WashingInstructionId = noBleach.Id, LanguageId = nlLang.Id, Instruction = "Niet bleken" },
                    new WashingInstructionTranslation { WashingInstructionId = noBleach.Id, LanguageId = enLang.Id, Instruction = "Do not bleach" },
                    new WashingInstructionTranslation { WashingInstructionId = airDry.Id, LanguageId = nlLang.Id, Instruction = "Aan de lucht drogen" },
                    new WashingInstructionTranslation { WashingInstructionId = airDry.Id, LanguageId = enLang.Id, Instruction = "Air dry" }
                };
                await context.WashingInstructionTranslations.AddRangeAsync(washingTranslations);
                await context.SaveChangesAsync();

                // Link washing instructions to materials
                var poly110 = await context.MaterialTypes.FirstAsync(mt => mt.Code == "POLY_110");
                var materialWashingInstructions = new[]
                {
                    new MaterialWashingInstruction { MaterialTypeId = poly110.Id, WashingInstructionId = machine30.Id, SortOrder = 1 },
                    new MaterialWashingInstruction { MaterialTypeId = poly110.Id, WashingInstructionId = noBleach.Id, SortOrder = 2 },
                    new MaterialWashingInstruction { MaterialTypeId = poly110.Id, WashingInstructionId = airDry.Id, SortOrder = 3 }
                };
                await context.MaterialWashingInstructions.AddRangeAsync(materialWashingInstructions);
                await context.SaveChangesAsync();
            }

            // Seed Order Statuses
            if (!await context.OrderStatuses.AnyAsync())
            {
                var orderStatuses = new[]
                {
                    new OrderStatus { Code = "PENDING", SortOrder = 1 },
                    new OrderStatus { Code = "PROCESSING", SortOrder = 2 },
                    new OrderStatus { Code = "SHIPPED", SortOrder = 3 },
                    new OrderStatus { Code = "DELIVERED", SortOrder = 4 },
                    new OrderStatus { Code = "CANCELLED", SortOrder = 5 }
                };
                await context.OrderStatuses.AddRangeAsync(orderStatuses);
                await context.SaveChangesAsync();

                // Add translations
                var pending = await context.OrderStatuses.FirstAsync(os => os.Code == "PENDING");
                var processing = await context.OrderStatuses.FirstAsync(os => os.Code == "PROCESSING");
                var shipped = await context.OrderStatuses.FirstAsync(os => os.Code == "SHIPPED");
                var delivered = await context.OrderStatuses.FirstAsync(os => os.Code == "DELIVERED");
                var cancelled = await context.OrderStatuses.FirstAsync(os => os.Code == "CANCELLED");

                var statusTranslations = new[]
                {
                    new OrderStatusTranslation { OrderStatusId = pending.Id, LanguageId = nlLang.Id, Name = "In behandeling", Description = "Bestelling wordt verwerkt", CustomerMessage = "We hebben uw bestelling ontvangen en verwerken deze." },
                    new OrderStatusTranslation { OrderStatusId = pending.Id, LanguageId = enLang.Id, Name = "Pending", Description = "Order is being processed", CustomerMessage = "We have received your order and are processing it." },
                    new OrderStatusTranslation { OrderStatusId = processing.Id, LanguageId = nlLang.Id, Name = "Wordt verwerkt", Description = "Bestelling wordt ingepakt", CustomerMessage = "Uw bestelling wordt ingepakt en klaargemaakt voor verzending." },
                    new OrderStatusTranslation { OrderStatusId = processing.Id, LanguageId = enLang.Id, Name = "Processing", Description = "Order is being packed", CustomerMessage = "Your order is being packed and prepared for shipment." },
                    new OrderStatusTranslation { OrderStatusId = shipped.Id, LanguageId = nlLang.Id, Name = "Verzonden", Description = "Bestelling is verzonden", CustomerMessage = "Uw bestelling is verzonden! Track uw pakket met het opgegeven trackingnummer." },
                    new OrderStatusTranslation { OrderStatusId = shipped.Id, LanguageId = enLang.Id, Name = "Shipped", Description = "Order has been shipped", CustomerMessage = "Your order has been shipped! Track your package with the provided tracking number." },
                    new OrderStatusTranslation { OrderStatusId = delivered.Id, LanguageId = nlLang.Id, Name = "Geleverd", Description = "Bestelling is afgeleverd", CustomerMessage = "Uw bestelling is succesvol afgeleverd. Bedankt voor uw bestelling!" },
                    new OrderStatusTranslation { OrderStatusId = delivered.Id, LanguageId = enLang.Id, Name = "Delivered", Description = "Order has been delivered", CustomerMessage = "Your order has been successfully delivered. Thank you for your order!" },
                    new OrderStatusTranslation { OrderStatusId = cancelled.Id, LanguageId = nlLang.Id, Name = "Geannuleerd", Description = "Bestelling is geannuleerd", CustomerMessage = "Uw bestelling is geannuleerd." },
                    new OrderStatusTranslation { OrderStatusId = cancelled.Id, LanguageId = enLang.Id, Name = "Cancelled", Description = "Order has been cancelled", CustomerMessage = "Your order has been cancelled." }
                };
                await context.OrderStatusTranslations.AddRangeAsync(statusTranslations);
                await context.SaveChangesAsync();
            }

            // Seed sample Flag Instructions
            if (!await context.FlagInstructions.AnyAsync())
            {
                var flagInstructions = new[]
                {
                    new FlagInstruction 
                    { 
                        CountryCode = "NL", 
                        LanguageId = nlLang.Id, 
                        Title = "Nederlandse Vlag Instructies", 
                        Instructions = "De Nederlandse vlag bestaat uit drie horizontale banen in rood, wit en blauw. Deze moet altijd met het rood bovenaan worden gehangen." 
                    },
                    new FlagInstruction 
                    { 
                        CountryCode = "NL", 
                        LanguageId = enLang.Id, 
                        Title = "Dutch Flag Instructions", 
                        Instructions = "The Dutch flag consists of three horizontal stripes in red, white and blue. It should always be hung with red at the top." 
                    }
                };
                await context.FlagInstructions.AddRangeAsync(flagInstructions);
                await context.SaveChangesAsync();
            }
        }
    }
}
