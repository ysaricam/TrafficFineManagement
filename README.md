# TrafficFineManagement

Modular monolith traffic fine management application built with .NET 10,
DDD, CQRS, EF Core, Dapper, PostgreSQL and transactional outbox processing.

## Modules

- `Users`: user profiles, password hashing, authentication and roles
- `Vehicles`: vehicles and vehicle usage assignments
- `TrafficFine`: fine creation, approval, rejection and completion workflow

## First run

1. Configure the PostgreSQL connection strings in configuration or user secrets.
2. Start the API project. In the Development environment, pending SQL files
   under `src/Database/Scripts` are applied automatically in numeric order.
3. Provision the initial administrator once through `POST /api/users/bootstrap`.
4. Sign in through the MVC login page at `/login`.

Applied scripts are recorded in `app."SchemaMigrations"`. Automatic migrations
are disabled by default outside Development and can be enabled with
`DatabaseMigrations__Enabled=true`.

## Authentication and roles

Authentication uses an HTTP-only cookie. Passwords are stored as salted
PBKDF2-SHA256 hashes.

| Operation | Allowed roles |
| --- | --- |
| View vehicles and fines | Any authenticated user |
| Create vehicles and assign users | Manager, Admin |
| Create users | Admin |
| Create fines and manager approval | Manager, Admin |
| Finance approval and completion | Finance, Admin |
| Reject fines | Manager, Finance, Admin |

API authentication endpoints:

- `POST /api/users/bootstrap`
- `POST /api/auth/login`
- `POST /api/auth/logout`

## Tests

Run all tests with:

```shell
dotnet test TrafficFineManagement.slnx
```

The API integration test requires Docker. It starts a temporary PostgreSQL
container, applies every migration and verifies authentication, authorization,
outbox projections and the complete fine approval workflow.
