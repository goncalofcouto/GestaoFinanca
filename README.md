# GestaoFinanca API

REST API built with ASP.NET Core 8 for personal finance management, with JWT authentication, user roles, and transaction CRUD (income and expense).

## Features
- JWT authentication (`login` and `register`).
- User roles (`Admin` and `Client`).
- User management restricted to `Admin`.
- CRUD for `income` and `expense` transactions.
- User-level data isolation (`NameIdentifier` claim in the token).
- Swagger UI with Bearer token support.

## Stack
- .NET 8 (`net8.0`)
- ASP.NET Core Web API
- Entity Framework Core 8
- MySQL (Pomelo)
- JWT Bearer Authentication
- BCrypt for password hashing
- Swashbuckle (Swagger)

## Project Structure
- `Controllers/` HTTP endpoints.
- `Models/` Entities and DTOs.
- `Data/AppDbContext.cs` EF Core context and initial seed.
- `Services/JwtService.cs` Login, register, and token generation.
- `Services/PasswordHelper.cs` Password hash/verify helpers.
- `Migrations/` EF migration history.

## Requirements
- .NET SDK 8+
- Running MySQL instance
- EF Core CLI tool (`dotnet-ef`)

Install EF CLI (if needed):

```bash
dotnet tool install --global dotnet-ef
```

## Configuration
Update `appsettings.json` with your environment values:

```json
"ConnectionStrings": {
  "AppDbConnectionString": "server=localhost; database=DbGestaoFinanca; user=YOUR_USER; password=YOUR_PASSWORD"
},
"JwtConfig": {
  "Issuer": "http://localhost:5270/",
  "Audience": "http://localhost:5270/",
  "Key": "YOUR_STRONG_SECRET_KEY",
  "TokenValidityInMinutes": 30
}
```

## Running the Project
1. Restore dependencies:

```bash
dotnet restore
```

2. Apply migrations:

```bash
dotnet ef database update
```

3. Run the API:

```bash
dotnet run
```

4. Open Swagger:
- `http://localhost:5270/swagger`

## Initial Seed
When the database is created/updated, one admin user is inserted automatically:
- Email: `admin@example.com`
- Password: `admin123`
- Role: `Admin`

File: `Data/AppDbContext.cs`.

## Authentication and Authorization
- `POST /api/auth/login` returns a JWT.
- The token includes these claims:
- `email`
- `role`
- `nameidentifier` (user id)

Use this value in Swagger `Authorize`:

```text
Bearer <your_token>
```

## Endpoints

### Auth
- `POST /api/auth/register`
- `POST /api/auth/login`

### Users (Admin only)
- `POST /api/users`
- `GET /api/users`
- `GET /api/users/{id}`
- `PUT /api/users/{id}`
- `DELETE /api/users/{id}`

### Transactions - Income
- `POST /api/transactions/income`
- `GET /api/transactions/income`
- `GET /api/transactions/income/{id}`
- `PUT /api/transactions/income/{id}`
- `DELETE /api/transactions/income/{id}`

### Transactions - Expense
- `POST /api/transactions/expense`
- `GET /api/transactions/expense`
- `GET /api/transactions/expense/{id}`
- `PUT /api/transactions/expense/{id}`
- `DELETE /api/transactions/expense/{id}`

## Main DTO Contracts
- `LoginRequest`: `email`, `password`
- `LoginResponse`: `email`, `token`, `expiresIn`
- `RegisterRequest`: `name`, `email`, `password`
- `RegisterResponse`: `name`, `email`
- `TransactionRequest`: `description`, `amount`, `type`, `date`
- `TransactionResponse`: `id`, `description`, `amount`, `type`, `date`

Note: for `/income` and `/expense` endpoints, `Type` is set by the backend.

## Quick Flow Example
1. Register:

```bash
curl -X POST http://localhost:5270/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"Joao","email":"joao@email.com","password":"123456"}'
```

2. Login:

```bash
curl -X POST http://localhost:5270/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@email.com","password":"123456"}'
```

3. Create an expense (with token):

```bash
curl -X POST http://localhost:5270/api/transactions/expense \
  -H "Authorization: Bearer <TOKEN>" \
  -H "Content-Type: application/json" \
  -d '{"description":"Groceries","amount":120.50,"date":"2026-03-15T10:00:00Z"}'
```

## Best Practices and Next Steps
- Replace `Ok` with `CreatedAtAction` in `POST` endpoints.
- Add pagination and date-range filtering (`startDate`, `endDate`) in `GET /transactions`.
- Split create/update DTOs for users and transactions.
- Move secrets (JWT key and DB password) to environment variables.
- Add automated tests (unit and integration).

