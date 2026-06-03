# Architecture Overview

The AI Secure File Access System follows a three-tier architecture.

## Components

### Angular Frontend

Provides the user interface for administrators to:

* Select users and files
* Simulate file access
* View alerts
* View activity logs

### ASP.NET Core Web API

Handles:

* File access logging
* Risk score calculation
* Alert generation
* AI explanation generation

### SQL Server Database

Stores:

* Users
* Files
* File access logs
* Risk logs

### OpenRouter AI

Generates human-readable explanations for high-risk activities.

## High-Level Flow

User → Angular UI → ASP.NET Core API → SQL Server

For high-risk events:

ASP.NET Core API → OpenRouter AI → Explanation → Dashboard
