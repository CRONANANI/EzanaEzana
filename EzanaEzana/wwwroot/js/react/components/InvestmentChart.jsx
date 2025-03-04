import React, { useState } from 'react';

/**
 * Investment Chart Component
 * 
 * This is a placeholder for a chart component. In a real application,
 * you would integrate with a charting library like Chart.js, Recharts,
 * or ApexCharts to display actual investment data.
 * 
 * @param {Object} props
 * @param {string} props.title - Chart title
 * @param {string} props.type - Chart type (line, bar, pie, etc.)
 * @param {Object} props.data - Chart data
 */
function InvestmentChart({ title = 'Investment Performance', type = 'line' }) {
  const [timeframe, setTimeframe] = useState('1M'); // Default to 1 month view
  
  // These would be the timeframe options
  const timeframes = ['1W', '1M', '3M', '6M', 'YTD', '1Y', '5Y', 'All'];
  
  return (
    <div className="card shadow-sm">
      <div className="card-header bg-white">
        <div className="d-flex justify-content-between align-items-center">
          <h5 className="card-title mb-0">{title}</h5>
          <div className="btn-group" role="group">
            {timeframes.map(tf => (
              <button
                key={tf}
                type="button"
                className={`btn btn-sm ${timeframe === tf ? 'btn-primary' : 'btn-outline-secondary'}`}
                onClick={() => setTimeframe(tf)}
              >
                {tf}
              </button>
            ))}
          </div>
        </div>
      </div>
      <div className="card-body">
        {/* This div would be replaced with an actual chart from a library */}
        <div className="chart-placeholder" style={{ height: '300px', position: 'relative' }}>
          <div className="position-absolute top-50 start-50 translate-middle text-center">
            <p className="text-muted mb-2">
              <i className="bi bi-bar-chart-line fs-1"></i>
            </p>
            <p className="text-muted">
              In a real implementation, this would be a {type} chart showing investment data for the {timeframe} timeframe.
            </p>
            <p className="text-muted small">
              <strong>Note:</strong> To implement actual charts, install a library like Chart.js:
              <br />
              <code>npm install chart.js react-chartjs-2</code>
            </p>
          </div>
        </div>
      </div>
      <div className="card-footer bg-white d-flex justify-content-between">
        <div>
          <span className="text-muted me-3">Current: <strong>$124,500.00</strong></span>
          <span className="text-success">
            <i className="bi bi-arrow-up-short"></i> +2.7%
          </span>
        </div>
        <div>
          <button className="btn btn-sm btn-outline-secondary">
            <i className="bi bi-download me-1"></i>
            Export
          </button>
        </div>
      </div>
    </div>
  );
}

export default InvestmentChart; 