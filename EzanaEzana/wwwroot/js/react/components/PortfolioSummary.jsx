import React, { useEffect } from 'react';
import DashboardCard from './DashboardCard';
import Loading from './common/Loading';
import ErrorMessage from './common/ErrorMessage';
import { useAppContext } from '../context/AppContext';

/**
 * Portfolio Summary component that displays key investment metrics
 * 
 * This component fetches data from the context and displays it
 * using the DashboardCard components
 */
function PortfolioSummary() {
  const { 
    portfolioSummary, 
    fetchPortfolioData, 
    isLoading, 
    error 
  } = useAppContext();

  // Fetch portfolio data when component mounts
  useEffect(() => {
    if (!portfolioSummary) {
      fetchPortfolioData();
    }
  }, []);

  if (isLoading && !portfolioSummary) {
    return <Loading message="Loading portfolio data..." />;
  }

  if (error) {
    return <ErrorMessage message={error} onRetry={fetchPortfolioData} />;
  }

  if (!portfolioSummary) {
    return (
      <div className="alert alert-warning">
        <i className="bi bi-exclamation-triangle me-2"></i>
        No portfolio data available.
      </div>
    );
  }

  return (
    <div className="portfolio-summary">
      <div className="row mb-4">
        <div className="col-12">
          <div className="card bg-light">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h2 className="mb-0">{portfolioSummary.totalValue}</h2>
                  <div className="d-flex align-items-center">
                    <span className="text-success me-2">
                      {portfolioSummary.totalChange} ({portfolioSummary.percentChange})
                    </span>
                    <span className="text-muted">{portfolioSummary.timeframe}</span>
                  </div>
                </div>
                <button 
                  className="btn btn-primary"
                  onClick={() => fetchPortfolioData()}
                  disabled={isLoading}
                >
                  {isLoading ? (
                    <>
                      <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                      Refreshing...
                    </>
                  ) : (
                    <>
                      <i className="bi bi-arrow-repeat me-2"></i>
                      Refresh
                    </>
                  )}
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row g-3">
        {[
          {
            title: 'Stocks',
            value: '$78,350.00',
            change: '+3.2%',
            timeframe: 'vs last month',
            icon: 'bi-graph-up',
            color: 'primary'
          },
          {
            title: 'Bonds',
            value: '$32,150.00',
            change: '+0.8%',
            timeframe: 'vs last month',
            icon: 'bi-shield-check',
            color: 'success'
          },
          {
            title: 'Cash',
            value: '$14,000.00',
            change: '-$2,000.00',
            timeframe: 'vs last month',
            icon: 'bi-cash-coin',
            color: 'info'
          }
        ].map((metric, index) => (
          <div className="col-md-4" key={index}>
            <DashboardCard
              title={metric.title}
              value={metric.value}
              change={metric.change}
              timeframe={metric.timeframe}
              icon={metric.icon}
              color={metric.color}
            />
          </div>
        ))}
      </div>
    </div>
  );
}

export default PortfolioSummary; 