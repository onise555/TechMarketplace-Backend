# TechMarketplace Backend (E-commerce API)

**STEP IT Academy Diploma Project (2025)**

**Student:** Onise Tsotskhalashvili
**Course:** Full Stack Development

---

## 🎯 Project Scope

Development of a comprehensive, RESTful Backend API for a technical product e-commerce platform using .NET 8. The architecture is designed to handle complex business logic, authorization, and inventory management.

### Core Functionality:

* Product Catalog Management (with Brands, Categories, and Specifications).
* Multi-tier Role-Based Authorization (Super Admin, Admin, Manager, User).
* Full Order Lifecycle and Payment Integration (PayPal/Mock).
* Secure User Authentication and Profile Management.

---

## 🛠️ Technology Stack

| Category | Technology |
| :--- | :--- |
| **Backend** | .NET 8 Web API (REST) |
| **ORM** | Entity Framework Core (Code First) |
| **Database** | SQL Server |
| **Security** | JWT Authentication |
| **Validation** | FluentValidation |
| **Documentation** | Swagger/OpenAPI |
| **Utilities** | AutoMapper, Serilog |
| **Tools** | Visual Studio 2022, Git, Postman |

---

## 👥 Access Roles and Responsibilities

| Role | Core Controllers | Responsibilities (Highest to Lowest Access) |
| :--- | :--- | :--- |
| **Super Admin** (Owner) | `AdminOwner` | System setup, creating initial Admins, and accessing system-level diagnostic info (`System-Info`). |
| **Admin** | `Admin`, `AdminCategory`, `AdminBrand`, etc. | Full administrative control: User role updates, managing managers, full Product/Catalog CRUD, payment history review. |
| **Manager** | `AdminProduct`, `AdminCategory` (limited) | Product, Specification, and Category CRUD. Inventory and stock status updates. |
| **User** | `UserCart`, `UserOrder`, `UserProfile`, `Payment` | Shopping, order placement, profile updates, address and wish list management. |

---

## 📊 API Endpoints Overview (by Controller Groups)

The API utilizes a granular structure for clear separation of concerns, reflected in the controller naming (`AdminOwner`, `AdminProduct`, `UserCart`, etc.).

Access full API details via Swagger: `https://localhost:7002/swagger/v1/swagger.json`

| Functionality Group | Key Endpoints (Examples) | Roles |
| :--- | :--- | :--- |
| **System/Owner** | `POST /api/AdminOwner/Create-Admin`, `GET /api/AdminOwner/System-Info` | Super Admin |
| **Authentication** | `POST /api/Auth/Registration`, `POST /api/Auth/Login`, `POST /api/Auth/Verify` | Anonymous |
| **Admin Control** | `GET /api/Admin/Users`, `POST /api/Admin/Create-Manager`, `PUT /api/Admin/Update-User/Role/{id}` | Admin |
| **Catalog (Public)** | `GET /api/Product/Get-All-Producs`, `GET /api/Category/Get-All-Categorys`, `GET /api/Filter/search-by/{name}` | Anonymous |
| **Product Management** | `POST /api/AdminProduct/Add-Product`, `PUT /api/AdminProductDetail/product-detail/{id}` | Admin/Manager |
| **Shopping Cart** | `POST /api/UserCartItem/Add-Item`, `DELETE /api/UserCart/{userId}/clear` | User |
| **Ordering & Payments** | `POST /api/UserOrder/Create-Order`, `GET /api/UserOrder/My-Orders`, `POST /api/Payment/create-payment/{orderId}` | User |
| **Profile Management** | `PUT /api/UserDetails/Update-User-Details/{id}`, `POST /api/UserAddress/User-addresses` | User |

---