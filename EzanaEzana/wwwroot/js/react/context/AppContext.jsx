import React, { createContext, useContext, useState, useEffect } from 'react';
import api from '../services/api';

// Create context
const AppContext = createContext();

/**
 * Custom hook to use the app context
 */
export const useAppContext = () => useContext(AppContext);

/**
 * App Context Provider
 * 
 * This component provides global state and methods to all child components
 */
export function AppProvider({ children }) {
  // User state
  const [user, setUser] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  
  // Portfolio state
  const [portfolioSummary, setPortfolioSummary] = useState(null);
  const [portfolioPerformance, setPortfolioPerformance] = useState(null);
  const [assetAllocation, setAssetAllocation] = useState(null);
  const [sectorBreakdown, setSectorBreakdown] = useState(null);
  
  // Stocks state
  const [stockHoldings, setStockHoldings] = useState([]);
  const [watchlist, setWatchlist] = useState([]);
  
  // Preferences state
  const [preferences, setPreferences] = useState(null);
  
  // Initialize app data
  useEffect(() => {
    const initializeApp = async () => {
      try {
        setIsLoading(true);
        
        // In a real app, we would fetch the current user and initial data
        // For demo purposes, we'll just set a mock user after a delay
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        setUser({
          id: 1,
          name: 'John Doe',
          email: 'john.doe@example.com',
          avatar: 'https://via.placeholder.com/150'
        });
        
        // Mock portfolio data
        setPortfolioSummary({
          totalValue: '$124,500.00',
          totalChange: '+$3,240.50',
          percentChange: '+2.7%',
          timeframe: 'Last 30 days'
        });
        
        setIsLoading(false);
      } catch (err) {
        console.error('Failed to initialize app:', err);
        setError('Failed to load application data. Please try again later.');
        setIsLoading(false);
      }
    };
    
    initializeApp();
  }, []);
  
  // Methods for portfolio data
  const fetchPortfolioData = async (timeframe = '1M') => {
    try {
      setIsLoading(true);
      
      // In a real app, we would fetch data from the API
      // For demo purposes, we'll just set mock data after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Mock data
      setPortfolioPerformance({
        timeframe,
        data: Array.from({ length: 30 }, (_, i) => ({
          date: new Date(Date.now() - (29 - i) * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
          value: 100000 + Math.random() * 30000
        }))
      });
      
      setAssetAllocation({
        stocks: 60,
        bonds: 25,
        cash: 10,
        other: 5
      });
      
      setSectorBreakdown({
        technology: 35,
        healthcare: 20,
        finance: 15,
        consumer: 10,
        industrial: 10,
        energy: 5,
        other: 5
      });
      
      setIsLoading(false);
    } catch (err) {
      console.error('Failed to fetch portfolio data:', err);
      setError('Failed to load portfolio data. Please try again later.');
      setIsLoading(false);
    }
  };
  
  // Methods for stock data
  const fetchStockHoldings = async () => {
    try {
      setIsLoading(true);
      
      // In a real app, we would fetch data from the API
      // For demo purposes, we'll just set mock data after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Mock data
      setStockHoldings([
        { symbol: 'AAPL', name: 'Apple Inc.', shares: 25, price: 178.72, change: 1.33, value: 4468.00 },
        { symbol: 'MSFT', name: 'Microsoft Corporation', shares: 15, price: 337.22, change: 0.87, value: 5058.30 },
        { symbol: 'GOOGL', name: 'Alphabet Inc.', shares: 10, price: 131.86, change: -0.45, value: 1318.60 }
      ]);
      
      setIsLoading(false);
    } catch (err) {
      console.error('Failed to fetch stock holdings:', err);
      setError('Failed to load stock holdings. Please try again later.');
      setIsLoading(false);
    }
  };
  
  const fetchWatchlist = async () => {
    try {
      setIsLoading(true);
      
      // In a real app, we would fetch data from the API
      // For demo purposes, we'll just set mock data after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Mock data
      setWatchlist([
        { symbol: 'AMZN', name: 'Amazon.com Inc.', price: 129.12, change: 0.78 },
        { symbol: 'TSLA', name: 'Tesla, Inc.', price: 237.49, change: -1.23 },
        { symbol: 'NVDA', name: 'NVIDIA Corporation', price: 437.53, change: 2.15 }
      ]);
      
      setIsLoading(false);
    } catch (err) {
      console.error('Failed to fetch watchlist:', err);
      setError('Failed to load watchlist. Please try again later.');
      setIsLoading(false);
    }
  };
  
  // Methods for preferences
  const fetchPreferences = async () => {
    try {
      setIsLoading(true);
      
      // In a real app, we would fetch data from the API
      // For demo purposes, we'll just set mock data after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      // Mock data
      setPreferences({
        riskTolerance: 'moderate',
        investmentGoals: ['retirement', 'growth'],
        investmentHorizon: '5-10',
        monthlyContribution: 500,
        autoRebalance: true,
        preferredSectors: ['technology', 'healthcare', 'finance']
      });
      
      setIsLoading(false);
    } catch (err) {
      console.error('Failed to fetch preferences:', err);
      setError('Failed to load preferences. Please try again later.');
      setIsLoading(false);
    }
  };
  
  const savePreferences = async (newPreferences) => {
    try {
      setIsLoading(true);
      
      // In a real app, we would save data to the API
      // For demo purposes, we'll just update state after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      setPreferences(newPreferences);
      
      setIsLoading(false);
      return { success: true };
    } catch (err) {
      console.error('Failed to save preferences:', err);
      setError('Failed to save preferences. Please try again later.');
      setIsLoading(false);
      return { success: false, error: err.message };
    }
  };
  
  // Authentication methods
  const login = async (credentials) => {
    try {
      setIsLoading(true);
      
      // In a real app, we would call the API
      // For demo purposes, we'll just set a mock user after a delay
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      setUser({
        id: 1,
        name: 'John Doe',
        email: credentials.email,
        avatar: 'https://via.placeholder.com/150'
      });
      
      setIsLoading(false);
      return { success: true };
    } catch (err) {
      console.error('Login failed:', err);
      setError('Login failed. Please check your credentials and try again.');
      setIsLoading(false);
      return { success: false, error: err.message };
    }
  };
  
  const logout = async () => {
    try {
      setIsLoading(true);
      
      // In a real app, we would call the API
      // For demo purposes, we'll just clear the user after a delay
      await new Promise(resolve => setTimeout(resolve, 500));
      
      setUser(null);
      
      setIsLoading(false);
      return { success: true };
    } catch (err) {
      console.error('Logout failed:', err);
      setError('Logout failed. Please try again later.');
      setIsLoading(false);
      return { success: false, error: err.message };
    }
  };
  
  // Context value
  const contextValue = {
    // State
    user,
    isLoading,
    error,
    portfolioSummary,
    portfolioPerformance,
    assetAllocation,
    sectorBreakdown,
    stockHoldings,
    watchlist,
    preferences,
    
    // Methods
    fetchPortfolioData,
    fetchStockHoldings,
    fetchWatchlist,
    fetchPreferences,
    savePreferences,
    login,
    logout,
    
    // Utility
    clearError: () => setError(null)
  };
  
  return (
    <AppContext.Provider value={contextValue}>
      {children}
    </AppContext.Provider>
  );
}

export default AppContext; 