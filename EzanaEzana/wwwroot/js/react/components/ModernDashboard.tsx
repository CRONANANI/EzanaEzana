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
  Search,
  BarChart3,
  Users,
  Activity,
  Target,
  Shield,
  Zap
} from 'lucide-react';
import { toast } from 'react-hot-toast';
import DashboardCard from './DashboardCard';
import InvestmentChart from './InvestmentChart';
import PortfolioSummary from './PortfolioSummary';

interface MetricCardProps {
  title: string;
  value: string;
  change: number;
  changePercent: number;
  icon: React.ReactNode;
  trend: 'up' | 'down' | 'neutral';
  color: string;
}

const MetricCard: React.FC<MetricCardProps> = ({ title, value, change, changePercent, icon, trend, color }) => (
  <div className="shadcn-card p-6 animate-scale-in" style={{ borderLeft: `4px solid ${color}` }}>
    <div className="d-flex justify-content-between align-items-start mb-3">
      <div className="d-flex align-items-center gap-2 text-muted-foreground">
        {icon}
        <span className="text-sm font-medium">{title}</span>
      </div>
      <div className="dropdown">
        <button className="btn btn-link btn-sm p-0" type="button" data-bs-toggle="dropdown">
          <Settings size={16} />
        </button>
        <ul className="dropdown-menu">
          <li><a className="dropdown-item" href="#"><Eye size={16} className="me-2" />View Details</a></li>
          <li><a className="dropdown-item" href="#"><BarChart3 size={16} className="me-2" />Analytics</a></li>
        </ul>
      </div>
    </div>
    
    <div className="mb-2" style={{ fontSize: '2rem', fontWeight: '700' }}>
      {value}
    </div>
    
    <div className="d-flex align-items-center gap-2">
      <span className={`${trend === 'up' ? 'text-green-600' : trend === 'down' ? 'text-red-600' : 'text-muted-foreground'}`}>
        {trend === 'up' && <TrendingUp size={16} />}
        {trend === 'down' && <TrendingDown size={16} />}
        {trend === 'neutral' && <Activity size={16} />}
        {change >= 0 ? '+' : ''}{change.toFixed(2)}%
      </span>
      <span className="text-xs text-muted-foreground">
        vs last month
      </span>
    </div>
  </div>
);

const ModernDashboard: React.FC = () => {
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
  const [activeTab, setActiveTab] = useState('overview');

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);

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

  const mockMetrics = [
    {
      title: 'Total Portfolio Value',
      value: `$${totalValue.toLocaleString()}`,
      change: totalChangePercent,
      changePercent: totalChangePercent,
      icon: <DollarSign size={20} />,
      trend: totalChangePercent >= 0 ? 'up' : 'down' as const,
              color: totalChangePercent >= 0 ? 'var(--chart-1)' : 'var(--destructive)'
    },
    {
      title: 'Active Investments',
      value: investments.length.toString(),
      change: 2.5,
      changePercent: 2.5,
      icon: <PieChart size={20} />,
      trend: 'up' as const,
              color: 'var(--primary)'
    },
    {
      title: 'Risk Score',
      value: '7.2/10',
      change: -0.8,
      changePercent: -0.8,
      icon: <Shield size={20} />,
      trend: 'down' as const,
              color: 'var(--warning-500)'
    },
    {
      title: 'Performance',
      value: '+12.4%',
      change: 12.4,
      changePercent: 12.4,
      icon: <Target size={20} />,
      trend: 'up' as const,
              color: 'var(--success-500)'
    }
  ];

  if (loading) {
    return (
      <div className="d-flex align-items-center justify-content-center" style={{ minHeight: '60vh' }}>
        <div className="text-center">
          <div className="spinner-border text-primary mb-3" role="status" style={{ width: '3rem', height: '3rem' }}>
            <span className="visually-hidden">Loading...</span>
          </div>
                  <p className="text-muted-foreground">
          Loading your investment dashboard...
        </p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="d-flex align-items-center justify-content-center" style={{ minHeight: '60vh' }}>
        <div className="text-center">
          <div className="mb-4" style={{ fontSize: '4rem' }}>⚠️</div>
                    <h2 className="text-2xl font-semibold mb-3">Something went wrong</h2>
          <p className="text-muted-foreground mb-4">{error}</p>
          <button 
            onClick={loadDashboardData}
            className="shadcn-btn shadcn-btn-primary"
          >
            <Zap size={16} className="me-2" />
            Try Again
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="ezana-animate-fade-in">
      {/* Header Actions */}
      <div className="row mb-4">
        <div className="col-md-8">
          <div className="d-flex align-items-center gap-3">
            <div className="position-relative">
              <Search size={20} className="position-absolute" style={{ left: '12px', top: '50%', transform: 'translateY(-50%)', color: 'var(--muted-foreground)' }} />
              <input
                type="text"
                className="shadcn-input"
                placeholder="Search investments..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                style={{ paddingLeft: '2.5rem' }}
              />
            </div>
            <div className="btn-group" role="group">
              {['1D', '1W', '1M', '3M', '1Y'].map((timeframe) => (
                <button
                  key={timeframe}
                  type="button"
                  className={`shadcn-btn ${selectedTimeframe === timeframe ? 'shadcn-btn-primary' : 'shadcn-btn-outline'}`}
                  onClick={() => setSelectedTimeframe(timeframe)}
                >
                  {timeframe}
                </button>
              ))}
            </div>
          </div>
        </div>
        <div className="col-md-4 text-md-end">
          <div className="d-flex gap-2 justify-content-md-end">
            <button className="shadcn-btn shadcn-btn-secondary">
              <Bell size={16} className="me-2" />
              Notifications
            </button>
            <button className="shadcn-btn shadcn-btn-primary">
              <Plus size={16} className="me-2" />
              Add Investment
            </button>
          </div>
        </div>
      </div>

      {/* Metrics Grid */}
      <div className="row mb-4">
        {mockMetrics.map((metric, index) => (
          <div key={metric.title} className="col-lg-3 col-md-6 mb-3">
            <MetricCard {...metric} />
          </div>
        ))}
      </div>

      {/* Main Content Tabs */}
      <div className="row mb-4">
        <div className="col-12">
          <div className="shadcn-card">
            <div className="p-4 border-b border-border">
              <ul className="nav nav-tabs border-0" role="tablist">
                {[
                  { id: 'overview', label: 'Portfolio Overview', icon: <PieChart size={18} /> },
                  { id: 'performance', label: 'Performance', icon: <BarChart3 size={18} /> },
                  { id: 'social', label: 'Social Insights', icon: <Users size={18} /> }
                ].map((tab) => (
                  <li key={tab.id} className="nav-item" role="presentation">
                    <button
                      className={`nav-link d-flex align-items-center gap-2 ${activeTab === tab.id ? 'active' : ''}`}
                      onClick={() => setActiveTab(tab.id)}
                      style={{ border: 'none', borderRadius: '0.5rem', marginRight: '0.5rem' }}
                    >
                      {tab.icon}
                      {tab.label}
                    </button>
                  </li>
                ))}
              </ul>
            </div>
            <div className="p-6">
              {activeTab === 'overview' && (
                <div className="row">
                  <div className="col-lg-8 mb-4">
                    <div className="p-4">
                      <h5 className="text-lg font-semibold mb-3">Portfolio Performance</h5>
                      <InvestmentChart />
                    </div>
                  </div>
                  <div className="col-lg-4 mb-4">
                    <div className="p-4">
                      <h5 className="text-lg font-semibold mb-3">Portfolio Summary</h5>
                      <PortfolioSummary />
                    </div>
                  </div>
                </div>
              )}
              
              {activeTab === 'performance' && (
                <div className="row">
                  <div className="col-12">
                    <div className="p-4">
                      <h5 className="text-lg font-semibold mb-3">Investment Performance Analysis</h5>
                      <p className="text-muted-foreground">
                        Detailed performance metrics and analysis for your investment portfolio.
                      </p>
                    </div>
                  </div>
                </div>
              )}
              
              {activeTab === 'social' && (
                <div className="row">
                  <div className="col-12">
                    <div className="p-4">
                      <h5 className="text-lg font-semibold mb-3">Social Investment Insights</h5>
                      <p className="text-muted-foreground">
                        See what other investors are doing and get social insights.
                      </p>
                    </div>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Recent Investments */}
      <div className="row">
        <div className="col-12">
          <div className="shadcn-card">
            <div className="p-4 border-b border-border d-flex justify-content-between align-items-center">
              <h5 className="text-lg font-semibold mb-0">Recent Investments</h5>
              <button className="shadcn-btn shadcn-btn-secondary text-sm">
                View All
              </button>
            </div>
            <div className="p-6">
              {filteredInvestments.length > 0 ? (
                <div className="table-responsive">
                  <table className="table table-hover">
                    <thead>
                      <tr>
                        <th>Investment</th>
                        <th>Type</th>
                        <th>Value</th>
                        <th>Change</th>
                        <th>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {filteredInvestments.slice(0, 5).map((investment) => (
                        <tr key={investment.id}>
                          <td>
                            <div className="d-flex align-items-center gap-2">
                              <div className="rounded-circle d-flex align-items-center justify-content-center" 
                                   style={{ width: '32px', height: '32px', backgroundColor: 'var(--primary)', color: 'white' }}>
                                {investment.symbol.charAt(0)}
                              </div>
                              <div>
                                <div className="fw-semibold">{investment.name}</div>
                                <small className="text-muted">{investment.symbol}</small>
                              </div>
                            </div>
                          </td>
                          <td>
                            <span className="px-2 py-1 bg-accent text-accent-foreground rounded text-sm">
                              {investment.type || 'Stock'}
                            </span>
                          </td>
                          <td className="fw-semibold">${investment.currentValue?.toLocaleString() || '0'}</td>
                          <td>
                            <span className={`${(investment.currentValue || 0) >= (investment.purchasePrice || 0) ? 'text-green-600' : 'text-red-600'}`}>
                              {(investment.currentValue || 0) >= (investment.purchasePrice || 0) ? <TrendingUp size={14} /> : <TrendingDown size={14} />}
                              {((investment.currentValue || 0) - (investment.purchasePrice || 0)).toFixed(2)}%
                            </span>
                          </td>
                          <td>
                            <div className="btn-group btn-group-sm">
                              <button className="btn btn-outline-primary btn-sm">
                                <Eye size={14} />
                              </button>
                              <button className="btn btn-outline-secondary btn-sm">
                                <BarChart3 size={14} />
                              </button>
                            </div>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              ) : (
                <div className="text-center py-4">
                  <PieChart size={48} className="mb-3 text-muted-foreground" />
                  <p className="text-muted-foreground">
                    No investments found. Start building your portfolio!
                  </p>
                  <button className="shadcn-btn shadcn-btn-primary">
                    <Plus size={16} className="me-2" />
                    Add Your First Investment
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ModernDashboard;
