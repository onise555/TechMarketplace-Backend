TechMarketplace Backend
🎓 STEP IT Academy - სადიპლომო პროექტი
Student: [თქვენი სახელი გვარი]
Course: Full Stack Development
Year: 2025
Institution: STEP IT Academy Georgia

📋 პროექტის მიზანი
ელექტრონული კომერციის პლატფორმის განვითარება ტექნიკური პროდუქტებისთვის, რომელიც დემონსტრირებს თანამედროვე web development ტექნოლოგიების გამოყენებას და programming best practices-ს.
ძირითადი ამოცანები:

🛒 პროდუქტების კატალოგის მართვის სისტემის შექმნა
👥 მომხმარებელთა მართვა role-based authorization-ით
📦 შეკვეთების სრული lifecycle-ის იმპლემენტაცია
🔍 მძლავრი საძიებო და ფილტრაციის სისტემა
📊 ადმინისტრაციული პანელი ანალიტიკით
🔐 მონაცემების უსაფრთხოება და validation

🛠️ ტექნოლოგიური სტეკი
Backend Technologies:

.NET 8 Web API (RESTful Architecture)
Entity Framework Core (Code First ORM)
SQL Server (რელაციური მონაცემთა ბაზა)
JWT Authentication (Token-based security)
AutoMapper (Object-Object mapping)
FluentValidation (Input validation)
Swagger/OpenAPI (API documentation)
Serilog (Structured logging)

Development Tools:

Visual Studio 2022
Git (Version control)
Postman (API testing)
SQL Server Management Studio

👥 მომხმარებელთა როლები
RolePermissionsAdminსრული წვდომა - მომხმარებლები, პროდუქტები, კატეგორიები, შეკვეთებიManagerპროდუქტების და შეკვეთების მართვა, საწყობის კონტროლიUserპროდუქტების ყიდვა, ანგარიშის მართვა, შეკვეთების ისტორია
📊 მონაცემთა არქიტექტურა
Core Domain Models
User System:
User (1:1) UserDetail
User (1:N) Address
User (1:N) Order
User (1:1) Cart
User (1:N) WishList

Product Catalog:
Brand (1:N) Product
Category (1:N) SubCategory (1:N) Product
Product (1:1) ProductDetail (1:N) ProductSpecification
ProductDetail (1:N) ProductImg

E-commerce Flow:
Cart (1:N) CartItem
Order (1:N) OrderItem (N:1) Product
Order (1:N) Payment
Product (1:N) Review
Key Entities:

User - Authentication, Profile, Roles
Product - Catalog item with specifications
Category/SubCategory - Hierarchical organization
Order - Purchase transaction with items
Cart - Session-based shopping cart
Brand - Product manufacturers
Review - Customer feedback system

🚀 API Architecture
RESTful Endpoints Structure:
🛍️ Product Management
httpGET    /api/products                    # პროდუქტების სია (pagination, filtering)
GET    /api/products/{id}               # პროდუქტის დეტალები
POST   /api/products                    # ახალი პროდუქტი [Admin/Manager]
PUT    /api/products/{id}               # პროდუქტის განახლება [Admin/Manager]
DELETE /api/products/{id}               # პროდუქტის წაშლა [Admin]
GET    /api/products/search?q={query}   # ძიება
GET    /api/products/category/{id}      # კატეგორიის პროდუქტები
GET    /api/products/featured           # რეკომენდებული პროდუქტები
POST   /api/products/{id}/images        # სურათების ატვირთვა
📂 Category Management
httpGET    /api/categories                  # კატეგორიების იერარქია
GET    /api/categories/{id}             # კატეგორიის დეტალები
POST   /api/categories                  # ახალი კატეგორია [Admin]
PUT    /api/categories/{id}             # კატეგორიის განახლება [Admin]
DELETE /api/categories/{id}             # კატეგორიის წაშლა [Admin]
GET    /api/subcategories               # ყველა ქვეკატეგორია
🛒 Shopping Experience
httpGET    /api/cart                        # კალათის შიგთავსი [User]
POST   /api/cart/items                  # პროდუქტის დამატება [User]
PUT    /api/cart/items/{id}             # რაოდენობის შეცვლა [User]
DELETE /api/cart/items/{id}             # კალათიდან წაშლა [User]
POST   /api/cart/checkout               # შეკვეთის განთავსება [User]
📦 Order Processing
httpGET    /api/orders                      # მომხმარებლის შეკვეთები [User]
GET    /api/orders/{id}                 # შეკვეთის დეტალები [User/Manager]
POST   /api/orders                      # ახალი შეკვეთა [User]
PUT    /api/orders/{id}/status          # სტატუსის განახლება [Manager]
GET    /api/orders/admin                # ყველა შეკვეთა [Admin/Manager]
POST   /api/orders/{id}/payment         # გადახდის დამუშავება [User]
👤 User Management
httpPOST   /api/auth/register               # რეგისტრაცია
POST   /api/auth/login                  # ავტორიზაცია
POST   /api/auth/refresh                # Token refresh
POST   /api/auth/logout                 # გასვლა
GET    /api/users/profile               # პროფილის ინფო [User]
PUT    /api/users/profile               # პროფილის განახლება [User]
GET    /api/users/orders                # შეკვეთების ისტორია [User]
🗄️ Database Setup
Migration Commands:
bash# Initial database creation
dotnet ef migrations add InitialCreate
dotnet ef database update

# Adding new migration
dotnet ef migrations add [MigrationName]
dotnet ef database update
Connection String:
json{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TechMarketplaceDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
🧪 Testing Strategy
Unit Testing:

Services Layer: Business logic testing
Controllers: API endpoint testing
Repositories: Data access testing
Validators: Input validation testing

Test Coverage:

Minimum 70% code coverage
Critical business logic 100% covered
All API endpoints tested

🔐 Security Implementation
Authentication & Authorization:
csharp[Authorize(Roles = "Admin,Manager")]
[Authorize(Roles = "User")]
[AllowAnonymous]
Security Features:

JWT Token expiration handling
Password strength validation
Rate limiting for API calls
SQL injection prevention
XSS protection

📧 SMTP Email Service

Order Confirmations
Password Reset
Account Verification
Stock Alerts (for admins)

🎯 Key Features Demonstrated
Technical Skills:

Clean Architecture principles
Repository Pattern implementation
Dependency Injection usage
Entity Relationships management
API Versioning support
Exception Handling centralized
Logging comprehensive

Business Logic:

Inventory Management (stock tracking)
Multi-level Categories (Category → SubCategory)
Shopping Cart persistence
Order State Machine (Pending → Paid → Shipped → Delivered)
Product Specifications dynamic system
Brand Management
Customer Reviews system

📁 Project Structure
TechMarketplace.API/
├── Controllers/
│   ├── AuthController.cs
│   ├── ProductsController.cs
│   ├── CategoriesController.cs
│   ├── OrdersController.cs
│   ├── CartController.cs
│   └── UsersController.cs
├── Models/
│   ├── User.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── Order.cs
│   └── [other entities]
├── Dtos/
│   ├── Request/
│   └── Response/
├── Services/
│   ├── Interfaces/
│   └── Implementation/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── Repositories/
│   └── Migrations/
├── Validators/
├── SMTP/
└── Program.cs
🚀 Development Phases
Phase 1: Core Setup ✅

Project initialization
Git repository setup
Basic API structure

Phase 2: Data Layer 🔄

Entity models creation
DbContext configuration
Database migrations

Phase 3: Business Logic

Services implementation
Repository pattern
Validation rules

Phase 4: API Layer

Controllers implementation
Authentication/Authorization
API documentation

Phase 5: Integration & Testing

Unit tests
Integration tests
Frontend integration

Phase 6: Deployment & Presentation

Production deployment
Documentation finalization
Presentation preparation
