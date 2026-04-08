# Daily Grind Backend - Multilingual E-Commerce Database Schema

## Overview
This backend implements a comprehensive multilingual database schema for an e-commerce platform selling flags, flag poles, and related accessories.

## Key Features

### 1. Products & Properties (1-to-N and N-to-M Relationships)

#### Product Entity
- Main table for all items (flags, poles, etc.)
- Each product has a unique SKU
- Linked to FlagType, MaterialType
- Supports multiple translations per language
- Stock management with quantity tracking

#### FlagType
- Categories: Country flags, Signal flags, Company flags
- Each type has translations in multiple languages
- 1-to-N relationship with Products

#### AttachmentMethod
- Methods: Cord/Loop, Hooks, Tunnel
- N-to-M relationship with Products (via ProductAttachmentMethod junction table)
- Allows customers to filter products by attachment method

#### MaterialType
- Materials like "110g/m² Gloss Poly" or "Spunpolyester"
- Price modifier for material upgrades
- 1-to-N relationship with Products
- N-to-M relationship with WashingInstructions

#### ProductBundle (Self-Referencing)
- N-to-M self-relationship for product bundles
- Allows bundling of mounting materials with flag poles
- Supports quantity and discount percentage

### 2. Multilingual & SEO (1-to-N Relationships)

#### Language
- Supported languages: Dutch (nl), English (en), German (de), French (fr)
- Each language can be set as default
- Active/inactive status per language

#### ProductTranslation
- Stores translated product names, descriptions
- SEO metadata: MetaTitle, MetaDescription, MetaKeywords, Slug
- Unique slug per language for SEO-friendly URLs
- Foreign keys to Product and Language

#### FlagInstruction
- Official flag instructions per country
- Multilingual support
- Includes title, instructions, and optional image URL

### 3. Users & Security (N-to-M for RBAC)

#### User
- Employee and customer accounts
- Email-based authentication
- First name, last name, phone
- Last login tracking

#### Role
- Predefined roles:
  - **ADMIN**: Full system access
  - **CONTENT_MANAGER**: Manage products and content
  - **INVENTORY_MANAGER**: Manage stock and raw materials
  - **CUSTOMER_SERVICE**: Manage orders and customers
- JSON-based permissions system
- Easily extensible for custom roles

#### UserRole (Junction Table)
- N-to-M relationship for RBAC (Role-Based Access Control)
- Tracks when roles were assigned
- Flexible: users can have multiple roles

### 4. Orders & Customers (1-to-N Relationships)

#### Order
- Linked to User (customer)
- Language field: determines communication language
- Order number, total amount
- Shipping and billing addresses
- Order date, shipped date, delivered date

#### OrderItem
- Line items for each order
- Links to Product
- Quantity, unit price, subtotal

#### OrderStatus & OrderStatusHistory
- Track & Trace functionality
- Status codes: PENDING, PROCESSING, SHIPPED, DELIVERED, CANCELLED
- **OrderStatusTranslation**: Multilingual status names and customer messages
- Status updates are sent to customers in their preferred language
- Tracking number support

### 5. Inventory & Raw Materials (1-to-1 or 1-to-N)

#### RawMaterial
- Materials like rolls of polyester fabric
- Tracks current stock and threshold level
- Unit of measurement (m², kg, rolls)
- Cost per unit
- Links to MaterialType

#### RawMaterialAlert
- Automatic alerts when stock falls below threshold
- Alert types: LOW_STOCK, OUT_OF_STOCK
- Resolution tracking

## Database Triggers/Alerts
The system can be extended with database triggers to:
1. Automatically create RawMaterialAlert when stock < threshold
2. Send notifications to inventory managers
3. Track stock movements

## API Endpoints

### Products
- `GET /api/product?languageCode=nl&flagTypeId=1` - Get all products with translations
- `GET /api/product/{id}?languageCode=nl` - Get single product with full details (bundles, washing instructions)
- `GET /api/product/filters?languageCode=nl` - Get all filter options (flag types, materials, attachment methods)
- `GET /api/product/filter?languageCode=nl&flagTypeId=1&materialTypeId=2&attachmentMethodId=1&minPrice=10&maxPrice=50` - Filter products

### Query Parameters
- `languageCode`: Language for translations (default: "nl")
- `flagTypeId`: Filter by flag type
- `materialTypeId`: Filter by material
- `attachmentMethodId`: Filter by attachment method
- `minPrice` / `maxPrice`: Price range filtering

## Running Migrations

### Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

### Update database:
```bash
dotnet ef database update
```

### Drop database (careful!):
```bash
dotnet ef database drop --force
```

## Database Seeding
The application automatically seeds the database with:
- 4 Languages (Dutch, English, German, French)
- 4 Roles (Admin, Content Manager, Inventory Manager, Customer Service)
- 3 Flag Types with translations
- 3 Attachment Methods with translations
- 3 Material Types with translations
- 4 Washing Instructions with translations
- 5 Order Statuses with translations
- Sample flag instructions for Dutch flag

## Entity Relationships Summary

```
Product (1) ←→ (N) ProductTranslation
Product (N) ←→ (1) FlagType (1) ←→ (N) FlagTypeTranslation
Product (N) ←→ (1) MaterialType (1) ←→ (N) MaterialTypeTranslation
Product (N) ←→ (M) AttachmentMethod (ProductAttachmentMethod junction)
Product (N) ←→ (M) Product (ProductBundle - self-referencing)
MaterialType (N) ←→ (M) WashingInstruction (MaterialWashingInstruction junction)
User (N) ←→ (M) Role (UserRole junction - RBAC)
Order (N) ←→ (1) User
Order (N) ←→ (1) Language
Order (1) ←→ (N) OrderItem (N) ←→ (1) Product
Order (1) ←→ (N) OrderStatusHistory (N) ←→ (1) OrderStatus
RawMaterial (N) ←→ (1) MaterialType
RawMaterial (1) ←→ (N) RawMaterialAlert
```

## Next Steps

1. **Authentication**: Implement JWT-based authentication
2. **Authorization**: Complete RBAC implementation in middleware
3. **Order Management**: Create OrderController with status updates
4. **Inventory Alerts**: Implement database triggers or background jobs for stock alerts
5. **Admin Panel**: Create controllers for managing products, translations, users
6. **Image Upload**: Implement image storage (Azure Blob, AWS S3, or local)
7. **Search**: Add full-text search for products
8. **Caching**: Implement Redis caching for frequently accessed data

## Technologies Used
- **.NET 10**: Latest .NET framework
- **Entity Framework Core 10**: ORM for database operations
- **SQL Server**: Database (LocalDB for development)
- **ASP.NET Core**: Web API framework
