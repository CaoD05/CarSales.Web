# CarSales.Web

A Razor Pages web application built on .NET 10.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (or SQL Server Express)
- A preferred IDE (Visual Studio 2022, Visual Studio Code, or JetBrains Rider)

## Installation Instructions

### 1. Clone the Repository
Clone the repository to your local machine using Git and navigate to the project directory:
```bash
git clone <repository-url>
cd <repository-directory>
```
*(If you already have the source code downloaded, simply navigate to the `CarSales.Web` folder in your terminal).*

### 2. Configure the Database
The application relies on SQL Server. By default, it connects to standard SQL Express:
1. Open [`appsettings.json`](appsettings.json).
2. Locate the `ConnectionStrings:CarSalesConnection`.
3. Update the connection string to match your local SQL Server instance if needed (e.g. `Server=YOUR-SERVER-NAME;Database=CarSalesDB;Trusted_Connection=True;TrustServerCertificate=True;`).

### 3. Restore Dependencies
Restore all the required NuGet packages using the .NET CLI:
```bash
dotnet restore
```

### 4. Create and Migrate the Database
A database script is included in the project for your convenience. To set up the database:
1. Open SQL Server Management Studio (SSMS).
2. Connect to your SQL Server instance.
3. Create an empty database named `CarSalesDB`.
4. Open the included SQL script file in SSMS.
5. Execute the script against the `CarSalesDB` database to create the required schema and tables.

**Optional (if the SQL script is missing or you want to generate a new one):** You can generate a SQL script from the Entity Framework Core migrations yourself:
```bash
dotnet tool install --global dotnet-ef  # Run only if you haven't installed EF Core tools globally
dotnet ef migrations script -o database_script.sql
```
*(Once generated, execute this script in SSMS following the steps 1-5 above).*

### 5. Build and Run the Application
Start the application from your terminal:
```bash
dotnet build
dotnet run
```
After running the command, check the terminal output for the local URL (typically `http://localhost:5xxx` or `https://localhost:7xxx`) and open it in your web browser.