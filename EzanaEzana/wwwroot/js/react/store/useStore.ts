import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';

// Types
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  avatar?: string;
  preferences: UserPreferences;
}

export interface UserPreferences {
  theme: 'light' | 'dark' | 'system';
  notifications: boolean;
  language: string;
}

export interface Investment {
  id: string;
  symbol: string;
  name: string;
  price: number;
  change: number;
  changePercent: number;
  volume: number;
  marketCap: number;
  sector: string;
}

export interface Portfolio {
  id: string;
  name: string;
  investments: PortfolioInvestment[];
  totalValue: number;
  totalChange: number;
  totalChangePercent: number;
}

export interface PortfolioInvestment {
  investmentId: string;
  shares: number;
  averagePrice: number;
  currentValue: number;
  totalChange: number;
  totalChangePercent: number;
}

export interface DashboardState {
  // User state
  user: User | null;
  isAuthenticated: boolean;
  
  // Investment data
  investments: Investment[];
  portfolios: Portfolio[];
  watchlist: string[];
  
  // UI state
  theme: 'light' | 'dark' | 'system';
  sidebarCollapsed: boolean;
  loading: boolean;
  error: string | null;
  
  // Actions
  setUser: (user: User | null) => void;
  setAuthenticated: (authenticated: boolean) => void;
  setInvestments: (investments: Investment[]) => void;
  setPortfolios: (portfolios: Portfolio[]) => void;
  addToWatchlist: (symbol: string) => void;
  removeFromWatchlist: (symbol: string) => void;
  setTheme: (theme: 'light' | 'dark' | 'system') => void;
  toggleSidebar: () => void;
  setLoading: (loading: boolean) => void;
  setError: (error: string | null) => void;
  
  // Computed values
  getTotalPortfolioValue: () => number;
  getTotalPortfolioChange: () => number;
  getInvestmentById: (id: string) => Investment | undefined;
}

export const useStore = create<DashboardState>()(
  devtools(
    persist(
      (set, get) => ({
        // Initial state
        user: null,
        isAuthenticated: false,
        investments: [],
        portfolios: [],
        watchlist: [],
        theme: 'system',
        sidebarCollapsed: false,
        loading: false,
        error: null,

        // Actions
        setUser: (user) => set({ user, isAuthenticated: !!user }),
        setAuthenticated: (authenticated) => set({ isAuthenticated: authenticated }),
        setInvestments: (investments) => set({ investments }),
        setPortfolios: (portfolios) => set({ portfolios }),
        addToWatchlist: (symbol) => set((state) => ({
          watchlist: [...state.watchlist.filter(s => s !== symbol), symbol]
        })),
        removeFromWatchlist: (symbol) => set((state) => ({
          watchlist: state.watchlist.filter(s => s !== symbol)
        })),
        setTheme: (theme) => set({ theme }),
        toggleSidebar: () => set((state) => ({ sidebarCollapsed: !state.sidebarCollapsed })),
        setLoading: (loading) => set({ loading }),
        setError: (error) => set({ error }),

        // Computed values
        getTotalPortfolioValue: () => {
          const { portfolios } = get();
          return portfolios.reduce((total, portfolio) => total + portfolio.totalValue, 0);
        },
        getTotalPortfolioChange: () => {
          const { portfolios } = get();
          return portfolios.reduce((total, portfolio) => total + portfolio.totalChange, 0);
        },
        getInvestmentById: (id) => {
          const { investments } = get();
          return investments.find(inv => inv.id === id);
        },
      }),
      {
        name: 'ezana-dashboard-storage',
        partialize: (state) => ({
          user: state.user,
          theme: state.theme,
          sidebarCollapsed: state.sidebarCollapsed,
          watchlist: state.watchlist,
        }),
      }
    )
  )
);
