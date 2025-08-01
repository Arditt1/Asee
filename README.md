# Asee

# ASE Fee Calculation Engine 💳

A flexible and extensible fee calculation engine built in ASP.NET 9 MVC.  
It processes financial transactions, calculates fees based on dynamic rules, logs results, and supports batch processing.

---

## ✅ Features

- 📥 Accepts JSON input with transaction and client data
- 🧠 Calculates fees using rule-based architecture
- 🧾 Logs fee history to SQLite (input/output per transaction)
- 🚀 High-performance batch processing (non-linear)
- 🔧 Easily extendable with new rules
- 🧩 Clean separation of layers: ViewModels, Domain, Entity, Rules

---

## ▶️ How to Run Locally (Under 5 min)

### 📦 Requirements
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 (Preview for .NET 9 support) or CLI

### 🚀 Steps

1. **Clone the repo**

    ```bash
    git clone https://github.com/Arditt1/Asee.git
    cd ase-fee-engine
    ```

2. **Run database migration**

    ```bash
    dotnet ef database update
    ```

3. **Run the app**

    ```bash
    dotnet run
    ```

4. **Visit API**

    ```
    http://localhost:5000/swagger  
    ```

---

## 🧪 Sample Request/Response

### 📬 POST `/api/fee/calculate`

**Request:**

```json
{
  "transactionId": "TX1001",
  "type": "POS",
  "amount": 150.0,
  "currency": "EUR",
  "isInternational": false,
  "client": {
    "id": "CL123",
    "creditScore": 420,
    "segment": "STANDARD"
  }
}
