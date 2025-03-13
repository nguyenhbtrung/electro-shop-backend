## appsettings.json template
```json
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
        "Password": " password"
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultSQLConnection": "Server=Your-Server;Database=ThreadCity2.0;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    },
    "JWT": {
        "Issuer": "https://localhost:7169",
        "Audience": "https://localhost:7169",
        "SigningKey": "Your-Key"
    }
}
```
