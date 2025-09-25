

# 📚 MyPersonalLibrary - Full-Stack Library Catalog

## Project Overview

**MyPersonalLibrary** is a full-stack demonstration application designed to catalog and manage a personal book collection. This project was developed as a practical exercise to showcase the effective integration and communication between a **SQL database**, a robust **RESTful API** developed in **.NET (C\#)**, and a modern, reactive user interface built with **Vue.js**.

The application allows users to quickly search for books within their catalog and perform comprehensive **CRUD** (Create, Read, Update, Delete) operations for managing book entries.

-----

## Technology Stack

The project leverages a modern and industry-standard technology stack:

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Database** | **SQL Server** | Stores catalog data (titles, authors, publication years, ISBNs, etc.). |
| **Backend / API** | **.NET (C\#)** | Implements a RESTful API to handle business logic and database interactions. |
| **Frontend** | **Vue.js** | Provides a dynamic and reactive user interface (UI) for catalog display and CRUD form management. |

-----

## Key Features

### 🔍 End-User Functionality

  * **Advanced Search:** Filter the catalog by title, author, publication year, or ISBN.
  * **Catalog Browsing:** Intuitive and paginated navigation through the entire library.
  * **Book Details:** View comprehensive metadata for each book entry.

### ⚙️ Management Functionality (CRUD)

  * **C - Create:** Add new books to the database via a dedicated form.
  * **R - Read:** Retrieve and display all books or a single record.
  * **U - Update:** Modify the details of an existing book.
  * **D - Delete:** Permanently remove a book from the catalog.

-----

## Architecture and Design

The project employs a three-tier architecture:

1.  **Data Layer (SQL Server):** Optimized table structure for efficient searching.
2.  **Logic/API Layer (.NET):** The API uses a **Repository Pattern** or **Service Layer** approach to separate business logic from data access, ensuring clean and testable code.
3.  **Presentation Layer (Vue.js):** The frontend communicates with the API via HTTP/Axios calls, operating as a **Single Page Application (SPA)** for a smooth user experience.

-----

## Local Setup and Running Instructions

### Prerequisites

  * [.NET SDK (Latest Stable Version)]
  * [Node.js / npm (Latest Stable Version)]
  * [SQL Server or equivalent local database setup]

### 1\. Database Setup

1.  Create a new SQL database named `MyPersonalLibrary`.
2.  Execute your database migration scripts or manually create the necessary tables (e.g., `Book`).
3.  Update the connection string (`DefaultConnection`) in the API project's **`appsettings.json`** file to match your local setup.

### 2\. Starting the Backend (.NET API)

```bash
# Navigate to the API project directory
cd MyPersonalLibrary.Api
dotnet restore
dotnet build
dotnet run
```

*The API will be running on [http://localhost:XXXX].*

### 3\. Starting the Frontend (Vue.js)

```bash
# Navigate to the Frontend project directory
cd MyPersonalLibrary.Frontend
npm install
npm run serve
```

*The user interface will be available on [http://localhost:8080].*

-----

## Portfolio Objectives

This project was developed to demonstrate the following technical skills:

  * **Full-Stack Proficiency:** Managing the entire application lifecycle from database design to frontend deployment.
  * **RESTful API Design:** Implementing clean and efficient data access endpoints using .NET.
  * **Frontend State Management:** Building a reactive UI with Vue.js, demonstrating efficient component design and data flow.
  * **Data Integration:** Practical application of database connectivity and manipulation (e.g., via Entity Framework Core).

-----
