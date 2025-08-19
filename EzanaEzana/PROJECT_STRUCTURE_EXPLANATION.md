# Ezana Project Structure Explanation

This document explains what each file in the Ezana project does in plain, simple terms.

## 📁 Root Directory Files

### **Ezana.csproj**
- **What it is**: The main project file that tells .NET how to build your application
- **What it does**: Lists all the packages your app needs, sets the target framework (.NET 9), and defines build settings
- **Why it's important**: Without this file, .NET doesn't know how to build your project

### **Program.cs**
- **What it is**: The main entry point of your application - the first file that runs when you start the app
- ** What it does**: 
  - Sets up all the services your app needs (database, authentication, etc.)
  - Configures how users log in (JWT tokens)
  - Sets up the database connection
  - Creates a test user for development
  - Starts the web server
- **Why it's important**: This is like the "main switchboard" that connects all parts of your app together

### **appsettings.json**
- **What it is**: Configuration file that stores important settings
- **What it does**: Contains database connection strings, JWT secret keys, Plaid API settings, and domain information
- **Why it's important**: Keeps all your configuration in one place so you can easily change settings without changing code

### **appsettings.Development.json**
- **What it is**: Development-specific configuration file
- **What it does**: Overrides the main settings when you're developing locally (like using in-memory database instead of real database)
- **Why it's important**: Lets you have different settings for development vs production

## 📁 Controllers (API Endpoints)

### **Controllers/AccountController.cs**
- **What it is**: Handles user account management
- **What it does**: 
  - Shows user profile information
  - Lets users change their password
  - Handles account deletion
- **Why it's important**: Gives users control over their accounts

### **Controllers/AuthController.cs**
- **What it is**: Handles user login, registration, and authentication
- **What it does**: 
  - Creates new user accounts
  - Logs users in and out
  - Handles password reset requests
  - Generates JWT tokens for secure access
- **Why it's important**: This is the security system that protects your app

### **Controllers/DashboardController.cs**
- **What it is**: Handles the main dashboard page
- **What it does**: Shows investment preferences and dashboard data to logged-in users
- **Why it's important**: Gives users access to their main investment dashboard

### **Controllers/GRPVController.cs**
- **What it is**: Handles GRPV (Growth, Risk, Profitability, Valuation) investment analysis
- **What it does**: 
  - Creates and manages investment analysis models
  - Shows detailed investment analysis results
- **Why it's important**: This is the core investment analysis feature of your app

### **Controllers/HomeController.cs**
- **What it is**: Handles the main landing page and home-related pages
- **What it does**: Shows the welcome page, privacy policy, and other public pages
- **Why it's important**: This is what new visitors see when they first visit your app

### **Controllers/PlaidController.cs**
- **What it is**: Handles all Plaid bank account integration
- **What it does**: 
  - Connects users' bank accounts to your app
  - Fetches account balances and transactions
  - Handles webhooks from Plaid for real-time updates
  - Syncs financial data
- **Why it's important**: This is how users connect their real financial accounts to your investment platform

### **Controllers/SocialController.cs**
- **What it is**: Handles social features like friends and messaging
- **What it does**: 
  - Manages friend requests and friendships
  - Handles private messaging between users
  - Shows user profiles and search functionality
- **Why it's important**: Makes your app social and engaging for users

## 📁 Models (Database Tables)

### **Models/ApplicationUser.cs**
- **What it is**: Defines what information is stored about each user
- **What it does**: 
  - Stores user details (name, email, username, etc.)
  - Links to their investment preferences, friends, and messages
  - Handles user authentication
- **Why it's important**: This is the core user data structure that everything else connects to

### **Models/ErrorViewModel.cs**
- **What it is**: Defines how error messages are displayed
- **What it does**: Provides a standard way to show errors to users
- **Why it's important**: Gives users clear information when something goes wrong

### **Models/Friendship.cs**
- **What it is**: Defines how friendships between users are stored
- **What it does**: 
  - Tracks who sent friend requests to whom
  - Stores the status of friendships (pending, accepted, etc.)
- **Why it's important**: Enables the social networking features

### **Models/GRPVModel.cs**
- **What it is**: Stores investment analysis data
- **What it does**: 
  - Holds growth, risk, profitability, and valuation factors
  - Stores the results of investment analysis
  - Links analysis to specific users
- **Why it's important**: This is the core data structure for your investment analysis feature

### **Models/InvestmentPreference.cs**
- **What it is**: Stores user investment preferences and settings
- **What it does**: 
  - Records what types of investments users prefer
  - Stores risk tolerance and investment goals
- **Why it's important**: Helps personalize investment recommendations

### **Models/Message.cs**
- **What it is**: Stores private messages between users
- **What it does**: 
  - Holds message content, sender, and recipient
  - Tracks when messages were sent and read
- **Why it's important**: Enables private communication between users

### **Models/PlaidModels.cs**
- **What it is**: Defines how Plaid financial data is stored
- **What it does**: 
  - **PlaidItem**: Stores connection to a bank (like "Chase Bank")
  - **PlaidAccount**: Stores individual accounts (like "Checking Account #1234")
  - **PlaidTransaction**: Stores individual transactions (like "Grocery store purchase")
  - **PlaidInstitution**: Stores bank information (logo, name, etc.)
- **Why it's important**: This is how you store real financial data from users' bank accounts

## 📁 Services (Business Logic)

### **Services/GRPVService.cs**
- **What it is**: Handles the business logic for investment analysis
- **What it does**: 
  - Performs calculations for growth, risk, profitability, and valuation
  - Manages investment analysis data
- **Why it's important**: Contains the core investment analysis algorithms

### **Services/IGRPVService.cs**
- **What it is**: Interface (contract) for the GRPV service
- **What it does**: Defines what methods the GRPV service must have
- **Why it's important**: Makes the code more flexible and testable

### **Services/InvestmentAnalyticsService.cs**
- **What it is**: Handles investment analytics and reporting
- **What it does**: 
  - Analyzes investment performance
  - Generates reports and insights
- **Why it's important**: Provides users with investment performance data

### **Services/IInvestmentAnalyticsService.cs**
- **What it is**: Interface for the investment analytics service
- **What it does**: Defines what methods the analytics service must have
- **Why it's important**: Makes the code more flexible and testable

### **Services/JwtService.cs**
- **What it is**: Handles JWT (JSON Web Token) creation and validation
- **What it does**: 
  - Creates secure tokens when users log in
  - Validates tokens to ensure users are who they say they are
- **Why it's important**: This is the security system that keeps your app safe

### **Services/PlaidService.cs**
- **What it is**: Handles all communication with Plaid (bank account integration)
- **What it does**: 
  - Connects to Plaid's API to get bank account data
  - Manages the flow of connecting bank accounts
  - Handles webhooks and data syncing
  - Currently uses mock data for MVP demo
- **Why it's important**: This is the bridge between your app and users' real financial data

### **Services/IPlaidService.cs**
- **What it is**: Interface for the Plaid service
- **What it does**: Defines what methods the Plaid service must have
- **Why it's important**: Makes the code more flexible and testable

### **Services/SocialService.cs**
- **What it is**: Handles social networking business logic
- **What it does**: 
  - Manages friend requests and relationships
  - Handles messaging between users
  - Manages user profiles
- **Why it's important**: Contains the logic for social features

### **Services/ISocialService.cs**
- **What it is**: Interface for the social service
- **What it does**: Defines what methods the social service must have
- **Why it's important**: Makes the code more flexible and testable

## 📁 ViewModels (Data Transfer Objects)

### **ViewModels/AccountViewModels.cs**
- **What it is**: Defines the data structure for account-related forms
- **What it does**: 
  - **AccountViewModel**: For viewing/editing user profiles
  - **LoginViewModel**: For login forms
  - **RegisterViewModel**: For registration forms
  - **ForgotPasswordViewModel**: For password reset forms
  - **ResetPasswordViewModel**: For setting new passwords
- **Why it's important**: Ensures data is properly formatted when moving between the web page and the server

### **ViewModels/DashboardViewModel.cs**
- **What it is**: Defines what data is shown on the dashboard
- **What it does**: Structures the data that appears on the main dashboard page
- **Why it's important**: Organizes dashboard information in a clean, structured way

### **ViewModels/GRPVViewModel.cs**
- **What it is**: Defines how GRPV analysis data is displayed
- **What it does**: Structures the investment analysis results for display on web pages
- **Why it's important**: Makes investment analysis data easy to read and understand

### **ViewModels/SocialViewModels.cs**
- **What it is**: Defines data structures for social features
- **What it does**: 
  - **UserProfileViewModel**: For user profiles
  - **FriendRequestViewModel**: For friend requests
  - **MessageViewModel**: For messaging
  - **SearchViewModel**: For user search
- **Why it's important**: Organizes social data in a structured way

## 📁 Views (Web Pages)

### **Views/Shared/_Layout.cshtml**
- **What it is**: The main template that wraps around every page
- **What it does**: 
  - Provides the header, navigation, and footer
  - Includes common CSS and JavaScript files
  - Ensures consistent look across all pages
- **Why it's important**: Gives your app a consistent, professional appearance

### **Views/Shared/_LoginPartial.cshtml**
- **What it is**: Shows login/logout buttons in the navigation
- **What it does**: Displays different navigation options based on whether users are logged in
- **Why it's important**: Provides easy access to login/logout functionality

### **Views/Shared/_ValidationScriptsPartial.cshtml**
- **What it is**: Includes JavaScript for form validation
- **What it does**: Makes sure users fill out forms correctly before submitting
- **Why it's important**: Improves user experience and data quality

### **Views/Account/_AccountNav.cshtml**
- **What it is**: Navigation menu for account-related pages
- **What it does**: Shows links to profile, password change, and account deletion
- **Why it's important**: Helps users navigate account management features

### **Views/Account/Index.cshtml**
- **What it is**: Main account management page
- **What it does**: Shows user profile information and account options
- **Why it's important**: Central hub for users to manage their accounts

### **Views/Account/ChangePassword.cshtml**
- **What it is**: Page for changing passwords
- **What it does**: Provides a form for users to update their passwords
- **Why it's important**: Security feature for users to protect their accounts

### **Views/Account/DeleteAccount.cshtml**
- **What it is**: Page for deleting user accounts
- **What it does**: Provides a form for users to permanently remove their accounts
- **Why it's important**: Gives users control over their data

### **Views/Auth/Login.cshtml**
- **What it is**: User login page
- **What it does**: Provides a form for users to sign in with email and password
- **Why it's important**: Main entry point for existing users

### **Views/Auth/Register.cshtml**
- **What it is**: User registration page
- **What it does**: Provides a form for new users to create accounts
- **Why it's important**: Main entry point for new users

### **Views/Auth/ForgotPassword.cshtml**
- **What it is**: Password recovery page
- **What it does**: Provides a form for users to request password reset emails
- **Why it's important**: Helps users who forget their passwords

### **Views/Dashboard/Index.cshtml**
- **What it is**: Main dashboard page
- **What it does**: Shows investment overview, recent activity, and quick actions
- **Why it's important**: Central hub for users to see their investment information

### **Views/Dashboard/InvestmentPreferences.cshtml**
- **What it is**: Investment preferences page
- **What it does**: Allows users to set and modify their investment goals and risk tolerance
- **Why it's important**: Personalizes the investment experience

### **Views/GRPV/Index.cshtml**
- **What it is**: GRPV analysis overview page
- **What it does**: Shows list of investment analyses and allows creating new ones
- **Why it's important**: Main interface for investment analysis feature

### **Views/GRPV/Details.cshtml**
- **What it is**: Detailed GRPV analysis page
- **What it does**: Shows comprehensive results of investment analysis
- **Why it's important**: Provides detailed investment insights

### **Views/Home/Index.cshtml**
- **What it is**: Landing page (what new visitors see)
- **What it does**: 
  - Explains what your app does
  - Shows benefits and features
  - Has call-to-action buttons
- **Why it's important**: First impression for potential users

### **Views/Home/Privacy.cshtml**
- **What it is**: Privacy policy page
- **What it does**: Explains how user data is handled and protected
- **Why it's important**: Required for legal compliance and user trust

### **Views/Home/React.cshtml**
- **What it is**: Page that loads React components
- **What it does**: Serves as a container for React-based features
- **Why it's important**: Enables modern, interactive user interfaces

### **Views/Home/ReactDashboard.cshtml**
- **What it is**: React-based dashboard page
- **What it does**: Loads the React dashboard component
- **Why it's important**: Provides a modern, interactive dashboard experience

### **Views/Social/Friends.cshtml**
- **What it is**: Friends management page
- **What it does**: Shows current friends, pending requests, and allows adding new friends
- **Why it's important**: Central hub for social networking features

### **Views/Social/Messages.cshtml**
- **What it is**: Messaging page
- **What it does**: Allows users to send and receive private messages
- **Why it's important**: Enables private communication between users

### **Views/Social/Profile.cshtml**
- **What it is**: User profile page
- **What it does**: Shows user information, bio, and profile picture
- **Why it's important**: Lets users present themselves to others

### **Views/Social/Search.cshtml**
- **What it is**: User search page
- **What it does**: Allows users to find and connect with other users
- **Why it's important**: Helps users discover and connect with others

## 📁 React Components (Frontend JavaScript)

### **wwwroot/js/react/components/App.jsx**
- **What it is**: Main React application component
- **What it does**: 
  - Sets up the overall React app structure
  - Handles routing between different pages
  - Manages global state
- **Why it's important**: This is the JavaScript equivalent of Program.cs - the main entry point for the frontend

### **wwwroot/js/react/components/Dashboard.jsx**
- **What it is**: React dashboard component
- **What it does**: 
  - Shows investment overview
  - Displays charts and data
  - Provides interactive dashboard features
- **Why it's important**: Modern, interactive version of the dashboard

### **wwwroot/js/react/components/DashboardCard.jsx**
- **What it is**: Individual dashboard card component
- **What it does**: Displays specific pieces of information in card format
- **Why it's important**: Makes dashboard information easy to read and organize

### **wwwroot/js/react/components/InvestmentChart.jsx**
- **What it is**: Chart component for investment data
- **What it does**: Creates visual charts showing investment performance over time
- **Why it's important**: Makes investment data easy to understand visually

### **wwwroot/js/react/components/InvestmentPreferences.jsx**
- **What it is**: React component for investment preferences
- **What it does**: Provides interactive form for setting investment goals
- **Why it's important**: Modern interface for investment preference management

### **wwwroot/js/react/components/LandingPage.jsx**
- **What it is**: React landing page component
- **What it does**: Creates an interactive, modern landing page
- **Why it's important**: Provides engaging first impression for visitors

### **wwwroot/js/react/components/PlaidDemo.tsx**
- **What it is**: Demo component for Plaid integration
- **What it does**: Shows how bank account connection works
- **Why it's important**: Demonstrates the core financial integration feature

### **wwwroot/js/react/components/PortfolioSummary.jsx**
- **What it is**: Portfolio overview component
- **What it does**: Shows summary of user's investment portfolio
- **Why it's important**: Gives users quick overview of their investments

### **wwwroot/js/react/components/RealTimeStockTicker.tsx**
- **What it is**: Live stock price component
- **What it does**: Shows real-time stock prices and changes
- **Why it's important**: Provides live market data to users

### **wwwroot/js/react/components/StockDetails.jsx**
- **What it is**: Detailed stock information component
- **What it does**: Shows comprehensive information about specific stocks
- **Why it's important**: Provides detailed stock analysis for users

### **wwwroot/js/react/components/WebApp.jsx**
- **What it is**: Main web application component
- **What it does**: Sets up the overall web app structure and navigation
- **Why it's important**: Main container for the web application

## 📁 React Services (API Communication)

### **wwwroot/js/react/services/api.ts**
- **What it is**: Main API service for communicating with the backend
- **What it does**: 
  - Handles all HTTP requests to your server
  - Manages authentication tokens
  - Provides methods for all API calls
- **Why it's important**: This is how the frontend talks to the backend

### **wwwroot/js/react/services/api.js**
- **What it is**: JavaScript version of the API service
- **What it does**: Same as api.ts but in JavaScript
- **Why it's important**: Alternative API service for JavaScript components

### **wwwroot/js/react/services/marketApi.ts**
- **What it is**: Service for market data
- **What it does**: Fetches stock prices, market information, and financial data
- **Why it's important**: Provides real-time market information

### **wwwroot/js/react/services/plaidApi.ts**
- **What it is**: Service for Plaid integration
- **What it does**: Handles communication with Plaid for bank account connection
- **Why it's important**: Frontend interface for financial account integration

### **wwwroot/js/react/services/quiverApi.ts**
- **What it is**: Service for Quiver API (alternative data provider)
- **What it does**: Fetches alternative investment data and insights
- **Why it's important**: Provides additional investment research data

## 📁 React Context and State Management

### **wwwroot/js/react/context/AppContext.jsx**
- **What it is**: Global state management for React app
- **What it does**: 
  - Stores user information, authentication status, and app settings
  - Provides data to all components
  - Manages app-wide state changes
- **Why it's important**: Central place for managing app data and state

### **wwwroot/js/react/store/useStore.ts**
- **What it is**: Custom hook for state management
- **What it does**: Provides easy access to global app state
- **Why it's important**: Makes it easy for components to access and update app data

## 📁 React Hooks

### **wwwroot/js/react/hooks/useRealTimeStockData.ts**
- **What it is**: Custom hook for real-time stock data
- **What it does**: 
  - Fetches live stock prices
  - Updates data automatically
  - Manages WebSocket connections
- **Why it's important**: Provides live market data to components

## 📁 Configuration Files

### **babel.config.json**
- **What it is**: Babel configuration for JavaScript compilation
- **What it does**: Tells Babel how to convert modern JavaScript to older versions for browser compatibility
- **Why it's important**: Ensures your React app works in all browsers

### **jest.config.js**
- **What it is**: Jest testing framework configuration
- **What it does**: Sets up how tests are run and what files to test
- **Why it's important**: Enables automated testing of your code

### **postcss.config.js**
- **What it is**: PostCSS configuration for CSS processing
- **What it does**: Processes CSS to add vendor prefixes and optimize styles
- **Why it's important**: Ensures CSS works across all browsers

### **tailwind.config.js**
- **What it is**: Tailwind CSS configuration
- **What it does**: Defines custom colors, spacing, and design system
- **Why it's important**: Provides consistent, professional styling

### **tsconfig.json**
- **What it is**: TypeScript configuration
- **What it does**: Tells TypeScript how to compile your code and what features to use
- **Why it's important**: Ensures TypeScript code is properly compiled

### **webpack.config.js**
- **What it is**: Webpack bundler configuration
- **What it does**: 
  - Bundles all your JavaScript, CSS, and other files
  - Optimizes code for production
  - Handles different file types
- **Why it's important**: Creates the final files that run in the browser

## 📁 Package Management

### **package.json**
- **What it is**: Node.js package configuration
- **What it does**: 
  - Lists all JavaScript dependencies
  - Defines build scripts
  - Sets project metadata
- **Why it's important**: Manages all the JavaScript libraries your frontend needs

### **package-lock.json**
- **What it is**: Locked version of package.json
- **What it does**: Ensures everyone gets exactly the same versions of dependencies
- **Why it's important**: Prevents compatibility issues between different development environments

## 📁 CSS and Styling

### **wwwroot/css/input.css**
- **What it is**: Main CSS input file for Tailwind CSS
- **What it does**: Imports Tailwind CSS and defines custom styles
- **Why it's important**: Provides the foundation for all styling

### **wwwroot/css/site.css**
- **What it is**: Custom CSS styles
- **What it does**: Adds custom styling beyond what Tailwind provides
- **Why it's important**: Allows for custom design elements

### **wwwroot/css/web-app.css**
- **What it is**: CSS for the web application
- **What it does**: Styles specific to the web app interface
- **Why it's important**: Provides styling for web-specific features

### **wwwroot/css/landing-page.css**
- **What it is**: CSS for the landing page
- **What it does**: Styles the main landing page to make it attractive
- **Why it's important**: Creates good first impression for visitors

### **wwwroot/css/plaid-demo.css**
- **What it is**: CSS for Plaid demo components
- **What it does**: Styles the Plaid integration demo
- **Why it's important**: Makes the financial integration demo look professional

## 📁 JavaScript Files

### **wwwroot/js/site.js**
- **What it is**: Main JavaScript file for non-React pages
- **What it does**: Handles JavaScript functionality for traditional web pages
- **Why it's important**: Provides interactivity for non-React parts of your app

### **wwwroot/js/react/index.js**
- **What it is**: Entry point for React application
- **What it does**: 
  - Starts the React app
  - Connects it to the HTML page
  - Sets up the root component
- **Why it's important**: This is how React gets started

### **wwwroot/js/react/dashboard-entry.js**
- **What it is**: Entry point for dashboard React app
- **What it does**: Specifically starts the dashboard React application
- **Why it's important**: Loads the dashboard when needed

### **wwwroot/js/react/home-entry.js**
- **What it is**: Entry point for home page React app
- **What it does**: Specifically starts the home page React application
- **Why it's important**: Loads the home page when needed

## 📁 Test Files

### **wwwroot/js/react/components/__tests__/Dashboard.test.tsx**
- **What it is**: Test file for Dashboard component
- **What it does**: Tests that the Dashboard component works correctly
- **Why it's important**: Ensures dashboard functionality works as expected

### **wwwroot/js/react/setupTests.js**
- **What it is**: Test setup configuration
- **What it does**: Configures the testing environment for React components
- **Why it's important**: Sets up proper testing conditions

## 📁 HTML Test Files

### **wwwroot/js/react/react-test.html**
- **What it is**: HTML file for testing React components
- **What it does**: Provides a simple page to test React features
- **Why it's important**: Allows testing React components in isolation

### **wwwroot/js/react/simple-react-test.html**
- **What it is**: Simple React testing page
- **What it does**: Basic page for testing React functionality
- **Why it's important**: Simple testing environment

### **wwwroot/js/react/simple-test.html**
- **What it is**: Basic testing page
- **What it does**: Simple HTML page for testing
- **Why it's important**: Basic testing environment

## 📁 Documentation

### **README.md**
- **What it is**: Main project documentation
- **What it does**: Explains what the project is, how to set it up, and how to use it
- **Why it's important**: Helps developers understand and work with the project

### **README_PLAID.md**
- **What it is**: Plaid integration documentation
- **What it does**: Explains how to set up and use Plaid bank account integration
- **Why it's important**: Specific guide for financial integration feature

### **README_WEBSOCKET.md**
- **What it is**: WebSocket documentation
- **What it does**: Explains how real-time data communication works
- **Why it's important**: Guide for implementing live features

## 📁 Database Migration Files

### **Migrations/20250304011420_InitialCreate.cs**
- **What it is**: Database migration file
- **What it does**: Creates the initial database structure
- **Why it's important**: Sets up the database tables for your app

### **Migrations/20250304011420_InitialCreate.Designer.cs**
- **What it is**: Migration designer file
- **What it does**: Generated file that helps Entity Framework manage migrations
- **Why it's important**: Required for database migration system to work

### **Migrations/ApplicationDbContextModelSnapshot.cs**
- **What it is**: Database model snapshot
- **What it does**: Current state of your database structure
- **Why it's important**: Helps Entity Framework track database changes

## 📁 Launch Settings

### **Properties/launchSettings.json**
- **What it is**: Visual Studio launch configuration
- **What it does**: Defines how the app starts when you run it from Visual Studio
- **Why it's important**: Ensures proper development environment setup

## 📁 Library Files

### **wwwroot/lib/bootstrap/LICENSE**
- **What it is**: Bootstrap license file
- **What it does**: Legal information for Bootstrap CSS framework
- **Why it's important**: Required for using Bootstrap

### **wwwroot/lib/jquery/LICENSE.txt**
- **What it is**: jQuery license file
- **What it does**: Legal information for jQuery library
- **Why it's important**: Required for using jQuery

### **wwwroot/lib/jquery-validation/LICENSE.md**
- **What it is**: jQuery validation license
- **What it does**: Legal information for form validation library
- **Why it's important**: Required for using form validation

### **wwwroot/lib/jquery-validation-unobtrusive/LICENSE.txt**
- **What it is**: Unobtrusive validation license
- **What it does**: Legal information for unobtrusive validation
- **Why it's important**: Required for using unobtrusive validation

## 📁 Images

### **wwwroot/images/task-management.svg**
- **What it is**: Task management illustration
- **What it does**: Visual representation for task management features
- **Why it's important**: Makes the app more visually appealing

## 📁 Favicon

### **wwwroot/favicon.ico**
- **What it is**: Website icon
- **What it does**: Shows in browser tabs and bookmarks
- **Why it's important**: Professional appearance and brand recognition

---

## 🔄 How Everything Works Together

1. **User visits your website** → `Views/Home/Index.cshtml` shows the landing page
2. **User clicks "Get Started"** → `Views/Auth/Register.cshtml` shows registration form
3. **User fills out form** → `Controllers/AuthController.cs` processes the registration
4. **User data is saved** → `Models/ApplicationUser.cs` stores user information in database
5. **User logs in** → `Services/JwtService.cs` creates secure authentication token
6. **User sees dashboard** → `Views/Dashboard/Index.cshtml` shows investment overview
7. **User connects bank** → `Controllers/PlaidController.cs` handles Plaid integration
8. **Financial data flows** → `Models/PlaidModels.cs` stores account and transaction data
9. **User sees analysis** → `Services/GRPVService.cs` performs investment calculations
10. **User interacts socially** → `Controllers/SocialController.cs` manages friends and messages

This architecture separates concerns clearly:
- **Controllers** handle user requests
- **Services** contain business logic
- **Models** define data structure
- **Views** display information
- **React components** provide modern interactivity

Each file has a specific purpose, making the code easy to understand, maintain, and extend.
