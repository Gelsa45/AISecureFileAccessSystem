# AI Secure File Access System

## Project Overview

AI Secure File Access System is a web-based application that monitors file access activities and helps identify potentially risky behavior. The system records file access events, calculates risk scores using predefined rules, and generates AI-powered explanations for high-risk activities.

## Tech Stack

### Frontend

* Angular
* TypeScript
* HTML/CSS

### Backend

* ASP.NET Core Web API
* C#

### Database

* SQL Server
* Entity Framework Core

### AI Integration

* OpenRouter API

---

## Features

* User and file selection
* File access logging
* Risk score calculation
* Dashboard alerts
* Audit trail activity logs
* AI-generated explanations for high-risk events

---

## Project Structure

```text
AISecureFileAccessSystem
│
├── api
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── Data
│   └── Migrations
│
├── ui
│   ├── src
│   └── assets
│
├── api.Tests
│
└── README.md
```

---

## How the System Works

1. A user accesses a file.
2. The system logs the activity.
3. A risk score is calculated based on predefined rules.
4. High-risk activities are flagged.
5. OpenRouter AI generates a human-readable explanation.
6. Alerts and activity logs are displayed on the dashboard.

---

## Running the Backend

```bash
cd api
dotnet restore
dotnet ef database update
dotnet run
```

Backend URL:

```text
http://localhost:5088
```

---

## Running the Frontend

```bash
cd ui
npm install
ng serve
```

Frontend URL:

```text
http://localhost:4200
```

---

## Database Setup

Update the SQL Server connection string in:

```text
api/appsettings.json
```

Apply migrations:

```bash
dotnet ef database update
```

---

## Sample Workflow

* Select a user from the dashboard.
* Select a file.
* Click **Simulate Access**.
* View generated risk scores and alerts.
* Review AI-generated explanations for high-risk events.

---

## Future Improvements

* User authentication and authorization
* AI-recommended actions
* Dashboard analytics and charts
* Email notifications for critical alerts
* Enhanced risk detection rules

---

## Author

**Gelsa Greenson**


