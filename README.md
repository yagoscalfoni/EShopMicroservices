# 🛒 EShopMicroservices

A fully modular, cloud-native **e-commerce backend** application built with **.NET 8**, implementing real-world microservices architecture using best practices such as **DDD**, **CQRS**, **Clean Architecture**, **JWT Authentication**, and asynchronous messaging with **Azure Service Bus**.

---

## 📌 Overview

This project is a result of my learning and hands-on implementation of modern software design principles and cloud-native development using .NET 8.  
It provides a solid foundation for any enterprise-level distributed system with scalable, testable, and maintainable services.

---

## 🧱 Architecture & Design

- ✅ **Domain-Driven Design (DDD)**
- ✅ **CQRS** – Command Query Responsibility Segregation
- ✅ **Vertical Slice Architecture** with feature folders
- ✅ **Clean Architecture** with separation of concerns
- ✅ **Microservices-based Architecture** (Catalog, Basket, Ordering, Identity, etc.)
- ✅ **JWT-based Authentication** via Identity microservice
- ✅ **API Gateway** using YARP Reverse Proxy
- ✅ **Sync Communication** with **gRPC**
- ✅ **Async Communication** with **Azure Service Bus + MassTransit**
- ✅ **Docker & Docker Compose** for containerized orchestration
- ✅ **Health Checks**, **OpenTelemetry**, and observability practices

---

## 🔐 Authentication

Authentication and user management are handled via the **Identity microservice**, which issues **JWT tokens** to secure access across all APIs.

---

## 📚 Technologies Used

| Layer                      | Stack / Tools                                                                 |
|---------------------------|-------------------------------------------------------------------------------|
| **Framework**             | .NET 8, ASP.NET Core Minimal APIs, Razor Pages, C# 12                        |
| **API Gateway**           | YARP (Yet Another Reverse Proxy)                                             |
| **Messaging**             | Azure Service Bus + MassTransit                                             |
| **Sync Communication**    | gRPC                                                                         |
| **Databases**             | PostgreSQL, Redis, SQLite, SQL Server, Marten (Document DB on PostgreSQL)    |
| **Libraries**             | MediatR, Mapster, Carter, Refit, FluentValidation, EF Core                   |
| **Auth**                  | JWT, Identity Service                                                        |
| **Containerization**      | Docker, Docker Compose                                                       |
| **Monitoring**            | Health Checks, Logging, OpenTelemetry                                        |

---

## 📂 Microservices

- **Catalog.API** – Product management using CQRS and Marten
- **Basket.API** – Shopping cart with Redis caching
- **Ordering.API** – Order processing with DDD and gRPC
- **Discount.Grpc** – Microservice for applying discounts
- **Identity.API** – Handles authentication and JWT token issuing
- **User.API** – Basic user registration and login endpoints
- **Gateway** – YARP-based API gateway routing traffic

### User API

`POST /user-service/register` – create a new user account

---

## ▶️ Running the Project

> Prerequisites: [.NET 8 SDK](https://dotnet.microsoft.com/download), [Docker](https://www.docker.com/), [Postman](https://www.postman.com/)

```bash
git clone https://github.com/yagoscalfoni/EShopMicroservices.git
cd EShopMicroservices

# Ambiente de debug/local (compose + sobreposição de debug)
docker compose -f src/docker-compose.yml -f src/docker-compose.debug.yml up --build

# Ambiente de produção (variáveis vindas do Key Vault para o .env.production)
docker compose -f src/docker-compose.yml -f src/docker-compose.prod.yml --env-file .env.production up -d
```

> Consulte `docs/keyvault-setup.md` para entender como popular o `.env.production` a partir do Azure Key Vault de forma segura e automatizada.
