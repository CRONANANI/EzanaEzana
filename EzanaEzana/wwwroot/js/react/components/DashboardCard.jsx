import React from 'react';

/**
 * A reusable card component for displaying investment metrics on the dashboard
 * 
 * @param {Object} props - Component props
 * @param {string} props.title - Card title
 * @param {string} props.value - Main value to display
 * @param {string} props.change - Change value (can be positive or negative)
 * @param {string} props.timeframe - Timeframe for the change (e.g., "vs last week")
 * @param {string} props.icon - Bootstrap icon class
 * @param {string} props.color - Card accent color (primary, success, danger, warning, info)
 */
function DashboardCard({ title, value, change, timeframe, icon, color = 'primary' }) {
  // Determine if change is positive, negative or neutral
  const isPositive = change && change.startsWith('+');
  const isNegative = change && change.startsWith('-');
  const changeColorClass = isPositive ? 'text-success' : isNegative ? 'text-danger' : 'text-muted';
  
  return (
    <div className={`card border-${color} h-100 shadow-sm`}>
      <div className="card-body">
        <div className="d-flex justify-content-between align-items-start mb-3">
          <h5 className="card-title text-muted mb-0">{title}</h5>
          {icon && <i className={`bi ${icon} text-${color} fs-4`}></i>}
        </div>
        
        <div className="d-flex align-items-baseline">
          <h3 className="mb-0 me-2">{value}</h3>
          {change && (
            <div className={`${changeColorClass} d-flex align-items-center`}>
              <span>{change}</span>
              {isPositive && <i className="bi bi-arrow-up-short"></i>}
              {isNegative && <i className="bi bi-arrow-down-short"></i>}
            </div>
          )}
        </div>
        
        {timeframe && <small className="text-muted">{timeframe}</small>}
      </div>
    </div>
  );
}

export default DashboardCard; 