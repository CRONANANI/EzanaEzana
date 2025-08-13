import axios, { AxiosInstance, AxiosResponse, AxiosError, AxiosRequestConfig } from 'axios';
import { toast } from 'react-hot-toast';

// Types
export interface ApiResponse<T = any> {
  data: T;
  message?: string;
  success: boolean;
  errors?: string[];
}

export interface PaginatedResponse<T> extends ApiResponse<T[]> {
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  };
}

export interface InvestmentData {
  id: string;
  symbol: string;
  name: string;
  price: number;
  change: number;
  changePercent: number;
  volume: number;
  marketCap: number;
  sector: string;
  lastUpdated: string;
}

export interface PortfolioData {
  id: string;
  name: string;
  description?: string;
  totalValue: number;
  totalChange: number;
  totalChangePercent: number;
  createdAt: string;
  updatedAt: string;
}

export interface UserProfile {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  avatar?: string;
  preferences: {
    theme: 'light' | 'dark' | 'system';
    notifications: boolean;
    language: string;
  };
}

// API Configuration
const API_BASE_URL = process.env.NODE_ENV === 'production' 
  ? 'https://api.ezana.com' 
  : 'https://localhost:7001';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    this.setupInterceptors();
  }

  private setupInterceptors() {
    // Request interceptor
    this.api.interceptors.request.use(
      (config: AxiosRequestConfig) => {
        const token = this.getAuthToken();
        if (token && config.headers) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error: any) => {
        return Promise.reject(error);
      }
    );

    // Response interceptor
    this.api.interceptors.response.use(
      (response: AxiosResponse) => {
        return response;
      },
      (error: AxiosError) => {
        this.handleApiError(error);
        return Promise.reject(error);
      }
    );
  }

  private getAuthToken(): string | null {
    return localStorage.getItem('auth_token');
  }

  private handleApiError(error: AxiosError) {
    if (error.response) {
      const { status, data } = error.response;
      
      switch (status) {
        case 401:
          toast.error('Authentication required. Please log in again.');
          this.handleUnauthorized();
          break;
        case 403:
          toast.error('Access denied. You don\'t have permission for this action.');
          break;
        case 404:
          toast.error('Resource not found.');
          break;
        case 422:
          toast.error('Validation error. Please check your input.');
          break;
        case 500:
          toast.error('Server error. Please try again later.');
          break;
        default:
          toast.error('An unexpected error occurred.');
      }
    } else if (error.request) {
      toast.error('Network error. Please check your connection.');
    } else {
      toast.error('An error occurred while processing your request.');
    }
  }

  private handleUnauthorized() {
    localStorage.removeItem('auth_token');
    window.location.href = '/Account/Login';
  }

  // Generic request methods
  async get<T>(url: string, params?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.api.get<ApiResponse<T>>(url, { params });
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  async post<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.api.post<ApiResponse<T>>(url, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  async put<T>(url: string, data?: any): Promise<ApiResponse<T>> {
    try {
      const response = await this.api.put<ApiResponse<T>>(url, data);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  async delete<T>(url: string): Promise<ApiResponse<T>> {
    try {
      const response = await this.api.delete<ApiResponse<T>>(url);
      return response.data;
    } catch (error) {
      throw error;
    }
  }

  // Investment endpoints
  async getInvestments(params?: {
    page?: number;
    pageSize?: number;
    sector?: string;
    search?: string;
  }): Promise<PaginatedResponse<InvestmentData>> {
    const response = await this.get<InvestmentData[]>('/api/investments', params);
    // Mock pagination for now - replace with actual API response structure
    return {
      ...response,
      pagination: {
        page: params?.page || 1,
        pageSize: params?.pageSize || 10,
        totalCount: response.data.length,
        totalPages: Math.ceil(response.data.length / (params?.pageSize || 10))
      }
    };
  }

  async getInvestmentById(id: string): Promise<ApiResponse<InvestmentData>> {
    return this.get<InvestmentData>(`/api/investments/${id}`);
  }

  async searchInvestments(query: string): Promise<ApiResponse<InvestmentData[]>> {
    return this.get<InvestmentData[]>('/api/investments/search', { q: query });
  }

  // Portfolio endpoints
  async getPortfolios(): Promise<ApiResponse<PortfolioData[]>> {
    return this.get<PortfolioData[]>('/api/portfolios');
  }

  async getPortfolioById(id: string): Promise<ApiResponse<PortfolioData>> {
    return this.get<PortfolioData>(`/api/portfolios/${id}`);
  }

  async createPortfolio(data: {
    name: string;
    description?: string;
  }): Promise<ApiResponse<PortfolioData>> {
    return this.post<PortfolioData>('/api/portfolios', data);
  }

  async updatePortfolio(id: string, data: Partial<PortfolioData>): Promise<ApiResponse<PortfolioData>> {
    return this.put<PortfolioData>(`/api/portfolios/${id}`, data);
  }

  async deletePortfolio(id: string): Promise<ApiResponse<void>> {
    return this.delete<void>(`/api/portfolios/${id}`);
  }

  // User endpoints
  async getUserProfile(): Promise<ApiResponse<UserProfile>> {
    return this.get<UserProfile>('/api/user/profile');
  }

  async updateUserProfile(data: Partial<UserProfile>): Promise<ApiResponse<UserProfile>> {
    return this.put<UserProfile>('/api/user/profile', data);
  }

  async updateUserPreferences(preferences: Partial<UserProfile['preferences']>): Promise<ApiResponse<UserProfile>> {
    return this.put<UserProfile>('/api/user/preferences', preferences);
  }

  // Authentication endpoints
  async login(credentials: { email: string; password: string }): Promise<ApiResponse<{ token: string; user: UserProfile }>> {
    const response = await this.post<{ token: string; user: UserProfile }>('/api/auth/login', credentials);
    if (response.success && response.data.token) {
      localStorage.setItem('auth_token', response.data.token);
    }
    return response;
  }

  async logout(): Promise<void> {
    try {
      await this.post('/api/auth/logout');
    } finally {
      localStorage.removeItem('auth_token');
    }
  }

  async register(userData: {
    email: string;
    password: string;
    firstName: string;
    lastName: string;
  }): Promise<ApiResponse<{ token: string; user: UserProfile }>> {
    const response = await this.post<{ token: string; user: UserProfile }>('/api/auth/register', userData);
    if (response.success && response.data.token) {
      localStorage.setItem('auth_token', response.data.token);
    }
    return response;
  }

  // Utility methods
  isAuthenticated(): boolean {
    return !!this.getAuthToken();
  }

  getAuthHeaders(): Record<string, string> {
    const token = this.getAuthToken();
    return token ? { Authorization: `Bearer ${token}` } : {};
  }
}

// Export singleton instance
export const apiService = new ApiService();
