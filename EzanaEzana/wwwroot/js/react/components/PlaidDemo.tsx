import React, { useState, useEffect } from 'react';
import { plaidApiService } from '../services/plaidApi';

const PlaidDemo: React.FC = () => {
  const [apiStatus, setApiStatus] = useState<any>(null);
  const [demoData, setDemoData] = useState<any>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    // Get initial API status
    setApiStatus(plaidApiService.getApiStatus());
  }, []);

  const runDemo = async () => {
    setLoading(true);
    setDemoData(null);

    try {
      // Demo 1: Create a link token
      const linkToken = await plaidApiService.createLinkToken('demo_user_123');
      
      // Demo 2: Get mock balance data
      const balance = await plaidApiService.getBalance();
      
      // Demo 3: Get mock transaction data
      const endDate = new Date().toISOString().split('T')[0];
      const startDate = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
      const transactions = await plaidApiService.getTransactions(startDate, endDate);

      setDemoData({
        linkToken,
        balance,
        transactions
      });
    } catch (error) {
      console.error('Demo error:', error);
      setDemoData({ error: error.message });
    } finally {
      setLoading(false);
    }
  };

  const toggleMockData = () => {
    plaidApiService.toggleMockData();
    setApiStatus(plaidApiService.getApiStatus());
  };

  return (
    <div className="plaid-demo">
      <div className="demo-header">
        <h2>Plaid API Integration Demo</h2>
        <p>This demo shows how to integrate Plaid's financial data APIs into your React application.</p>
      </div>

      <div className="demo-status">
        <h3>API Status</h3>
        <div className="status-grid">
          <div className="status-item">
            <span className="label">Configured:</span>
            <span className={`value ${apiStatus?.configured ? 'success' : 'error'}`}>
              {apiStatus?.configured ? 'Yes' : 'No'}
            </span>
          </div>
          <div className="status-item">
            <span className="label">Environment:</span>
            <span className="value">{apiStatus?.environment || 'Unknown'}</span>
          </div>
          <div className="status-item">
            <span className="label">Data Source:</span>
            <span className={`value ${apiStatus?.usingMockData ? 'warning' : 'success'}`}>
              {apiStatus?.usingMockData ? 'Mock Data' : 'Real API'}
            </span>
          </div>
          <div className="status-item">
            <span className="label">Access Token:</span>
            <span className={`value ${apiStatus?.hasAccessToken ? 'success' : 'info'}`}>
              {apiStatus?.hasAccessToken ? 'Present' : 'None'}
            </span>
          </div>
        </div>
      </div>

      <div className="demo-controls">
        <button 
          onClick={runDemo}
          disabled={loading}
          className="demo-btn primary"
        >
          {loading ? 'Running Demo...' : 'Run Demo'}
        </button>
        
        <button 
          onClick={toggleMockData}
          className="demo-btn secondary"
        >
          Toggle Mock Data
        </button>
      </div>

      {demoData && (
        <div className="demo-results">
          <h3>Demo Results</h3>
          
          {demoData.error ? (
            <div className="error-box">
              <h4>Error</h4>
              <p>{demoData.error}</p>
            </div>
          ) : (
            <>
              <div className="result-section">
                <h4>Link Token Created</h4>
                <div className="token-box">
                  <code>{demoData.linkToken.link_token}</code>
                  <small>Expires: {new Date(demoData.linkToken.expiration).toLocaleString()}</small>
                </div>
              </div>

              <div className="result-section">
                <h4>Account Balances</h4>
                <div className="accounts-preview">
                  {demoData.balance.accounts.slice(0, 3).map((account: any) => (
                    <div key={account.id} className="account-preview">
                      <div className="account-name">{account.name}</div>
                      <div className="account-balance">
                        ${account.balances.current?.toFixed(2) || 'N/A'}
                      </div>
                      <div className="account-type">{account.type}</div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="result-section">
                <h4>Recent Transactions</h4>
                <div className="transactions-preview">
                  {demoData.transactions.transactions.slice(0, 5).map((transaction: any) => (
                    <div key={transaction.id} className="transaction-preview">
                      <div className="transaction-name">{transaction.name}</div>
                      <div className={`transaction-amount ${transaction.amount >= 0 ? 'positive' : 'negative'}`}>
                        ${Math.abs(transaction.amount).toFixed(2)}
                      </div>
                      <div className="transaction-date">{transaction.date}</div>
                    </div>
                  ))}
                </div>
              </div>
            </>
          )}
        </div>
      )}

      <div className="demo-info">
        <h3>How It Works</h3>
        <div className="info-grid">
          <div className="info-item">
            <h4>1. Configuration</h4>
            <p>The Plaid API service is configured with your sandbox credentials and automatically falls back to mock data when needed.</p>
          </div>
          <div className="info-item">
            <h4>2. Link Token</h4>
            <p>Creates a secure link token that allows users to connect their bank accounts through Plaid Link.</p>
          </div>
          <div className="info-item">
            <h4>3. Data Retrieval</h4>
            <p>Fetches account balances, transactions, and other financial data using the Plaid API endpoints.</p>
          </div>
          <div className="info-item">
            <h4>4. Mock Data</h4>
            <p>Provides realistic sample data for development and testing when the real API is not available.</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default PlaidDemo;
