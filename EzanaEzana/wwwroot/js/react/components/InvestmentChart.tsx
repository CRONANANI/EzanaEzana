import React, { useMemo } from 'react';
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Area,
  AreaChart
} from 'recharts';

interface InvestmentChartProps {
  timeframe: string;
  data?: any[];
}

const InvestmentChart: React.FC<InvestmentChartProps> = ({ timeframe, data }) => {
  // Generate mock data based on timeframe
  const chartData = useMemo(() => {
    const now = new Date();
    const points = timeframe === '1D' ? 24 : 
                   timeframe === '1W' ? 7 : 
                   timeframe === '1M' ? 30 : 
                   timeframe === '3M' ? 90 : 
                   timeframe === '1Y' ? 365 : 30;
    
    const mockData = [];
    let baseValue = 10000;
    
    for (let i = 0; i < points; i++) {
      const date = new Date(now);
      if (timeframe === '1D') {
        date.setHours(date.getHours() - (points - i));
      } else if (timeframe === '1W') {
        date.setDate(date.getDate() - (points - i));
      } else if (timeframe === '1M') {
        date.setDate(date.getDate() - (points - i));
      } else if (timeframe === '3M') {
        date.setDate(date.getDate() - (points - i));
      } else if (timeframe === '1Y') {
        date.setDate(date.getDate() - (points - i));
      }
      
      // Add some realistic volatility
      const change = (Math.random() - 0.5) * 0.02; // ±1% daily change
      baseValue *= (1 + change);
      
      mockData.push({
        date: timeframe === '1D' ? date.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit' }) :
              timeframe === '1W' ? date.toLocaleDateString('en-US', { weekday: 'short' }) :
              date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' }),
        value: Math.round(baseValue),
        change: change > 0 ? 'positive' : 'negative'
      });
    }
    
    return mockData;
  }, [timeframe]);

  const CustomTooltip = ({ active, payload, label }: any) => {
    if (active && payload && payload.length) {
      return (
        <div className="bg-white p-3 border border-gray-200 rounded-lg shadow-lg">
          <p className="text-sm text-gray-600">{label}</p>
          <p className="text-lg font-semibold text-gray-900">
            ${payload[0].value.toLocaleString()}
          </p>
        </div>
      );
    }
    return null;
  };

  const formatYAxis = (tickItem: number) => {
    if (tickItem >= 1000) {
      return `$${(tickItem / 1000).toFixed(0)}K`;
    }
    return `$${tickItem.toLocaleString()}`;
  };

  return (
    <div className="w-full h-80">
      <ResponsiveContainer width="100%" height="100%">
        <AreaChart data={chartData} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
          <defs>
            <linearGradient id="colorValue" x1="0" y1="0" x2="0" y2="1">
              <stop offset="5%" stopColor="#3b82f6" stopOpacity={0.3}/>
              <stop offset="95%" stopColor="#3b82f6" stopOpacity={0.05}/>
            </linearGradient>
          </defs>
          <CartesianGrid strokeDasharray="3 3" stroke="#f3f4f6" />
          <XAxis 
            dataKey="date" 
            stroke="#9ca3af"
            fontSize={12}
            tickLine={false}
            axisLine={false}
          />
          <YAxis 
            stroke="#9ca3af"
            fontSize={12}
            tickLine={false}
            axisLine={false}
            tickFormatter={formatYAxis}
          />
          <Tooltip content={<CustomTooltip />} />
          <Area
            type="monotone"
            dataKey="value"
            stroke="#3b82f6"
            strokeWidth={2}
            fill="url(#colorValue)"
            dot={false}
            activeDot={{ r: 6, fill: '#3b82f6', stroke: '#ffffff', strokeWidth: 2 }}
          />
        </AreaChart>
      </ResponsiveContainer>
      
      {/* Chart Legend */}
      <div className="flex items-center justify-center mt-4 space-x-6">
        <div className="flex items-center space-x-2">
          <div className="w-3 h-3 bg-primary-600 rounded-full"></div>
          <span className="text-sm text-gray-600">Portfolio Value</span>
        </div>
        <div className="flex items-center space-x-2">
          <div className="w-3 h-3 bg-gray-300 rounded-full"></div>
          <span className="text-sm text-gray-600">Benchmark (S&P 500)</span>
        </div>
      </div>
    </div>
  );
};

export default InvestmentChart;
