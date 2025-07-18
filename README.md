# Tekton Challenge Products API

## Overview
This project is a REST API built with .NET 8, designed to manage products. It follows Clean Architecture principles, uses vertical slice architecture for use cases, and applies DDD, TDD, and SOLID principles. The solution includes:

- **CQRS & Mediator Pattern:** All use cases are implemented as request/handler pairs using MediatR.
- **Repository & UnitOfWork Patterns:** Data access is abstracted via repositories and a unit of work for transactional consistency.
- **Validation:** FluentValidation is used for request validation, with proper HTTP status codes for errors.
- **Caching:** Product status dictionary is cached in memory for 5 minutes using MemoryCache.
- **External Services:** Discount percentage is fetched from an external API using HttpClientFactory.
- **Logging:** Request timing is logged to a plain text file for each request.
- **OpenAPI/Swagger:** The API is fully documented and browsable via Swagger UI.
- **TDD:** The solution includes unit tests for use cases and services.

## Requirements Fulfilled
- REST API in .NET 8
- Swagger documentation
- SOLID, Clean Code, Clean Architecture, N-layered structure
- CQRS, Mediator, Repository, UnitOfWork patterns
- TDD and validation with FluentValidation
- Proper HTTP status codes
- Caching and external service consumption
- Logging to text file

## Prerequisites
- **SQL Server:** You need a running instance of SQL Server (local or remote). Configure the connection string in `appsettings.json`.
- **.NET 8 SDK**

## How to Run Locally
1. **Configure the database connection** in `appsettings.json` (and environment-specific files if needed).
2. **Build the project:**
   ```bash
   dotnet build
   ```
3. **Run the API:**
   ```bash
   dotnet run --project src/Api/TektonChallengeProducts.Api.csproj
   ```
4. **Automatic migrations:** Pending migrations are applied automatically at startup.
5. **Test the API:** Use Swagger UI (`/swagger`), Postman, or any HTTP client. A Postman collection is included for convenience.

## Useful Information
- **Database mapping:** Entity Framework is used for ORM and database mapping.
- **Product status dictionary** is cached for 5 minutes using MemoryCache. The cache implementation can be easily replaced (e.g., Redis) in the infrastructure layer.
- **Discounts** are fetched from an external API and applied to the product's final price.
- **Logging:** Request timing and errors are logged to text files in the `src/Api/logs` folder.
- **Unit tests** are provided for main use cases and services.
- **Postman collection:** Included in the repository to facilitate API testing in `src/Api/Collection/Tekton Challenge Products.postman_collection.json` path.

## Patterns & Architecture
- **Vertical Slice Architecture:** Each use case is isolated in its own handler.
- **Domain-Driven Design (DDD):** Domain entities and logic are separated from infrastructure.
- **SOLID Principles:** The codebase is modular, testable, and maintainable.

## Project Structure
- `src/Api`: API layer, controllers, middleware
- `src/Application`: Use cases, validators, queries, commands
- `src/Domain`: Domain entities, enums, abstractions
- `src/Infrastructure`: Persistence, services, DI
- `tests`: Unit tests

## Contact
For questions or improvements, feel free to contact me.
