# Data Flow

1. User selects a user and file from the dashboard.
2. Angular sends a request to the backend.
3. Backend logs the access event.
4. Risk score is calculated using predefined rules.
5. High-risk events are sent to OpenRouter AI.
6. AI generates an explanation.
7. Risk information is stored in SQL Server.
8. Dashboard displays alerts and activity logs.
