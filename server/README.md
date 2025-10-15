# OurCity Server

## Required Setup

- Ensure you have [Docker Desktop](https://www.docker.com/products/docker-desktop/).
- Ensure you have [.NET 9](https://dotnet.microsoft.com/en-us/download).
- Ensure you have .env files setup
  - To run the dev environment, you will need .env.development (even if empty) or docker compose will error
  - To run the prod environment, you will need .env.production (even if empty) or docker compose will error

## Configuration

- For secrets, do not commit to appsettings.json.
  - On local machine, can just do with .env
    - To see the formatting of .env files, see .env.example
    - The .env files you add should correspond to the environment
      - e.g. .env.development, .env.production

## IMPORTANT NOTE: ALL COMMANDS ARE WRITTEN WITH PRESUMPTION YOU ARE IN THE /SERVER FOLDER

## Running app with Docker

### Development Environment (HMR)

API documentation should be available at http://localhost:8000/scalar or http://localhost:8000/swagger

To (re)build image, and spin up .NET API and Postgres Docker containers in the background

```sh
docker compose up -d --build
```

To clean up the Docker containers

```sh
docker compose down
```

To run migrations

```sh
docker compose --profile migrate up ourcity.migrate.dev --build
```

### Production Environment

API documentation is not available for production.

To (re)build image, and spin up .NET API and Postgres Docker containers in the background

```sh
docker compose -f docker-compose.prod.yml up -d --build
```

To clean up the Docker containers

```sh
docker compose -f docker-compose.prod.yml down
```

To run migrations

```sh
docker compose -f docker-compose.prod.yml --profile migrate up ourcity.migrate.prod --build
```

## Tooling

### Get mandatory dotnet tools

```sh
dotnet tool restore
```

### Create migrations

```sh
dotnet ef migrations add <migration-name> -p OurCity.Api
```

### Run the tests

NOTE: There's a chance tests might take a long time on first start due to setting up Testcontainers.

```sh
dotnet test
```

For running tests, you can also run by type of test / what it tests

```sh
dotnet test --filter "Type=Unit"
dotnet test --filter "Type=Integration"
dotnet test --filter "Domain=Comment"
etc
```

Getting coverage
```sh
dotnet test --collect:"XPlat Code Coverage" && dotnet reportgenerator -reports:"**/OurCity.Api.Test/TestResults/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html && open coveragereport/index.html
```

Note: The coverage generation creates a TestResults entry in OurCity.Api.Test. If you don't delete, future runs for checking coverage might include them.

### Linting and formatting

Check formatting

```sh
dotnet csharpier check <file_path>
```

Run formatting

```sh
dotnet csharpier format <file_path>
```

Check analyzer errors (lint)

```sh
dotnet build -p lint=true
```