import React, { useState, useEffect } from 'react';
import { quiverApiService, CongressPerson, CongressionalTrade, TradingAnalytics } from '../services/quiverApi';

interface CongressionalTradingProps {
  className?: string;
}

const CongressionalTrading: React.FC<CongressionalTradingProps> = ({ className = '' }) => {
  const [congressPeople, setCongressPeople] = useState<CongressPerson[]>([]);
  const [recentTrades, setRecentTrades] = useState<CongressionalTrade[]>([]);
  const [analytics, setAnalytics] = useState<TradingAnalytics | null>(null);
  const [selectedPerson, setSelectedPerson] = useState<CongressPerson | null>(null);
  const [personTrades, setPersonTrades] = useState<CongressionalTrade[]>([]);
  const [searchQuery, setSearchQuery] = useState('');
  const [filterParty, setFilterParty] = useState<'all' | 'D' | 'R' | 'I'>('all');
  const [filterChamber, setFilterChamber] = useState<'all' | 'House' | 'Senate'>('all');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [activeTab, setActiveTab] = useState<'overview' | 'trades' | 'analytics'>('overview');

  useEffect(() => {
    loadInitialData();
  }, []);

  const loadInitialData = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const [people, trades, tradingAnalytics] = await Promise.all([
        quiverApiService.getCongressPeople(),
        quiverApiService.getRecentTrades(20),
        quiverApiService.getTradingAnalytics()
      ]);
      
      setCongressPeople(people);
      setRecentTrades(trades);
      setAnalytics(tradingAnalytics);
    } catch (err) {
      setError('Failed to load congressional trading data');
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  const handlePersonSelect = async (person: CongressPerson) => {
    setSelectedPerson(person);
    setActiveTab('trades');
    
    try {
      const trades = await quiverApiService.getCongressPersonTrades(person.id, 50);
      setPersonTrades(trades);
    } catch (err) {
      console.error('Error loading person trades:', err);
    }
  };

  const handleSearch = async () => {
    if (!searchQuery.trim()) {
      setCongressPeople(await quiverApiService.getCongressPeople());
      return;
    }
    
    try {
      const results = await quiverApiService.searchCongressPeople(searchQuery);
      setCongressPeople(results);
    } catch (err) {
      console.error('Error searching:', err);
    }
  };

  const getFilteredCongressPeople = () => {
    return congressPeople.filter(person => {
      const partyMatch = filterParty === 'all' || person.party === filterParty;
      const chamberMatch = filterChamber === 'all' || person.chamber === filterChamber;
      return partyMatch && chamberMatch;
    });
  };

  const getPartyColor = (party: string) => {
    switch (party) {
      case 'D': return 'bg-blue-500';
      case 'R': return 'bg-red-500';
      case 'I': return 'bg-purple-500';
      default: return 'bg-gray-500';
    }
  };

  const getTradeTypeColor = (type: string) => {
    return type === 'buy' ? 'text-green-600' : 'text-red-600';
  };

  const formatAmount = (amount: number) => {
    if (amount >= 1000000) {
      return `$${(amount / 1000000).toFixed(1)}M`;
    } else if (amount >= 1000) {
      return `$${(amount / 1000).toFixed(1)}K`;
    }
    return `$${amount.toFixed(0)}`;
  };

  if (loading) {
    return (
      <div className={`flex items-center justify-center p-8 ${className}`}>
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading congressional trading data...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className={`p-6 bg-red-50 border border-red-200 rounded-lg ${className}`}>
        <div className="flex items-center">
          <div className="flex-shrink-0">
            <svg className="h-5 w-5 text-red-400" viewBox="0 0 20 20" fill="currentColor">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
          </div>
          <div className="ml-3">
            <h3 className="text-sm font-medium text-red-800">Error Loading Data</h3>
            <p className="text-sm text-red-700 mt-1">{error}</p>
          </div>
        </div>
        <button
          onClick={loadInitialData}
          className="mt-4 bg-red-100 text-red-800 px-4 py-2 rounded-md text-sm font-medium hover:bg-red-200"
        >
          Retry
        </button>
      </div>
    );
  }

  return (
    <div className={`bg-white rounded-lg shadow-lg ${className}`}>
      {/* Header */}
      <div className="px-6 py-4 border-b border-gray-200">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-2xl font-bold text-gray-900">Congressional Trading Dashboard</h2>
            <p className="text-gray-600 mt-1">Track investments made by members of Congress</p>
          </div>
          <div className="flex items-center space-x-2">
            <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-green-100 text-green-800">
              Live Data
            </span>
          </div>
        </div>
      </div>

      {/* Search and Filters */}
      <div className="px-6 py-4 border-b border-gray-200 bg-gray-50">
        <div className="flex flex-wrap gap-4 items-center">
          <div className="flex-1 min-w-64">
            <div className="relative">
              <input
                type="text"
                placeholder="Search congresspeople by name, state, or party..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                onKeyPress={(e) => e.key === 'Enter' && handleSearch()}
                className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
              />
              <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                <svg className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                </svg>
              </div>
            </div>
          </div>
          
          <select
            value={filterParty}
            onChange={(e) => setFilterParty(e.target.value as any)}
            className="px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="all">All Parties</option>
            <option value="D">Democrat</option>
            <option value="R">Republican</option>
            <option value="I">Independent</option>
          </select>
          
          <select
            value={filterChamber}
            onChange={(e) => setFilterChamber(e.target.value as any)}
            className="px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          >
            <option value="all">All Chambers</option>
            <option value="House">House</option>
            <option value="Senate">Senate</option>
          </select>
          
          <button
            onClick={handleSearch}
            className="px-4 py-2 bg-blue-600 text-white rounded-md hover:bg-blue-700 focus:ring-2 focus:ring-blue-500 focus:ring-offset-2"
          >
            Search
          </button>
        </div>
      </div>

      {/* Tabs */}
      <div className="border-b border-gray-200">
        <nav className="-mb-px flex space-x-8 px-6">
          <button
            onClick={() => setActiveTab('overview')}
            className={`py-4 px-1 border-b-2 font-medium text-sm ${
              activeTab === 'overview'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            Overview
          </button>
          <button
            onClick={() => setActiveTab('trades')}
            className={`py-4 px-1 border-b-2 font-medium text-sm ${
              activeTab === 'trades'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            Recent Trades
          </button>
          <button
            onClick={() => setActiveTab('analytics')}
            className={`py-4 px-1 border-b-2 font-medium text-sm ${
              activeTab === 'analytics'
                ? 'border-blue-500 text-blue-600'
                : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300'
            }`}
          >
            Analytics
          </button>
        </nav>
      </div>

      {/* Content */}
      <div className="p-6">
        {activeTab === 'overview' && (
          <div className="space-y-6">
            {/* Stats Cards */}
            <div className="grid grid-cols-1 md:grid-cols-4 gap-6">
              <div className="bg-blue-50 p-6 rounded-lg">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <svg className="h-8 w-8 text-blue-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                    </svg>
                  </div>
                  <div className="ml-4">
                    <p className="text-sm font-medium text-blue-600">Total Congresspeople</p>
                    <p className="text-2xl font-semibold text-blue-900">{congressPeople.length}</p>
                  </div>
                </div>
              </div>
              
              <div className="bg-green-50 p-6 rounded-lg">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <svg className="h-8 w-8 text-green-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z" />
                    </svg>
                  </div>
                  <div className="ml-4">
                    <p className="text-sm font-medium text-green-600">Total Trades</p>
                    <p className="text-2xl font-semibold text-green-900">{analytics?.totalTrades || 0}</p>
                  </div>
                </div>
              </div>
              
              <div className="bg-purple-50 p-6 rounded-lg">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <svg className="h-8 w-8 text-purple-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1" />
                    </svg>
                  </div>
                  <div className="ml-4">
                    <p className="text-sm font-medium text-purple-600">Total Volume</p>
                    <p className="text-2xl font-semibold text-purple-900">{analytics ? formatAmount(analytics.totalVolume) : '$0'}</p>
                  </div>
                </div>
              </div>
              
              <div className="bg-orange-50 p-6 rounded-lg">
                <div className="flex items-center">
                  <div className="flex-shrink-0">
                    <svg className="h-8 w-8 text-orange-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 7h8m0 0v8m0-8l-8 8-4-4-6 6" />
                    </svg>
                  </div>
                  <div className="ml-4">
                    <p className="text-sm font-medium text-orange-600">Recent Activity</p>
                    <p className="text-2xl font-semibold text-orange-900">{recentTrades.length}</p>
                  </div>
                </div>
              </div>
            </div>

            {/* Congresspeople Grid */}
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-4">Congresspeople</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {getFilteredCongressPeople().map((person) => (
                  <div
                    key={person.id}
                    onClick={() => handlePersonSelect(person)}
                    className="border border-gray-200 rounded-lg p-4 cursor-pointer hover:bg-gray-50 hover:border-blue-300 transition-colors"
                  >
                    <div className="flex items-start justify-between">
                      <div className="flex-1">
                        <h4 className="text-sm font-medium text-gray-900">{person.name}</h4>
                        <p className="text-xs text-gray-500">{person.state} • {person.chamber}</p>
                        <div className="flex items-center mt-2">
                          <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium text-white ${getPartyColor(person.party)}`}>
                            {person.party === 'D' ? 'Democrat' : person.party === 'R' ? 'Republican' : 'Independent'}
                          </span>
                        </div>
                        {person.committee.length > 0 && (
                          <p className="text-xs text-gray-500 mt-2">
                            Committees: {person.committee.slice(0, 2).join(', ')}
                            {person.committee.length > 2 && '...'}
                          </p>
                        )}
                      </div>
                      <div className="text-right">
                        <p className="text-xs text-gray-500">Trades</p>
                        <p className="text-sm font-medium text-gray-900">{person.totalTrades || 0}</p>
                        {person.lastTradeDate && (
                          <p className="text-xs text-gray-400 mt-1">
                            Last: {new Date(person.lastTradeDate).toLocaleDateString()}
                          </p>
                        )}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </div>
        )}

        {activeTab === 'trades' && (
          <div className="space-y-6">
            {selectedPerson && (
              <div className="bg-blue-50 p-4 rounded-lg">
                <h3 className="text-lg font-medium text-blue-900 mb-2">
                  Trades by {selectedPerson.name}
                </h3>
                <p className="text-sm text-blue-700">
                  {selectedPerson.state} • {selectedPerson.chamber} • {selectedPerson.party === 'D' ? 'Democrat' : selectedPerson.party === 'R' ? 'Republican' : 'Independent'}
                </p>
              </div>
            )}
            
            <div className="overflow-x-auto">
              <table className="min-w-full divide-y divide-gray-200">
                <thead className="bg-gray-50">
                  <tr>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Congressperson
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Company
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Trade
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Amount
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Date
                    </th>
                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                      Owner
                    </th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-200">
                  {(selectedPerson ? personTrades : recentTrades).map((trade) => (
                    <tr key={trade.id} className="hover:bg-gray-50">
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="flex items-center">
                          <div className="flex-shrink-0 h-8 w-8">
                            <div className="h-8 w-8 rounded-full bg-gray-300 flex items-center justify-center">
                              <span className="text-sm font-medium text-gray-700">
                                {trade.congressPersonName.charAt(0)}
                              </span>
                            </div>
                          </div>
                          <div className="ml-4">
                            <div className="text-sm font-medium text-gray-900">
                              {trade.congressPersonName}
                            </div>
                          </div>
                        </div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <div className="text-sm text-gray-900">{trade.ticker}</div>
                        <div className="text-sm text-gray-500">{trade.companyName}</div>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap">
                        <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                          trade.tradeType === 'buy' 
                            ? 'bg-green-100 text-green-800' 
                            : 'bg-red-100 text-red-800'
                        }`}>
                          {trade.tradeType.toUpperCase()}
                        </span>
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-900">
                        {formatAmount(trade.amount)}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                        {new Date(trade.tradeDate).toLocaleDateString()}
                      </td>
                      <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                        {trade.owner}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}

        {activeTab === 'analytics' && analytics && (
          <div className="space-y-6">
            {/* Most Traded Stocks */}
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-4">Most Traded Stocks</h3>
              <div className="bg-gray-50 rounded-lg p-4">
                <div className="space-y-3">
                  {analytics.mostTradedStocks.map((stock, index) => (
                    <div key={stock.ticker} className="flex items-center justify-between">
                      <div className="flex items-center">
                        <span className="text-sm font-medium text-gray-500 w-6">{index + 1}.</span>
                        <span className="text-sm font-medium text-gray-900">{stock.ticker}</span>
                        <span className="text-sm text-gray-500 ml-2">({stock.count} trades)</span>
                      </div>
                      <span className="text-sm font-medium text-gray-900">
                        {formatAmount(stock.volume)}
                      </span>
                    </div>
                  ))}
                </div>
              </div>
            </div>

            {/* Party Breakdown */}
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-4">Trading by Party</h3>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                {analytics.partyBreakdown.map((party) => (
                  <div key={party.party} className="bg-gray-50 rounded-lg p-4">
                    <div className="flex items-center justify-between">
                      <span className="text-sm font-medium text-gray-900">
                        {party.party === 'D' ? 'Democrats' : party.party === 'R' ? 'Republicans' : 'Independents'}
                      </span>
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${
                        party.party === 'D' ? 'bg-blue-100 text-blue-800' :
                        party.party === 'R' ? 'bg-red-100 text-red-800' :
                        'bg-purple-100 text-purple-800'
                      }`}>
                        {party.count} trades
                      </span>
                    </div>
                    <div className="mt-2 text-2xl font-bold text-gray-900">
                      {formatAmount(party.volume)}
                    </div>
                  </div>
                ))}
              </div>
            </div>

            {/* Monthly Trends */}
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-4">Monthly Trading Trends</h3>
              <div className="bg-gray-50 rounded-lg p-4">
                <div className="space-y-3">
                  {analytics.monthlyTrends.map((trend) => (
                    <div key={trend.month} className="flex items-center justify-between">
                      <span className="text-sm font-medium text-gray-900">{trend.month}</span>
                      <div className="flex items-center space-x-4">
                        <span className="text-sm text-gray-500">{trend.count} trades</span>
                        <span className="text-sm font-medium text-gray-900">
                          {formatAmount(trend.volume)}
                        </span>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default CongressionalTrading;
