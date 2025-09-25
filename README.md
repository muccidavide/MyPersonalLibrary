📚 MyPersonalLibrary - Full-Stack Library Catalog
Project Overview
MyPersonalLibrary is a full-stack demonstration application designed to catalog and manage a personal book collection. This project serves as a practical exercise to showcase the effective integration and communication between a SQL database, a robust RESTful API developed in .NET, and a modern, reactive user interface built with Vue.js.

The application allows users to quickly search for books within their catalog and perform comprehensive CRUD (Create, Read, Update, Delete) operations for managing book entries.

Technology Stack
The project is built using a modern and industry-standard technology stack:

Component	Technology	Description
Database	SQL Server	Stores catalog data (titles, authors, publication years, ISBNs, etc.).
Backend / API	.NET (C#)	Implements a RESTful API to handle business logic and database interactions.
Frontend	Vue.js	Provides a dynamic and reactive user interface (UI) for catalog display and CRUD form management.
Dependency Management	NuGet (for .NET), npm/Yarn (for Vue.js)	Efficient management of external libraries and packages.

Esporta in Fogli
Key Features
🔍 End-User Functionality
Advanced Search: Filter the catalog by title, author, publication year, or ISBN.

Catalog Browsing: Intuitive and paginated navigation through the entire library.

Book Details: View comprehensive metadata for each book entry.

⚙️ Management Functionality (CRUD)
C - Create: Add new books to the database via a dedicated form.

R - Read: Retrieve and display all books or a single record.

U - Update: Modify the details of an existing book.

D - Delete: Permanently remove a book from the catalog.

Architecture and Design
The project employs a three-tier architecture:

Data Layer (SQL Server): Optimized table structure for efficient searching and key catalog relationships (e.g., Book table).

Logic/API Layer (.NET): The API acts as an intermediary. It utilizes a Repository Pattern or Service Layer approach to separate business logic from data access, ensuring clean, testable, and scalable code.

Presentation Layer (Vue.js): The frontend communicates with the API via HTTP/Axios calls, managing the application's state efficiently for a smooth user experience (SPA - Single Page Application).

Local Setup and Running Instructions
Prerequisites
[.NET SDK (Version X.X)]

[Node.js / npm (Version Y.Y)]

[SQL Server / SQL Express / Docker with SQL Image]

1. Database Setup
Create a new SQL database named MyPersonalLibrary.

Execute any migration scripts (if applicable) or manually create the Book table with necessary fields (ID, Title, Author, Year, etc.).

Update the connection string (DefaultConnection) in the API project's appsettings.json file.

2. Starting the Backend (.NET API)
Bash

# Navigate to the API project directory
cd MyPersonalLibrary.Api
dotnet restore
dotnet build
dotnet run
The API will be running on [http://localhost:XXXX].

3. Starting the Frontend (Vue.js)
Bash

# Navigate to the Frontend project directory
cd MyPersonalLibrary.Frontend
npm install  # or yarn install
npm run serve  # or yarn serve
The user interface will be available on [http://localhost:8080].

Portfolio Objectives
This project was developed to demonstrate the following technical skills:

Full-Stack Development: Ability to manage the entire application lifecycle.

RESTful API Design: Implementation of clear and REST-compliant endpoints in .NET.

Reactive User Interface: Understanding and application of the Vue.js framework for a dynamic user experience.

State Management: Comprehension of frontend state management (potentially using Vuex/Pinia if applicable).

Data Integration: Connecting to and manipulating data via an ORM (like Entity Framework Core) and a SQL database.
