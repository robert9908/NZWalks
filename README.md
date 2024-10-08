## Project Title: NZWalks API

### Project Overview
The NZWalks API is a RESTful web service built using ASP.NET Core. It provides endpoints for managing walks, regions, and user authentication in a secure and efficient manner. This project demonstrates the implementation of JSON Web Token (JWT) authentication, automatic object mapping using AutoMapper, and custom middleware for handling specific application requirements.

### Key Features
1. JWT Authentication:
  - The API implements secure user authentication using JSON Web Tokens. Upon successful login, users receive a JWT, which is required for accessing protected endpoints. This ensures that only authorized users can perform certain actions.
  - The token has a configurable expiration time, enhancing security.
  - The API validates the token on each request, ensuring that it is still valid and has not been tampered with.

2. Custom Middleware:
  - Custom middleware has been implemented to handle specific cross-cutting concerns, such as logging request and response details, handling exceptions, and enforcing any required rules (e.g., ensuring that a certain header is present).
  - Middleware is added to the request processing pipeline to perform operations before and after the routing of requests.

3. AutoMapper Integration:
  - AutoMapper is used to simplify the mapping between domain models and Data Transfer Objects (DTOs). This reduces boilerplate code and improves maintainability.
  - It automates the conversion of complex types, allowing developers to focus on the business logic instead of worrying about manual object property assignments.

4. API Endpoints:
  - Authentication: Endpoints for user login and registration, providing JWT upon successful authentication.
  - Walks Management: CRUD operations for creating, reading, updating, and deleting walks, including validation rules.
  - Regions Management: Similar CRUD functionality for managing geographical regions.
  - Image Uploads: Endpoints for uploading images related to walks or regions, utilizing both local and database storage options.

5. Exception Handling:
  - Custom middleware captures any unhandled exceptions during the middleware pipeline and returns a structured response with appropriate HTTP status codes.
  - This ensures that clients receive meaningful error messages without exposing implementation details.

### Tech Stack
- Backend: ASP.NET Core Web API
- Authentication: JWT (JSON Web Tokens)
- Database: SQL Server (with Entity Framework Core for ORM)
- Object Mapping: AutoMapper
- Middleware: Custom middleware for logging and error handling
- Configuration: appsettings.json for environment-specific settings
