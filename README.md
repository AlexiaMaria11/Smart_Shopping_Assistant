<div align="center">

  <div>
    <img src="https://img.shields.io/badge/-.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
    <img src="https://img.shields.io/badge/-C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#" />
    <img src="https://img.shields.io/badge/-EF_Core-512BD4?style=for-the-badge&logo=nuget&logoColor=white" alt="EF Core" />
    <img src="https://img.shields.io/badge/-SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" alt="SQL Server" />
    <img src="https://img.shields.io/badge/-OpenAI_GPT--4o-412991?style=for-the-badge&logo=openai&logoColor=white" alt="OpenAI" />
    <img src="https://img.shields.io/badge/-Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  </div>

  <h3 align="center">Smart Shopping Assistant — API</h3>

  <p align="center">
    An ASP.NET Core 8 backend that doesn't just serve a shopping cart — it <strong>reasons about it</strong>.
    Two chained LLM agents analyze the cart in real time, surface the deals you're missing by a hair,
    and recommend concrete products to complete them.
  </p>

</div>

## 📋 Overview

Smart Shopping Assistant is the backend for an e-commerce cart experience where an AI layer sits on top of
the catalog and promotions engine. Instead of a generic "recommended for you" widget, the assistant:

- Reads the current cart and every promotion that applies to it
- Classifies each promotion as **ACTIVE** (already qualifying) or **NEAR-MISS** (close, with the exact gap and savings calculated)
- Turns each near-miss into a concrete suggestion — a real product from the catalog that would unlock the deal
- Adds complementary product suggestions (a case for a phone, a charger for a laptop) on top

The AI never invents products, prices, or promotions — every suggestion is grounded through function-calling
tools that query the actual database.

## 🤖 How the AI pipeline works

```
Cart ──▶ PromotionCheckerAgent ──▶ ACTIVE / NEAR-MISS deals ──▶ SuggestionComposerAgent ──▶ Ranked suggestions
```

1. **`PromotionCheckerAgent`** calls a tool to fetch every promotion tied to each cart item (directly or via
   category), compares cart quantities/totals against each promotion's rules, and classifies it.
2. **`SuggestionComposerAgent`** takes that analysis and, using its own tools to query real products by
   category, builds up to 5 ranked suggestions — prioritizing near-miss deals by potential savings, then
   complementary items — always citing which deal or item each suggestion relates to.

Both agents are built with **Microsoft.Agents.AI** / **Microsoft.Extensions.AI** on top of an OpenAI
`IChatClient` (GPT-4o), with structured JSON-schema output and function invocation enabled.

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
| `ProductsController` | CRUD for the product catalog |
| `CategoriesController` | CRUD for categories |
| `PromotionsController` | CRUD for promotions |
| `CartController` | Cart items, and `POST /api/cart/analyze` — triggers the full AI pipeline |

## ⚙️ Getting started

```bash
# 1. Clone
git clone https://github.com/AlexiaMaria11/Smart_Shopping_Assistant.git
cd Smart_Shopping_Assistant

# 2. Configure appsettings.Development.json
#    - ConnectionStrings:SmartShoppingAssistantContext (SQL Server)
#    - OpenAI:ApiKey / OpenAI:Model (defaults to gpt-4o)

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

- **ASP.NET Core 8** Web API, Swagger / Swashbuckle
- **Entity Framework Core** (SQL Server, code-first migrations)
- **Microsoft.Agents.AI** + **Microsoft.Extensions.AI** with OpenAI GPT-4o, function calling, JSON-schema responses
- **Repository + Service + Mapper** layering, DTO-based contracts
