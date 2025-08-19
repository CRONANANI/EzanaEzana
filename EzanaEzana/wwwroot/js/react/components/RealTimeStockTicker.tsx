import React, { useState, useEffect } from 'react';
import { useMultipleRealTimeStocks } from '../hooks/useRealTimeStockData';
import { marketApiService } from '../services/marketApi';

interface StockTickerProps {
  symbols?: string[];
  autoConnect?: boolean;
  showConnectionStatus?: boolean;
  refreshInterval?: number; // in milliseconds, 0 to disable
}

/**
 * Real-time Stock Ticker Component
 * 
 * Displays live stock prices with WebSocket real-time updates
 * Automatically connects to Finnhub WebSocket API for live data
 */
const RealTimeStockTicker: React.FC<StockTickerProps> = ({
  symbols = ['AAPL', 'MSFT', 'GOOGL', 'AMZN', 'TSLA'],
  autoConnect = true,
  showConnectionStatus = true,
  refreshInterval = 0 // 0 means no manual refresh, only WebSocket updates
}) => {
  const [isWebSocketConnected, setIsWebSocketConnected] = useState(false);
  const [connectionError, setConnectionError] = useState<string | null>(null);
  const [manualRefreshCount, setManualRefreshCount] = useState(0);

  const {
    quotes,
    isConnected,
    error,
    isLoading,
    refreshData,
    allQuotes,
    totalValue
  } = useMultipleRealTimeStocks(symbols);

  // Manual refresh interval
  useEffect(() => {
    if (refreshInterval > 0) {
      const interval = setInterval(() => {
        refreshData();
        setManualRefreshCount(prev => prev + 1);
      }, refreshInterval);

      return () => clearInterval(interval);
    }
  }, [refreshInterval, refreshData]);

  // WebSocket connection status
  useEffect(() => {
    const checkWebSocketStatus = () => {
      const status = marketApiService.getWebSocketStatus();
      setIsWebSocketConnected(status);
    };

    if (autoConnect) {
      // Try to connect WebSocket if not connected
      if (!marketApiService.getWebSocketStatus()) {
        marketApiService.connectWebSocket()
          .then(() => {
            setIsWebSocketConnected(true);
            setConnectionError(null);
          })
          .catch((error) => {
            console.error('WebSocket connection failed:', error);
            setConnectionError('Real-time updates unavailable');
            setIsWebSocketConnected(false);
          });
      } else {
        setIsWebSocketConnected(true);
      }
    }

    // Check status periodically
    const statusInterval = setInterval(checkWebSocketStatus, 5000);
    return () => clearInterval(statusInterval);
  }, [autoConnect]);

  const handleManualRefresh = async () => {
    try {
      await refreshData();
      setManualRefreshCount(prev => prev + 1);
    } catch (error) {
      console.error('Manual refresh failed:', error);
    }
  };

  const toggleWebSocket = async () => {
    if (isWebSocketConnected) {
      // Disconnect
      marketApiService['webSocketService'].disconnect();
      setIsWebSocketConnected(false);
    } else {
      // Connect
      try {
        await marketApiService.connectWebSocket();
        setIsWebSocketConnected(true);
        setConnectionError(null);
      } catch (error) {
        setConnectionError('Failed to connect');
      }
    }
  };

  if (isLoading && allQuotes.length === 0) {
    return (
      <div className="stock-ticker loading">
        <div className="loading-spinner">Loading stock data...</div>
      </div>
    );
  }

  return (
    <div className="stock-ticker">
      {/* Connection Status */}
      {showConnectionStatus && (
        <div className="connection-status">
          <div className="status-indicators">
            <span className={`status-dot ${isWebSocketConnected ? 'connected' : 'disconnected'}`}></span>
            <span className="status-text">
              {isWebSocketConnected ? 'Real-time Connected' : 'Real-time Disconnected'}
            </span>
            {connectionError && (
              <span className="error-text">{connectionError}</span>
            )}
          </div>
          <div className="connection-controls">
            <button 
              onClick={toggleWebSocket}
              className={`btn ${isWebSocketConnected ? 'btn-danger' : 'btn-success'}`}
            >
              {isWebSocketConnected ? 'Disconnect' : 'Connect'}
            </button>
            <button onClick={handleManualRefresh} className="btn btn-primary">
              Refresh ({manualRefreshCount})
            </button>
          </div>
        </div>
      )}

      {/* Stock Ticker */}
      <div className="ticker-container">
        <div className="ticker-header">
          <h3>Live Stock Prices</h3>
          <div className="ticker-info">
            <span>Total Value: ${totalValue.toLocaleString()}</span>
            <span>Last Update: {new Date().toLocaleTimeString()}</span>
          </div>
        </div>
        
        <div className="ticker-grid">
          {allQuotes.map((quote) => (
            <div key={quote.symbol} className="stock-card">
              <div className="stock-header">
                <h4 className="stock-symbol">{quote.symbol}</h4>
                <span className="stock-name">{quote.name}</span>
                <span className={`real-time-indicator ${quote.isRealTime ? 'active' : 'inactive'}`}>
                  {quote.isRealTime ? 'LIVE' : 'CACHED'}
                </span>
              </div>
              
              <div className="stock-price">
                <span className="price">${quote.price.toFixed(2)}</span>
                <span className={`change ${quote.change >= 0 ? 'positive' : 'negative'}`}>
                  {quote.change >= 0 ? '+' : ''}{quote.change.toFixed(2)} ({quote.changePercent.toFixed(2)}%)
                </span>
              </div>
              
              <div className="stock-details">
                <span className="volume">Vol: {quote.volume.toLocaleString()}</span>
                <span className="last-updated">
                  {quote.lastUpdated ? new Date(quote.lastUpdated).toLocaleTimeString() : 'N/A'}
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Error Display */}
      {error && (
        <div className="error-message">
          <strong>Error:</strong> {error}
        </div>
      )}
    </div>
  );
};

export default RealTimeStockTicker;
