# TinyHome To-Do API

To-do list backend for the TinyHome exercise: create tasks, list them with a `completed`
filter and a sort, fetch / update / delete by id. ASP.NET Core 8, EF Core 8, Postgres 16.

## Running it

```bash
docker compose up --build
```

Postgres and the API come up, migrations run on boot, and the API is published on
`http://localhost:8090`:

- `/api/tasks` and the rest of the CRUD surface
- `/swagger` for Swagger UI and the full request and response contract
- `/swagger/v1/swagger.json` for the raw OpenAPI document

```bash
curl localhost:8090/api/tasks   # [] on a fresh database
```

To start with sample data in the table instead of an empty one, add the `seed` profile:

```bash
docker compose --profile seed up --build
```

`docker compose down -v` stops everything and drops the database volume.

Copy `.env.example` to `.env` to change the database name, user or password. The defaults
work as they are.

## Configuration

The API reads one connection string, `ConnectionStrings__DefaultConnection`, which compose
builds from `POSTGRES_DB`, `POSTGRES_USER` and `POSTGRES_PASSWORD` (see `.env.example`).
The base `appsettings.json` has no connection string of its own.

Ports: the API is on host `8090` (`8080` in the container), Postgres on host `5433`
(`5432` is usually already taken by a local one).

## Seed data

The `seed` profile adds a throwaway container that waits for the `Tasks` table, then pipes
`scripts/seed-dev-data.sql` into Postgres. It is 15 rows with a spread of due dates,
created dates and completed flags, enough for the filter and the sort to show something.
The script opens with `TRUNCATE`, so re-running it resets to the same 15 rows rather than
piling on.

Against an already-running stack:

```bash
docker compose exec -T postgres psql -U postgres -d tinyhome < scripts/seed-dev-data.sql
```

## Migrations

Migrations apply on startup (`Database.Migrate()`, with a few retries so it survives
Postgres not being up yet), so there is nothing to run by hand. To add one after changing
an entity, using the local `dotnet-ef` tool:

```bash
dotnet tool restore
dotnet ef migrations add <Name> \
  --project src/TinyHomeTodo.Infrastructure \
  --startup-project src/TinyHomeTodo.Api
```

## Layout

```
src/
  TinyHomeTodo.Api             controllers, exception middleware, Swagger, DI
  TinyHomeTodo.Application     entities, DTOs, TaskService, validation
  TinyHomeTodo.Infrastructure  DbContext, EF config, repository, migrations
```

`Application` depends on nothing. `Api` and `Infrastructure` depend on it, and `Api` pulls
in `Infrastructure` only to wire up EF at startup.

## Deviations from the spec

- Routes are `/api/tasks`, not `/tasks`.
- `sort_by` takes `dueDate`, `-dueDate`, `createdDate`, `-createdDate`. The spec's
  `+dueDate` can't work as a query parameter, `+` decodes to a space, so ascending is
  just the bare name. A null `dueDate` sorts last either way.
- `completed` is optional. Leave it off to get everything.
- `DELETE` returns `204`, not `200`.
- A malformed id in the path or a broken JSON body comes back as `400 { "message": ... }`
  like every other error, not ASP.NET's default problem-details shape.
- `dueDate` has to be UTC (`2026-08-30T00:00:00Z`). Anything else is a `400`.
- Unknown fields in a request body are ignored. `id` on POST and `createdDate` on PUT are
  server-owned.
- Swagger is served in every environment, including the Production container.

## Assumptions

- `id` is a caller-visible GUID, created in the domain layer rather than by the database.
- `taskDescription` only has to be non-blank. I didn't cap the length, the spec doesn't
  give one and the column is `text`.
- `dueDate` is a nullable instant. No "all day" handling.
- No auth, single user, every task is visible to everyone.
- Controllers rather than Minimal API, mostly for the consistent `400` shape without extra
  wiring.

## Not done

Deliberate cuts for the exercise:

- **Tests.** No test project. I checked the endpoints by hand with Swagger and curl.
  Unit tests for `TaskService` and an integration pass over `WebApplicationFactory` are
  the first thing I'd add.
- **Auth.** Single-user, no authentication. A real scheme lands in the composition root
  and a per-caller filter on the queries. The domain does not change.
- **Pagination.** `GET /api/tasks` returns the whole table. Real volume would need
  `page` / `pageSize` and a `{ data, total }` envelope.
