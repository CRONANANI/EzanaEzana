import React, { useEffect, useState } from 'react';
import { useStore } from '../store/useStore';
import { apiService } from '../services/api';
import { 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  PieChart, 
  Eye,
  Plus,
  Settings,
  Bell,
  Search
} from 'lucide-react';
import { toast } from 'react-hot-toast';
import DashboardCard from './DashboardCard';
import InvestmentChart from './InvestmentChart';
import PortfolioSummary from './PortfolioSummary';

const Dashboard: React.FC = () => {
  const {
    user,
    investments,
    portfolios,
    loading,
    error,
    setInvestments,
    setPortfolios,
    setLoading,
    setError,
    getTotalPortfolioValue,
    getTotalPortfolioChange
  } = useStore();

  const [searchQuery, setSearchQuery] = useState('');
  const [selectedTimeframe, setSelectedTimeframe] = useState('1M');

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);

      // Load investments and portfolios in parallel
      const [investmentsResponse, portfoliosResponse] = await Promise.all([
        apiService.getInvestments({ pageSize: 50 }),
        apiService.getPortfolios()
      ]);

      if (investmentsResponse.success) {
        setInvestments(investmentsResponse.data);
      }

      if (portfoliosResponse.success) {
        setPortfolios(portfoliosResponse.data);
      }
    } catch (err) {
      setError('Failed to load dashboard data');
      toast.error('Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  const totalValue = getTotalPortfolioValue();
  const totalChange = getTotalPortfolioChange();
  const totalChangePercent = totalValue > 0 ? (totalChange / totalValue) * 100 : 0;

  const filteredInvestments = investments.filter(inv =>
    inv.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
    inv.symbol.toLowerCase().includes(searchQuery.toLowerCase())
  );

  if (loading) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex items-center justify-center min-h-screen">
        <div className="text-center">
          <div className="text-red-500 text-6xl mb-4">⚠️</div>
          <h2 className="text-2xl font-bold text-gray-900 mb-2">Something went wrong</h2>
          <p className="text-gray-600 mb-4">{error}</p>
          <button
            onClick={loadDashboardData}
            className="btn btn-primary"
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center py-4">
            <div>
              <h1 className="text-2xl font-bold text-gray-900">
                Welcome back, {user?.firstName || 'User'}!
              </h1>
              <p className="text-gray-600">Here's what's happening with your investments today.</p>
            </div>
            <div className="flex items-center space-x-3">
              <button className="btn btn-ghost btn-sm">
                <Bell className="h-5 w-5" />
              </button>
              <button className="btn btn-ghost btn-sm">
                <Settings className="h-5 w-5" />
              </button>
              <div className="w-10 h-10 bg-primary-600 rounded-full flex items-center justify-center text-white font-semibold">
                {user?.firstName?.[0] || 'U'}
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Search and Filters */}
        <div className="mb-8">
          <div className="flex flex-col sm:flex-row gap-4">
            <div className="flex-1 relative">
              <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-5 w-5" />
              <input
                type="text"
                placeholder="Search investments..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="input pl-10 w-full"
              />
            </div>
            <div className="flex gap-2">
              {['1D', '1W', '1M', '3M', '1Y', 'ALL'].map((timeframe) => (
                <button
                  key={timeframe}
                  onClick={() => setSelectedTimeframe(timeframe)}
                  className={`btn btn-sm ${
                    selectedTimeframe === timeframe
                      ? 'btn-primary'
                      : 'btn-outline'
                  }`}
                >
                  {timeframe}
                </button>
              ))}
            </div>
          </div>
        </div>

        {/* Portfolio Summary Cards */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <DashboardCard
            title="Total Portfolio Value"
            value={`$${totalValue.toLocaleString()}`}
            change={totalChange}
            changePercent={totalChangePercent}
            icon={DollarSign}
            trend={totalChange >= 0 ? 'up' : 'down'}
          />
          <DashboardCard
            title="Active Investments"
            value={investments.length.toString()}
            change={investments.filter(inv => inv.change > 0).length}
            changePercent={(investments.filter(inv => inv.change > 0).length / investments.length) * 100}
            icon={PieChart}
            trend="up"
          />
          <DashboardCard
            title="Portfolios"
            value={portfolios.length.toString()}
            change={portfolios.length}
            changePercent={100}
            icon={Eye}
            trend="up"
          />
          <DashboardCard
            title="Watchlist"
            value="12"
            change={3}
            changePercent={25}
            icon={TrendingUp}
            trend="up"
          />
        </div>

        {/* Charts and Tables */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 mb-8">
          {/* Investment Chart */}
          <div className="card">
            <div className="card-header">
              <div className="flex justify-between items-center">
                <div>
                  <h3 className="card-title">Portfolio Performance</h3>
                  <p className="card-description">Your investment growth over time</p>
                </div>
                <button className="btn btn-outline btn-sm">
                  <Plus className="h-4 w-4 mr-2" />
                  Add Investment
                </button>
              </div>
            </div>
            <div className="card-content">
              <InvestmentChart timeframe={selectedTimeframe} />
            </div>
          </div>

          {/* Portfolio Summary */}
          <div className="card">
            <div className="card-header">
              <h3 className="card-title">Portfolio Summary</h3>
              <p className="card-description">Overview of your investment portfolios</p>
            </div>
            <div className="card-content">
              <PortfolioSummary portfolios={portfolios} />
            </div>
          </div>
        </div>

        {/* Recent Investments Table */}
        <div className="card">
          <div className="card-header">
            <div className="flex justify-between items-center">
              <div>
                <h3 className="card-title">Recent Investments</h3>
                <p className="card-description">Latest updates on your portfolio</p>
              </div>
              <button className="btn btn-primary btn-sm">
                View All
              </button>
            </div>
          </div>
          <div className="card-content">
            <div className="overflow-x-auto">
              <table className="w-full">
                <thead>
                  <tr className="border-b">
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Symbol</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Name</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Price</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Change</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Volume</th>
                    <th className="text-left py-3 px-4 font-medium text-gray-900">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredInvestments.slice(0, 10).map((investment) => (
                    <tr key={investment.id} className="border-b hover:bg-gray-50">
                      <td className="py-3 px-4 font-mono font-semibold text-gray-900">
                        {investment.symbol}
                      </td>
                      <td className="py-3 px-4 text-gray-900">{investment.name}</td>
                      <td className="py-3 px-4 font-semibold text-gray-900">
                        ${investment.price.toFixed(2)}
                      </td>
                      <td className="py-3 px-4">
                        <div className="flex items-center">
                          {investment.change >= 0 ? (
                            <TrendingUp className="h-4 w-4 text-success-600 mr-1" />
                          ) : (
                            <TrendingDown className="h-4 w-4 text-danger-600 mr-1" />
                          )}
                          <span
                            className={`font-semibold ${
                              investment.change >= 0 ? 'text-success-600' : 'text-danger-600'
                            }`}
                          >
                            {investment.change >= 0 ? '+' : ''}${investment.change.toFixed(2)} ({investment.changePercent.toFixed(2)}%)
                          </span>
                        </div>
                      </td>
                      <td className="py-3 px-4 text-gray-600">
                        {investment.volume.toLocaleString()}
                      </td>
                      <td className="py-3 px-4">
                        <div className="flex space-x-2">
                          <button className="btn btn-ghost btn-sm">
                            <Eye className="h-4 w-4" />
                          </button>
                          <button className="btn btn-outline btn-sm">
                            Trade
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
