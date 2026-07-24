**# Banking App

A small personal C# banking project built to practice **object-oriented programming**, **unit testing**, **ASP.NET Core Minimal APIs**, **dependency injection**, and **JSON persistence**.

The project started as a simple banking console application and has grown into a multi-project solution with a separated Core layer, Web API, tests, and local file persistence.

---

## Project Goals

The main goal of this project is to practice and improve C# skills by building a small but structured banking system.

The project focuses on:

* Clean object-oriented design
* Separating business logic from UI/API logic
* Unit testing with xUnit
* Building a Web API with ASP.NET Core Minimal API
* Using dependency injection
* Saving and loading data with JSON persistence
* Practicing real-world project structure

---

## Solution Structure

```text
BankingApp
│
├── BankingApp.Core
│   ├── BankAccount.cs
│   ├── BankSystem.cs
│   ├── Transaction.cs
│   ├── TransactionType.cs
│   └── OperationResult.cs
│
├── BankingApp.Api
│   ├── Program.cs
│   │
│   ├── Requests
│   │   ├── CreateAccountRequest.cs
│   │   ├── MoneyRequest.cs
│   │   └── TransferRequest.cs
│   │
│   ├── Responses
│   │   ├── AccountResponse.cs
│   │   ├── OperationResponse.cs
│   │   ├── TransferResponse.cs
│   │   └── TransactionResponse.cs
│   │
│   ├── Persistence
│   │   ├── IBankStorage.cs
│   │   ├── JsonBankStorage.cs
│   │   │
│   │   └── Models
│   │       ├── StoredBankAccount.cs
│   │       └── StoredTransaction.cs
│   │
│   └── Data
│       └── accounts.json
│
├── BankingApp.Console
│   └── Program.cs
│
└── BankingApp.Tests
    ├── BankAccountTests.cs
    ├── BankSystemTests.cs
    ├── TransactionTests.cs
    └── OperationResultTests.cs
```

---

## Projects Explained

### BankingApp.Core

Contains the main business logic of the application.

This project includes:

* Bank account creation
* Deposit logic
* Withdraw logic
* Transfer logic
* Transaction history
* Operation result handling
* Account searching and storage in memory

The Core project does not depend on the API or Console project. This keeps the business logic reusable and testable.

---

### BankingApp.Api

Contains the ASP.NET Core Minimal API.

The API exposes endpoints for:

* Creating accounts
* Getting all accounts
* Getting account details
* Depositing money
* Withdrawing money
* Transferring money
* Getting transaction history

The API uses dependency injection and JSON persistence.

---

### BankingApp.Console

A console application that can use the Core project.

This project is useful for practicing basic console UI and calling the business logic directly.

---

### BankingApp.Tests

Contains unit tests written with xUnit.

The tests cover the main business logic from the Core project.

---

## Features

### Account Management

* Create bank accounts
* Generate unique account numbers
* Search accounts by account number
* Get all accounts

### Deposit

* Deposit money into an account
* Reject zero or negative deposits
* Add successful deposits to transaction history

### Withdraw

* Withdraw money from an account
* Reject zero or negative withdrawals
* Reject withdrawals with insufficient funds
* Add successful withdrawals to transaction history

### Transfer

* Transfer money between accounts
* Reject transfers to null accounts
* Reject transfers to the same account
* Reject zero or negative transfer amounts
* Reject transfers with insufficient funds
* Add transfer transactions to both sender and receiver history

### Transaction History

* Store transaction type
* Store transaction amount
* Store transaction date
* Return formatted transaction history through the API

### JSON Persistence

Accounts and transactions are saved to a local JSON file.

This means data can survive after stopping and restarting the API.

The saved data is stored in:

```text
BankingApp.Api/Data/accounts.json
```

---

## API Endpoints

### Health Check

```http
GET /
```

Returns a simple message confirming the API is running.

Example response:

```text
Banking API is running
```

---

### Get All Accounts

```http
GET /accounts
```

Returns all accounts.

Example response:

```json
[
  {
    "ownerName": "John Doe",
    "accountNumber": "123",
    "balance": 1000
  }
]
```

---

### Get Account By Number

```http
GET /accounts/{accountNumber}
```

Example:

```http
GET /accounts/123
```

Returns account details.

Example response:

```json
{
  "ownerName": "John Doe",
  "accountNumber": "123",
  "balance": 1000
}
```

---

### Create Account

```http
POST /accounts
```

Request body:

```json
{
  "ownerName": "John Doe"
}
```

Example response:

```json
{
  "ownerName": "John Doe",
  "accountNumber": "123",
  "balance": 0
}
```

---

### Deposit Money

```http
POST /accounts/{accountNumber}/deposit
```

Example:

```http
POST /accounts/123/deposit
```

Request body:

```json
{
  "amount": 100
}
```

Example response:

```json
{
  "message": "Deposit successful.",
  "balance": 1100
}
```

---

### Withdraw Money

```http
POST /accounts/{accountNumber}/withdraw
```

Example:

```http
POST /accounts/123/withdraw
```

Request body:

```json
{
  "amount": 50
}
```

Example response:

```json
{
  "message": "Withdraw successful.",
  "balance": 1050
}
```

---

### Transfer Money

```http
POST /accounts/{accountNumber}/transfer
```

Example:

```http
POST /accounts/123/transfer
```

Request body:

```json
{
  "receiverAccountNumber": "321",
  "amount": 100
}
```

Example response:

```json
{
  "message": "Transfer successful.",
  "senderBalance": 950,
  "receiverBalance": 100
}
```

---

### Get Transaction History

```http
GET /accounts/{accountNumber}/transactions
```

Example:

```http
GET /accounts/123/transactions
```

Example response:

```json
[
  {
    "type": "Deposit",
    "amount": 1000,
    "date": "2026-06-26 17:48:54"
  },
  {
    "type": "Transfer",
    "amount": 100,
    "date": "2026-06-26 17:49:45"
  }
]
```

---

## Core Classes

### BankAccount

Represents a single bank account.

Main responsibilities:

* Store owner name
* Store account number
* Store balance
* Deposit money
* Withdraw money
* Transfer money to another account
* Store transaction history

---

### BankSystem

Represents the banking system.

Main responsibilities:

* Store all accounts
* Create accounts
* Find accounts by account number
* Return all accounts
* Load saved accounts into the system

---

### Transaction

Represents a single transaction.

Stores:

* Transaction type
* Amount
* Date

---

### TransactionType

Enum used to describe transaction types.

```csharp
public enum TransactionType
{
    Deposit,
    Withdraw,
    Transfer
}
```

---

### OperationResult

Represents the result of an operation.

Stores:

* Whether the operation succeeded
* A message explaining the result

Example:

```csharp
new OperationResult(true, "Deposit successful.");
```

---

## Persistence

The API uses JSON persistence through the `IBankStorage` interface and `JsonBankStorage` implementation.

### IBankStorage

Defines what a storage service must do:

```csharp
List<BankAccount> LoadAccounts();

void SaveAccounts(IReadOnlyList<BankAccount> accounts);
```

### JsonBankStorage

Handles saving and loading accounts from:

```text
BankingApp.Api/Data/accounts.json
```

The API loads saved accounts when it starts and saves accounts after successful changes such as:

* Creating an account
* Depositing money
* Withdrawing money
* Transferring money

---

## Dependency Injection

The API uses dependency injection to manage services.

In `Program.cs`:

```csharp
builder.Services.AddSingleton<BankSystem>();
builder.Services.AddSingleton<IBankStorage, JsonBankStorage>();
```

`BankSystem` is registered as a singleton because the app currently stores accounts in memory while the API is running.

`IBankStorage` is mapped to `JsonBankStorage`, which means the API depends on the interface, not directly on the implementation.

---

## Testing

The project uses xUnit for unit testing.

Tests are located in:

```text
BankingApp.Tests
```

Test coverage includes:

### BankSystemTests

* Creating accounts
* Finding accounts
* Getting all accounts
* Invalid account creation

### BankAccountTests

* Constructor validation
* Deposit success and failure cases
* Withdraw success and failure cases
* Transfer success and failure cases
* Transaction history behavior

### TransactionTests

* Transaction creation
* Type, amount, and date initialization

### OperationResultTests

* Successful result creation
* Failed result creation

---

## Running the Tests

From the solution folder, run:

```bash
dotnet test
```

Or use Visual Studio:

```text
Test -> Run All Tests
```

---

## Running the API

Set `BankingApp.Api` as the startup project and run it.

Swagger should open automatically.

Swagger URL usually looks like:

```text
https://localhost:7031/swagger
```

Through Swagger, you can test all API endpoints directly in the browser.

---

## Example API Flow

1. Create first account:

```json
{
  "ownerName": "John Doe"
}
```

2. Create second account:

```json
{
  "ownerName": "Vasil"
}
```

3. Deposit money into the first account:

```json
{
  "amount": 1000
}
```

4. Transfer money to the second account:

```json
{
  "receiverAccountNumber": "321",
  "amount": 100
}
```

5. Check all accounts.

6. Check transaction history.

7. Stop and restart the API.

8. Check that the accounts are still loaded from the JSON file.

---

## Technologies Used

* C#
* .NET
* ASP.NET Core Minimal API
* xUnit
* Swagger / OpenAPI
* JSON serialization
* Dependency Injection
* Visual Studio

---

## Current Limitations

This is a learning project, so some features are intentionally simple.

Current limitations:

* No real database yet
* No authentication or users
* No account deletion
* No concurrency handling
* JSON file storage is local only
* Account numbers are simple generated numbers
* No frontend UI yet

---

## Possible Future Improvements

Planned or possible next steps:

* Move API endpoints into separate endpoint extension files
* Add integration tests for the API
* Add database persistence with Entity Framework Core
* Add SQLite or SQL Server
* Add account deletion or account closing
* Add account types such as Checking and Savings
* Add validation with FluentValidation
* Add a frontend UI
* Add authentication
* Improve transaction descriptions
* Add pagination/filtering for transaction history
* Add logging

---

## What I Learned

This project helped practice:

* C# classes and encapsulation
* Object-oriented programming
* Private fields and read-only access
* Result objects
* Unit testing with xUnit
* Testing success and failure cases
* ASP.NET Core Minimal APIs
* Swagger documentation
* Dependency injection
* JSON serialization and deserialization
* Basic persistence
* Separating Core logic from API logic

---

## Status

The project is currently a working small banking backend with:

* Core banking logic
* Unit tests
* Minimal Web API
* Swagger documentation
* Request and response models
* JSON persistence

This project is built mainly for learning and practice.
**
