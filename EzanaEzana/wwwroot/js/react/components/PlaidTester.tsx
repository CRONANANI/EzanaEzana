import React, { useState, useEffect } from 'react';
import { plaidApiService, PlaidAccount, PlaidTransaction, PlaidBalance, PlaidTransactions } from '../services/plaidApi';

interface PlaidTesterProps {
  title?: string;
}

const PlaidTester: React.FC<PlaidTesterProps> = ({ title = 'Plaid API Tester' }) => {
  const [apiStatus, setApiStatus] = useState<any>(null);
  const [balance, setBalance] = useState<PlaidBalance | null>(null);
  const [transactions, setTransactions] = useState<PlaidTransactions | null>(null);
  const [linkToken, setLinkToken] = useState<string | null>(null);
  const [accessToken, setAccessToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [testResults, setTestResults] = useState<any[]>([]);

  useEffect(() => {
    // Initialize API status
    setApiStatus(plaidApiService.getApiStatus());
    setAccessToken(plaidApiService.getAccessToken());
  }, []);

  const addTestResult = (test: string, success: boolean, data?: any, error?: any) => {
    setTestResults(prev => [...prev, {
      test,
      success,
      timestamp: new Date().toISOString(),
      data,
      error: error?.message || error
    }]);
  };

  const testApiConnection = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const result = await plaidApiService.testConnection();
      addTestResult('API Connection Test', result);
      setApiStatus(plaidApiService.getApiStatus());
    } catch (err: any) {
      addTestResult('API Connection Test', false, null, err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const testCreateLinkToken = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const result = await plaidApiService.createLinkToken('test_user_123');
      setLinkToken(result.link_token);
      addTestResult('Create Link Token', true, result);
    } catch (err: any) {
      addTestResult('Create Link Token', false, null, err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const testGetBalance = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const result = await plaidApiService.getBalance();
      setBalance(result);
      addTestResult('Get Balance', true, result);
    } catch (err: any) {
      addTestResult('Get Balance', false, null, err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const testGetTransactions = async () => {
    setLoading(true);
    setError(null);
    
    try {
      const endDate = new Date().toISOString().split('T')[0];
      const startDate = new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0];
      
      const result = await plaidApiService.getTransactions(startDate, endDate);
      setTransactions(result);
      addTestResult('Get Transactions', true, result);
    } catch (err: any) {
      addTestResult('Get Transactions', false, null, err);
      setError(err.message);
    } finally {
      setLoading(false);
    }
    }

  const testMockDataToggle = () => {
    plaidApiService.toggleMockData();
    setApiStatus(plaidApiService.getApiStatus());
    addTestResult('Toggle Mock Data', true, { usingMockData: plaidApiService.getApiStatus().usingMockData });
  };

  const testSetAccessToken = () => {
    const testToken = `test_access_token_${Math.random().toString(36).substr(2, 9)}`;
    plaidApiService.setAccessToken(testToken);
    setAccessToken(testToken);
    setApiStatus(plaidApiService.getApiStatus());
    addTestResult('Set Access Token', true, { token: testToken });
  };

  const clearTestResults = () => {
    setTestResults([]);
  };

  const formatCurrency = (amount: number | null): string => {
    if (amount === null) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString();
  };

  return (
    <div className="plaid-tester">
      <div className="plaid-tester-header">
        <h2>{title}</h2>
        <div className="api-status">
          <span className={`status-indicator ${apiStatus?.configured ? 'configured' : 'not-configured'}`}>
            {apiStatus?.configured ? 'Configured' : 'Not Configured'}
          </span>
          <span className={`status-indicator ${apiStatus?.usingMockData ? 'mock' : 'real'}`}>
            {apiStatus?.usingMockData ? 'Mock Data' : 'Real API'}
          </span>
          <span className={`status-indicator ${apiStatus?.hasAccessToken ? 'has-token' : 'no-token'}`}>
            {apiStatus?.hasAccessToken ? 'Has Token' : 'No Token'}
          </span>
        </div>
      </div>

      <div className="plaid-tester-controls">
        <h3>API Tests</h3>
        <div className="button-group">
          <button 
            onClick={testApiConnection}
            disabled={loading}
            className="btn btn-primary"
          >
            {loading ? 'Testing...' : 'Test Connection'}
          </button>
          
          <button 
            onClick={testCreateLinkToken}
            disabled={loading}
            className="btn btn-secondary"
          >
            {loading ? 'Creating...' : 'Create Link Token'}
          </button>
          
          <button 
            onClick={testGetBalance}
            disabled={loading}
            className="btn btn-secondary"
          >
            {loading ? 'Loading...' : 'Get Balance'}
          </button>
          
          <button 
            onClick={testGetTransactions}
            disabled={loading}
            className="btn btn-secondary"
          >
            {loading ? 'Loading...' : 'Get Transactions'}
          </button>
        </div>

        <div className="button-group">
          <button 
            onClick={testMockDataToggle}
            className="btn btn-warning"
          >
            Toggle Mock Data
          </button>
          
          <button 
            onClick={testSetAccessToken}
            className="btn btn-info"
          >
            Set Test Access Token
          </button>
          
          <button 
            onClick={clearTestResults}
            className="btn btn-outline"
          >
            Clear Results
          </button>
        </div>
      </div>

      {error && (
        <div className="error-message">
          <strong>Error:</strong> {error}
        </div>
      )}

      {linkToken && (
        <div className="result-section">
          <h3>Link Token</h3>
          <div className="token-display">
            <code>{linkToken}</code>
            <small>Expires: {new Date(Date.now() + 24 * 60 * 60 * 1000).toLocaleString()}</small>
          </div>
        </div>
      )}

      {balance && (
        <div className="result-section">
          <h3>Account Balances</h3>
          <div className="accounts-grid">
            {balance.accounts.map((account: PlaidAccount) => (
              <div key={account.id} className="account-card">
                <div className="account-header">
                  <h4>{account.name}</h4>
                  <span className="account-type">{account.type}</span>
                </div>
                <div className="account-mask">****{account.mask}</div>
                <div className="account-balances">
                  <div className="balance-item">
                    <span className="label">Available:</span>
                    <span className="amount">{formatCurrency(account.balances.available)}</span>
                  </div>
                  <div className="balance-item">
                    <span className="label">Current:</span>
                    <span className="amount">{formatCurrency(account.balances.current)}</span>
                  </div>
                  {account.balances.limit && (
                    <div className="balance-item">
                      <span className="label">Limit:</span>
                      <span className="amount">{formatCurrency(account.balances.limit)}</span>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {transactions && (
        <div className="result-section">
          <h3>Recent Transactions</h3>
          <div className="transactions-list">
            {transactions.transactions.slice(0, 10).map((transaction: PlaidTransaction) => (
              <div key={transaction.id} className="transaction-item">
                <div className="transaction-header">
                  <span className="transaction-name">{transaction.name}</span>
                  <span className={`transaction-amount ${transaction.amount >= 0 ? 'positive' : 'negative'}`}>
                    {formatCurrency(Math.abs(transaction.amount))}
                  </span>
                </div>
                <div className="transaction-details">
                  <span className="transaction-date">{formatDate(transaction.date)}</span>
                  <span className="transaction-category">{transaction.category.join(' > ')}</span>
                  {transaction.merchant_name && (
                    <span className="transaction-merchant">{transaction.merchant_name}</span>
                  )}
                  {transaction.pending && <span className="transaction-pending">Pending</span>}
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      <div className="result-section">
        <h3>Test Results</h3>
        <div className="test-results">
          {testResults.length === 0 ? (
            <p className="no-results">No tests run yet. Use the buttons above to test the Plaid API.</p>
          ) : (
            testResults.map((result, index) => (
              <div key={index} className={`test-result ${result.success ? 'success' : 'error'}`}>
                <div className="test-result-header">
                  <span className="test-name">{result.test}</span>
                  <span className={`test-status ${result.success ? 'success' : 'error'}`}>
                    {result.success ? '✓' : '✗'}
                  </span>
                  <span className="test-timestamp">{new Date(result.timestamp).toLocaleTimeString()}</span>
                </div>
                {result.error && (
                  <div className="test-error">Error: {result.error}</div>
                )}
                {result.data && (
                  <details className="test-data">
                    <summary>Data</summary>
                    <pre>{JSON.stringify(result.data, null, 2)}</pre>
                  </details>
                )}
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
};

export default PlaidTester;
