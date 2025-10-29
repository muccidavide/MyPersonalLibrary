# 📚 MyPersonalLibrary - Full-Stack Library Catalog

## Project Overview

**MyPersonalLibrary** is a full-stack demonstration application designed to catalog and manage a personal book collection. This project was developed as a practical exercise to showcase the effecti[...]  

The application allows users to quickly search for books within their catalog and perform comprehensive **CRUD** (Create, Read, Update, Delete) operations for managing book entries.

-----

## Technology Stack

The project leverages a modern and industry-standard technology stack:

| Component | Technology | Description |
| :--- | :--- | :--- |
| **Database** | **SQL Server** | Stores catalog data (titles, authors, publication years, ISBNs, etc.). |
| **Backend / API** | **.NET (C#)** | Implements a RESTful API to handle business logic and database interactions. |
| **Frontend** | **Vue.js** | Provides a dynamic and reactive user interface (UI) for catalog display and CRUD form management. |

-----

## Key Features

### 🔍 End-User Functionality

  * **Advanced Search (con lazy loading):** Filter the catalog by title, author, publication year, or ISBN. Implementata la ricerca avanzata con lazy loading per caricare i risultati on-demand e migliorare le performance lato client e server.
  * **Catalog Browsing:** Intuitive and paginated navigation through the entire library.
  * **Book Details:** View comprehensive metadata for each book entry.

  ### 🧰 Implementazioni recenti

  * È stata aggiunta la ricerca avanzata con lazy loading (caricamento on-demand dei risultati) per migliorare le performance e l'esperienza utente.
  * Gestione globale delle eccezioni implementata a livello di API e frontend.
  * Logging centralizzato: i log vengono persistiti sia su database che su file per facilitare il monitoraggio e il troubleshooting.

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

### 1. Database Setup

1.  Ensure your SQL server instance is running.
2.  Navigate to the `SQL/` directory and execute the **`01_Schema.sql`** script to create the necessary tables (e.g., `[dbo].[book]`).
3.  *Optional:* Execute the **`02_SeedData.sql`** script to populate the database with initial sample entries.

### 2. Starting the Backend (.NET API)

1.  Update the connection string (`DefaultConnection`) in the **`MyPersonalLibrary.Api/appsettings.json`** file to match your local SQL connection details.
2.  Navigate to the API project directory and run the application:

```bash
# Navigate to the API project directory
cd MyPersonalLibrary.Api
dotnet restore
dotnet run
```

*The API will be running on [http://localhost:XXXX].*

### 3. Starting the Frontend (Vue.js)

1.  Navigate to the Frontend project directory:
2.  Install dependencies and start the development server:

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