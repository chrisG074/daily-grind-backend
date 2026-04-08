using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.Linq;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get all products with translations
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string? languageCode = "nl", [FromQuery] int? flagTypeId = null)
        {
            var query = _context.Products
                .Include(p => p.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.FlagType)
                    .ThenInclude(ft => ft.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.MaterialType)
                    .ThenInclude(mt => mt.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.ProductAttachmentMethods)
                    .ThenInclude(pam => pam.AttachmentMethod)
                        .ThenInclude(am => am.Translations.Where(t => t.Language.Code == languageCode))
                .Where(p => p.IsActive);

            if (flagTypeId.HasValue)
            {
                query = query.Where(p => p.FlagTypeId == flagTypeId.Value);
            }

            var products = await query.ToListAsync();

            var result = products.Select(p => new
            {
                p.Id,
                p.Sku,
                Name = p.Translations.FirstOrDefault()?.Name ?? "No translation",
                Description = p.Translations.FirstOrDefault()?.Description,
                ShortDescription = p.Translations.FirstOrDefault()?.ShortDescription,
                p.BasePrice,
                p.StockQuantity,
                p.Image,
                p.Rating,
                p.ReviewCount,
                p.Badge,
                FlagType = new
                {
                    p.FlagType.Id,
                    p.FlagType.Code,
                    Name = p.FlagType.Translations.FirstOrDefault()?.Name ?? p.FlagType.Code
                },
                Material = p.MaterialType != null ? new
                {
                    p.MaterialType.Id,
                    p.MaterialType.Code,
                    Name = p.MaterialType.Translations.FirstOrDefault()?.Name ?? p.MaterialType.Specification,
                    p.MaterialType.Specification
                } : null,
                AttachmentMethods = p.ProductAttachmentMethods.Select(pam => new
                {
                    pam.AttachmentMethod.Id,
                    pam.AttachmentMethod.Code,
                    Name = pam.AttachmentMethod.Translations.FirstOrDefault()?.Name ?? pam.AttachmentMethod.Code
                }).ToList(),
                Seo = new
                {
                    p.Translations.FirstOrDefault()?.MetaTitle,
                    p.Translations.FirstOrDefault()?.MetaDescription,
                    p.Translations.FirstOrDefault()?.MetaKeywords,
                    p.Translations.FirstOrDefault()?.Slug
                }
            });

            return Ok(result);
        }

        /// <summary>
        /// Get product by ID with translations
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id, [FromQuery] string? languageCode = "nl")
        {
            var product = await _context.Products
                .Include(p => p.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.FlagType)
                    .ThenInclude(ft => ft.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.MaterialType)
                    .ThenInclude(mt => mt!.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.MaterialType)
                    .ThenInclude(mt => mt!.MaterialWashingInstructions)
                        .ThenInclude(mwi => mwi.WashingInstruction)
                            .ThenInclude(wi => wi.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.ProductAttachmentMethods)
                    .ThenInclude(pam => pam.AttachmentMethod)
                        .ThenInclude(am => am.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.BundleProducts)
                    .ThenInclude(bp => bp.BundledProduct)
                        .ThenInclude(bp => bp.Translations.Where(t => t.Language.Code == languageCode))
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null)
                return NotFound();

            var result = new
            {
                product.Id,
                product.Sku,
                Name = product.Translations.FirstOrDefault()?.Name ?? "No translation",
                Description = product.Translations.FirstOrDefault()?.Description,
                ShortDescription = product.Translations.FirstOrDefault()?.ShortDescription,
                product.BasePrice,
                product.StockQuantity,
                product.Image,
                product.Rating,
                product.ReviewCount,
                product.Badge,
                FlagType = new
                {
                    product.FlagType.Id,
                    product.FlagType.Code,
                    Name = product.FlagType.Translations.FirstOrDefault()?.Name ?? product.FlagType.Code,
                    Description = product.FlagType.Translations.FirstOrDefault()?.Description
                },
                Material = product.MaterialType != null ? new
                {
                    product.MaterialType.Id,
                    product.MaterialType.Code,
                    Name = product.MaterialType.Translations.FirstOrDefault()?.Name ?? product.MaterialType.Specification,
                    product.MaterialType.Specification,
                    product.MaterialType.PriceModifier,
                    WashingInstructions = product.MaterialType.MaterialWashingInstructions
                        .OrderBy(mwi => mwi.SortOrder)
                        .Select(mwi => new
                        {
                            mwi.WashingInstruction.Id,
                            mwi.WashingInstruction.Code,
                            Instruction = mwi.WashingInstruction.Translations.FirstOrDefault()?.Instruction,
                            mwi.WashingInstruction.IconUrl
                        }).ToList()
                } : null,
                AttachmentMethods = product.ProductAttachmentMethods.Select(pam => new
                {
                    pam.AttachmentMethod.Id,
                    pam.AttachmentMethod.Code,
                    Name = pam.AttachmentMethod.Translations.FirstOrDefault()?.Name ?? pam.AttachmentMethod.Code,
                    Description = pam.AttachmentMethod.Translations.FirstOrDefault()?.Description
                }).ToList(),
                Bundles = product.BundleProducts.Select(bp => new
                {
                    ProductId = bp.BundledProduct.Id,
                    bp.BundledProduct.Sku,
                    Name = bp.BundledProduct.Translations.FirstOrDefault()?.Name,
                    bp.Quantity,
                    bp.DiscountPercentage,
                    bp.BundledProduct.BasePrice
                }).ToList(),
                Seo = new
                {
                    product.Translations.FirstOrDefault()?.MetaTitle,
                    product.Translations.FirstOrDefault()?.MetaDescription,
                    product.Translations.FirstOrDefault()?.MetaKeywords,
                    product.Translations.FirstOrDefault()?.Slug
                }
            };

            return Ok(result);
        }

        /// <summary>
        /// Get filter options for product filtering
        /// </summary>
        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters([FromQuery] string? languageCode = "nl")
        {
            var flagTypes = await _context.FlagTypes
                .Include(ft => ft.Translations.Where(t => t.Language.Code == languageCode))
                .OrderBy(ft => ft.SortOrder)
                .Select(ft => new
                {
                    ft.Id,
                    ft.Code,
                    Name = ft.Translations.FirstOrDefault()!.Name ?? ft.Code
                })
                .ToListAsync();

            var materials = await _context.MaterialTypes
                .Include(mt => mt.Translations.Where(t => t.Language.Code == languageCode))
                .Select(mt => new
                {
                    mt.Id,
                    mt.Code,
                    Name = mt.Translations.FirstOrDefault()!.Name ?? mt.Specification,
                    mt.Specification
                })
                .ToListAsync();

            var attachmentMethods = await _context.AttachmentMethods
                .Include(am => am.Translations.Where(t => t.Language.Code == languageCode))
                .OrderBy(am => am.SortOrder)
                .Select(am => new
                {
                    am.Id,
                    am.Code,
                    Name = am.Translations.FirstOrDefault()!.Name ?? am.Code
                })
                .ToListAsync();

            return Ok(new
            {
                FlagTypes = flagTypes,
                Materials = materials,
                AttachmentMethods = attachmentMethods
            });
        }

        /// <summary>
        /// Filter products by attachment method and/or material
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> FilterProducts(
            [FromQuery] string? languageCode = "nl",
            [FromQuery] int? flagTypeId = null,
            [FromQuery] int? materialTypeId = null,
            [FromQuery] int? attachmentMethodId = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            var query = _context.Products
                .Include(p => p.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.FlagType)
                    .ThenInclude(ft => ft.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.MaterialType)
                    .ThenInclude(mt => mt.Translations.Where(t => t.Language.Code == languageCode))
                .Include(p => p.ProductAttachmentMethods)
                    .ThenInclude(pam => pam.AttachmentMethod)
                        .ThenInclude(am => am.Translations.Where(t => t.Language.Code == languageCode))
                .Where(p => p.IsActive);

            if (flagTypeId.HasValue)
            {
                query = query.Where(p => p.FlagTypeId == flagTypeId.Value);
            }

            if (materialTypeId.HasValue)
            {
                query = query.Where(p => p.MaterialTypeId == materialTypeId.Value);
            }

            if (attachmentMethodId.HasValue)
            {
                query = query.Where(p => p.ProductAttachmentMethods.Any(pam => pam.AttachmentMethodId == attachmentMethodId.Value));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= maxPrice.Value);
            }

            var products = await query.ToListAsync();

            var result = products.Select(p => new
            {
                p.Id,
                p.Sku,
                Name = p.Translations.FirstOrDefault()?.Name ?? "No translation",
                ShortDescription = p.Translations.FirstOrDefault()?.ShortDescription,
                p.BasePrice,
                p.StockQuantity,
                p.Image,
                p.Rating,
                p.ReviewCount,
                p.Badge,
                FlagType = new
                {
                    p.FlagType.Id,
                    Name = p.FlagType.Translations.FirstOrDefault()?.Name ?? p.FlagType.Code
                },
                Material = p.MaterialType != null ? new
                {
                    p.MaterialType.Id,
                    Name = p.MaterialType.Translations.FirstOrDefault()?.Name ?? p.MaterialType.Specification
                } : null,
                AttachmentMethods = p.ProductAttachmentMethods.Select(pam => new
                {
                    pam.AttachmentMethod.Id,
                    Name = pam.AttachmentMethod.Translations.FirstOrDefault()?.Name ?? pam.AttachmentMethod.Code
                }).ToList()
            });

            return Ok(result);
        }
    }
}
