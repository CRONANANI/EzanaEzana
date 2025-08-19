# Quiver API Integration Documentation

## Overview
This document details the integration of the Quiver Quant API into the Ezana application for accessing alternative financial data including congressional trading, government contracts, lobbying activity, and more.

## Configuration

### API Credentials
- **Base URL**: `https://api.quiverquant.com/beta`
- **API Key**: `2fb95c89103d4cb07b26fff07c8cfa77626291da`
- **Authentication**: X-API-KEY header
- **Timeout**: 30 seconds

### Configuration File
The API credentials are stored in `appsettings.json`:

```json
{
  "Quiver": {
    "ApiKey": "2fb95c89103d4cb07b26fff07c8cfa77626291da",
    "BaseUrl": "https://api.quiverquant.com/beta",
    "TimeoutSeconds": 30
  }
}
```

## Backend Architecture

### Models (`Models/QuiverModels.cs`)
The application uses strongly-typed C# models to represent Quiver API responses:

- **CongressionalTrading**: Senate and House trading data
- **GovernmentContract**: Government contract information
- **HouseTrading**: House of Representatives trading data
- **SenatorTrading**: Senate trading data
- **LobbyingActivity**: Corporate lobbying information
- **PatentMomentum**: Patent activity data
- **CongresspersonPortfolio**: Individual congressperson portfolio details

### Service Layer (`Services/QuiverService.cs`)
The `QuiverService` implements the `IQuiverService` interface and provides:

- HTTP client configuration with API key authentication
- Generic API call method with error handling
- Fallback to mock data when API calls fail
- Methods for all Quiver data types

### API Controllers

#### QuiverController (`/api/quiver/`)
- `GET /api/quiver/market-intelligence/summary` - Aggregated market intelligence data
- `GET /api/quiver/congressional-trading` - Congressional trading data
- `GET /api/quiver/government-contracts` - Government contract data
- `GET /api/quiver/house-trading` - House trading data
- `GET /api/quiver/senator-trading` - Senate trading data
- `GET /api/quiver/lobbying-activity` - Lobbying activity data
- `GET /api/quiver/patent-momentum` - Patent momentum data
- `GET /api/quiver/congressperson/{name}/portfolio` - Individual congressperson portfolio

#### DashboardController (`/api/`)
- `GET /api/portfolio/summary` - User portfolio summary
- `GET /api/portfolio/asset-allocation` - Portfolio asset allocation
- `GET /api/portfolio/top-holdings` - Top portfolio holdings
- `GET /api/portfolio/history` - Portfolio performance history
- `GET /api/market-intelligence/summary` - Market intelligence summary
- `GET /api/congressperson/{name}/portfolio` - Congressperson portfolio (redirects to QuiverController)

## Frontend Integration

### API Base URL
The frontend uses `/api` as the base URL for all API calls.

### Data Fetching
The `playground-test.html` file includes JavaScript functions that:

1. **Load Card Data**: `loadCardData(dataType, contentContainer)` fetches data from appropriate endpoints
2. **Show Congressperson Portfolio**: `showCongressPersonPortfolio(congressPersonName)` displays individual portfolio details
3. **Render Data Tables**: `renderCardDataTable(dataType)` creates HTML tables from API responses

### Error Handling
- Loading states with spinner animations
- Error messages with retry buttons
- Graceful fallback to mock data when APIs are unavailable

## API Endpoints Reference

### Congressional Trading
- **Endpoint**: `GET /api/quiver/congressional-trading`
- **Purpose**: Retrieve recent trading activity by members of Congress
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `CongressionalTrading` objects

### Government Contracts
- **Endpoint**: `GET /api/quiver/government-contracts`
- **Purpose**: Access government contract award data
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `GovernmentContract` objects

### House Trading
- **Endpoint**: `GET /api/quiver/house-trading`
- **Purpose**: House of Representatives trading data
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `HouseTrading` objects

### Senator Trading
- **Endpoint**: `GET /api/quiver/senator-trading`
- **Purpose**: Senate trading data
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `SenatorTrading` objects

### Lobbying Activity
- **Endpoint**: `GET /api/quiver/lobbying-activity`
- **Purpose**: Corporate lobbying information
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `LobbyingActivity` objects

### Patent Momentum
- **Endpoint**: `GET /api/quiver/patent-momentum`
- **Purpose**: Patent activity and momentum data
- **Parameters**: `limit` (optional, default: 100)
- **Response**: List of `PatentMomentum` objects

### Congressperson Portfolio
- **Endpoint**: `GET /api/quiver/congressperson/{name}/portfolio`
- **Purpose**: Individual congressperson portfolio details
- **Parameters**: `name` (path parameter)
- **Response**: `CongresspersonPortfolio` object

## Testing

### Test Page
Access `/Dashboard/TestQuiver` to test individual API endpoints independently.

### Health Check
- **Endpoint**: `GET /api/quiver/health`
- **Purpose**: Verify API connectivity and configuration
- **Access**: Public (no authentication required)
- **Response**: JSON object with service status, API key configuration, and connectivity test results

### Testing API Connectivity
1. **Health Check**: First test the health endpoint to verify basic connectivity
2. **Individual Endpoints**: Use the test page to test specific data endpoints
3. **Browser Developer Tools**: Check the Network tab for HTTP status codes and response details
4. **Application Logs**: Review backend logs for detailed error information

## Deployment Considerations

### Environment Variables
For production deployment, consider using environment variables instead of hardcoded API keys:

```bash
export QUIVER_API_KEY="your-production-api-key"
export QUIVER_BASE_URL="https://api.quiverquant.com"
```

### API Rate Limits
- Monitor API usage to stay within Quiver's rate limits
- Implement caching where appropriate
- Use mock data as fallback during high-traffic periods

### Security
- API keys are stored in configuration files
- All endpoints require authentication (`[Authorize]` attribute)
- HTTPS is enforced in production

## Troubleshooting

### Common Issues

1. **"Failed to fetch data from quiver"**
   - Check API key configuration in `appsettings.json`
   - Verify network connectivity to `https://api.quiverquant.com/beta`
   - Check application logs for detailed error messages
   - Verify the API key is valid and has necessary permissions
   - Test the health check endpoint: `GET /api/quiver/health`

2. **Authentication Errors**
   - Ensure API key is correctly configured
   - Verify the API key has necessary permissions
   - Check if the API key has expired

3. **Timeout Errors**
   - Increase `TimeoutSeconds` in configuration if needed
   - Check network latency to Quiver API
   - Consider implementing retry logic

### Debug Steps

1. **Check Configuration**
   ```bash
   # Verify appsettings.json contains correct values
   cat appsettings.json | grep -A 5 "Quiver"
   ```

2. **Test API Connectivity**
   ```bash
   # Test health check endpoint
   curl -X GET "https://your-domain.com/api/quiver/health"
   ```

3. **Review Logs**
   - Check application logs for detailed error messages
   - Look for HTTP status codes and error details

4. **Verify Frontend Routes**
   - Ensure frontend JavaScript uses correct API endpoints
   - Check browser developer tools for network request errors

## Mock Data

When the Quiver API is unavailable, the system automatically falls back to realistic mock data:

- **Congressional Trading**: Simulated trading data with realistic amounts and dates
- **Government Contracts**: Mock contract awards with various agencies and amounts
- **Portfolio Data**: Sample portfolio holdings and performance metrics

This ensures the application remains functional for development and testing purposes.

## License

This integration is for use with the Ezana application. Please refer to Quiver Quant's terms of service for API usage rights and limitations.

## Support

For issues with the Quiver API integration:
1. Check this documentation
2. Review application logs
3. Test individual endpoints using the test page
4. Verify API credentials and configuration

For Quiver API-specific issues, refer to their official documentation at [https://api.quiverquant.com](https://api.quiverquant.com).
