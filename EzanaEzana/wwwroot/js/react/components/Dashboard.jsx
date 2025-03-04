import React, { useState, useEffect } from 'react';
import PortfolioSummary from './PortfolioSummary';
import InvestmentChart from './InvestmentChart';
import InvestmentPreferences from './InvestmentPreferences';
import StockDetails from './StockDetails';
import Loading from './common/Loading';
import ErrorMessage from './common/ErrorMessage';
import { useAppContext } from '../context/AppContext';

/**
 * Dashboard Component
 * 
 * This component combines various dashboard elements to create
 * a comprehensive investment dashboard view.
 */
function Dashboard() {
  const { 
    fetchPortfolioData, 
    fetchStockHoldings, 
    fetchWatchlist,
    stockHoldings,
    watchlist,
    isLoading,
    error
  } = useAppContext();
  
  const [activeTab, setActiveTab] = useState('overview');
  const [selectedStock, setSelectedStock] = useState(null);
  
  // Fetch initial data when component mounts
  useEffect(() => {
    const loadDashboardData = async () => {
      if (activeTab === 'overview') {
        await fetchPortfolioData();
      } else if (activeTab === 'stocks') {
        await Promise.all([fetchStockHoldings(), fetchWatchlist()]);
      }
    };
    
    loadDashboardData();
  }, [activeTab]);
  
  // Handle tab change
  const handleTabChange = (tab) => {
    setActiveTab(tab);
    setSelectedStock(null);
  };
  
  if (isLoading && !selectedStock) {
    return <Loading message="Loading dashboard data..." />;
  }
  
  return (
    <div className="dashboard">
      {error && (
        <div className="mb-4">
          <ErrorMessage message={error} />
        </div>
      )}
      
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex justify-content-between align-items-center">
            <h2 className="h3">Investment Dashboard</h2>
            <div className="d-flex">
              <div className="input-group me-2">
                <span className="input-group-text">
                  <i className="bi bi-calendar3"></i>
                </span>
                <select className="form-select">
                  <option>Last 30 days</option>
                  <option>Last 90 days</option>
                  <option>Year to date</option>
                  <option>Last 12 months</option>
                  <option>Custom range</option>
                </select>
              </div>
              <button className="btn btn-outline-primary">
                <i className="bi bi-sliders me-1"></i>
                Customize
              </button>
            </div>
          </div>
        </div>
      </div>
      
      <div className="row mb-4">
        <div className="col-12">
          <ul className="nav nav-tabs">
            <li className="nav-item">
              <button 
                className={`nav-link ${activeTab === 'overview' ? 'active' : ''}`}
                onClick={() => handleTabChange('overview')}
              >
                <i className="bi bi-grid me-2"></i>
                Overview
              </button>
            </li>
            <li className="nav-item">
              <button 
                className={`nav-link ${activeTab === 'stocks' ? 'active' : ''}`}
                onClick={() => handleTabChange('stocks')}
              >
                <i className="bi bi-graph-up me-2"></i>
                Stocks
              </button>
            </li>
            <li className="nav-item">
              <button 
                className={`nav-link ${activeTab === 'bonds' ? 'active' : ''}`}
                onClick={() => handleTabChange('bonds')}
              >
                <i className="bi bi-shield-check me-2"></i>
                Bonds
              </button>
            </li>
            <li className="nav-item">
              <button 
                className={`nav-link ${activeTab === 'cash' ? 'active' : ''}`}
                onClick={() => handleTabChange('cash')}
              >
                <i className="bi bi-cash-coin me-2"></i>
                Cash
              </button>
            </li>
            <li className="nav-item">
              <button 
                className={`nav-link ${activeTab === 'preferences' ? 'active' : ''}`}
                onClick={() => handleTabChange('preferences')}
              >
                <i className="bi bi-gear me-2"></i>
                Preferences
              </button>
            </li>
          </ul>
        </div>
      </div>
      
      {activeTab === 'overview' && (
        <>
          <div className="row mb-4">
            <div className="col-12">
              <PortfolioSummary />
            </div>
          </div>
          
          <div className="row mb-4">
            <div className="col-12">
              <InvestmentChart title="Portfolio Performance" />
            </div>
          </div>
          
          <div className="row">
            <div className="col-md-6 mb-4">
              <InvestmentChart title="Asset Allocation" type="pie" />
            </div>
            <div className="col-md-6 mb-4">
              <InvestmentChart title="Sector Breakdown" type="bar" />
            </div>
          </div>
        </>
      )}
      
      {activeTab === 'stocks' && (
        <div className="row">
          {selectedStock ? (
            <div className="col-12 mb-4">
              <div className="mb-3">
                <button 
                  className="btn btn-outline-secondary" 
                  onClick={() => setSelectedStock(null)}
                >
                  <i className="bi bi-arrow-left me-2"></i>
                  Back to Stocks
                </button>
              </div>
              <StockDetails symbol={selectedStock} />
            </div>
          ) : (
            <>
              <div className="col-12 mb-4">
                <div className="card shadow-sm">
                  <div className="card-header bg-white">
                    <div className="d-flex justify-content-between align-items-center">
                      <h5 className="card-title mb-0">Your Stock Holdings</h5>
                      <div className="input-group" style={{ width: '300px' }}>
                        <input type="text" className="form-control" placeholder="Search stocks..." />
                        <button className="btn btn-outline-secondary" type="button">
                          <i className="bi bi-search"></i>
                        </button>
                      </div>
                    </div>
                  </div>
                  <div className="card-body p-0">
                    {isLoading ? (
                      <div className="p-4">
                        <Loading message="Loading stock holdings..." />
                      </div>
                    ) : stockHoldings && stockHoldings.length > 0 ? (
                      <div className="table-responsive">
                        <table className="table table-hover m-0">
                          <thead>
                            <tr>
                              <th>Symbol</th>
                              <th>Name</th>
                              <th>Shares</th>
                              <th>Price</th>
                              <th>Change</th>
                              <th>Market Value</th>
                              <th>Actions</th>
                            </tr>
                          </thead>
                          <tbody>
                            {stockHoldings.map(stock => (
                              <tr key={stock.symbol}>
                                <td>{stock.symbol}</td>
                                <td>{stock.name}</td>
                                <td>{stock.shares}</td>
                                <td>${stock.price.toFixed(2)}</td>
                                <td className={stock.change >= 0 ? 'text-success' : 'text-danger'}>
                                  {stock.change >= 0 ? '+' : ''}{stock.change}%
                                </td>
                                <td>${stock.value.toFixed(2)}</td>
                                <td>
                                  <button 
                                    className="btn btn-sm btn-outline-primary me-1"
                                    onClick={() => setSelectedStock(stock.symbol)}
                                  >
                                    <i className="bi bi-info-circle"></i>
                                  </button>
                                  <button className="btn btn-sm btn-outline-success me-1">
                                    <i className="bi bi-plus-circle"></i>
                                  </button>
                                  <button className="btn btn-sm btn-outline-danger">
                                    <i className="bi bi-dash-circle"></i>
                                  </button>
                                </td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    ) : (
                      <div className="p-4 text-center">
                        <p className="text-muted mb-3">You don't have any stock holdings yet.</p>
                        <button className="btn btn-primary">
                          <i className="bi bi-plus-circle me-2"></i>
                          Add Stocks to Portfolio
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </div>
              
              <div className="col-md-6 mb-4">
                <InvestmentChart title="Stock Performance" />
              </div>
              
              <div className="col-md-6 mb-4">
                <div className="card shadow-sm">
                  <div className="card-header bg-white">
                    <h5 className="card-title mb-0">Stock Watchlist</h5>
                  </div>
                  <div className="card-body p-0">
                    {isLoading ? (
                      <div className="p-4">
                        <Loading message="Loading watchlist..." />
                      </div>
                    ) : watchlist && watchlist.length > 0 ? (
                      <div className="table-responsive">
                        <table className="table table-hover m-0">
                          <thead>
                            <tr>
                              <th>Symbol</th>
                              <th>Name</th>
                              <th>Price</th>
                              <th>Change</th>
                              <th>Actions</th>
                            </tr>
                          </thead>
                          <tbody>
                            {watchlist.map(stock => (
                              <tr key={stock.symbol}>
                                <td>{stock.symbol}</td>
                                <td>{stock.name}</td>
                                <td>${stock.price.toFixed(2)}</td>
                                <td className={stock.change >= 0 ? 'text-success' : 'text-danger'}>
                                  {stock.change >= 0 ? '+' : ''}{stock.change}%
                                </td>
                                <td>
                                  <button 
                                    className="btn btn-sm btn-outline-primary me-1"
                                    onClick={() => setSelectedStock(stock.symbol)}
                                  >
                                    <i className="bi bi-info-circle"></i>
                                  </button>
                                  <button className="btn btn-sm btn-outline-success">
                                    <i className="bi bi-plus-circle"></i>
                                  </button>
                                </td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      </div>
                    ) : (
                      <div className="p-4 text-center">
                        <p className="text-muted mb-3">Your watchlist is empty.</p>
                        <button className="btn btn-outline-primary">
                          <i className="bi bi-plus-circle me-2"></i>
                          Add Stocks to Watchlist
                        </button>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </>
          )}
        </div>
      )}
      
      {activeTab === 'bonds' && (
        <div className="row">
          <div className="col-12">
            <div className="alert alert-info">
              <i className="bi bi-info-circle me-2"></i>
              The Bonds tab content would be implemented here.
            </div>
          </div>
        </div>
      )}
      
      {activeTab === 'cash' && (
        <div className="row">
          <div className="col-12">
            <div className="alert alert-info">
              <i className="bi bi-info-circle me-2"></i>
              The Cash tab content would be implemented here.
            </div>
          </div>
        </div>
      )}
      
      {activeTab === 'preferences' && (
        <div className="row">
          <div className="col-12">
            <InvestmentPreferences />
          </div>
        </div>
      )}
    </div>
  );
}

export default Dashboard; 