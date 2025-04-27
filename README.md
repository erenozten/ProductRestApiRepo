# ProductWebAPI 🛒📦✨

## 🚀 About the Project

**ProductWebAPI** is a modern RESTful Web API developed using **.NET 9**.  
This project follows best practices in software architecture, error handling, logging, and API design.  
It is designed for production-ready environments, focusing on maintainability, scalability, and clean architecture principles. 🚀

---

## 📦 Key Features

- ✅ **Global Exception Handling Middleware** for unified error processing
- ✅ **Global Logging Middleware** powered by **SEQ** and structured logs
- ✅ **Generic Repository Pattern** for clean and flexible database access
- ✅ **GenericApiResponse<T>** standard for consistent API responses
- ✅ **FluentValidation** for strong and scalable validation logic
- ✅ **Swagger + Example Responses** for perfect OpenAPI documentation
- ✅ **Extension Methods** for utility helpers and better code organization
- ✅ **Dependency Injection (DI)** based service and repository registration
- ✅ **Clean Layered Architecture** (Controllers, Services, Repositories, DTOs, Common)
- ✅ **DTOs (Data Transfer Objects)** for strict separation of entity and API layers
- ✅ **AutoMapper** for seamless object mapping

---

## 📚 Technologies Used

- ASP.NET Core 9
- Entity Framework Core 9
- SQL Server (or switchable to any RDBMS)
- AutoMapper
- FluentValidation
- SEQ (structured logging)
- Swashbuckle.AspNetCore (Swagger with Example Filters)
- Microsoft Dependency Injection
- System.Text.Json

---

## 🔥 Core Concepts

### 📄 Standardized API Response

All endpoints return a unified, typed response using `GenericApiResponse<T>`:

```json
{
  "isSuccess": true,
  "statusCode": 200,
  "message": "[200 OK] Successful operation.",
  "data": {
    "id": 1,
    "name": "Sample Product",
    "about": "Beautiful place."
  }
}
```

Failed responses:

```json
{
  "isSuccess": false,
  "statusCode": 404,
  "message": "[404 NotFound] Error! -15- entity not found.",
  "data": null,
  "errorCode": "PRODUCT_NOTFOUND"
}
```

---

### 🛡️ Global Middlewares

- **Exception Middleware:** Centralizes exception catching and transforms them into readable, structured error responses.
- **Logging Middleware:** Every incoming request and outgoing response is logged in structured JSON format and sent to a SEQ server.
- **Validation Middleware:** Handles `422 Unprocessable Entity` responses automatically for validation errors.

---

### 🏛️ Architecture Overview

```
/Controllers
    - BaseController.cs
    - ProductsController.cs

/Services
    - ProductService.cs

/Repositories
    - GenericRepository<T>.cs
    - IGenericRepository<T>.cs
    - IProductRepository.cs

/DTOs
    - ProductCreateRequestDto.cs
    - ProductGetResponseDto.cs

/Common
    /Responses
        - GenericApiResponse.cs
        - ListWithCountDto.cs
    /Extensions
        - ServiceCollectionExtensions.cs
        - ExceptionMiddlewareExtensions.cs
    /Middlewares
        - GlobalExceptionMiddleware.cs
        - LoggingMiddleware.cs

/SwaggerExamples
    /Common
        - Swagger500Response.cs
        - Swagger404Response.cs
        - Swagger422Response.cs
        - Swagger401Response.cs
        - Swagger403Response.cs
        - SwaggerErrorExamplesFactory.cs
    /Product
        - Swagger201Response.cs
        - SwaggerGetProductList200Response.cs
```

---

## ⚙️ Setup & Run Locally

### Clone the repository:

```bash
git clone https://github.com/your-repo/productwebapi.git
cd productwebapi
```

### Restore packages and run:

```bash
dotnet restore
dotnet run
```

### Open in your browser:

```bash
http://localhost:5078/swagger/index.html
```

---

## 📑 Global Swagger Response Types

Automatically applied to all endpoints via `BaseController`:

| Status Code | Description           |
| ----------- | --------------------- |
| 404         | Resource not found    |
| 500         | Internal server error |
| 422         | Validation error      |
| 401         | Unauthorized access   |
| 403         | Forbidden access      |

✅ Success examples like `200 OK`, `201 Created`, and `204 No Content` are defined individually per action method where necessary.

---

## 🔍 Logging with SEQ

- Every HTTP request and response is automatically logged.
- All logs are structured (JSON format) and sent to a SEQ server.
- Example logged properties:
  - Request Path
  - Query Parameters
  - Response Status Code
  - Execution Time
  - User Claims (if available)

---

## 🧩 Extension Methods

Organized reusable code:

- `ServiceCollectionExtensions.cs` → All services and repositories are registered here.
- `ExceptionMiddlewareExtensions.cs` → Middlewares are cleanly added here.

---

# 🚀 Vision

> "**To build clean, standardized, and production-ready APIs that make developers' and users' lives easier.**" 🌟

---

# 🌟 Let's Connect!

Pull requests, suggestions, and ideas are always welcome! 💬  
Let's grow the clean architecture ecosystem together! 🚀

---

## 🎯 Next Steps We Can Add (Optional)

- API Versioning
- JWT Authentication
- Pagination Parameters (pageIndex, pageSize)
- Response Caching
- Dockerization
- Health Checks
- CQRS + MediatR

---

# 🛠️ Under Construction, Improving Every Day! 🏗️

---

# ✅ Status: Production-Ready & Swagger-Polished! 😎

---

# 📣 Thank you for checking out **ProductWebAPI**! 🙌
