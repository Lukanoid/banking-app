# Banking App

A simple banking system built with C# and ASP.NET Core.

The project started as a core banking logic application and was later expanded with a Web API, SQLite persistence, unit tests, API integration tests, SQLite storage tests, and GitHub Actions CI.

![.NET Tests](https://github.com/Lukanoid/banking-app/actions/workflows/dotnet.yml/badge.svg)

---

## Features

- Create bank accounts
- Find accounts by account number
- Deposit money
- Withdraw money
- Transfer money between accounts
- View transaction history
- Save and load accounts using SQLite database persistence
- REST API using ASP.NET Core Minimal APIs
- Swagger UI for testing API endpoints
- Unit tests for core business logic
- API integration tests for endpoints
- SQLite storage integration tests
- GitHub Actions workflow for automatic test runs

---

## Project Structure

```text
BankingApp
├── BankingApp.Core
│   ├── BankAccount.cs
│   ├── BankSystem.cs
│   ├── OperationResult.cs
│   ├── Transaction.cs
│   └── TransactionType.cs
│
├── BankingApp.Api
│   ├── Endpoints
│   │   └── AccountEndpoints.cs
│   ├── Persistence
│   │   ├── Entities
│   │   │   ├── BankAccountEntity.cs
│   │   │   └── TransactionEntity.cs
│   │   ├── Models
│   │   ├── BankDbContext.cs
│   │   ├── IBankStorage.cs
│   │   ├── JsonBankStorage.cs
│   │   └── SqliteBankStorage.cs
│   ├── Requests
│   ├── Responses
│   └── Program.cs
│
├── BankingApp.Console
│   └── Console application
│
├── BankingApp.Tests
│   └── Unit tests for Core logic
│
├── BankingApp.Api.Tests
│   ├── AccountsApiTests.cs
│   ├── DepositsApiTests.cs
│   ├── WithdrawalsApiTests.cs
│   ├── TransfersApiTests.cs
│   ├── TransactionsApiTests.cs
│   ├── SqliteBankStorageTests.cs
│   ├── ApiTestHelpers.cs
│   ├── CustomWebApplicationFactory.cs
│   └── TestBankStorage.cs
│
└── .github
    └── workflows
        └── dotnet.yml
```

---

## Technologies Used

- C#
- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core
- SQLite
- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- Swagger / OpenAPI
- GitHub Actions

---

## Core Concepts

The main business logic is stored inside `BankingApp.Core`.

### BankAccount

Responsible for account-level operations:

- Deposit
- Withdraw
- Transfer
- Transaction history

### BankSystem

Responsible for managing multiple accounts:

- Creating accounts
- Finding accounts
- Returning all accounts
- Loading saved accounts

### OperationResult

Used to return the result of banking operations.

Example:

```csharp
public class OperationResult
{
    public bool IsSuccess { get; }
    public string Message { get; }

    public OperationResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
}
```

---

## Persistence

The project uses SQLite database persistence through Entity Framework Core.

The main database classes are located in:

```text
BankingApp.Api/Persistence
```

### BankDbContext

`BankDbContext` is the EF Core database context. It represents the connection between the application and the database.

It contains:

```text
Accounts table
Transactions table
```

### BankAccountEntity

Represents a bank account in the database.

### TransactionEntity

Represents a transaction in the database.

### SqliteBankStorage

Handles saving and loading accounts from the SQLite database.

The API depends on the `IBankStorage` interface, which allows the storage implementation to be changed without changing the endpoint logic.

---

## API Endpoints

### Root

```http
GET /
```

Returns a simple message confirming that the API is running.

---

### Accounts

```http
GET /accounts
```

Returns all accounts.

```http
POST /accounts
```

Creates a new account.

Request body:

```json
{
  "ownerName": "John Doe"
}
```

---

### Get Account By Number

```http
GET /accounts/{accountNumber}
```

Returns a specific account by account number.

---

### Deposit

```http
POST /accounts/{accountNumber}/deposit
```

Request body:

```json
{
  "amount": 1000
}
```

---

### Withdraw

```http
POST /accounts/{accountNumber}/withdraw
```

Request body:

```json
{
  "amount": 100
}
```

---

### Transfer

```http
POST /accounts/{accountNumber}/transfer
```

Request body:

```json
{
  "receiverAccountNumber": "12345",
  "amount": 100
}
```

---

### Transaction History

```http
GET /accounts/{accountNumber}/transactions
```

Returns the transaction history for an account.

---

## Running the API

From the solution folder, run:

```bash
dotnet run --project BankingApp.Api
```

Then open Swagger in the browser:

```text
https://localhost:xxxx/swagger
```

The exact port may be different on your machine.

---

## Running Tests

Run the Core unit tests:

```bash
dotnet test BankingApp.Tests/BankingApp.Tests.csproj
```

Run the API integration tests and SQLite storage tests:

```bash
dotnet test BankingApp.Api.Tests/BankingApp.Api.Tests.csproj
```

Run all tests:

```bash
dotnet test
```

---

## Testing

The project contains three main types of tests.

### Unit Tests

Located in:

```text
BankingApp.Tests
```

These tests check the core business logic directly, for example:

- Creating accounts
- Depositing money
- Withdrawing money
- Transferring money
- Transaction history
- Validation rules

### API Integration Tests

Located in:

```text
BankingApp.Api.Tests
```

These tests start the API in memory and send real HTTP requests using `HttpClient`.

They test the full API flow:

```text
HttpClient
-> API endpoint
-> BankSystem
-> BankAccount
-> API response
```

The API tests use `CustomWebApplicationFactory` and `TestBankStorage` so that endpoint tests do not write to the real database.

### SQLite Storage Integration Tests

Located in:

```text
BankingApp.Api.Tests/SqliteBankStorageTests.cs
```

These tests check that `SqliteBankStorage` can correctly:

- Load an empty database
- Save and load accounts
- Save and load transactions
- Save and load multiple accounts

Each SQLite storage test uses a temporary database file, so the real local database is not affected.

---

## GitHub Actions CI

This project uses GitHub Actions to automatically run tests when code is pushed.

Workflow file:

```text
.github/workflows/dotnet.yml
```

The workflow runs:

- Core unit tests
- API integration tests
- SQLite storage integration tests

This helps make sure new changes do not break existing functionality.

---

## Example API Response

Example response after creating an account:

```json
{
  "ownerName": "John Doe",
  "accountNumber": "12345",
  "balance": 0
}
```

Example response after deposit:

```json
{
  "message": "Deposit successful.",
  "balance": 1000
}
```

Example response after transfer:

```json
{
  "message": "Transfer successful.",
  "senderBalance": 900,
  "receiverBalance": 100
}
```

---

## Notes

The SQLite database file is used only for local persistence and should not be committed to GitHub.

The local data folder is ignored with `.gitignore`:

```gitignore
/BankingApp.Api/Data/
```

---

## Future Improvements

Possible future improvements:

- Add account update endpoint
- Add account delete endpoint
- Add authentication and users
- Add database migrations
- Improve transaction details
- Add account ownership
- Add frontend UI
- Add Docker support
- Add SQL Server support

---

## Author

Created by [Lukanoid](https://github.com/Lukanoid)
