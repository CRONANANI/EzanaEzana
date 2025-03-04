/**
 * API Service
 * 
 * This service handles all API calls to the backend.
 * It provides methods for fetching data from various endpoints.
 */

// Base API URL - in a real app, this would be configured based on environment
const API_BASE_URL = '/api';

/**
 * Generic fetch wrapper with error handling
 */
async function fetchApi(endpoint, options = {}) {
  try {
    const response = await fetch(`${API_BASE_URL}${endpoint}`, {
      headers: {
        'Content-Type': 'application/json',
        ...options.headers
      },
      ...options
    });

    if (!response.ok) {
      throw new Error(`API error: ${response.status} ${response.statusText}`);
    }

    return await response.json();
  } catch (error) {
    console.error('API request failed:', error);
    throw error;
  }
}

/**
 * Portfolio API methods
 */
export const portfolioApi = {
  /**
   * Get portfolio summary
   */
  getSummary: async () => {
    return fetchApi('/portfolio/summary');
  },

  /**
   * Get portfolio performance data
   */
  getPerformance: async (timeframe = '1M') => {
    return fetchApi(`/portfolio/performance?timeframe=${timeframe}`);
  },

  /**
   * Get asset allocation
   */
  getAssetAllocation: async () => {
    return fetchApi('/portfolio/allocation');
  },

  /**
   * Get sector breakdown
   */
  getSectorBreakdown: async () => {
    return fetchApi('/portfolio/sectors');
  }
};

/**
 * Stocks API methods
 */
export const stocksApi = {
  /**
   * Get user's stock holdings
   */
  getHoldings: async () => {
    return fetchApi('/stocks/holdings');
  },

  /**
   * Get stock details
   */
  getStockDetails: async (symbol) => {
    return fetchApi(`/stocks/${symbol}`);
  },

  /**
   * Get stock price history
   */
  getPriceHistory: async (symbol, timeframe = '1M') => {
    return fetchApi(`/stocks/${symbol}/history?timeframe=${timeframe}`);
  },

  /**
   * Get stock news
   */
  getStockNews: async (symbol) => {
    return fetchApi(`/stocks/${symbol}/news`);
  },

  /**
   * Get watchlist
   */
  getWatchlist: async () => {
    return fetchApi('/stocks/watchlist');
  },

  /**
   * Add stock to watchlist
   */
  addToWatchlist: async (symbol) => {
    return fetchApi('/stocks/watchlist', {
      method: 'POST',
      body: JSON.stringify({ symbol })
    });
  },

  /**
   * Remove stock from watchlist
   */
  removeFromWatchlist: async (symbol) => {
    return fetchApi(`/stocks/watchlist/${symbol}`, {
      method: 'DELETE'
    });
  }
};

/**
 * Preferences API methods
 */
export const preferencesApi = {
  /**
   * Get user preferences
   */
  getPreferences: async () => {
    return fetchApi('/preferences');
  },

  /**
   * Save user preferences
   */
  savePreferences: async (preferences) => {
    return fetchApi('/preferences', {
      method: 'POST',
      body: JSON.stringify(preferences)
    });
  }
};

/**
 * Authentication API methods
 */
export const authApi = {
  /**
   * Get current user
   */
  getCurrentUser: async () => {
    return fetchApi('/auth/user');
  },

  /**
   * Login
   */
  login: async (credentials) => {
    return fetchApi('/auth/login', {
      method: 'POST',
      body: JSON.stringify(credentials)
    });
  },

  /**
   * Logout
   */
  logout: async () => {
    return fetchApi('/auth/logout', {
      method: 'POST'
    });
  }
};

// Export all API services
export default {
  portfolio: portfolioApi,
  stocks: stocksApi,
  preferences: preferencesApi,
  auth: authApi
}; 