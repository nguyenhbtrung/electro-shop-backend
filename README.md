# Electro Shop Backend - .NET 8 Web API

## 📌 Overview
This is a backend API built with **.NET 8**, using **Entity Framework Core** with **SQL Server** as the database.
The system supports real-time chat with **SignalR**, authentication with **JWT**, and email services.

Frontend repository: [electro-shop-frontend](https://github.com/nguyenhbtrung/electro-shop-frontend)

---

## 🚀 Tech Stack
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- [SQL Server](https://www.microsoft.com/sql-server)
- [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr)
- [ASP.NET Core Identity](https://docs.microsoft.com/aspnet/core/security/authentication/identity)
- [JWT Authentication](https://jwt.io/)
- [VNPay Integration](https://sandbox.vnpayment.vn/apis/docs/gioi-thieu/)

---

## 📂 Project Structure

```
.
├── Controllers/       # API Controllers
├── Services/          # Business Logic Services
├── Models/            # Data Models & DTOs
├── Data/              # DbContext & Database Configuration
├── Configurations/    # Application Configuration
├── Exceptions/        # Custom Exception Handling
├── Extensions/        # Extension Methods
├── Hubs/              # SignalR Hubs
└── Program.cs         # Application Entry Point
```

---

## ⚙️ Configuration

### 1. Database & JWT Setup
Create an `appsettings.json` file in the project root and configure as follows:

```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "EmailConfiguration": {
        "From": "dutshop66@gmail.com",
        "SmtpServer": "smtp.gmail.com",
        "Port": 587,
        "Username": "dutshop66@gmail.com",
        "Password": "your_password"
    },
    "BaseUrl": "https://localhost:7169",
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultSQLConnection": "Server=Your-Server;Database=ElectroShop;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    },
    "ClientUrls": [
        "http://localhost:5173"
    ]
    "JWT": {
        "Issuer": "https://localhost:7169",
        "Audience": "https://localhost:7169",
        "SigningKey": "Your-512-bit-Key"
    }
}
```

### 2. VNPay Integration Setup
Add the following to your `appsettings.json`:

```
"VnPay": {
  "Url": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
  "TmnCode": "your_tmn_code",
  "HashSecret": "your_hash_secret",
  "Version": "2.1.0",
  "Command": "pay",
  "CurrencyCode": "VND",
  "Locale": "vn",
  "ReturnUrl": "https://localhost:7169/api/Order/vnpay-callback"
}
```

---

## 📦 Installation & Setup

### 1. Prerequisites
- Visual Studio 2022
- .NET 8 SDK
- SQL Server

### 2. Database Setup
1. Open Package Manager Console in Visual Studio
2. Run the following command:
```
update-database

```

### 3. Running the Application
1. Open the solution in Visual Studio 2022
2. Build the solution
3. Press F5 to run the application

The API will be available at:
- HTTPS: [https://localhost:7169](https://localhost:7169)
- Swagger UI: [https://localhost:7169/swagger](https://localhost:7169/swagger)

---

## 💳 VNPay Test Integration

1. Register at [VNPay Sandbox](https://sandbox.vnpayment.vn/devreg/)
2. Configure VNPay settings in `appsettings.json`
3. Visit https://sandbox.vnpayment.vn/apis/vnpay-demo/#th%C3%B4ng-tin-th%E1%BA%BB-test to get test credentials, or test with these credentials:
   - Bank: NCB
   - Card Number: 9704198526191432198
   - Card Holder: NGUYEN VAN A
   - Issue Date: 07/15
   - OTP: 123456

---

## 🔑 Authentication

* The backend uses **JWT** for authentication.
* On successful login, the server returns a **JWT token**.
* Include the token in request headers for protected routes:
```
Authorization: Bearer <token>
```

## 👥 Authorization Policies

- `AdminPolicy`: Requires "Admin" role
- `UserPolicy`: Requires "User" or "Admin" role

---

## 📡 Real-time Communication

SignalR hub is available at:
```
/chathub

```

---

## 🌐 Client Integration

The frontend application is available at: [electro-shop-frontend](https://github.com/nguyenhbtrung/electro-shop-frontend)

The frontend is expected to run at:
[http://localhost:5173](http://localhost:5173) (configured in `appsettings.json`).

---

## 📝 Notes

- Ensure proper CORS configuration for your production environment
- Secure your JWT signing key and database connection strings
- Configure email settings with valid SMTP credentials
- For production deployment, update the JWT issuer and audience URLs
