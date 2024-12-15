# TECHNICAL-ASSIGMENT

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


