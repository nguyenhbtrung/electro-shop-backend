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
    "BaseUrl": "https://localhost:7169",
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
## test thanh toan bang vnpay
B1: Đăng ký tài khoản tại https://sandbox.vnpayment.vn/devreg/
B2: Xác nhận email
B3: Chỉnh sửa appsettings.json với thông số nhận được trong email
```json
{
    "VnPay": {
        "Url": "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html",
        "TmnCode": "7662VTDW",
        "HashSecret": "2C3T6AXPI4XHA47LOKBMYHO6EM8P2VWJ",
        "Version": "2.1.0",
        "Command": "pay",
        "CurrencyCode": "VND",
        "Locale": "vn",
        "ReturnUrl": "https://localhost:7169/api/Order/vnpay-callback"
    }
}
```
B4: Chạy api CreateOrder với paymentmethod là "vnpay"
B5: Truy cập https://sandbox.vnpayment.vn/apis/vnpay-demo/ để lấy tài khoản test
