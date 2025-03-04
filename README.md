# Ezana Investment Analytics

Ezana is a web application that allows users to connect their bank accounts and receive personalized investment analytics and recommendations based on their financial data and preferences.

## Features

- **Bank Account Integration**: Connect your bank accounts to automatically import transactions and balances.
- **Financial Analytics**: View your spending patterns, income sources, and savings rate.
- **Investment Recommendations**: Get personalized investment recommendations based on your risk tolerance and financial goals.
- **Portfolio Allocation**: Set and track your investment portfolio allocation across different asset classes.
- **Financial Health Score**: Monitor your overall financial health with a comprehensive score and recommendations.

## Technology Stack

- **Backend**: ASP.NET Core 8.0, C#
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Razor Pages, Bootstrap 5, JavaScript, Chart.js
- **Authentication**: ASP.NET Core Identity
- **Bank Integration**: Mock implementation (can be replaced with Plaid or similar API)

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server (or SQL Server Express)
- Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/ezana.git
   cd ezana
   ```

2. Update the connection string in `appsettings.json` to point to your SQL Server instance.

3. Apply the database migrations:
   ```
   dotnet ef database update
   ```

4. Run the application:
   ```
   dotnet run
   ```

5. Navigate to `https://localhost:5001` in your web browser.

## Usage

1. Register a new account or log in with an existing account.
2. Connect your bank account(s) using the "Connect Bank Account" button.
3. Set your investment preferences to get personalized recommendations.
4. Explore your financial analytics and investment recommendations on the dashboard.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- [Bootstrap](https://getbootstrap.com/)
- [Chart.js](https://www.chartjs.org/)
- [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/) 