import { useState, useEffect } from 'react';
import { marketApiService, StockQuote } from '../services/marketApi';

/**
 * React Hook for Real-time Stock Data
 * 
 * This hook provides real-time stock price updates using WebSocket connections
 * through the Finnhub API. It automatically manages subscriptions and cleanup.
 * 
 * @param symbol - The stock symbol to track (e.g., 'AAPL', 'MSFT')
 * @returns Object containing quote data, connection status, and utility functions
 */
export const useRealTimeStockData = (symbol: string) => {
  const [quote, setQuote] = useState<StockQuote | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    let isMounted = true;

    const handlePriceUpdate = (updatedQuote: StockQuote) => {
      if (isMounted) {
        setQuote(updatedQuote);
        setIsLoading(false);
      }
    };

    const initializeData = async () => {
      try {
        setIsLoading(true);
        setError(null);

        // Get initial quote
        const initialQuote = await marketApiService.getStockQuote(symbol);
        if (isMounted) {
          setQuote(initialQuote);
          setIsLoading(false);
        }

        // Subscribe to real-time updates
        marketApiService.subscribeToPriceUpdates(symbol, handlePriceUpdate);

        // Check WebSocket connection status
        setIsConnected(marketApiService.getWebSocketStatus());

        // Connect WebSocket if not already connected
        if (!marketApiService.getWebSocketStatus()) {
          try {
            await marketApiService.connectWebSocket();
            setIsConnected(true);
          } catch (wsError) {
            console.error('WebSocket connection failed:', wsError);
            setError('Real-time updates unavailable');
          }
        }
      } catch (err) {
        if (isMounted) {
          setError(err instanceof Error ? err.message : 'Failed to load stock data');
          setIsLoading(false);
        }
      }
    };

    initializeData();

    return () => {
      isMounted = false;
      marketApiService.unsubscribeFromPriceUpdates(symbol, handlePriceUpdate);
    };
  }, [symbol]);

  const refreshData = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const updatedQuote = await marketApiService.getStockQuote(symbol);
      setQuote(updatedQuote);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to refresh data');
    } finally {
      setIsLoading(false);
    }
  };

  const toggleRealTime = async () => {
    if (isConnected) {
      // Disconnect WebSocket
      marketApiService.unsubscribeFromPriceUpdates(symbol, () => {});
      setIsConnected(false);
    } else {
      // Reconnect WebSocket
      try {
        await marketApiService.connectWebSocket();
        marketApiService.subscribeToPriceUpdates(symbol, (updatedQuote) => {
          setQuote(updatedQuote);
        });
        setIsConnected(true);
      } catch (error) {
        setError('Failed to enable real-time updates');
      }
    }
  };

  return {
    quote,
    isConnected,
    error,
    isLoading,
    refreshData,
    toggleRealTime,
    isRealTime: quote?.isRealTime || false,
    lastUpdated: quote?.lastUpdated,
    // Helper properties for easy access
    price: quote?.price || 0,
    change: quote?.change || 0,
    changePercent: quote?.changePercent || 0,
    volume: quote?.volume || 0,
    name: quote?.name || symbol
  };
};

/**
 * Hook for multiple stock symbols
 */
export const useMultipleRealTimeStocks = (symbols: string[]) => {
  const [quotes, setQuotes] = useState<Map<string, StockQuote>>(new Map());
  const [isConnected, setIsConnected] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    let isMounted = true;

    const handlePriceUpdate = (symbol: string, updatedQuote: StockQuote) => {
      if (isMounted) {
        setQuotes(prev => new Map(prev).set(symbol, updatedQuote));
      }
    };

    const initializeData = async () => {
      try {
        setIsLoading(true);
        setError(null);

        // Get initial quotes for all symbols
        const initialQuotes = await marketApiService.getMultipleStockQuotes(symbols);
        if (isMounted) {
          const quotesMap = new Map();
          initialQuotes.forEach(quote => {
            quotesMap.set(quote.symbol, quote);
            // Subscribe to real-time updates for each symbol
            marketApiService.subscribeToPriceUpdates(quote.symbol, (updatedQuote) => {
              handlePriceUpdate(quote.symbol, updatedQuote);
            });
          });
          setQuotes(quotesMap);
          setIsLoading(false);
        }

        // Check WebSocket connection status
        setIsConnected(marketApiService.getWebSocketStatus());

        // Connect WebSocket if not already connected
        if (!marketApiService.getWebSocketStatus()) {
          try {
            await marketApiService.connectWebSocket();
            setIsConnected(true);
          } catch (wsError) {
            console.error('WebSocket connection failed:', wsError);
            setError('Real-time updates unavailable');
          }
        }
      } catch (err) {
        if (isMounted) {
          setError(err instanceof Error ? err.message : 'Failed to load stock data');
          setIsLoading(false);
        }
      }
    };

    initializeData();

    return () => {
      isMounted = false;
      // Unsubscribe from all symbols
      symbols.forEach(symbol => {
        marketApiService.unsubscribeFromPriceUpdates(symbol, () => {});
      });
    };
  }, [symbols.join(',')]); // Re-run when symbols change

  const refreshData = async () => {
    try {
      setIsLoading(true);
      setError(null);
      const updatedQuotes = await marketApiService.getMultipleStockQuotes(symbols);
      const quotesMap = new Map();
      updatedQuotes.forEach(quote => {
        quotesMap.set(quote.symbol, quote);
      });
      setQuotes(quotesMap);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to refresh data');
    } finally {
      setIsLoading(false);
    }
  };

  return {
    quotes,
    isConnected,
    error,
    isLoading,
    refreshData,
    getQuote: (symbol: string) => quotes.get(symbol) || null,
    // Helper properties for easy access
    allQuotes: Array.from(quotes.values()),
    totalValue: Array.from(quotes.values()).reduce((sum, quote) => sum + quote.price, 0)
  };
};
