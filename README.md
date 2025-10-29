# 📚 MyPersonalLibrary - Catalogo Full-Stack di Libri

## Panoramica del Progetto

**MyPersonalLibrary** è un'applicazione full-stack di esempio progettata per catalogare e gestire una collezione personale di libri. Questo progetto è stato realizzato come esercizio pratico per mostrare le competenze nello sviluppo end-to-end.

L'applicazione permette agli utenti di cercare rapidamente libri nel proprio catalogo e di eseguire operazioni **CRUD** (Create, Read, Update, Delete) complete per la gestione delle schede dei libri.

-----

## Stack Tecnologico

Il progetto utilizza uno stack moderno e diffuso in ambito industriale:

| Componente | Tecnologia | Descrizione |
| :--- | :--- | :--- |
| **Database** | **SQL Server** | Memorizza i dati del catalogo (titoli, autori, anni di pubblicazione, ISBN, ecc.). |
| **Backend / API** | **.NET (C#)** | Implementa un'API RESTful per gestire la logica di business e le interazioni con il database. |
| **Frontend** | **Vue.js** | Fornisce un'interfaccia utente dinamica e reattiva per la visualizzazione del catalogo e la gestione dei form CRUD. |

-----

## Funzionalità Principali

### 🔍 Funzionalità per l'Utente

  * **Ricerca Avanzata (con lazy loading):** Filtra il catalogo per titolo, autore, anno di pubblicazione o ISBN. La ricerca avanzata è implementata con lazy loading per caricare i risultati on-demand e migliorare le prestazioni lato client e server.
  * **Navigazione Catalogo:** Navigazione intuitiva e paginata attraverso l'intera libreria.
  * **Dettagli Libro:** Visualizza i metadati completi per ogni voce di libro.

  * **Gestione Globale delle Eccezioni:** L'app dispone di una gestione centralizzata delle eccezioni sia a livello di API che nel frontend per migliorare l'affidabilità e l'esperienza utente.
  * **Logging Centralizzato:** I log dell'applicazione vengono persistiti sia su database che su file per facilitare il monitoraggio e il troubleshooting.

### ⚙️ Funzionalità di Amministrazione (CRUD)

  * **C - Create:** Aggiungi nuovi libri al database tramite un form dedicato.
  * **R - Read:** Recupera e visualizza tutti i libri o una singola voce.
  * **U - Update:** Modifica i dettagli di un libro esistente.
  * **D - Delete:** Rimuovi definitivamente un libro dal catalogo.

-----

## Architettura e Design

Il progetto segue un'architettura a tre livelli:

1.  **Data Layer (SQL Server):** Struttura di tabelle ottimizzata per ricerche efficienti.
2.  **Logic/API Layer (.NET):** L'API utilizza il **Repository Pattern** o un **Service Layer** per separare la logica di business dall'accesso ai dati, garantendo codice pulito e testabile.
3.  **Presentation Layer (Vue.js):** Il frontend comunica con l'API tramite chiamate HTTP/Axios, operando come **Single Page Application (SPA)** per un'esperienza fluida.

-----

## Configurazione Locale e Avvio

### Prerequisiti

  * [.NET SDK (ultima versione stabile)]
  * [Node.js / npm (ultima versione stabile)]
  * [SQL Server o equivalente per ambiente locale]

### 1. Configurazione Database

1.  Assicurati che l'istanza di SQL Server sia in esecuzione.
2.  Naviga nella cartella `SQL/` ed esegui lo script **`01_Schema.sql`** per creare le tabelle necessarie (es. `[dbo].[book]`).
3.  *Opzionale:* Esegui lo script **`02_SeedData.sql`** per popolare il database con voci di esempio.

### 2. Avvio del Backend (.NET API)

1.  Aggiorna la stringa di connessione (`DefaultConnection`) nel file **`MyPersonalLibrary.Api/appsettings.json`** con i dettagli della tua connessione SQL locale.
2.  Spostati nella directory del progetto API e avvia l'applicazione:

```bash
# Vai nella directory del progetto API
cd MyPersonalLibrary.Api
dotnet restore
dotnet run
```

*L'API sarà disponibile su [http://localhost:XXXX].*

### 3. Avvio del Frontend (Vue.js)

1.  Spostati nella directory del progetto frontend:
2.  Installa le dipendenze e avvia il server di sviluppo:

```bash
# Vai nella directory del progetto Frontend
cd MyPersonalLibrary.Frontend
npm install
npm run serve
```

*L'interfaccia utente sarà disponibile su [http://localhost:8080].*

-----

## Obiettivi del Portfolio

Questo progetto è stato sviluppato per dimostrare le seguenti competenze tecniche:

  * **Competenze Full-Stack:** Gestione dell'intero ciclo di vita dell'applicazione, dalla progettazione del database al deployment del frontend.
  * **Design di API RESTful:** Implementazione di endpoint chiari ed efficienti con .NET.
  * **Gestione dello Stato nel Frontend:** Costruzione di un'interfaccia reattiva con Vue.js, dimostrando un design efficiente dei componenti e del flusso dei dati.
  * **Integrazione Dati:** Applicazione pratica della connettività e manipolazione del database (es. tramite Entity Framework Core).