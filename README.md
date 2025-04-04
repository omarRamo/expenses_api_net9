# Expenses Sample Project

Welcome to the **Expenses Sample Project**, a .NET-based demonstration of Domain-Driven Design (DDD), Clean Architecture principles, and Entity Framework Core for persistence.

## Overview

This sample illustrates how you might structure an application to **create** and **list expenses**, with an emphasis on:

- Layered architecture (API, Application, Domain, Infrastructure)
- Domain-driven design principles
- Data persistence in a SQL database
- Validation rules and sorting features

Although inspired by a real-world scenario, the code here is mainly for **demonstration** and **learning** purposes.

## Features

### 1. Creating an Expense

The application validates and creates new expenses, enforcing business rules such as:

- No future-dated expenses
- No expenses older than three months
- Mandatory comment field
- No duplicate expenses (date + amount)
- Matching currency between the expense and the user

### 2. Listing Expenses

The application can list all expenses for a specific user and supports sorting them by date or amount. It also includes:

- A user’s full name in the format `{FirstName} {LastName}` (e.g., "Anthony Stark")
- A filtering mechanism to retrieve expenses by user

### 3. Data Model

**Entities** include:

- **Expense:** Holds details about the date, amount, currency, type, and comment.
- **User:** The individual who incurs expenses, having a first name, last name, and default currency.
- **ExpenseType:** Categorizes expenses (e.g., Restaurant, Hotel, Misc).
- **Currency:** Associates a code and/or name to an expense or user.

## Architecture

### Domain-Driven Design (DDD)

This sample applies basic DDD patterns:

- **Entities**: Core objects with identity (Expense, User, etc.).
- **Value Objects**: Potentially used for currency representation.
- **Domain Services**: Where heavier business logic resides (e.g., `ExpenseService`).
- **Repositories**: (`IExpenseRepository`, `IUserRepository`, etc.) for data persistence and to isolate infrastructure concerns.

### Application Layer

A **Facade** (`ExpenseFacade`) in the application layer simplifies interaction with domain logic. It manages:

- Mapping between DTOs and domain objects.
- Sorting and filtering logic.
- Validation flows before persisting data.

### Infrastructure & Persistence

- **Entity Framework Core** for SQL database operations.
- **Repositories** that implement the domain interfaces.
- **Migrations** to set up initial user data (e.g., Anthony Stark, Natasha Romanova) with respective currencies.

## How to Run

1. **Clone the Repository**  
```bash
git clone https://github.com/omarRamo/expenses_api_net9.git 
cd expenses_api_net9
```

2. **Set Up the Database**

- Update `appsettings.json` or connection strings to point to a local or remote SQL database.
- Run EF Core migrations to initialize tables:

```bash
dotnet ef database update
```

3. **Build and Run**

```bash
dotnet build
dotnet run
```

The API should be available at `http://localhost:<port>` (default port depends on your `.launchSettings.json`).

## Test the Endpoints

Use Swagger UI or Postman to interact with the API.

- Create new expenses, list expenses by user, and sort them by date or amount.

### Validation Rules

- **Date Constraints**: No future expenses, none older than three months.
- **Mandatory Fields**: Comment must not be empty.
- **Duplicates**: No two expenses with the same date and amount for the same user.
- **Currency Match**: The expense currency must match the user’s currency.

### Example Endpoints

- **Create an Expense**:
  ```
  POST /api/v1/expenses
  ```

- **Get Expense by ID**:
  ```
  GET /api/v1/expenses/{id}
  ```

- **List Expenses (filter, sort)**:
  ```
  GET /api/v1/expenses?userId={userId}&sortBy={expenseDate|amount}&sortOrder={Ascending|Descending}
  ```

## Roadmap / Future Enhancements

- Add authentication and authorization for secure endpoints.
- Extend domain with additional features such as expense approval workflows or user role management.
- Include more comprehensive integration tests.

## License

This sample project is provided as-is for learning and demonstration purposes. You’re free to fork it and adapt it for your own needs. Please check the repository license file for more details.

Thanks for checking out this sample project! Feel free to submit issues, suggestions, or pull requests if you have ideas for improvements.

