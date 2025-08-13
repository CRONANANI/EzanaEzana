import React from 'react';
import { LucideIcon, TrendingUp, TrendingDown } from 'lucide-react';

interface DashboardCardProps {
  title: string;
  value: string;
  change: number;
  changePercent: number;
  icon: LucideIcon;
  trend: 'up' | 'down' | 'neutral';
  className?: string;
}

const DashboardCard: React.FC<DashboardCardProps> = ({
  title,
  value,
  change,
  changePercent,
  icon: Icon,
  trend,
  className = ''
}) => {
  const getTrendColor = () => {
    switch (trend) {
      case 'up':
        return 'text-success-600';
      case 'down':
        return 'text-danger-600';
      default:
        return 'text-gray-600';
    }
  };

  const getTrendIcon = () => {
    switch (trend) {
      case 'up':
        return <TrendingUp className="h-4 w-4" />;
      case 'down':
        return <TrendingDown className="h-4 w-4" />;
      default:
        return null;
    }
  };

  const formatChange = (change: number) => {
    if (change >= 0) {
      return `+${change.toLocaleString()}`;
    }
    return change.toLocaleString();
  };

  const formatChangePercent = (percent: number) => {
    if (isNaN(percent) || !isFinite(percent)) {
      return '0.00%';
    }
    return `${percent >= 0 ? '+' : ''}${percent.toFixed(2)}%`;
  };

  return (
    <div className={`card hover:shadow-lg transition-all duration-200 ${className}`}>
      <div className="card-content">
        <div className="flex items-center justify-between">
          <div className="flex-1">
            <p className="text-sm font-medium text-gray-600 mb-1">{title}</p>
            <p className="text-2xl font-bold text-gray-900 mb-2">{value}</p>
            <div className="flex items-center space-x-2">
              {getTrendIcon()}
              <span className={`text-sm font-medium ${getTrendColor()}`}>
                {formatChange(change)}
              </span>
              <span className={`text-sm font-medium ${getTrendColor()}`}>
                ({formatChangePercent(changePercent)})
              </span>
            </div>
          </div>
          <div className="flex-shrink-0">
            <div className="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
              <Icon className="h-6 w-6 text-primary-600" />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default DashboardCard;
