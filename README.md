# MyPersonalLibrary

A **full-stack web application** built to explore modern backend architecture using **ASP.NET Core, Vue.js and SQL Server / Azure SQL**.

The project focuses on **API design, data access patterns, and application observability** rather than the domain itself.
The frontend is implemented as a **Single Page Application (SPA)** that communicates with a REST API backend.

---

## 🏗 Architecture

The system follows a **SPA + API architecture**, keeping the frontend completely separated from the backend and data layer.

```
Vue.js SPA
     │
     │ HTTP / JSON
     ▼
ASP.NET Core Web API
     │
     │ Entity Framework Core
     ▼
SQL Server / Azure SQL
```

Key architectural aspects:

* layered backend architecture *(Controllers → Services → Data Access)*
* **RESTful API design**
* **Entity Framework Core** for persistence
* **server-side filtering and pagination**
* **centralized exception handling**
* **structured logging** (database + file)

---

## ⚙ Backend

The backend is implemented using **ASP.NET Core Web API** and focuses on clean separation of responsibilities.

Notable implementation aspects:

* **global exception handling middleware**
* **repository / service pattern**
* **LINQ-based querying** with Entity Framework Core
* consistent API response handling
* logging persisted to **database and log files** for observability

These patterns improve **maintainability, debugging and extensibility**.

---

## 🧩 Frontend

The frontend is built with **Vue.js** as a **Single Page Application**.

The UI communicates with the API through **Axios HTTP calls**, allowing the interface to dynamically update without full page reloads.

Main characteristics:

* component-based architecture
* reactive UI updates
* API-driven data flow

---

## 🗄 Data Layer

The application uses **SQL Server**, with compatibility for **Azure SQL Database**.

Persistence is managed through **Entity Framework Core**, enabling:

* strongly-typed queries
* migrations and schema evolution
* efficient search and filtering

## 🧠 Architecture Diagram

A simplified view of the application components and interactions.

```
            ┌───────────────────┐
            │     Vue.js SPA    │
            │  (Client / UI)    │
            └─────────┬─────────┘
                      │ HTTP / JSON
                      ▼
            ┌───────────────────┐
            │ ASP.NET Core API  │
            │ Controllers       │
            │ Services          │
            │ Repositories      │
            └─────────┬─────────┘
                      │
                      ▼
            ┌───────────────────┐
            │ Entity Framework  │
            │ Core (ORM)        │
            └─────────┬─────────┘
                      │
                      ▼
            ┌───────────────────┐
            │ SQL Server /      │
            │ Azure SQL DB      │
            └───────────────────┘
```

---

## 🚀 Technical Focus

This project was created to demonstrate practical experience with:

* **ASP.NET Core API development**
* **SPA architectures**
* **Entity Framework Core data access**
* **structured logging and exception handling**
* **SQL-based persistence in a cloud-ready environment**
