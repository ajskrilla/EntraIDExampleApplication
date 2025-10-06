# EntraID Scanner Web Server

A full-stack application that connects to Microsoft Entra ID (Azure AD) to sync and display user and device data using a Node.js frontend, C# backend, and MongoDB database.

---

## 📁 Project Structure

/csharp-back 
/node-server/public 
/scripts


Description on each folder’s role:
- `csharp-back`: ASP.NET Core API to interact with Microsoft Graph
- `node-server`: Express-based frontend and API proxy
- `public`: Static assets like stylesheets
- `scripts`: PowerShell tools for automation and testing

---

## ⚙️ Prerequisites

Needs to be installed:
- [.NET SDK 9.0](https://dotnet.microsoft.com/)
- [Node.js (LTS)](https://nodejs.org/)
- [MongoDB](https://www.mongodb.com/)
- PowerShell (if using scripts)

📂 Environment Configuration
- Create a .env file in node-server/:
```bash
MONGO_URI=mongodb://localhost:27017/EntraDB
PORT=3000
```

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/entra-id-scanner.git
cd entra-id-scanner
```

### 2. Restore the dep libs
```pwsh
cd ".\csharp-back\EntraIDScanner.API"
dotnet restore
```
```pwsh
cd ".\node-server\"
npm install
```

### 3. Start Project
```./start-project.ps1```
☑️ This will:
- Start MongoDB
- Run the C# backend on https://localhost:7120
- Launch the Node.js frontend on http://localhost:3000

---

## 🔐 Authentication

### Explanation of Authentication
- To Authenticate, there is an example in the scripts folder, but the scriptlet is:
```pwsh
Invoke-RestMethod -Uri "http://localhost:3000/api/auth" -Method Post -Body @{
    tenantId = "<your-tenant-id>"
    clientId = "<your-client-id>"
    clientSecret = "<your-client-secret>"
} | ConvertTo-Json
```
- This will be an EntraID enterprise application leveraging Client Creds OAUTH2
- This is what is in SSC as the standard authentication

## ✨ Features
 Sync Entra ID users and devices

 View data via dark-themed frontend

 MongoDB storage and retrieval

 Reset database with admin endpoint

 Logging to file and console

 Pagination support for large datasets

## 📬 API Endpoints (Backend)

| Method | Route               | Description                  |
|--------|---------------------|------------------------------|
| GET    | `/api/users`        | Returns all synced users     |
| GET    | `/api/devices`      | Returns all synced devices   |
| POST   | `/api/sync`         | Triggers sync from Entra ID  |
| DELETE | `/api/admin/reset`  | Drops all MongoDB collections|

## 🖥️ Frontend Routes

 Route                | Description                  |
|---------------------|------------------------------|
| `/api/users`        | Returns all synced users     |
| `/api/devices`      | Returns all synced devices   |
