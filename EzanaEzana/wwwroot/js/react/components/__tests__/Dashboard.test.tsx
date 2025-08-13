import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import Dashboard from '../Dashboard';

// Mock the store and API service
jest.mock('../../store/useStore');
jest.mock('../../services/api');
jest.mock('react-hot-toast');

// Mock the child components
jest.mock('../DashboardCard', () => {
  return function MockDashboardCard({ title, value }: any) {
    return <div data-testid="dashboard-card">{title}: {value}</div>;
  };
});

jest.mock('../InvestmentChart', () => {
  return function MockInvestmentChart({ timeframe }: any) {
    return <div data-testid="investment-chart">Chart for {timeframe}</div>;
  };
});

jest.mock('../PortfolioSummary', () => {
  return function MockPortfolioSummary({ portfolios }: any) {
    return <div data-testid="portfolio-summary">{portfolios.length} portfolios</div>;
  };
});

describe('Dashboard Component', () => {
  const mockUseStore = {
    user: { name: 'Test User' },
    investments: [
      { id: '1', name: 'Apple Inc', symbol: 'AAPL', value: 150, change: 5 },
      { id: '2', name: 'Microsoft Corp', symbol: 'MSFT', value: 300, change: -10 }
    ],
    portfolios: [
      { id: '1', name: 'Growth Portfolio', value: 10000, change: 500 }
    ],
    loading: false,
    error: null,
    setInvestments: jest.fn(),
    setPortfolios: jest.fn(),
    setLoading: jest.fn(),
    setError: jest.fn(),
    getTotalPortfolioValue: jest.fn().mockReturnValue(10000),
    getTotalPortfolioChange: jest.fn().mockReturnValue(500)
  };

  beforeEach(() => {
    jest.clearAllMocks();
    require('../../store/useStore').useStore.mockReturnValue(mockUseStore);
  });

  test('renders dashboard title', () => {
    render(<Dashboard />);
    expect(screen.getByText(/Investment Dashboard/i)).toBeInTheDocument();
  });

  test('renders search input', () => {
    render(<Dashboard />);
    expect(screen.getByPlaceholderText(/Search investments/i)).toBeInTheDocument();
  });

  test('renders portfolio summary cards', () => {
    render(<Dashboard />);
    expect(screen.getByTestId('portfolio-summary')).toBeInTheDocument();
  });

  test('renders investment chart', () => {
    render(<Dashboard />);
    expect(screen.getByTestId('investment-chart')).toBeInTheDocument();
  });

  test('filters investments based on search query', async () => {
    render(<Dashboard />);
    const searchInput = screen.getByPlaceholderText(/Search investments/i);
    
    fireEvent.change(searchInput, { target: { value: 'Apple' } });
    
    await waitFor(() => {
      expect(screen.getByText('Apple Inc')).toBeInTheDocument();
    });
  });

  test('shows loading state', () => {
    const loadingStore = { ...mockUseStore, loading: true };
    require('../../store/useStore').useStore.mockReturnValue(loadingStore);
    
    render(<Dashboard />);
    expect(screen.getByRole('status')).toBeInTheDocument();
  });

  test('shows error state', () => {
    const errorStore = { ...mockUseStore, error: 'Failed to load data' };
    require('../../store/useStore').useStore.mockReturnValue(errorStore);
    
    render(<Dashboard />);
    expect(screen.getByText('Something went wrong')).toBeInTheDocument();
    expect(screen.getByText('Failed to load data')).toBeInTheDocument();
  });
});
