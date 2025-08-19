import axios, { AxiosInstance } from 'axios';

// Plaid API Service for Financial Data Integration
// Using sandbox environment for testing

// API Configuration
const PLAID_CONFIG = {
  CLIENT_ID: '689cbb13c94f320025624ac0',
  SANDBOX_SECRET: 'bd9b488d967507564c0aa646d9a852',
  BASE_URL: 'https://sandbox.plaid.com',
  ENVIRONMENT: 'sandbox'
};

// Plaid API Types
export interface PlaidAccount {
  id: string;
  name: string;
  mask: string;
  type: 'depository' | 'credit' | 'loan' | 'investment' | 'other';
  subtype: string;
  balances: {
    available: number | null;
    current: number | null;
    limit: number | null;
    iso_currency_code: string;
    unofficial_currency_code: string | null;
  };
}

export interface PlaidTransaction {
  id: string;
  account_id: string;
  amount: number;
  date: string;
  name: string;
  merchant_name?: string;
  category: string[];
  category_id: string;
  pending: boolean;
  account_owner?: string;
  iso_currency_code: string;
  unofficial_currency_code?: string;
  payment_channel: string;
  transaction_type: string;
}

export interface PlaidBalance {
  accounts: PlaidAccount[];
  item: {
    available_products: string[];
    billed_products: string[];
    error: any;
    institution_id: string;
    item_id: string;
    webhook: string;
  };
  request_id: string;
}

export interface PlaidTransactions {
  accounts: PlaidAccount[];
  transactions: PlaidTransaction[];
  total_transactions: number;
  request_id: string;
}

export interface PlaidInstitution {
  institution_id: string;
  name: string;
  products: string[];
  country_codes: string[];
  url?: string;
  primary_color?: string;
  logo?: string;
  routing_numbers?: string[];
  oauth?: boolean;
}

export interface PlaidLinkToken {
  link_token: string;
  expiration: string;
  request_id: string;
}

export interface PlaidAccessToken {
  access_token: string;
  item_id: string;
  request_id: string;
}

export interface PlaidError {
  error_type: string;
  error_code: string;
  error_message: string;
  display_message?: string;
  request_id?: string;
  causes?: any[];
  documentation_url?: string;
  suggested_action?: string;
}

// Mock data for testing when Plaid API is not available
class PlaidMockDataGenerator {
  generateAccount(): PlaidAccount {
    const accountTypes = ['depository', 'credit', 'loan', 'investment'];
    const accountSubtypes = ['checking', 'savings', 'credit card', 'mortgage', 'student loan'];
    
    return {
      id: `mock_account_${Math.random().toString(36).substr(2, 9)}`,
      name: `Mock ${accountSubtypes[Math.floor(Math.random() * accountSubtypes.length)]} Account`,
      mask: Math.floor(Math.random() * 10000).toString().padStart(4, '0'),
      type: accountTypes[Math.floor(Math.random() * accountTypes.length)] as any,
      subtype: accountSubtypes[Math.floor(Math.random() * accountSubtypes.length)],
      balances: {
        available: Math.random() * 10000,
        current: Math.random() * 10000,
        limit: Math.random() * 50000,
        iso_currency_code: 'USD',
        unofficial_currency_code: null
      }
    };
  }

  generateTransaction(): PlaidTransaction {
    const categories = [
      ['Food and Drink', 'Restaurants'],
      ['Transportation', 'Public Transit'],
      ['Shopping', 'Online Shopping'],
      ['Bills and Utilities', 'Electricity'],
      ['Entertainment', 'Movies']
    ];
    
    return {
      id: `mock_transaction_${Math.random().toString(36).substr(2, 9)}`,
      account_id: `mock_account_${Math.floor(Math.random() * 3)}`,
      amount: (Math.random() * 200 - 100).toFixed(2),
      date: new Date(Date.now() - Math.random() * 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
      name: `Mock Transaction ${Math.floor(Math.random() * 1000)}`,
      merchant_name: `Mock Merchant ${Math.floor(Math.random() * 100)}`,
      category: categories[Math.floor(Math.random() * categories.length)],
      category_id: `mock_category_${Math.floor(Math.random() * 1000)}`,
      pending: Math.random() > 0.8,
      account_owner: 'Mock User',
      iso_currency_code: 'USD',
      unofficial_currency_code: undefined,
      payment_channel: Math.random() > 0.5 ? 'online' : 'in store',
      transaction_type: Math.random() > 0.5 ? 'place' : 'special'
    };
  }

  generateBalance(): PlaidBalance {
    return {
      accounts: Array.from({ length: 3 }, () => this.generateAccount()),
      item: {
        available_products: ['balance', 'transactions'],
        billed_products: [],
        error: null,
        institution_id: 'mock_institution',
        item_id: 'mock_item',
        webhook: ''
      },
      request_id: `mock_request_${Math.random().toString(36).substr(2, 9)}`
    };
  }

  generateTransactions(): PlaidTransactions {
    return {
      accounts: Array.from({ length: 2 }, () => this.generateAccount()),
      transactions: Array.from({ length: 10 }, () => this.generateTransaction()),
      total_transactions: 10,
      request_id: `mock_request_${Math.random().toString(36).substr(2, 9)}`
    };
  }
}

// Main Plaid API Service
export class PlaidApiService {
  private axiosInstance: AxiosInstance;
  private mockDataGenerator: PlaidMockDataGenerator;
  private useMockData: boolean = false;
  private accessToken: string | null = null;

  constructor() {
    this.axiosInstance = axios.create({
      baseURL: PLAID_CONFIG.BASE_URL,
      headers: {
        'Content-Type': 'application/json',
        'PLAID-CLIENT-ID': PLAID_CONFIG.CLIENT_ID,
        'PLAID-SECRET': PLAID_CONFIG.SANDBOX_SECRET
      }
    });

    this.mockDataGenerator = new PlaidMockDataGenerator();
    
    // Check if we have valid Plaid credentials
    if (!PLAID_CONFIG.CLIENT_ID || !PLAID_CONFIG.SANDBOX_SECRET) {
      console.warn('Plaid credentials not configured, using mock data');
      this.useMockData = true;
    }
  }

  // Create a link token for Plaid Link
  async createLinkToken(userId: string): Promise<PlaidLinkToken> {
    if (this.useMockData) {
      return {
        link_token: `mock_link_token_${Math.random().toString(36).substr(2, 9)}`,
        expiration: new Date(Date.now() + 24 * 60 * 60 * 1000).toISOString(),
        request_id: `mock_request_${Math.random().toString(36).substr(2, 9)}`
      };
    }

    try {
      const response = await this.axiosInstance.post('/link/token/create', {
        user: { client_user_id: userId },
        client_name: 'Ezana Financial App',
        products: ['transactions', 'auth', 'balance'],
        country_codes: ['US'],
        language: 'en'
      });
      return response.data;
    } catch (error) {
      console.error('Error creating link token:', error);
      throw error;
    }
  }

  // Exchange public token for access token
  async exchangePublicToken(publicToken: string): Promise<PlaidAccessToken> {
    if (this.useMockData) {
      return {
        access_token: `mock_access_token_${Math.random().toString(36).substr(2, 9)}`,
        item_id: `mock_item_${Math.random().toString(36).substr(2, 9)}`,
        request_id: `mock_request_${Math.random().toString(36).substr(2, 9)}`
      };
    }

    try {
      const response = await this.axiosInstance.post('/item/public_token/exchange', {
        public_token: publicToken
      });
      
      this.accessToken = response.data.access_token;
      return response.data;
    } catch (error) {
      console.error('Error exchanging public token:', error);
      throw error;
    }
  }

  // Get account balances
  async getBalance(): Promise<PlaidBalance> {
    if (this.useMockData || !this.accessToken) {
      return this.mockDataGenerator.generateBalance();
    }

    try {
      const response = await this.axiosInstance.post('/accounts/balance/get', {
        access_token: this.accessToken
      });
      return response.data;
    } catch (error) {
      console.error('Error getting balance:', error);
      // Fallback to mock data
      return this.mockDataGenerator.generateBalance();
    }
  }

  // Get transactions
  async getTransactions(startDate: string, endDate: string, accountIds?: string[]): Promise<PlaidTransactions> {
    if (this.useMockData || !this.accessToken) {
      return this.mockDataGenerator.generateTransactions();
    }

    try {
      const response = await this.axiosInstance.post('/transactions/get', {
        access_token: this.accessToken,
        start_date: startDate,
        end_date: endDate,
        options: {
          account_ids: accountIds
        }
      });
      return response.data;
    } catch (error) {
      console.error('Error getting transactions:', error);
      // Fallback to mock data
      return this.mockDataGenerator.generateTransactions();
    }
  }

  // Get institution information
  async getInstitution(institutionId: string): Promise<PlaidInstitution> {
    if (this.useMockData) {
      return {
        institution_id: institutionId,
        name: 'Mock Bank',
        products: ['auth', 'balance', 'transactions'],
        country_codes: ['US'],
        url: 'https://mockbank.com',
        primary_color: '#0066cc',
        logo: 'https://mockbank.com/logo.png',
        routing_numbers: ['123456789'],
        oauth: false
      };
    }

    try {
      const response = await this.axiosInstance.post('/institutions/get_by_id', {
        institution_id: institutionId,
        country_codes: ['US'],
        options: {
          include_optional_metadata: true
        }
      });
      return response.data.institution;
    } catch (error) {
      console.error('Error getting institution:', error);
      throw error;
    }
  }

  // Set access token manually (for testing)
  setAccessToken(token: string) {
    this.accessToken = token;
  }

  // Get current access token
  getAccessToken(): string | null {
    return this.accessToken;
  }

  // Toggle between mock and real data
  toggleMockData() {
    this.useMockData = !this.useMockData;
    console.log(`Plaid API using ${this.useMockData ? 'mock' : 'real'} data`);
  }

  // Get API status
  getApiStatus() {
    return {
      configured: !!(PLAID_CONFIG.CLIENT_ID && PLAID_CONFIG.SANDBOX_SECRET),
      environment: PLAID_CONFIG.ENVIRONMENT,
      baseUrl: PLAID_CONFIG.BASE_URL,
      usingMockData: this.useMockData,
      hasAccessToken: !!this.accessToken
    };
  }

  // Test connection
  async testConnection(): Promise<boolean> {
    try {
      if (this.useMockData) {
        return true;
      }
      
      // Try to create a link token as a connection test
      await this.createLinkToken('test_user');
      return true;
    } catch (error) {
      console.error('Plaid API connection test failed:', error);
      return false;
    }
  }
}

// Create and export service instance
export const plaidApiService = new PlaidApiService();

// Export for direct access
export default plaidApiService;
