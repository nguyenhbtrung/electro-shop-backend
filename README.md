## appsettings.json template
```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "DefaultSQLConnection": "Server=Your-Server;Database=ThreadCity2.0;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    },
    "JWT": {
        "Issuer": "https://localhost:7135",
        "Audience": "https://localhost:7135",
        "SigningKey": "Your-Key"
    }
    "Kestrel": {
      "Endpoints": {
        "Http": {
          "Url": "https://localhost:7135"
        },
        "Http2": {
          "Url": "https://192.168.0.103:7135"
        }
      }
    }
}
```
