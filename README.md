# 📚 MyPersonalLibrary - Full-Stack Library Catalog

## Project Overview

**MyPersonalLibrary** is a full-stack demonstration application designed to catalog and manage a personal book collection. This project was developed as a practical exercise to showcase end-to-end development skills.

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

  * **Advanced Search (with lazy loading):** Filter the catalog by title, author, publication year, or ISBN. The advanced search is implemented with lazy loading to fetch results on-demand and improve performance on both client and server sides.
  * **Catalog Browsing:** Intuitive and paginated navigation through the entire library.
  * **Book Details:** View comprehensive metadata for each book entry.

  * **Global Exception Handling:** Centralized exception handling implemented in the API and propagated to the frontend for consistent error responses and improved reliability.
  * **Centralized Logging:** Application logs are persisted to both the database and log files to simplify monitoring and troubleshooting.

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

## Portfolio Objectives

This project was developed to demonstrate the following technical skills:

  * **Full-Stack Proficiency:** Managing the entire application lifecycle from database design to frontend deployment.
  * **RESTful API Design:** Implementing clean and efficient data access endpoints using .NET.
  * **Frontend State Management:** Building a reactive UI with Vue.js, demonstrating efficient component design and data flow.
  * **Data Integration:** Practical application of database connectivity and manipulation (e.g., via Entity Framework Core).
