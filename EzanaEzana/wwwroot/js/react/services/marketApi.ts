import axios, { AxiosInstance } from 'axios';

// Market API Service - Now with Finnhub Integration + Mock Data Fallbacks
// We'll gradually add real API integrations while keeping mock data as backup

// API Configuration
const API_CONFIG = {
  FINNHUB: {
    BASE_URL: 'https://finnhub.io/api/v1',
    API_KEY: 'd2e9vqhr01qr1ro8tkn0d2e9vqhr01qr1ro8tkng',
    FREE_TIER_LIMIT: 60 // requests per minute
  }
};

// Types for market data
export interface StockQuote {
  symbol: string;
  name: string;
  price: number;
  change: number;
  changePercent: number;
  volume: number;
  marketCap: number;
  pe: number;
  dividend: number;
  dividendYield: number;
  sector: string;
  industry: string;
}

export interface CompanyInfo {
  symbol: string;
  name: string;
  sector: string;
  industry: string;
  description: string;
  employees: number;
  website: string;
  ceo: string;
  marketCap: number;
  pe: number;
  dividend: number;
  dividendYield: number;
}

export interface MarketNews {
  id: string;
  title: string;
  summary: string;
  url: string;
  publishedAt: string;
  source: string;
  sentiment: 'positive' | 'negative' | 'neutral';
  category: 'general' | 'earnings' | 'merger' | 'crypto' | 'forex';
}

export interface HistoricalData {
  date: string;
  open: number;
  high: number;
  low: number;
  close: number;
  volume: number;
}

export interface CryptoData {
  id: string;
  symbol: string;
  name: string;
  currentPrice: number;
  marketCap: number;
  volume24h: number;
  change24h: number;
  changePercent24h: number;
}

export interface ForexRate {
  fromCurrency: string;
  toCurrency: string;
  rate: number;
  change: number;
  changePercent: number;
  lastUpdated: string;
}

export interface EconomicIndicator {
  name: string;
  value: number;
  unit: string;
  date: string;
  previousValue: number;
  change: number;
  changePercent: number;
}

// Mock data generators (kept as fallbacks)
class MockDataGenerator {
  private static generateRandomPrice(basePrice: number, volatility: number = 0.02): number {
    const change = (Math.random() - 0.5) * volatility * basePrice;
    return Math.round((basePrice + change) * 100) / 100;
  }

  private static generateRandomChange(basePrice: number, volatility: number = 0.03): number {
    const change = (Math.random() - 0.5) * volatility * basePrice;
    return Math.round(change * 100) / 100;
  }

  static generateStockQuote(symbol: string, basePrice: number): StockQuote {
    const price = this.generateRandomPrice(basePrice);
    const change = this.generateRandomChange(basePrice);
    const changePercent = (change / (price - change)) * 100;
    
    return {
      symbol,
      name: this.getCompanyName(symbol),
      price,
      change,
      changePercent: Math.round(changePercent * 100) / 100,
      volume: Math.floor(Math.random() * 10000000) + 1000000,
      marketCap: Math.floor(Math.random() * 100000000000) + 1000000000,
      pe: Math.round((Math.random() * 30 + 10) * 100) / 100,
      dividend: Math.round((Math.random() * 5) * 100) / 100,
      dividendYield: Math.round((Math.random() * 3) * 100) / 100,
      sector: this.getRandomSector(),
      industry: this.getRandomIndustry()
    };
  }

  static generateHistoricalData(symbol: string, days: number = 30): HistoricalData[] {
    const data: HistoricalData[] = [];
    let basePrice = 100 + Math.random() * 200; // Random starting price
    
    for (let i = days - 1; i >= 0; i--) {
      const date = new Date();
      date.setDate(date.getDate() - i);
      
      const open = basePrice;
      const high = open + Math.random() * 10;
      const low = open - Math.random() * 10;
      const close = this.generateRandomPrice(open, 0.02);
      const volume = Math.floor(Math.random() * 10000000) + 1000000;
      
      data.push({
        date: date.toISOString().split('T')[0],
        open: Math.round(open * 100) / 100,
        high: Math.round(high * 100) / 100,
        low: Math.round(low * 100) / 100,
        close: Math.round(close * 100) / 100,
        volume
      });
      
      basePrice = close; // Next day starts with previous close
    }
    
    return data;
  }

  static generateMarketNews(): MarketNews[] {
    const newsTemplates = [
      {
        title: "Tech Stocks Rally on Strong Earnings Reports",
        summary: "Major technology companies exceeded quarterly expectations, driving market gains.",
        category: "earnings" as const,
        sentiment: "positive" as const
      },
      {
        title: "Federal Reserve Signals Potential Rate Changes",
        summary: "Central bank officials hint at possible monetary policy adjustments.",
        category: "general" as const,
        sentiment: "neutral" as const
      },
      {
        title: "Crypto Market Shows Signs of Recovery",
        summary: "Digital assets gain momentum after recent market correction.",
        category: "crypto" as const,
        sentiment: "positive" as const
      },
      {
        title: "Merger Talks Between Major Banks Surface",
        summary: "Industry consolidation discussions could reshape financial landscape.",
        category: "merger" as const,
        sentiment: "neutral" as const
      },
      {
        title: "Economic Data Shows Mixed Signals",
        summary: "Inflation concerns balanced against strong employment numbers.",
        category: "general" as const,
        sentiment: "negative" as const
      }
    ];

    return newsTemplates.map((template, index) => ({
      id: `news-${index + 1}`,
      title: template.title,
      summary: template.summary,
      url: `https://example.com/news/${index + 1}`,
      publishedAt: new Date(Date.now() - Math.random() * 86400000).toISOString(), // Random time in last 24h
      source: this.getRandomSource(),
      sentiment: template.sentiment,
      category: template.category
    }));
  }

  static generateCryptoData(symbol: string): CryptoData {
    const basePrice = 1000 + Math.random() * 50000;
    const currentPrice = this.generateRandomPrice(basePrice, 0.05);
    const change24h = this.generateRandomChange(basePrice, 0.08);
    
    return {
      id: symbol.toLowerCase(),
      symbol: symbol.toUpperCase(),
      name: this.getCryptoName(symbol),
      currentPrice: Math.round(currentPrice * 100) / 100,
      marketCap: Math.floor(Math.random() * 1000000000000) + 1000000000,
      volume24h: Math.floor(Math.random() * 1000000000) + 10000000,
      change24h: Math.round(change24h * 100) / 100,
      changePercent24h: Math.round((change24h / (currentPrice - change24h)) * 10000) / 100
    };
  }

  static generateForexRate(from: string, to: string): ForexRate {
    const baseRate = 0.5 + Math.random() * 2;
    const change = this.generateRandomChange(baseRate, 0.01);
    
    return {
      fromCurrency: from,
      toCurrency: to,
      rate: Math.round(baseRate * 10000) / 10000,
      change: Math.round(change * 10000) / 10000,
      changePercent: Math.round((change / (baseRate - change)) * 10000) / 100,
      lastUpdated: new Date().toISOString()
    };
  }

  static generateEconomicIndicator(name: string): EconomicIndicator {
    const value = 100 + Math.random() * 50;
    const previousValue = value + this.generateRandomChange(value, 0.05);
    const change = value - previousValue;
    
    return {
      name,
      value: Math.round(value * 100) / 100,
      unit: this.getUnitForIndicator(name),
      date: new Date().toISOString().split('T')[0],
      previousValue: Math.round(previousValue * 100) / 100,
      change: Math.round(change * 100) / 100,
      changePercent: Math.round((change / previousValue) * 10000) / 100
    };
  }

  // Helper methods - Made public so they can be accessed
  static getCompanyName(symbol: string): string {
    const names: { [key: string]: string } = {
      'AAPL': 'Apple Inc.',
      'MSFT': 'Microsoft Corporation',
      'GOOGL': 'Alphabet Inc.',
      'AMZN': 'Amazon.com Inc.',
      'TSLA': 'Tesla Inc.',
      'META': 'Meta Platforms Inc.',
      'NVDA': 'NVIDIA Corporation',
      'JPM': 'JPMorgan Chase & Co.',
      'JNJ': 'Johnson & Johnson',
      'V': 'Visa Inc.'
    };
    return names[symbol] || `${symbol} Corporation`;
  }

  static getCryptoName(symbol: string): string {
    const names: { [key: string]: string } = {
      'BTC': 'Bitcoin',
      'ETH': 'Ethereum',
      'USDT': 'Tether',
      'BNB': 'Binance Coin',
      'ADA': 'Cardano',
      'SOL': 'Solana',
      'DOT': 'Polkadot',
      'DOGE': 'Dogecoin',
      'AVAX': 'Avalanche',
      'MATIC': 'Polygon'
    };
    return names[symbol] || symbol;
  }

  private static getRandomSector(): string {
    const sectors = ['Technology', 'Healthcare', 'Financial', 'Consumer Discretionary', 'Industrial', 'Energy', 'Materials', 'Utilities', 'Real Estate', 'Communication Services'];
    return sectors[Math.floor(Math.random() * sectors.length)];
  }

  private static getRandomIndustry(): string {
    const industries = ['Software', 'Pharmaceuticals', 'Banking', 'Retail', 'Manufacturing', 'Oil & Gas', 'Mining', 'Electric Utilities', 'Real Estate Investment Trusts', 'Media'];
    return industries[Math.floor(Math.random() * industries.length)];
  }

  private static getRandomSource(): string {
    const sources = ['Reuters', 'Bloomberg', 'CNBC', 'MarketWatch', 'Yahoo Finance', 'Financial Times', 'Wall Street Journal', 'Forbes', 'Investing.com', 'Seeking Alpha'];
    return sources[Math.floor(Math.random() * sources.length)];
  }

  private static getUnitForIndicator(name: string): string {
    const units: { [key: string]: string } = {
      'GDP Growth': '%',
      'Inflation Rate': '%',
      'Unemployment Rate': '%',
      'Interest Rate': '%',
      'Consumer Confidence': 'Index',
      'Housing Starts': 'Thousands',
      'Retail Sales': 'Billions USD',
      'Industrial Production': 'Index'
    };
    return units[name] || 'Units';
  }
}

// Main Market API Service with Finnhub Integration
class MarketApiService {
  private finnhub: AxiosInstance;
  private popularStocks = ['AAPL', 'MSFT', 'GOOGL', 'AMZN', 'TSLA', 'META', 'NVDA', 'JPM', 'JNJ', 'V'];
  private popularCrypto = ['BTC', 'ETH', 'USDT', 'BNB', 'ADA', 'SOL', 'DOT', 'DOGE', 'AVAX', 'MATIC'];
  private popularForex = ['EUR/USD', 'GBP/USD', 'USD/JPY', 'USD/CHF', 'AUD/USD', 'USD/CAD', 'NZD/USD'];
  private useMockData: boolean = false; // Now using real Finnhub API calls

  constructor() {
    this.finnhub = axios.create({
      baseURL: API_CONFIG.FINNHUB.BASE_URL,
      timeout: 10000
    });
    console.log('Market API Service initialized with Finnhub API integration');
  }

  // Toggle between real API and mock data
  setUseMockData(useMock: boolean) {
    this.useMockData = useMock;
    console.log(`Market API: ${useMock ? 'Using mock data' : 'Using Finnhub API'}`);
  }

  // Stock methods with Finnhub integration
  async getStockQuote(symbol: string): Promise<StockQuote> {
    if (this.useMockData) {
      await this.simulateDelay();
      const basePrice = 50 + Math.random() * 200;
      return MockDataGenerator.generateStockQuote(symbol, basePrice);
    }

    try {
      console.log(`Fetching real stock quote for ${symbol} from Finnhub...`);
      const response = await this.finnhub.get('/quote', {
        params: {
          symbol: symbol.toUpperCase(),
          token: API_CONFIG.FINNHUB.API_KEY
        }
      });

      const data = response.data;
      if (!data.c) {
        throw new Error(`No data found for symbol: ${symbol}`);
      }

      // Get company profile for additional info
      const profile = await this.getCompanyProfile(symbol);
      
      return {
        symbol: symbol.toUpperCase(),
        name: profile.name || symbol,
        price: data.c, // Current price
        change: data.d, // Change
        changePercent: data.dp, // Change percent
        volume: data.v || 0, // Volume
        marketCap: profile.marketCap || 0,
        pe: profile.pe || 0,
        dividend: profile.dividend || 0,
        dividendYield: profile.dividendYield || 0,
        sector: profile.sector || 'Unknown',
        industry: profile.industry || 'Unknown'
      };
    } catch (error) {
      console.error(`Error fetching stock quote for ${symbol} from Finnhub:`, error);
      console.log(`Falling back to mock data for ${symbol}`);
      
      // Fallback to mock data
      const basePrice = 50 + Math.random() * 200;
      return MockDataGenerator.generateStockQuote(symbol, basePrice);
    }
  }

  async getCompanyProfile(symbol: string): Promise<CompanyInfo> {
    try {
      const response = await this.finnhub.get('/stock/profile2', {
        params: {
          symbol: symbol.toUpperCase(),
          token: API_CONFIG.FINNHUB.API_KEY
        }
      });

      const data = response.data;
      return {
        symbol: data.ticker || symbol,
        name: data.name || symbol,
        sector: data.finnhubIndustry || 'Unknown',
        industry: data.finnhubIndustry || 'Unknown',
        description: data.description || 'No description available',
        employees: data.employeeTotal || 0,
        website: data.weburl || '',
        ceo: data.ceo || 'Unknown',
        marketCap: data.marketCapitalization || 0,
        pe: data.peRatio || 0,
        dividend: data.dividendYield || 0,
        dividendYield: data.dividendYield || 0
      };
    } catch (error) {
      console.error(`Error fetching company profile for ${symbol}:`, error);
      return {
        symbol,
        name: MockDataGenerator.getCompanyName(symbol),
        sector: 'Unknown',
        industry: 'Unknown',
        description: 'No description available',
        employees: 0,
        website: '',
        ceo: 'Unknown',
        marketCap: 0,
        pe: 0,
        dividend: 0,
        dividendYield: 0
      };
    }
  }

  async getMultipleStockQuotes(symbols: string[]): Promise<StockQuote[]> {
    const promises = symbols.map(symbol => this.getStockQuote(symbol));
    return Promise.all(promises);
  }

  async getPopularStocks(): Promise<StockQuote[]> {
    return this.getMultipleStockQuotes(this.popularStocks);
  }

  async searchStocks(query: string): Promise<{ symbol: string; name: string }[]> {
    await this.simulateDelay();
    
    // Filter popular stocks based on query
    const matches = this.popularStocks
      .filter(symbol => symbol.toLowerCase().includes(query.toLowerCase()))
      .map(symbol => ({
        symbol,
        name: MockDataGenerator.getCompanyName(symbol)
      }));
    
    return matches.slice(0, 10);
  }

  async getHistoricalData(symbol: string, interval: 'daily' | 'weekly' | 'monthly' = 'daily'): Promise<HistoricalData[]> {
    await this.simulateDelay();
    
    const days = interval === 'daily' ? 30 : interval === 'weekly' ? 52 : 12;
    return MockDataGenerator.generateHistoricalData(symbol, days);
  }

  // News methods (using mock data for now)
  async getMarketNews(category?: 'general' | 'earnings' | 'merger' | 'crypto' | 'forex'): Promise<MarketNews[]> {
    await this.simulateDelay();
    
    let news = MockDataGenerator.generateMarketNews();
    if (category) {
      news = news.filter(item => item.category === category);
    }
    
    return news;
  }

  // Crypto methods (using mock data for now)
  async getCryptoData(symbol: string): Promise<CryptoData> {
    await this.simulateDelay();
    return MockDataGenerator.generateCryptoData(symbol);
  }

  async getPopularCrypto(): Promise<CryptoData[]> {
    const promises = this.popularCrypto.map(symbol => this.getCryptoData(symbol));
    return Promise.all(promises);
  }

  // Forex methods (using mock data for now)
  async getForexRate(from: string, to: string): Promise<ForexRate> {
    await this.simulateDelay();
    return MockDataGenerator.generateForexRate(from, to);
  }

  async getPopularForex(): Promise<ForexRate[]> {
    const promises = this.popularForex.map(pair => {
      const [from, to] = pair.split('/');
      return this.getForexRate(from, to);
    });
    return Promise.all(promises);
  }

  // Economic indicators (using mock data for now)
  async getEconomicIndicators(): Promise<EconomicIndicator[]> {
    await this.simulateDelay();
    
    const indicators = [
      'GDP Growth',
      'Inflation Rate',
      'Unemployment Rate',
      'Interest Rate',
      'Consumer Confidence',
      'Housing Starts',
      'Retail Sales',
      'Industrial Production'
    ];
    
    return indicators.map(name => MockDataGenerator.generateEconomicIndicator(name));
  }

  // Utility methods
  private async simulateDelay(min: number = 100, max: number = 500): Promise<void> {
    const delay = Math.random() * (max - min) + min;
    return new Promise(resolve => setTimeout(resolve, delay));
  }

  // Get available symbols for testing
  getAvailableSymbols() {
    return {
      stocks: this.popularStocks,
      crypto: this.popularCrypto,
      forex: this.popularForex
    };
  }

  // Get API status
  getApiStatus() {
    return {
      finnhub: {
        configured: true,
        apiKey: API_CONFIG.FINNHUB.API_KEY ? 'Set' : 'Not Set',
        baseUrl: API_CONFIG.FINNHUB.BASE_URL,
        status: 'Active - Real API calls enabled'
      },
      mockData: {
        enabled: this.useMockData,
        available: true,
        note: 'Available as fallback if API calls fail'
      }
    };
  }
}

export const marketApiService = new MarketApiService();
