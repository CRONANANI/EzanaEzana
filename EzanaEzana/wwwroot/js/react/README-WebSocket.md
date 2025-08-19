# Real-Time Stock Data with WebSocket Integration

This project now includes WebSocket connections to Finnhub API for real-time stock price updates, eliminating the need for constant API polling.

## Features

- **Real-time Stock Prices**: Live price updates via WebSocket connections
- **Automatic Reconnection**: Handles connection drops and automatically reconnects
- **Price Caching**: Intelligent caching to reduce API calls
- **React Hooks**: Easy-to-use hooks for real-time data in React components
- **Connection Management**: Visual indicators and controls for WebSocket status

## Architecture

### WebSocket Service (`WebSocketService`)
- Manages WebSocket connections to Finnhub API
- Handles automatic reconnection with exponential backoff
- Manages subscriptions to stock symbols
- Implements ping/pong heartbeat mechanism

### Market API Service (`MarketApiService`)
- Integrates WebSocket data with REST API calls
- Caches stock quotes and updates them in real-time
- Provides fallback to mock data when APIs fail
- Manages price update subscriptions

### React Hooks
- `useRealTimeStockData`: Single stock symbol tracking
- `useMultipleRealTimeStocks`: Multiple stock symbols tracking

## Usage

### Basic Real-Time Stock Tracking

```tsx
import { useRealTimeStockData } from './hooks/useRealTimeStockData';

function StockTracker({ symbol }) {
  const { quote, isConnected, error, isLoading, isRealTime } = useRealTimeStockData(symbol);

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div>
      <h3>{quote.symbol} - {quote.name}</h3>
      <div className={`price ${isRealTime ? 'live' : 'cached'}`}>
        ${quote.price}
        {isRealTime && <span className="live-indicator">LIVE</span>}
      </div>
      <div className="change">
        {quote.change >= 0 ? '+' : ''}{quote.change} ({quote.changePercent}%)
      </div>
      <div className="connection-status">
        {isConnected ? '🟢 Connected' : '🔴 Disconnected'}
      </div>
    </div>
  );
}
```

### Multiple Stocks with Real-Time Updates

```tsx
import { useMultipleRealTimeStocks } from './hooks/useRealTimeStockData';

function StockPortfolio() {
  const symbols = ['AAPL', 'MSFT', 'GOOGL', 'AMZN', 'TSLA'];
  const { quotes, isConnected, allQuotes, totalValue } = useMultipleRealTimeStocks(symbols);

  return (
    <div>
      <h2>Portfolio Overview</h2>
      <div className="connection-status">
        WebSocket: {isConnected ? '🟢 Connected' : '🔴 Disconnected'}
      </div>
      <div className="total-value">Total: ${totalValue.toLocaleString()}</div>
      
      <div className="stocks-grid">
        {allQuotes.map(quote => (
          <div key={quote.symbol} className="stock-card">
            <h3>{quote.symbol}</h3>
            <div className="price">${quote.price}</div>
            <div className="change">{quote.change}%</div>
            <div className="status">
              {quote.isRealTime ? '🟢 LIVE' : '⚪ CACHED'}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
```

### Real-Time Stock Ticker Component

```tsx
import RealTimeStockTicker from './components/RealTimeStockTicker';

function Dashboard() {
  return (
    <div>
      <h1>Investment Dashboard</h1>
      
      <RealTimeStockTicker 
        symbols={['AAPL', 'MSFT', 'GOOGL', 'AMZN', 'TSLA']}
        autoConnect={true}
        showConnectionStatus={true}
        refreshInterval={0} // 0 = WebSocket only, no manual refresh
      />
    </div>
  );
}
```

## Configuration

### Finnhub API Setup

1. Get your API key from [Finnhub](https://finnhub.io/)
2. Update the API key in `marketApi.ts`:

```typescript
const API_CONFIG = {
  FINNHUB: {
    BASE_URL: 'https://finnhub.io/api/v1',
    WEBSOCKET_URL: 'wss://ws.finnhub.io',
    API_KEY: 'your-api-key-here',
    FREE_TIER_LIMIT: 60
  }
};
```

### WebSocket Connection Options

- **Auto-connect**: Automatically establish WebSocket connection on component mount
- **Manual control**: Use `connectWebSocket()` and `disconnect()` methods
- **Reconnection**: Automatic reconnection with configurable retry limits
- **Heartbeat**: Ping/pong mechanism to maintain connection health

## API Methods

### WebSocket Service

```typescript
// Connect to WebSocket
await webSocketService.connect();

// Subscribe to stock symbol
webSocketService.subscribe('AAPL');

// Unsubscribe from stock symbol
webSocketService.unsubscribe('AAPL');

// Check connection status
const isConnected = webSocketService.isConnectedStatus();

// Get subscribed symbols
const symbols = webSocketService.getSubscribedSymbols();

// Disconnect
webSocketService.disconnect();
```

### Market API Service

```typescript
// Get real-time stock quote
const quote = await marketApiService.getStockQuote('AAPL');

// Subscribe to price updates
marketApiService.subscribeToPriceUpdates('AAPL', (updatedQuote) => {
  console.log('Price updated:', updatedQuote);
});

// Get cached real-time data
const cachedQuote = marketApiService.getCachedQuote('AAPL');

// Connect WebSocket
await marketApiService.connectWebSocket();

// Check WebSocket status
const status = marketApiService.getWebSocketStatus();
```

## Error Handling

The service includes comprehensive error handling:

- **API Failures**: Automatic fallback to mock data
- **WebSocket Disconnections**: Automatic reconnection attempts
- **Rate Limiting**: Respects Finnhub's free tier limits
- **Network Issues**: Graceful degradation and user feedback

## Performance Considerations

- **Connection Pooling**: Single WebSocket connection for multiple symbols
- **Smart Caching**: Avoids redundant API calls
- **Efficient Updates**: Only updates changed data
- **Memory Management**: Automatic cleanup of unused subscriptions

## Browser Support

- Modern browsers with WebSocket support
- Fallback to REST API for older browsers
- Progressive enhancement approach

## Troubleshooting

### Common Issues

1. **WebSocket Connection Fails**
   - Check API key validity
   - Verify network connectivity
   - Check browser WebSocket support

2. **No Real-Time Updates**
   - Ensure WebSocket is connected
   - Check symbol subscriptions
   - Verify API rate limits

3. **High Memory Usage**
   - Unsubscribe from unused symbols
   - Check for memory leaks in React components
   - Monitor WebSocket connection count

### Debug Mode

Enable debug logging:

```typescript
// In browser console
localStorage.setItem('debug', 'marketApi,webSocket');
```

## Future Enhancements

- [ ] Support for additional data types (options, futures)
- [ ] Advanced filtering and alerting
- [ ] Historical data streaming
- [ ] Multi-exchange support
- [ ] Offline mode with local caching
- [ ] WebSocket compression for high-frequency data

## Contributing

When adding new features:

1. Follow the existing WebSocket service pattern
2. Add proper error handling and fallbacks
3. Include TypeScript types for all new interfaces
4. Add comprehensive tests for new functionality
5. Update this documentation

## License

This WebSocket integration is part of the EzanaEzana project and follows the same licensing terms.
