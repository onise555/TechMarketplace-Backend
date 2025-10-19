# TechMarketplace Backend (E-commerce API)

**STEP IT Academy Diploma Project (2025)**

**Student:** Onise Tsotskhalashvili
**Course:** Full Stack Development

---

## 🎯 Project Scope

Development of a RESTful Backend API for a technical product e-commerce platform.

### Core Functionality:

* Product and Category Management (CRUD).
* Role-Based Authorization (Super Admin, Admin, Manager, User).
* Order Processing and Lifecycle Management.
* Advanced Search, Filtering, and Pagination.
* Secure User Authentication (JWT).

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

## 👥 Access Roles

| Role | Permissions |
| :--- | :--- |
| **Super Admin** (Owner) | **Highest level control.** Can manage all users, including other Admins, and access system-level diagnostics. |
| **Admin** | Full system control (Users, Products, Orders, Payments). |
| **Manager** | Product Catalog and Order status management. |
| **User** | Shopping, Checkout, Profile management. |

---

## 📊 Data Models (Key Entities)

* `User`: Authentication, Roles.
* `Product`: Catalog item, linked to `Category` and `Brand`.
* `ProductDetail`: Specifications, Images.
* `Cart` & `CartItem`: Session and user-persistent shopping.
* `Order` & `OrderItem`: Purchase transaction.
* `Review`: Customer feedback.

---

## 🚀 API Endpoints Summary

Access full API details via Swagger: `https://localhost:7002/swagger/index.html`

| Functionality | Example Endpoints | Role |
| :--- | :--- | :--- |
| **Auth** | `POST /api/Auth/Login`, `POST /api/Auth/Registration` | Anonymous |
| **Catalog** | `GET /api/Product/Get-All-Producs`, `GET /api/Category/Get-All-Categorys` | Anonymous |
| **Super Admin** | `POST /api/AdminOwner/Create-Admin`, `GET /api/AdminOwner/System-Info` | Super Admin |
| **Admin (Users)** | `GET /api/Admin/Users`, `PUT /api/Admin/Update-User/Role/{id}` | Admin |
| **Admin (Product)** | `POST /api/AdminProduct/Add-Product`, `DELETE /api/AdminProduct/Delete-Product/{id}` | Admin/Manager |
| **Shopping** | `POST /api/UserCartItem/Add-Item`, `POST /api/UserOrder/Create-Order` | User |
| **Payment** | `POST /api/Payment/create-payment/{orderId}` | User |

---