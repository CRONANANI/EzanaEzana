# Auth0 Integration Setup Guide

This guide explains how to set up Auth0 OAuth integration with your Ezana application.

## 🔧 Prerequisites

- Auth0 account (free tier available)
- Your application running locally or deployed

## 📋 Step-by-Step Setup

### 1. Auth0 Application Configuration

1. **Log into Auth0 Dashboard**
   - Go to [auth0.com](https://auth0.com) and sign in
   - Navigate to your tenant: `dev-epju3rl0oby7xrxm.us.auth0.com`

2. **Create/Configure Application**
   - Go to **Applications** → **Applications**
   - Select your existing application or create a new one
   - Application Type: **Regular Web Application**

3. **Configure URLs**
   - **Allowed Callback URLs**: `http://localhost:5226/callback, https://yourdomain.com/callback`
   - **Allowed Logout URLs**: `http://localhost:5226, https://yourdomain.com`
   - **Allowed Web Origins**: `http://localhost:5226, https://yourdomain.com`

4. **Get Credentials**
   - Copy your **Domain**: `dev-epju3rl0oby7xrxm.us.auth0.com`
   - Copy your **Client ID**: `A2ulhcMNyyVriJ4inY6zHRBgd5gj7YI1`
   - **Note**: Client Secret is not needed with the official SDK

### 2. Update Configuration

1. **Update `appsettings.json`**
   ```json
   "Auth0": {
     "Domain": "dev-epju3rl0oby7xrxm.us.auth0.com",
     "ClientId": "A2ulhcMNyyVriJ4inY6zHRBgd5gj7YI1"
   }
   ```

2. **No additional configuration needed** - The official SDK handles everything automatically

### 3. Database Migration

Since we added the `Auth0Id` field to the `ApplicationUser` model, you'll need to create and run a migration:

```bash
dotnet ef migrations add AddAuth0Id
dotnet ef database update
```

**Note**: For development with in-memory database, this isn't necessary as the database is recreated each time.

## 🚀 How It Works

### Authentication Flow

1. **User clicks "Continue with Auth0"**
   - Redirects to Auth0 login page
   - User authenticates with Auth0 (Google, Microsoft, etc.)

2. **Auth0 redirects back to your app**
   - Callback URL: `/callback` (handled automatically by the SDK)
   - User is automatically signed in to your application

3. **User creation/update**
   - If new user: Creates account in your database
   - If existing user: Updates Auth0 ID if needed
   - Signs user into your application

4. **User is redirected to dashboard**
   - Ready to use all features

### User Management

- **Users are automatically authenticated** when they log in via Auth0
- **User information is extracted** from the ID Token and available as User.Claims
- **Profile information** includes name, email, and profile picture
- **No database user creation needed** - Auth0 handles user management

## 🔒 Security Features

- **JWT token validation** for API requests
- **OpenID Connect** for secure authentication
- **Automatic token refresh** handled by Auth0
- **Secure logout** that clears both local and Auth0 sessions

## 🧪 Testing

1. **Start your application**: `dotnet run`
2. **Navigate to**: `http://localhost:5226`
3. **Click**: "Continue with Auth0"
4. **Complete Auth0 login**
5. **Verify**: User is created and redirected to dashboard

## 🐛 Troubleshooting

### Common Issues

1. **"Invalid redirect URI"**
   - Check callback URLs in Auth0 dashboard
   - Ensure localhost URLs are included for development

2. **"Client ID not found"**
   - Verify Client ID in `appsettings.json`
   - Check Auth0 application settings

3. **"Invalid audience"**
   - Verify audience URL in Auth0 dashboard
   - Check `appsettings.json` audience setting

4. **User not created**
   - Check database connection
   - Verify user creation logic in `Auth0Controller.Callback()`

### Debug Steps

1. **Check browser console** for JavaScript errors
2. **Check application logs** for .NET errors
3. **Verify Auth0 configuration** matches your settings
4. **Test with simple Auth0 application** first

## 📱 Mobile/SPA Support

For mobile apps or single-page applications, you can also configure:
- **Mobile Application** type in Auth0
- **Single Page Application** type for React apps
- **Native Application** type for desktop apps

## 🔄 Next Steps

After successful integration:

1. **Customize user profile** creation
2. **Add role-based access control**
3. **Implement user preferences sync**
4. **Add social login providers** (Google, Facebook, etc.)
5. **Set up user analytics** and tracking

## 📚 Resources

- [Auth0 Documentation](https://auth0.com/docs)
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
- [OpenID Connect Specification](https://openid.net/connect/)

## 🆘 Support

If you encounter issues:

1. Check this guide first
2. Review Auth0 application logs
3. Check your application logs
4. Verify all configuration settings
5. Test with minimal configuration

---

**Note**: Keep your Client Secret secure and never commit it to version control. Use environment variables or secure configuration management in production.
