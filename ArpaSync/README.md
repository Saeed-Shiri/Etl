# ArpaSync - E-commerce to Arpa Accounting System Integration

سیستم سینک اطلاعات فروشگاه آنلاین با سیستم حسابداری آرپا

## 📋 Overview

ArpaSync یک سیستم یکپارچه‌سازی است که اطلاعات فروش، مشتریان و پرداخت‌های فروشگاه آنلاین را به صورت خودکار با سیستم حسابداری آرپا همگام‌سازی می‌کند.

## 🏗️ Architecture

پروژه بر اساس معماری Clean Architecture پیاده‌سازی شده است:

```
├── src/
│   ├── Domain/              # Business entities, value objects, domain events
│   ├── Application/         # Use cases, interfaces, DTOs
│   ├── Infrastructure/      # Data access, external services
│   └── WebApi/             # REST API endpoints
```

## ✨ Features

### 🔄 سینک خودکار
- **مشتریان**: چک کردن وجود مشتری در آرپا و ایجاد در صورت عدم وجود
- **سفارشات**: ارسال سفارشات پرداخت شده به آرپا
- **پرداخت‌ها**: ارسال اطلاعات پرداخت‌های تکمیل شده

### 🛡️ قابلیت‌های پیشرفته
- **Outbox Pattern**: اطمینان از reliable messaging
- **Retry Mechanism**: تلاش مجدد در صورت خطا با Exponential Backoff
- **Circuit Breaker**: جلوگیری از ارسال درخواست‌های مکرر در صورت خرابی
- **Background Processing**: پردازش خودکار events در پس‌زمینه
- **Comprehensive Logging**: لاگ‌گیری کامل با Serilog

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- SQL Server / SQL Server LocalDB
- Visual Studio 2022 یا VS Code

### Installation

1. **Clone the repository:**
```bash
git clone <repository-url>
cd ArpaSync
```

2. **Update connection strings:**
```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "your-sql-server-connection-string"
  },
  "ArpaApi": {
    "BaseUrl": "https://your-arpa-api-url.com",
    "ApiKey": "your-arpa-api-key"
  }
}
```

3. **Run database migrations:**
```bash
cd src/WebApi/ArpaSync.WebApi
dotnet ef database update
```

4. **Run the application:**
```bash
dotnet run
```

## 📡 API Endpoints

### Sync Operations
- `POST /api/sync/customer/{customerId}` - سینک دستی مشتری
- `POST /api/sync/order/{orderId}` - سینک دستی سفارش
- `POST /api/sync/payment/{paymentId}` - سینک دستی پرداخت
- `GET /api/sync/status` - وضعیت سیستم سینک

### Health Check
- `GET /health` - بررسی سلامت سیستم

## 🔧 Configuration

### Arpa API Settings
```json
{
  "ArpaApi": {
    "BaseUrl": "https://api.arpa.example.com",
    "ApiKey": "your-api-key",
    "Timeout": 30
  }
}
```

### Logging Configuration
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      {
        "Name": "File",
        "Args": {
          "path": "logs/arpa-sync-.txt",
          "rollingInterval": "Day"
        }
      }
    ]
  }
}
```

## 🔄 How It Works

### 1. Event-Driven Architecture
وقتی در سیستم فروش اتفاقات زیر رخ می‌دهد:
- مشتری جدید ثبت‌نام می‌کند → `CustomerCreatedEvent`
- سفارش پرداخت می‌شود → `OrderPaidEvent`  
- پرداخت تکمیل می‌شود → `PaymentCompletedEvent`

### 2. Outbox Pattern
Events در جدول `OutboxEvents` ذخیره می‌شوند تا اطمینان از پردازش آن‌ها وجود داشته باشد.

### 3. Background Processing
سرویس `OutboxEventProcessorService` به صورت مداوم events pending را پردازش می‌کند.

### 4. Reliable Communication
- **Retry Policy**: 3 بار تلاش با Exponential Backoff
- **Circuit Breaker**: بعد از 5 خطای متوالی، مدار باز می‌شود
- **Timeout**: 30 ثانیه timeout برای هر درخواست

## 📊 Database Schema

### Core Tables
- `Customers` - اطلاعات مشتریان
- `Orders` - اطلاعات سفارشات
- `OrderItems` - آیتم‌های سفارش
- `Payments` - اطلاعات پرداخت‌ها
- `OutboxEvents` - Events برای پردازش

### Key Fields for Arpa Integration
- `Customer.ArpaBusinessId` - شناسه مشتری در آرپا
- `Order.ArpaOrderId` - شناسه سفارش در آرپا
- `Payment.ArpaPaymentId` - شناسه پرداخت در آرپا
- `*.IsSyncedWithArpa` - وضعیت سینک با آرپا

## 🧪 Testing

### Manual Testing
```bash
# سینک مشتری
curl -X POST https://localhost:7000/api/sync/customer/1

# سینک سفارش
curl -X POST https://localhost:7000/api/sync/order/1

# بررسی وضعیت
curl -X GET https://localhost:7000/api/sync/status
```

## 📈 Monitoring

### Logs
لاگ‌ها در مسیر `logs/arpa-sync-{date}.txt` ذخیره می‌شوند.

### Health Checks
Health check endpoint برای monitoring استفاده کنید:
```
GET /health
```

## 🛠️ Development

### Adding New Sync Operations
1. ایجاد Event جدید در `Domain/Events`
2. اضافه کردن Use Case در `Application/UseCases`
3. به‌روزرسانی `OutboxEventProcessorService`
4. اضافه کردن Controller endpoint

### Custom Arpa API Integration
Interface `IArpaApiService` را برای customization پیاده‌سازی کنید.

## 🤝 Contributing

1. Fork the project
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License.

## 📞 Support

For support and questions, please contact the development team.