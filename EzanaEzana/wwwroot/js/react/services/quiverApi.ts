import axios, { AxiosInstance } from 'axios';

// Quiver Quantitative API Service for Congressional Trading Data
// https://www.quiverquant.com/

// API Configuration
const API_CONFIG = {
  QUIVER: {
    BASE_URL: 'https://api.quiverquant.com/beta',
    API_KEY: '2fb95c89103d4cb07b26fff07c8cfa77626291da',
    FREE_TIER_LIMIT: 1000 // requests per month
  }
};

// Types for congressional trading data
export interface CongressPerson {
  id: string;
  name: string;
  party: 'D' | 'R' | 'I';
  state: string;
  chamber: 'House' | 'Senate';
  committee: string[];
  imageUrl?: string;
  lastTradeDate?: string;
  totalTrades?: number;
  isTracked?: boolean;
}

export interface CongressionalTrade {
  id: string;
  congressPersonId: string;
  congressPersonName: string;
  ticker: string;
  companyName: string;
  tradeType: 'buy' | 'sell';
  amount: number; // Amount in USD
  amountRange: '1,001-15,000' | '15,001-50,000' | '50,001-100,000' | '100,001-250,000' | '250,001-500,000' | '500,001-1,000,000' | '1,000,001+';
  tradeDate: string;
  disclosureDate: string;
  owner: 'self' | 'spouse' | 'dependent';
  sector?: string;
  industry?: string;
}

export interface TradeNotification {
  id: string;
  congressPersonId: string;
  congressPersonName: string;
  ticker: string;
  companyName: string;
  tradeType: 'buy' | 'sell';
  amount: number;
  tradeDate: string;
  isRead: boolean;
  createdAt: string;
}

export interface UserSubscription {
  userId: string;
  congressPersonId: string;
  congressPersonName: string;
  notificationsEnabled: boolean;
  emailAlerts: boolean;
  pushNotifications: boolean;
  createdAt: string;
  lastNotificationDate?: string;
}

export interface TradingAnalytics {
  totalTrades: number;
  totalVolume: number;
  mostTradedStocks: Array<{ ticker: string; count: number; volume: number }>;
  sectorBreakdown: Array<{ sector: string; count: number; volume: number }>;
  partyBreakdown: Array<{ party: string; count: number; volume: number }>;
  monthlyTrends: Array<{ month: string; count: number; volume: number }>;
}

// Main Quiver Quantitative API Service
class QuiverApiService {
  private quiver: AxiosInstance;
  private useMockData: boolean = false; // Default to real API calls

  constructor() {
    this.quiver = axios.create({
      baseURL: API_CONFIG.QUIVER.BASE_URL,
      timeout: 15000,
      headers: {
        'Authorization': `Bearer ${API_CONFIG.QUIVER.API_KEY}`,
        'Content-Type': 'application/json'
      }
    });
    console.log('Quiver Quantitative API Service initialized');
  }

  // Toggle between real API and mock data
  setUseMockData(useMock: boolean) {
    this.useMockData = useMock;
    console.log(`Quiver API: ${useMock ? 'Using mock data' : 'Using Quiver Quantitative API'}`);
  }

  // Get all congresspeople
  async getCongressPeople(): Promise<CongressPerson[]> {
    if (this.useMockData) {
      return this.generateMockCongressPeople();
    }

    try {
      console.log('Fetching congresspeople from Quiver Quantitative...');
      const response = await this.quiver.get('/congresspeople');
      
      if (!response.data || !Array.isArray(response.data)) {
        throw new Error('Invalid response format from Quiver API');
      }

      return response.data.map((person: any) => ({
        id: person.id || person.congressperson_id,
        name: person.name || person.congressperson_name,
        party: person.party || 'I',
        state: person.state || 'Unknown',
        chamber: person.chamber || 'House',
        committee: person.committee || [],
        imageUrl: person.image_url,
        lastTradeDate: person.last_trade_date,
        totalTrades: person.total_trades || 0
      }));
    } catch (error) {
      console.error('Error fetching congresspeople from Quiver:', error);
      console.log('Falling back to mock data');
      return this.generateMockCongressPeople();
    }
  }

  // Get congressional trades for a specific person
  async getCongressPersonTrades(congressPersonId: string, limit: number = 50): Promise<CongressionalTrade[]> {
    if (this.useMockData) {
      return this.generateMockTrades(congressPersonId, limit);
    }

    try {
      console.log(`Fetching trades for congressperson ${congressPersonId} from Quiver...`);
      const response = await this.quiver.get(`/congressperson/${congressPersonId}/trades`, {
        params: { limit }
      });

      if (!response.data || !Array.isArray(response.data)) {
        throw new Error('Invalid response format from Quiver API');
      }

      return response.data.map((trade: any) => ({
        id: trade.id || trade.trade_id,
        congressPersonId: trade.congressperson_id,
        congressPersonName: trade.congressperson_name,
        ticker: trade.ticker,
        companyName: trade.company_name,
        tradeType: trade.trade_type?.toLowerCase() || 'buy',
        amount: trade.amount || 0,
        amountRange: trade.amount_range || '1,001-15,000',
        tradeDate: trade.trade_date,
        disclosureDate: trade.disclosure_date,
        owner: trade.owner || 'self',
        sector: trade.sector,
        industry: trade.industry
      }));
    } catch (error) {
      console.error(`Error fetching trades for congressperson ${congressPersonId}:`, error);
      console.log('Falling back to mock data');
      return this.generateMockTrades(congressPersonId, limit);
    }
  }

  // Get recent congressional trades across all congresspeople
  async getRecentTrades(limit: number = 100): Promise<CongressionalTrade[]> {
    if (this.useMockData) {
      return this.generateMockRecentTrades(limit);
    }

    try {
      console.log('Fetching recent congressional trades from Quiver...');
      const response = await this.quiver.get('/congressional-trades', {
        params: { limit }
      });

      if (!response.data || !Array.isArray(response.data)) {
        throw new Error('Invalid response format from Quiver API');
      }

      return response.data.map((trade: any) => ({
        id: trade.id || trade.trade_id,
        congressPersonId: trade.congressperson_id,
        congressPersonName: trade.congressperson_name,
        ticker: trade.ticker,
        companyName: trade.company_name,
        tradeType: trade.trade_type?.toLowerCase() || 'buy',
        amount: trade.amount || 0,
        amountRange: trade.amount_range || '1,001-15,000',
        tradeDate: trade.trade_date,
        disclosureDate: trade.disclosure_date,
        owner: trade.owner || 'self',
        sector: trade.sector,
        industry: trade.industry
      }));
    } catch (error) {
      console.error('Error fetching recent congressional trades:', error);
      console.log('Falling back to mock data');
      return this.generateMockRecentTrades(limit);
    }
  }

  // Get trading analytics and insights
  async getTradingAnalytics(): Promise<TradingAnalytics> {
    if (this.useMockData) {
      return this.generateMockTradingAnalytics();
    }

    try {
      console.log('Fetching trading analytics from Quiver...');
      const response = await this.quiver.get('/congressional-trading-analytics');
      
      if (!response.data) {
        throw new Error('Invalid response format from Quiver API');
      }

      return {
        totalTrades: response.data.total_trades || 0,
        totalVolume: response.data.total_volume || 0,
        mostTradedStocks: response.data.most_traded_stocks || [],
        sectorBreakdown: response.data.sector_breakdown || [],
        partyBreakdown: response.data.party_breakdown || [],
        monthlyTrends: response.data.monthly_trends || []
      };
    } catch (error) {
      console.error('Error fetching trading analytics:', error);
      console.log('Falling back to mock data');
      return this.generateMockTradingAnalytics();
    }
  }

  // Search congresspeople by name, state, or party
  async searchCongressPeople(query: string): Promise<CongressPerson[]> {
    const allPeople = await this.getCongressPeople();
    
    return allPeople.filter(person => 
      person.name.toLowerCase().includes(query.toLowerCase()) ||
      person.state.toLowerCase().includes(query.toLowerCase()) ||
      person.party.toLowerCase().includes(query.toLowerCase())
    );
  }

  // Get congresspeople by state
  async getCongressPeopleByState(state: string): Promise<CongressPerson[]> {
    const allPeople = await this.getCongressPeople();
    return allPeople.filter(person => 
      person.state.toLowerCase() === state.toLowerCase()
    );
  }

  // Get congresspeople by party
  async getCongressPeopleByParty(party: 'D' | 'R' | 'I'): Promise<CongressPerson[]> {
    const allPeople = await this.getCongressPeople();
    return allPeople.filter(person => person.party === party);
  }

  // Get congresspeople by chamber
  async getCongressPeopleByChamber(chamber: 'House' | 'Senate'): Promise<CongressPerson[]> {
    const allPeople = await this.getCongressPeople();
    return allPeople.filter(person => person.chamber === chamber);
  }

  // Mock data generators for fallback
  private generateMockCongressPeople(): CongressPerson[] {
    const mockPeople = [
      {
        id: '1',
        name: 'Nancy Pelosi',
        party: 'D' as const,
        state: 'CA',
        chamber: 'House' as const,
        committee: ['Appropriations', 'Intelligence'],
        lastTradeDate: '2024-01-15',
        totalTrades: 45
      },
      {
        id: '2',
        name: 'Mitch McConnell',
        party: 'R' as const,
        state: 'KY',
        chamber: 'Senate' as const,
        committee: ['Appropriations', 'Rules'],
        lastTradeDate: '2024-01-10',
        totalTrades: 32
      },
      {
        id: '3',
        name: 'Chuck Schumer',
        party: 'D' as const,
        state: 'NY',
        chamber: 'Senate' as const,
        committee: ['Finance', 'Judiciary'],
        lastTradeDate: '2024-01-12',
        totalTrades: 28
      },
      {
        id: '4',
        name: 'Kevin McCarthy',
        party: 'R' as const,
        state: 'CA',
        chamber: 'House' as const,
        committee: ['Financial Services'],
        lastTradeDate: '2024-01-08',
        totalTrades: 19
      },
      {
        id: '5',
        name: 'Alexandria Ocasio-Cortez',
        party: 'D' as const,
        state: 'NY',
        chamber: 'House' as const,
        committee: ['Oversight', 'Financial Services'],
        lastTradeDate: '2024-01-05',
        totalTrades: 12
      }
    ];

    return mockPeople;
  }

  private generateMockTrades(congressPersonId: string, limit: number): CongressionalTrade[] {
    const mockTrades: CongressionalTrade[] = [];
    const companies = ['AAPL', 'MSFT', 'GOOGL', 'TSLA', 'META', 'NVDA', 'JPM', 'JNJ', 'V', 'PG'];
    const companyNames = ['Apple Inc.', 'Microsoft Corp.', 'Alphabet Inc.', 'Tesla Inc.', 'Meta Platforms Inc.', 'NVIDIA Corp.', 'JPMorgan Chase & Co.', 'Johnson & Johnson', 'Visa Inc.', 'Procter & Gamble Co.'];
    
    for (let i = 0; i < limit; i++) {
      const companyIndex = Math.floor(Math.random() * companies.length);
      const tradeType = Math.random() > 0.5 ? 'buy' : 'sell';
      const amountRanges = ['1,001-15,000', '15,001-50,000', '50,001-100,000', '100,001-250,000', '250,001-500,000', '500,001-1,000,000', '1,000,001+'];
      const amountRange = amountRanges[Math.floor(Math.random() * amountRanges.length)];
      
      mockTrades.push({
        id: `trade-${congressPersonId}-${i}`,
        congressPersonId,
        congressPersonName: 'Mock Congressperson',
        ticker: companies[companyIndex],
        companyName: companyNames[companyIndex],
        tradeType: tradeType as 'buy' | 'sell',
        amount: Math.floor(Math.random() * 1000000) + 1000,
        amountRange: amountRange as any,
        tradeDate: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
        disclosureDate: new Date(Date.now() - Math.random() * 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
        owner: Math.random() > 0.7 ? 'spouse' : 'self',
        sector: 'Technology',
        industry: 'Software'
      });
    }

    return mockTrades.sort((a, b) => new Date(b.tradeDate).getTime() - new Date(a.tradeDate).getTime());
  }

  private generateMockRecentTrades(limit: number): CongressionalTrade[] {
    const mockTrades: CongressionalTrade[] = [];
    const congressPeople = this.generateMockCongressPeople();
    
    for (let i = 0; i < limit; i++) {
      const congressPerson = congressPeople[Math.floor(Math.random() * congressPeople.length)];
      const trades = this.generateMockTrades(congressPerson.id, 1);
      mockTrades.push({
        ...trades[0],
        congressPersonName: congressPerson.name
      });
    }

    return mockTrades.sort((a, b) => new Date(b.tradeDate).getTime() - new Date(a.tradeDate).getTime());
  }

  private generateMockTradingAnalytics(): TradingAnalytics {
    return {
      totalTrades: 1250,
      totalVolume: 45000000,
      mostTradedStocks: [
        { ticker: 'AAPL', count: 45, volume: 2500000 },
        { ticker: 'MSFT', count: 38, volume: 2100000 },
        { ticker: 'GOOGL', count: 32, volume: 1800000 },
        { ticker: 'TSLA', count: 28, volume: 1500000 },
        { ticker: 'META', count: 25, volume: 1200000 }
      ],
      sectorBreakdown: [
        { sector: 'Technology', count: 45, volume: 2500000 },
        { sector: 'Healthcare', count: 38, volume: 2100000 },
        { sector: 'Financial', count: 32, volume: 1800000 },
        { sector: 'Energy', count: 28, volume: 1500000 },
        { sector: 'Consumer', count: 25, volume: 1200000 }
      ],
      partyBreakdown: [
        { party: 'D', count: 650, volume: 23000000 },
        { party: 'R', count: 550, volume: 20000000 },
        { party: 'I', count: 50, volume: 2000000 }
      ],
      monthlyTrends: [
        { month: 'Jan 2024', count: 120, volume: 4500000 },
        { month: 'Dec 2023', count: 95, volume: 3800000 },
        { month: 'Nov 2023', count: 110, volume: 4200000 },
        { month: 'Oct 2023', count: 105, volume: 4000000 }
      ]
    };
  }

  // Get API status
  getApiStatus() {
    return {
      quiver: {
        configured: true,
        apiKey: API_CONFIG.QUIVER.API_KEY ? 'Set' : 'Not Set',
        baseUrl: API_CONFIG.QUIVER.BASE_URL,
        status: this.useMockData ? 'Using Mock Data' : 'Active - Real API calls enabled'
      },
      mockData: {
        enabled: this.useMockData,
        available: true,
        note: 'Available as fallback if API calls fail'
      }
    };
  }
}

export const quiverApiService = new QuiverApiService();
