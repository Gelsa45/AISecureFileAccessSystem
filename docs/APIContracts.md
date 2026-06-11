# API Contracts

## Log File Access

### Endpoint

POST /api/fileaccess/log

### Parameters

| Parameter | Type |
| --------- | ---- |
| userId    | int  |
| fileId    | int  |

### Response

```json
{
  "riskScore": 70,
  "riskLevel": "High",
  "aiReason": "AI-generated explanation"
}
```

---

## Get Alerts

### Endpoint

GET /api/fileaccess/alerts

Returns all high-risk alerts.

---

## Get Users

### Endpoint

GET /api/fileaccess/users

Returns available users.

---

## Get Files

### Endpoint

GET /api/fileaccess/files

Returns available files.

---

## Get Activity Logs

### Endpoint

GET /api/fileaccess/activity

Returns file access history.
