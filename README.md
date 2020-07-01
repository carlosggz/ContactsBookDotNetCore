# Contacts Book API example using dotnet core 3.1

API example using best practices:
- DDD (layers, services, repositories, entities, value objects, domain events, event bus, domain events subscribers)
- Unit of work
- TDD at all levels (Unit tests, integration tests and acceptance tests)
- Logging
- IoC (using the integrated dotnet core container)
- API Rest entrypoints and responses
- Swagger IO
- Docker compose
- Entity framework core with migrations (tests use in memory database, and the api uses the sql server provider)
- Dynamic register of subscribers to domain events (using an attributte decoration)

## Instructions ##

First, clone the repository.

To run directly:
- Change your parameters on the app settings file
- Run the application on your IDE or at the cli. (If you are in the development environment, migrations will be applied automatically.)
- Open your browser at http://localhost:5000/ to test the api

To run using docker:
- Run docker-compose up --build
(The composer will wait until the database is up, and the will the run the application. Migrations will be applied automatically.)
- Open your browser at http://localhost:5000/ to test the api
- Mail server will be available at http://localhost:1080/
