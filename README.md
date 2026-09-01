# Banking App

A full-stack banking application built with **C#**, **ASP.NET Core Minimal API**, **Entity Framework Core**, **PostgreSQL**, and **React**.

The project started as a backend learning project and evolved into a full-stack application with a real database, API tests, GitHub Actions, and a React frontend.

---

## Features

### Backend

- Create bank accounts
- View all accounts
- View account by account number
- Deposit money
- Withdraw money
- Transfer money between accounts
- Update account owner name
- View transaction history
- Store account and transaction data in PostgreSQL
- Use Entity Framework Core migrations
- Swagger/OpenAPI support
- GitHub Actions CI pipeline

### Frontend

- React client created with Vite
- View all accounts
- Create new accounts
- Open account details page
- Deposit and withdraw money
- Update owner name
- View transaction history
- Transfer money from a separate transfer page
- Styled UI with CSS

---

## Technologies Used

### Backend

- C#
- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core
- PostgreSQL
- Npgsql Entity Framework Core Provider
- Swagger / OpenAPI
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- GitHub Actions

### Frontend

- React
- Vite
- JavaScript
- React Router
- CSS

### Database Tools

- PostgreSQL
- pgAdmin

---

## Project Structure

```text
banking-app
├── BankingApp.Api
│   ├── Endpoints
│   │   └── AccountEndpoints.cs
│   ├── Persistence
│   │   ├── Entities
│   │   │   ├── BankAccountEntity.cs
│   │   │   └── TransactionEntity.cs
│   │   ├── Migrations
│   │   ├── BankDbContext.cs
│   │   ├── EfCoreBankStorage.cs
│   │   └── IBankStorage.cs
│   ├── Requests
│   ├── Responses
│   └── Program.cs
│
├── BankingApp.Core
│   ├── BankAccount.cs
│   ├── BankSystem.cs
│   ├── OperationResult.cs
│   ├── Transaction.cs
│   └── TransactionType.cs
│
├── BankingApp.Console
│
├── BankingApp.Tests
│
├── BankingApp.Api.Tests
│
├── BankingApp.Client
│   ├── src
│   │   ├── api
│   │   │   └── accountsApi.js
│   │   ├── pages
│   │   │   ├── AccountsPage.jsx
│   │   │   ├── AccountDetailsPage.jsx
│   │   │   └── TransferPage.jsx
│   │   ├── App.jsx
│   │   ├── App.css
│   │   └── main.jsx
│   ├── package.json
│   └── vite.config.js
│
└── .github
    └── workflows
        └── dotnet.yml
```

---

## Backend Overview

The backend is built with ASP.NET Core Minimal API.

The main endpoint setup is kept in:

```text
BankingApp.Api/Endpoints/AccountEndpoints.cs
```

The core business logic is stored in:

```text
BankingApp.Core
```

This keeps the project clean by separating:

```text
API layer         → HTTP endpoints
Core layer        → banking logic
Persistence layer → database storage
Frontend layer    → React UI
```

---

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/accounts` | Get all accounts |
| POST | `/accounts` | Create new account |
| GET | `/accounts/{accountNumber}` | Get account by account number |
| POST | `/accounts/{accountNumber}/deposit` | Deposit money |
| POST | `/accounts/{accountNumber}/withdraw` | Withdraw money |
| POST | `/accounts/{accountNumber}/transfer` | Transfer money |
| PUT | `/accounts/{accountNumber}/owner` | Update account owner name |
| GET | `/accounts/{accountNumber}/transactions` | Get transaction history |

---

## Database

The application uses **PostgreSQL** as the main database.

Entity Framework Core is used for:

- Database mapping
- Relationships
- Migrations
- Saving and loading data

The main database context is:

```text
BankingApp.Api/Persistence/BankDbContext.cs
```

The main storage class is:

```text
BankingApp.Api/Persistence/EfCoreBankStorage.cs
```

The database contains:

```text
Accounts
Transactions
__EFMigrationsHistory
```

`__EFMigrationsHistory` is created by Entity Framework Core and stores which migrations have already been applied.

---

## Entity Framework Core Migrations

The project uses EF Core migrations to manage database schema changes.

Migrations are stored in:

```text
BankingApp.Api/Persistence/Migrations
```

The API applies pending migrations automatically on startup using:

```csharp
context.Database.Migrate();
```

This allows the database structure to evolve safely when new columns, tables, or relationships are added.

---

## Important Security Note

The PostgreSQL connection string should not be committed to GitHub.

The project uses **User Secrets** for local database credentials.

Example setup:

```bash
dotnet user-secrets init --project BankingApp.Api
```

```bash
dotnet user-secrets set "ConnectionStrings:PostgresConnection" "Host=localhost;Port=5432;Database=banking_app;Username=postgres;Password=your_password" --project BankingApp.Api
```

The real password should stay only on the local machine.

---

## Running the Backend

Make sure PostgreSQL is running and that a database named `banking_app` exists.

Run the API:

```bash
dotnet run --project BankingApp.Api
```

Then open Swagger:

```text
https://localhost:<port>/swagger
```

Example:

```text
https://localhost:7031/swagger
```

---

## Running the Frontend

Go into the React client folder:

```bash
cd BankingApp.Client
```

Install dependencies:

```bash
npm install
```

Run the Vite development server:

```bash
npm run dev
```

The React app usually runs at:

```text
http://localhost:5173
```

The backend API must also be running for the frontend to load accounts.

---

## CORS

The API allows the React frontend to call it through a CORS policy.

In development, the React app runs on:

```text
http://localhost:5173
```

The API allows that origin using:

```csharp
.WithOrigins("http://localhost:5173")
```

The origin should not include a trailing slash.

Correct:

```text
http://localhost:5173
```

Incorrect:

```text
http://localhost:5173/
```

---

## Running Tests

Run all backend tests:

```bash
dotnet test
```

The solution includes:

- Core unit tests
- API integration tests
- EF Core storage tests

The API tests use a test storage implementation so they do not depend on the real PostgreSQL database.

---

## GitHub Actions

The repository includes a GitHub Actions workflow for running .NET tests automatically.

Workflow file:

```text
.github/workflows/dotnet.yml
```

The workflow runs on pushes and pull requests to:

```text
master
main
```

It can also be run manually from the GitHub Actions tab.

---

## Current Frontend Pages

### Accounts Page

Route:

```text
/
```

Features:

- Shows all accounts
- Creates a new account
- Links to account details

### Account Details Page

Route:

```text
/accounts/{accountNumber}
```

Features:

- Shows account owner
- Shows account number
- Shows balance
- Deposit money
- Withdraw money
- Update owner name
- View transaction history
- Link to transfer page

### Transfer Page

Route:

```text
/accounts/{accountNumber}/transfer
```

Features:

- Shows sender account
- Shows sender owner name
- Allows entering receiver account number
- Allows entering transfer amount
- Sends money between accounts

---

## Current Full-Stack Flow

```text
React frontend
    ↓
ASP.NET Core API
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

The React frontend does not talk directly to the database. It sends HTTP requests to the ASP.NET Core API, and the API handles the banking logic and database updates.

---

## Future Improvements

Planned improvements:

- Add delete / close account endpoint
- Prevent closing accounts with non-zero balance
- Improve frontend error messages from backend responses
- Add loading indicators
- Add better form validation in React
- Add transaction IDs to API responses
- Add transaction filtering by type or date
- Improve EF Core storage to update only affected accounts and transactions
- Add authentication and users
- Add account ownership per user
- Add Docker support
- Add deployment configuration
- Add frontend tests

---

## Project Status

The project currently supports a working full-stack banking flow.

Accounts can be created, viewed, updated, and used for deposits, withdrawals, transfers, and transaction history.

The backend uses ASP.NET Core and PostgreSQL, while the frontend uses React with Vite and React Router.
