# TECHNICAL-ASSIGMENT
[![TECHNICAL-ASSIGMENT workflow](https://github.com/PopaLorena/TECHNICAL-ASSIGMENT/actions/workflows/main.yml/badge.svg?branch=main)](https://github.com/PopaLorena/TECHNICAL-ASSIGMENT/actions/workflows/main.yml?query=branch%3Amain)
This project is a backend application designed to manage negotiations, proposals, counterproposals, and related entities (parties, items, users). It facilitates communication and interactions between parties within a negotiation context, supporting features such as user authentication, proposal management, and handling counterproposals.

## Features

- **User Management**: Register and log in users.
- **Party Management**: Create and manage parties involved in the negotiation process.
- **Item Management**: Add and retrieve items that are part of a proposal.
- **Proposal Management**: Create proposals for items, involve parties, and handle negotiations.
- **Counterproposal**: Support the creation and management of counterproposals.
- **Involved Parties**: Track the status of parties involved in negotiations (pending, accepted, or rejected).

## Technologies

- **Backend**: ASP.NET Core
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **API Documentation**: Swagger (using Swashbuckle)
- **AutoMapper**: To simplify object mapping between DTOs and database models
- **Authentication**: JWT (JSON Web Tokens) for user authentication

## Prerequisites

Before running the application, ensure you have the following installed:

- **.NET 8** (or higher)
- **SQL Server** (or use an in-memory database for development)
- **Postman/Swagger** for API testing

## Build
Run using powerhell

- ./build-app.ps1


## Run UnitTest
Run using powerhell this line:
-  dotnet test ./Assigment/Assigment.sln --configuration Release --logger "trx;LogFileName=test-results.trx"

## start the application
3. **Create the Database** (Code-First Approach):
   - Open PowerShell and navigate to the project directory.
   - Run the following command to apply migrations and create the database:
     ```powershell
     dotnet ef database update --project ./Assigment/Assigment.Infrastructure
     ```
   - If no migrations exist, generate one using:
     ```powershell
     dotnet ef migrations add InitialCreate --project ./Assigment/Assigment.Infrastructure
     ```
   - Then, run the `dotnet ef database update` command again to apply the migration.

4. **Run the Application**:
   - Use an IDE like Visual Studio and run the `Assigment` project directly.

5. **Access the API**:
   - Open your browser and navigate to `https://localhost:7179/swagger/index.html` to view the API documentation and test the endpoints.

6. **Testing the API**:
   - Use Swagger's UI to interact with the API or tools like Postman to test the endpoints.
   - Ensure that user authentication is tested by acquiring a JWT token from the `/login` endpoint and using it for secured endpoints.

Once running, the application is ready for use, supporting user management, proposals, counterproposals, and related functionalities.