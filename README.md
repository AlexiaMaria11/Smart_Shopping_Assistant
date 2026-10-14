<div align="center">

  <div>
    <img src="https://img.shields.io/badge/-.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
    <img src="https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
    <img src="https://img.shields.io/badge/-EF_Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white" alt="EF Core" />
    <img src="https://img.shields.io/badge/-SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
    <img src="https://img.shields.io/badge/-JWT_Auth-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" alt="JWT" />
    <img src="https://img.shields.io/badge/-OpenAI_GPT--4o--mini-412991?style=for-the-badge&logo=openai&logoColor=white" alt="OpenAI" />
    <img src="https://img.shields.io/badge/-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  </div>

  <h3 align="center">Smart Shopping Assistant — API</h3>

  <p align="center">
    The ASP.NET Core backend for <strong>Maison</strong>, a multi-seller fashion marketplace: catalog, companies,
    orders and reviews — plus an AI layer that doesn't just serve the cart, it <strong>reasons about it</strong>.
    Two chained LLM agents analyze the cart in real time, surface the deals you're missing by a hair,
    and recommend concrete products to complete them.
  </p>

</div>

## 📋 Overview

Smart Shopping Assistant (Maison) is a multi-seller marketplace backend: independent sellers register, get
approved, and manage their own companies, products and promotions; customers browse, buy and review across all
of them; an AI layer sits on top of the cart and promotions engine. Instead of a generic "recommended for you"
widget, the assistant:

- Reads the current cart and every promotion that applies to it
- Classifies each promotion as **ACTIVE** (already qualifying) or **NEAR-MISS** (close, with the exact gap and savings calculated)
- Turns each near-miss into a concrete suggestion — a real product from the catalog that would unlock the deal
- Adds complementary product suggestions on top

The AI never invents products, prices, or promotions — every suggestion is grounded through function-calling
tools that query the actual database. The AI integration is entirely **optional**: if no API key is configured,
`GET /api/features` reports it as unavailable and the rest of the app (catalog, cart, checkout, orders) works
exactly the same, just without the "Analyze" step.

## 🤖 How the AI pipeline works

```
Cart ──▶ PromotionCheckerAgent ──▶ ACTIVE / NEAR-MISS deals ──▶ SuggestionComposerAgent ──▶ Ranked suggestions
```

1. **`PromotionCheckerAgent`** calls a tool to fetch every promotion tied to each cart item (directly or via
   category), compares cart quantities/totals against each promotion's rules, and classifies it.
2. **`SuggestionComposerAgent`** takes that analysis and, using its own tools to query real products by
   category, builds up to 5 ranked suggestions — prioritizing near-miss deals by potential savings, then
   complementary items — always citing which deal or item each suggestion relates to.

Both agents are built with **Microsoft.Agents.AI** / **Microsoft.Extensions.AI.Workflows** on top of an
OpenAI-compatible `IChatClient` (default model `gpt-4o-mini`, swappable endpoint), with structured JSON-schema
output and function invocation enabled.

## 🔐 Accounts, roles and sellers

- **JWT auth** (ASP.NET Identity Core + `IdentityRole`) with three roles: **Customer**, **Seller**, **Admin**.
- Customers self-register normally. Sellers self-register through a dedicated endpoint that creates their
  **Company** at the same time, in status `Pending`, until an Admin approves it (`Approved` / `Rejected` /
  `Suspended`). A seller can only manage their own company's products, promotions and orders; an Admin can
  manage any company and platform-wide promotions.
- Cart and favorites are tied to the account (persisted in the database), not to the browser session.

## 🏗️ Architecture

The solution follows a clean, layered structure:

```text
SmartShoppingAssistant.Api/            REST endpoints, Swagger, DI composition root
SmartShoppingAssistant.BusinessLogic/  Services, AI Agents, Tools, DTOs, Mappers
SmartShoppingAssistant.DataAccess/     EF Core DbContext, Entities, Repositories, Migrations
```

- **Repository pattern** for data access, with a generic `IRepository<T>` base
- **DTOs + dedicated mappers** to keep API contracts decoupled from EF entities
- **Code-first EF Core migrations** against SQL Server
- **Postman collection** included for exercising the API without Swagger

## 🔌 Endpoints

| Controller | Responsibility |
|---|---|
| `AuthController` | Register (customer / seller + company), login, `GET /me` |
| `ProductsController` | Catalog CRUD, server-side `search` (filter/sort/pagination), `price-range`, `{id}/similar`, per-role `manage` listing |
| `CategoriesController` | CRUD for categories (Admin-only writes) |
| `PromotionsController` | CRUD for promotions, `product/{productId}` lookup, scoped by product/category/company |
| `CompaniesController` | Public company directory + storefront by slug, Admin approval workflow, seller self-service profile |
| `CartController` | Cart items, totals, shipping, and `POST /api/cart/analyze` — triggers the full AI pipeline |
| `FavoritesController` | Per-account wishlist |
| `OrdersController` | Checkout, order history, per-seller package status, seller/admin stats |
| `ReviewsController` | Product reviews and 1–5 star ratings, gated to delivered orders |
| `FeaturesController` | `GET /api/features` — tells the frontend whether the AI assistant is configured |

## 🛍️ Catalog, cart and checkout

- **Products** carry a stock quantity, multiple images (one marked as main), and belong to a company and one
  or more categories; `GET /api/products/search` filters by name/category/company/price/in-stock, sorts
  (price or name, asc/desc) and paginates server-side.
- **Promotions** trigger on item quantity or cart total, and reward either a percent discount or free items;
  scope narrows from product → category → whole company, and multiple promotions can stack (capped at the
  cart subtotal).
- **Cart** quantities are capped at live stock, re-validated again at checkout inside a DB transaction
  (handles race conditions), and restored automatically on order cancellation.
- **Checkout** captures a shipping address, charges via a simulated card form (Luhn-checked, only the last
  4 digits stored) or cash on delivery, applies free shipping over a configurable threshold, and snapshots
  product name/image/price onto the order so later catalog edits don't rewrite history.
- **Orders** are tracked per seller "package" inside the same order (each seller only sees/updates their own
  items: Pending → Confirmed → Shipped → Delivered, or Cancelled before shipping); customers can cancel their
  own order while cancellable.
- **Reviews**: a customer can rate (1–5 stars) and comment on a product only once it has actually been
  delivered to them; the author or an Admin can delete a review.
- **Favorites** and **cart** are both per-account, not per-browser.
- **Dashboards**: sellers get today / last‑30‑days / all‑time revenue and order counts plus their top
  products; admins get the same, platform-wide, plus top categories.

## ⚙️ Getting started

```bash
# 1. Clone
git clone https://github.com/AlexiaMaria11/Smart_Shopping_Assistant.git
cd Smart_Shopping_Assistant

# 2. Configure appsettings.Development.json
#    - ConnectionStrings:SmartShoppingAssistantContext (SQL Server)
#    - Jwt:Key / Jwt:Issuer / Jwt:Audience (JWT signing)
#    - OpenAI:ApiKey / OpenAI:Model (optional — defaults to gpt-4o-mini; leave ApiKey empty to run without AI)

# 3. Apply migrations
dotnet ef database update --project SmartShoppingAssistant.DataAccess --startup-project SmartShoppingAssistant.Api

# 4. Run
dotnet run --project SmartShoppingAssistant.Api
```

The database is seeded automatically on startup (`DataSeeder`), and Swagger UI is available in the
Development environment at `/swagger`.

## 🧩 Companion project

The React + TypeScript frontend for this API lives in
[Smart-Shopping-Assistant-FE](https://github.com/AlexiaMaria11/Smart-Shopping-Assistant-FE).

## 🛠️ Tech stack

- **ASP.NET Core 10** Web API, Swagger / Swashbuckle
- **Entity Framework Core 10** (SQL Server, code-first migrations)
- **ASP.NET Identity Core + JWT** (HMAC-SHA256) for auth and role-based authorization
- **Microsoft.Agents.AI** + **Microsoft.Agents.AI.Workflows** / **Microsoft.Extensions.AI.OpenAI**, with
  function calling and JSON-schema structured responses, against `gpt-4o-mini` by default
- **Repository + Service + Mapper** layering, DTO-based contracts
- Global exception handling middleware, startup DB seeding
