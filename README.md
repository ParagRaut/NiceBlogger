# NiceBlogger


Niceblogger is very nice fake blogging API which uses [Postgres](https://www.postgresql.org) as database, [Redis](https://redis.io) as distributed caching system and [Seq](https://datalust.co/seq) as structured logging and tracing system for observability and analysis.

Project structure is based on clean architecture and effort is applied to make it loosely coupled.

For higher degree of cohesion domain and application layers are merged to "UseCases" which is more vertically sliced and includes everything related to respective features.

### Steps to debug this project
 - Open NiceBlogger.Api.sln in Visual Studio or Rider.
 - Select docker-compose as startup project and click run.

Project uses docker compose for container orchestration and ease of local development and debugging.

### Steps to run this on Windows/macOS

Open [docker-compose.override.yml](docker-compose.override.yml) and follow the instructions and then run commands as below.

```bash
cd NiceBlogger
docker compose up
```

After successful build interact with API in following ways
 - [NiceBlogger.Api.http](src/NiceBlogger.Api/NiceBlogger.Api.http) file
 - [OpenApi](https://localhost:5001/swagger/index.html) (Swagger) page
 - With either tools like postman/insomnia. 

 Launch [Seq dashboard](http://localhost:8081) Here

## Technical Details

Project includes 3 layers namely
 1. UseCases
 2. Infrastructure
 3. Api

Dependency direction is only towards innermost (UseCases) layer, i.e. Api >  UseCases < Infrastructure. This has been enforced by architecture unit tests.

## Features of each layer
 ### 1. Api Layer
 - Uses controllers which can consume and produce Json and Xml outputs.
 - Uses custom global exception handling mechanism to show validation errors and commonly thrown exception elegantly.
 - Uses Postgres as database and Redis for distributed caching mechanism.
 - Uses Seq for interactive logging aggregation and as observability platform.
 - Has health check endpoints for Api, Database and Cache.
 - Health check pings and Serilog request logging are also sinked to Seq.
 - Create development time seed data using Bogus.
 - Wires up all dependencies from every layer.

### 2. Infrastructure Layer
 - Implements all the abstractions defined in UseCases layer.
 - Implements repository caching with configurable expiration.
 - Implements decorator pattern as repository caching strategy.

### 3. UseCases Layer
 - It is the core and has all the business logic related to the application.
 - Separated by Features/UseCases to achieve higher cohesion.
 - Consist of common Entities, Exceptions & abstracted Commands, Queries and Repositories.
 - Defines MediatR pattern to achieve lose coupling with outer layers.
 - Has common validation behaviours added to MediatR pipeline.

## Automated Testing
### There are 3 types of automated testing implemented

#### Focus is much more on Integration and Functional testing rather than unit testing with mocks.

 1. Integration Tests
  - This layer demonstrates testing integration of abstract commands/queries in UseCases layer with concrete implementation in Infrastructure layer. 
  - Spins up whole application using WebApplicationFactory including database and caching in memory using TestContainers which uses docker to spin up new instance per test class.
 2. Functional Tests
  - These tests demonstrates how actual clients will interact with API on http level.
  - Theses tests uses default HttpClient from WebApplicationFactory to send GET & POST request to the controllers and assert on it's responses.
  - Spins up whole application using WebApplicationFactory including database and caching in memory using TestContainers which uses docker to spin up new instance per test class.
 3. Architecture Tests
  - These tests are written only for the sake of future proofing the architecture pattern. So if someone create cyclic dependency or inside out dependency these tests can prevent one from doing so.
  - It uses NArch together with simple xUnit assertions.

## Continuous Integration
 - Uses GitHub Actions for CI pipeline which triggers builds and run all the tests on every code commits.

 ## Seq
 - Used for interactive logging.
 - Have health check sinks for API, Database and Caching.

## Assumptions 
### Functional
 - Authors Entity is already exists and is populated by identity services i.e. user is logged in and while creating a new post with author context.
 - Create post has constrains such as Title should fall between 8 to 80 chars, Description and Content should have minimum length of 20 chars

### Technical
 - Api is hosted behind an application/API gateway (and or load balancer), so no resiliency and circuit breaker is implemented.
 - Api, Database, Cache store & Logging Aggregator is hosted on same network/vNet.
 - Visual studio 2022 on Windows or Rider on macOS/Linux is used for local debugging.
 - [PUT] & [Delete] endpoints for post is already implemented.
 - [GET] GetAllPosts is implemented plainly for testing purpose.
 
 #### Caveats include
 - Non conventional separation between Domain & Application layer.
 - Separate Read & Write i.e. IReadRepository and IWriteRepository could have been part of UseCases layer for more readable separation of concern and CQRS.
 - Naming conventions might make someone mad 😜. (Maybe use of this emoji as well.)
 - It's not the greatest solution but also not terrible considering the time spent on it.
