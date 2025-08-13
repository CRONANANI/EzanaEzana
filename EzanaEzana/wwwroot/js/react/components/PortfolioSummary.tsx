import React from 'react';
import { TrendingUp, TrendingDown, Plus, Eye } from 'lucide-react';

interface Portfolio {
  id: string;
  name: string;
  description?: string;
  totalValue: number;
  totalChange: number;
  totalChangePercent: number;
  createdAt: string;
  updatedAt: string;
}

interface PortfolioSummaryProps {
  portfolios: Portfolio[];
}

const PortfolioSummary: React.FC<PortfolioSummaryProps> = ({ portfolios }) => {
  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  };

  const formatChange = (change: number) => {
    if (change >= 0) {
      return `+${formatCurrency(change)}`;
    }
    return formatCurrency(change);
  };

  const formatChangePercent = (percent: number) => {
    if (isNaN(percent) || !isFinite(percent)) {
      return '0.00%';
    }
    return `${percent >= 0 ? '+' : ''}${percent.toFixed(2)}%`;
  };

  const getTrendIcon = (change: number) => {
    if (change >= 0) {
      return <TrendingUp className="h-4 w-4 text-success-600" />;
    }
    return <TrendingDown className="h-4 w-4 text-danger-600" />;
  };

  const getChangeColor = (change: number) => {
    if (change >= 0) {
      return 'text-success-600';
    }
    return 'text-danger-600';
  };

  if (portfolios.length === 0) {
    return (
      <div className="text-center py-8">
        <div className="w-16 h-16 bg-gray-100 rounded-full flex items-center justify-center mx-auto mb-4">
          <Plus className="h-8 w-8 text-gray-400" />
        </div>
        <h3 className="text-lg font-medium text-gray-900 mb-2">No portfolios yet</h3>
        <p className="text-gray-600 mb-4">Create your first portfolio to start tracking your investments.</p>
        <button className="btn btn-primary btn-sm">
          <Plus className="h-4 w-4 mr-2" />
          Create Portfolio
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {/* Portfolio List */}
      {portfolios.map((portfolio) => (
        <div key={portfolio.id} className="border border-gray-200 rounded-lg p-4 hover:shadow-md transition-shadow">
          <div className="flex items-center justify-between mb-3">
            <div className="flex-1">
              <h4 className="font-semibold text-gray-900">{portfolio.name}</h4>
              {portfolio.description && (
                <p className="text-sm text-gray-600 mt-1">{portfolio.description}</p>
              )}
            </div>
            <div className="flex items-center space-x-2">
              <button className="btn btn-ghost btn-sm">
                <Eye className="h-4 w-4" />
              </button>
              <button className="btn btn-outline btn-sm">
                Edit
              </button>
            </div>
          </div>
          
          <div className="grid grid-cols-2 gap-4">
            <div>
              <p className="text-sm text-gray-600 mb-1">Total Value</p>
              <p className="text-lg font-semibold text-gray-900">
                {formatCurrency(portfolio.totalValue)}
              </p>
            </div>
            <div>
              <p className="text-sm text-gray-600 mb-1">Today's Change</p>
              <div className="flex items-center space-x-2">
                {getTrendIcon(portfolio.totalChange)}
                <span className={`font-semibold ${getChangeColor(portfolio.totalChange)}`}>
                  {formatChange(portfolio.totalChange)}
                </span>
              </div>
              <p className={`text-sm ${getChangeColor(portfolio.totalChange)}`}>
                {formatChangePercent(portfolio.totalChangePercent)}
              </p>
            </div>
          </div>
          
          <div className="mt-3 pt-3 border-t border-gray-100">
            <div className="flex justify-between text-xs text-gray-500">
              <span>Created: {new Date(portfolio.createdAt).toLocaleDateString()}</span>
              <span>Updated: {new Date(portfolio.updatedAt).toLocaleDateString()}</span>
            </div>
          </div>
        </div>
      ))}
      
      {/* Add Portfolio Button */}
      <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-primary-400 hover:bg-primary-50 transition-colors">
        <Plus className="h-8 w-8 text-gray-400 mx-auto mb-2" />
        <h3 className="text-sm font-medium text-gray-900 mb-1">Add New Portfolio</h3>
        <p className="text-xs text-gray-500 mb-3">Organize your investments into different portfolios</p>
        <button className="btn btn-primary btn-sm">
          <Plus className="h-4 w-4 mr-2" />
          Create Portfolio
        </button>
      </div>
    </div>
  );
};

export default PortfolioSummary;
