import React, { useState, useEffect } from 'react';
import { useAppContext } from '../context/AppContext';
import Loading from './common/Loading';
import ErrorMessage from './common/ErrorMessage';

/**
 * Stock Details Component
 * 
 * Displays detailed information about a specific stock
 * including price history, key metrics, and news
 */
function StockDetails({ symbol = 'AAPL' }) {
  const { isLoading, error } = useAppContext();
  const [activeTab, setActiveTab] = useState('overview');
  const [stockData, setStockData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [stockError, setStockError] = useState(null);
  
  // Fetch stock data when component mounts or symbol changes
  useEffect(() => {
    const fetchStockData = async () => {
      try {
        setLoading(true);
        setStockError(null);
        
        // In a real app, this would be an API call
        // For demo purposes, we're using a timeout to simulate network request
        await new Promise(resolve => setTimeout(resolve, 1000));
        
        // Mock stock data
        setStockData({
          symbol: symbol,
          name: symbol === 'AAPL' ? 'Apple Inc.' : 
                symbol === 'MSFT' ? 'Microsoft Corporation' : 
                symbol === 'GOOGL' ? 'Alphabet Inc.' : 'Unknown Company',
          price: 178.72,
          change: +2.35,
          changePercent: +1.33,
          marketCap: '2.82T',
          peRatio: 29.45,
          dividend: 0.92,
          dividendYield: 0.51,
          volume: '52.3M',
          avgVolume: '58.7M',
          high52Week: 199.62,
          low52Week: 143.90,
          open: 176.15,
          previousClose: 176.37,
          news: [
            {
              id: 1,
              title: `${symbol} Announces New Product Line`,
              date: '2023-06-15',
              source: 'Financial Times'
            },
            {
              id: 2,
              title: `${symbol} Exceeds Quarterly Earnings Expectations`,
              date: '2023-05-02',
              source: 'Wall Street Journal'
            },
            {
              id: 3,
              title: `Analysts Raise Price Target for ${symbol}`,
              date: '2023-04-18',
              source: 'Bloomberg'
            }
          ]
        });
        
        setLoading(false);
      } catch (err) {
        console.error(`Failed to fetch data for ${symbol}:`, err);
        setStockError(`Failed to load data for ${symbol}. Please try again later.`);
        setLoading(false);
      }
    };
    
    fetchStockData();
  }, [symbol]);
  
  if (loading || isLoading) {
    return <Loading message={`Loading data for ${symbol}...`} />;
  }
  
  if (stockError || error) {
    return <ErrorMessage message={stockError || error} />;
  }
  
  if (!stockData) {
    return <ErrorMessage message={`No data available for ${symbol}`} />;
  }

  return (
    <div className="stock-details">
      <div className="card shadow-sm mb-4">
        <div className="card-body">
          <div className="d-flex justify-content-between align-items-start mb-3">
            <div>
              <h2 className="mb-0">{stockData.name}</h2>
              <div className="text-muted">{stockData.symbol}</div>
            </div>
            <div className="text-end">
              <h3 className="mb-0">${stockData.price.toFixed(2)}</h3>
              <div className={stockData.change >= 0 ? 'text-success' : 'text-danger'}>
                {stockData.change >= 0 ? '+' : ''}{stockData.change.toFixed(2)} 
                ({stockData.change >= 0 ? '+' : ''}{stockData.changePercent.toFixed(2)}%)
              </div>
            </div>
          </div>
          
          <div className="row mb-3">
            <div className="col-md-3 col-6 mb-2">
              <div className="text-muted small">Market Cap</div>
              <div>${stockData.marketCap}</div>
            </div>
            <div className="col-md-3 col-6 mb-2">
              <div className="text-muted small">P/E Ratio</div>
              <div>{stockData.peRatio}</div>
            </div>
            <div className="col-md-3 col-6 mb-2">
              <div className="text-muted small">Dividend Yield</div>
              <div>{stockData.dividendYield}%</div>
            </div>
            <div className="col-md-3 col-6 mb-2">
              <div className="text-muted small">Volume</div>
              <div>{stockData.volume}</div>
            </div>
          </div>
          
          <div className="d-flex gap-2">
            <button className="btn btn-primary">
              <i className="bi bi-plus-circle me-1"></i> Add to Portfolio
            </button>
            <button className="btn btn-outline-secondary">
              <i className="bi bi-bell me-1"></i> Set Alert
            </button>
            <button className="btn btn-outline-secondary">
              <i className="bi bi-star me-1"></i> Add to Watchlist
            </button>
          </div>
        </div>
      </div>
      
      <ul className="nav nav-tabs mb-4">
        <li className="nav-item">
          <button 
            className={`nav-link ${activeTab === 'overview' ? 'active' : ''}`}
            onClick={() => setActiveTab('overview')}
          >
            Overview
          </button>
        </li>
        <li className="nav-item">
          <button 
            className={`nav-link ${activeTab === 'chart' ? 'active' : ''}`}
            onClick={() => setActiveTab('chart')}
          >
            Chart
          </button>
        </li>
        <li className="nav-item">
          <button 
            className={`nav-link ${activeTab === 'financials' ? 'active' : ''}`}
            onClick={() => setActiveTab('financials')}
          >
            Financials
          </button>
        </li>
        <li className="nav-item">
          <button 
            className={`nav-link ${activeTab === 'news' ? 'active' : ''}`}
            onClick={() => setActiveTab('news')}
          >
            News
          </button>
        </li>
      </ul>
      
      {activeTab === 'overview' && (
        <div className="row">
          <div className="col-md-8">
            <div className="card shadow-sm mb-4">
              <div className="card-header bg-white">
                <h5 className="card-title mb-0">Price History</h5>
              </div>
              <div className="card-body">
                <div className="chart-placeholder" style={{ height: '300px', position: 'relative' }}>
                  <div className="position-absolute top-50 start-50 translate-middle text-center">
                    <p className="text-muted mb-2">
                      <i className="bi bi-graph-up fs-1"></i>
                    </p>
                    <p className="text-muted">
                      In a real implementation, this would be a chart showing price history for {stockData.symbol}.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <div className="col-md-4">
            <div className="card shadow-sm mb-4">
              <div className="card-header bg-white">
                <h5 className="card-title mb-0">Key Statistics</h5>
              </div>
              <div className="card-body p-0">
                <table className="table table-sm m-0">
                  <tbody>
                    <tr>
                      <td>Open</td>
                      <td className="text-end">${stockData.open.toFixed(2)}</td>
                    </tr>
                    <tr>
                      <td>Previous Close</td>
                      <td className="text-end">${stockData.previousClose.toFixed(2)}</td>
                    </tr>
                    <tr>
                      <td>Day Range</td>
                      <td className="text-end">${(stockData.price - 1.5).toFixed(2)} - ${(stockData.price + 0.8).toFixed(2)}</td>
                    </tr>
                    <tr>
                      <td>52 Week Range</td>
                      <td className="text-end">${stockData.low52Week.toFixed(2)} - ${stockData.high52Week.toFixed(2)}</td>
                    </tr>
                    <tr>
                      <td>Volume</td>
                      <td className="text-end">{stockData.volume}</td>
                    </tr>
                    <tr>
                      <td>Avg. Volume</td>
                      <td className="text-end">{stockData.avgVolume}</td>
                    </tr>
                    <tr>
                      <td>Market Cap</td>
                      <td className="text-end">${stockData.marketCap}</td>
                    </tr>
                    <tr>
                      <td>P/E Ratio</td>
                      <td className="text-end">{stockData.peRatio}</td>
                    </tr>
                    <tr>
                      <td>Dividend</td>
                      <td className="text-end">${stockData.dividend} ({stockData.dividendYield}%)</td>
                    </tr>
                  </tbody>
                </table>
              </div>
            </div>
            
            <div className="card shadow-sm">
              <div className="card-header bg-white">
                <h5 className="card-title mb-0">Recent News</h5>
              </div>
              <div className="card-body p-0">
                <ul className="list-group list-group-flush">
                  {stockData.news.map(item => (
                    <li className="list-group-item" key={item.id}>
                      <h6 className="mb-1">{item.title}</h6>
                      <div className="d-flex justify-content-between">
                        <small className="text-muted">{item.source}</small>
                        <small className="text-muted">{item.date}</small>
                      </div>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>
        </div>
      )}
      
      {activeTab === 'chart' && (
        <div className="card shadow-sm">
          <div className="card-header bg-white">
            <div className="d-flex justify-content-between align-items-center">
              <h5 className="card-title mb-0">Interactive Chart</h5>
              <div className="btn-group" role="group">
                {['1D', '1W', '1M', '3M', '6M', 'YTD', '1Y', '5Y'].map(period => (
                  <button key={period} type="button" className="btn btn-sm btn-outline-secondary">
                    {period}
                  </button>
                ))}
              </div>
            </div>
          </div>
          <div className="card-body">
            <div className="chart-placeholder" style={{ height: '500px', position: 'relative' }}>
              <div className="position-absolute top-50 start-50 translate-middle text-center">
                <p className="text-muted mb-2">
                  <i className="bi bi-graph-up fs-1"></i>
                </p>
                <p className="text-muted">
                  In a real implementation, this would be an interactive chart for {stockData.symbol}.
                </p>
                <p className="text-muted small">
                  <strong>Note:</strong> To implement actual charts, install a library like Chart.js or Recharts.
                </p>
              </div>
            </div>
          </div>
        </div>
      )}
      
      {activeTab === 'financials' && (
        <div className="alert alert-info">
          <i className="bi bi-info-circle me-2"></i>
          The Financials tab would display quarterly and annual financial statements.
        </div>
      )}
      
      {activeTab === 'news' && (
        <div className="alert alert-info">
          <i className="bi bi-info-circle me-2"></i>
          The News tab would display a comprehensive list of news articles related to {stockData.symbol}.
        </div>
      )}
    </div>
  );
}

export default StockDetails; 