Order Management System API
===========================

This project is a .NET 8 Web API that simulates an order management system. It's designed with a layered architecture, emphasizing clean code, proper design patterns, and efficient implementation within a given set of requirements. For demonstration and development, the system utilizes **in-memory data storage**, eliminating the need for a persistent database setup.

Table of Contents
-----------------

*   [Overview](#Overview)
    
*   [Features](#features)
    
*   [Architecture & Design](#architecture--design)
    
*   [In-Memory Data Storage](#in-memory-data-storage)
    
*   [Testing](#testing)
    
*   [API Documentation (Swagger/OpenAPI)](#api-documentation-swaggeropenapi)
    
*   [Performance Considerations](#performance-considerations)
    
*   [Getting Started](#getting-started)
    
*   [Assumptions](#assumptions)
    

Overview
--------

This project focuses on implementing key functionalities for an order management system, including a dynamic discounting system, robust order status tracking, and analytical reporting. It demonstrates proficiency in .NET development, API design, testing, and architectural best practices.

Features
--------

The API provides the following core functionalities:

1.  **Order Management:**
    
    *   **Create Orders:** Place new orders with customer and product details.
        
    *   **Retrieve Orders:** Fetch specific order details by ID.
        
    *   **Order Items:** Orders consist of multiple line items, each linked to a product.
        
2.  **Discounting System:**
    
    *   Applies **different promotion rules** based on customer segments (e.g., Platinum, New Customer) and order history.
        
    *   Supports various discount types (e.g., percentage, fixed amount).
        
    *   Uses the **Strategy Pattern** for extensible discount rules.
        
3.  **Order Status Tracking:**
    
    *   Tracks order status through predefined **state transitions** (e.g., Pending -> Processing -> Shipped -> Delivered).
        
    *   Enforces **valid transitions** to prevent invalid status changes.
        
    *   Exposes an endpoint to update order status.
        
4.  **Order Analytics:**
    
    *   Provides an endpoint to retrieve **key order statistics**.
        
    *   Calculates metrics like:
        
        *   Total number of orders.
            
        *   Average order value.
            
        *   Average fulfillment time (from order placement to delivery).
            
        *   Order count by status.
            
        *   Total sales by customer segment.
            
        *   Top-selling products by quantity.
            

Architecture & Design
---------------------

The project is structured following a **layered architecture** to promote separation of concerns, maintainability, and scalability.

*   **OrderManagementSystem.API (Presentation Layer):**
    
    *   Handles HTTP requests and responses.
        
    *   Contains Controllers, DTOs (Data Transfer Objects), and Application Services.
        
    *   Application Services (OrderService, DiscountService, OrderTrackingService, AnalyticsService) orchestrate business logic by interacting with Domain Services and Repositories.
        
*   **OrderManagementSystem.Core (Domain Layer):**
    
    *   Houses the core business logic, entities (Order, Customer, Product, Discount, OrderItem), value objects, and domain interfaces (e.g., IDiscountRule, IOrderRepository).
        
    *   Entities encapsulate their own behavior (e.g., Order status transitions).
        
    *   Defines custom exceptions for domain rule violations (e.g., InvalidOrderStatusTransitionException).
        
*   **OrderManagementSystem.Infrastructure (Infrastructure Layer):**
    
    *   Provides concrete implementations for interfaces defined in the Core layer.
        
    *   Contains **in-memory repository implementations** (e.g., InMemoryOrderRepository) for data persistence during runtime.
        
    *   Houses specific IDiscountRule implementations (e.g., NewCustomerDiscountRule).
        

In-Memory Data Storage
----------------------

For rapid development and ease of demonstration, this project uses **in-memory data storage** instead of a traditional database.

*   **Implementation:** Repositories are implemented using ConcurrentDictionary to store data in the application's memory.
    
*   **Seeding:** Initial mock data for customers, products, and orders is pre-loaded into these in-memory repositories when the application starts.
    
*   **Implication:** Any changes made via the API will persist only for the lifetime of the application instance. Restarting the application will reset the data to its initial seeded state.
    

Testing
-------

The project includes both **Unit Tests** and **Integration Tests** to ensure code quality and functionality.

*   **OrderManagementSystem.UnitTests:**
    
    *   Focuses on testing individual units of code in isolation (e.g., DiscountService calculations, Order entity state transitions).
        
    *   Uses **Moq** for mocking dependencies.
        
*   **OrderManagementSystem.IntegrationTests:**
    
    *   Tests the interaction between multiple components, including API endpoints, services, and the in-memory repositories.
        
    *   Utilizes Microsoft.AspNetCore.Mvc.Testing to host an in-memory test server.
        

API Documentation (Swagger/OpenAPI)
-----------------------------------

The API is fully documented using Swagger/OpenAPI.

*   **Access:** Once the application is running, navigate to https://localhost:/swagger (or http://localhost:/swagger if HTTPS is not configured) in your browser.
    
*   **Example Values:** Request and response bodies in the Swagger UI are populated with realistic **mock data examples** using the Swashbuckle.AspNetCore.Filters library. This helps API consumers understand the expected data formats.
    

Performance Considerations
--------------------------

While the current version uses in-memory data, the design anticipates future performance optimizations.

*   **Asynchronous Operations (async/await):** All I/O-bound operations (even the in-memory ones) are asynchronous to ensure responsiveness and efficient resource utilization, laying the groundwork for a real database.
    
*   **Repository Pattern:** Decouples data access, allowing for easy integration of performance optimizations at the data layer (e.g., caching, optimized database queries, indexing) if a persistent database were introduced.
    
*   **Calculations:** Analytics calculations are performed efficiently in-memory, but for large datasets, these could be offloaded or optimized with more advanced data querying techniques.
    

Getting Started
---------------

To run this project locally:

1.  git clone cd OrderManagementSystem
    
2.  dotnet restore
    
3.  dotnet run --project src/OrderManagementSystem.APIThe API will typically run on https://localhost:7080 (or a similar port). The console output will show the exact URLs.
    
4.  **Access Swagger UI:** Open your web browser and navigate to the /swagger endpoint (e.g., https://localhost:7080/swagger).
    

Assumptions
-----------

*   **In-Memory Persistence:** Data is not persisted beyond the application's runtime. All data resets upon application restart.
    
*   **Simplified Data Models:** Entity properties and relationships are simplified for the scope of this assessment.
    
*   **Error Handling:** Basic error handling is in place, but a production system would require more comprehensive global error handling strategies (e.g., middleware).
    
*   **Security:** Authentication and authorization mechanisms are not implemented in this version.
    
*   **Concurrency:** In-memory repositories use ConcurrentDictionary for basic thread safety, suitable for demonstration purposes. More advanced concurrency control might be needed for high-load production scenarios.
