import React, { useState } from 'react';
import LandingPage from './LandingPage';

const WebApp = () => {
  const [currentView, setCurrentView] = useState('landing'); // 'landing' or 'playground'
  const [user, setUser] = useState(null);
  const [showPlayground, setShowPlayground] = useState(false);

  const handleLogin = (userData) => {
    setUser(userData);
    setCurrentView('playground');
  };

  const handleRegister = (userData) => {
    setUser(userData);
    setCurrentView('playground');
  };

  const handleDemo = () => {
    setUser({ email: 'demo@ezana.com', name: 'Demo User' });
    setCurrentView('playground');
  };

  const handleLogout = () => {
    setUser(null);
    setCurrentView('landing');
  };

  const handleBackToLanding = () => {
    setCurrentView('landing');
  };

  if (currentView === 'landing') {
    return (
      <LandingPage 
        onLogin={handleLogin}
        onRegister={handleRegister}
        onDemo={handleDemo}
      />
    );
  }

  // Component Playground View
  return (
    <div className="web-app">
      {/* Header */}
      <header className="app-header">
        <div className="header-content">
          <div className="header-left">
            <button 
              className="btn btn-outline-secondary btn-sm"
              onClick={handleBackToLanding}
            >
              <i className="bi bi-arrow-left"></i> Back to Landing
            </button>
            <h1 className="app-title">
              <i className="bi bi-graph-up-arrow"></i> Ezana Component Playground
            </h1>
          </div>
          <div className="header-right">
            {user && (
              <div className="user-info">
                <span className="user-name">Welcome, {user.name}!</span>
                <button 
                  className="btn btn-outline-danger btn-sm"
                  onClick={handleLogout}
                >
                  <i className="bi bi-box-arrow-right"></i> Logout
                </button>
              </div>
            )}
          </div>
        </div>
      </header>

      {/* Main Content */}
      <main className="app-main">
        <div className="playground-container">
          <div className="playground-intro">
            <h2>Welcome to the Component Playground!</h2>
            <p>
              You're now logged in as <strong>{user?.email}</strong>. 
              Use this playground to test and develop React components for the Ezana application.
            </p>
            <div className="playground-features">
              <div className="feature-item">
                <i className="bi bi-check-circle text-success"></i>
                <span>Test components in isolation</span>
              </div>
              <div className="feature-item">
                <i className="bi bi-check-circle text-success"></i>
                <span>Real-time stock data with WebSockets</span>
              </div>
              <div className="feature-item">
                <i className="bi bi-check-circle text-success"></i>
                <span>Plaid API integration testing</span>
              </div>
              <div className="feature-item">
                <i className="bi bi-check-circle text-success"></i>
                <span>Dashboard and analytics components</span>
              </div>
            </div>
          </div>

          {/* Component Testing Sections */}
          <div className="component-sections">
            <div className="section-group">
              <h3>Core Components</h3>
              <div className="section-grid">
                <div className="section-card" onClick={() => testDashboard()}>
                  <i className="bi bi-speedometer2"></i>
                  <h4>Dashboard</h4>
                  <p>Test the main dashboard component</p>
                </div>
                <div className="section-card" onClick={() => testDashboardCard()}>
                  <i className="bi bi-card-text"></i>
                  <h4>Dashboard Cards</h4>
                  <p>Test individual dashboard cards</p>
                </div>
                <div className="section-card" onClick={() => testChart()}>
                  <i className="bi bi-graph-up"></i>
                  <h4>Investment Charts</h4>
                  <p>Test chart components</p>
                </div>
              </div>
            </div>

            <div className="section-group">
              <h3>Market Data</h3>
              <div className="section-grid">
                <div className="section-card" onClick={() => testStockQuotes()}>
                  <i className="bi bi-currency-dollar"></i>
                  <h4>Stock Quotes</h4>
                  <p>Test stock price data</p>
                </div>
                <div className="section-card" onClick={() => testRealTimeStockTicker()}>
                  <i className="bi bi-lightning"></i>
                  <h4>Real-Time Ticker</h4>
                  <p>Test WebSocket live data</p>
                </div>
                <div className="section-card" onClick={() => testMarketNews()}>
                  <i className="bi bi-newspaper"></i>
                  <h4>Market News</h4>
                  <p>Test news components</p>
                </div>
              </div>
            </div>

            <div className="section-group">
              <h3>Advanced Features</h3>
              <div className="section-grid">
                <div className="section-card" onClick={() => testCongressionalTrading()}>
                  <i className="bi bi-building"></i>
                  <h4>Congressional Trading</h4>
                  <p>Test trading dashboard</p>
                </div>
                <div className="section-card" onClick={() => testPlaidIntegration()}>
                  <i className="bi bi-bank"></i>
                  <h4>Plaid Integration</h4>
                  <p>Test financial data API</p>
                </div>
                <div className="section-card" onClick={() => testCryptoData()}>
                  <i className="bi bi-currency-bitcoin"></i>
                  <h4>Crypto Data</h4>
                  <p>Test cryptocurrency data</p>
                </div>
              </div>
            </div>
          </div>

          {/* Test Results Container */}
          <div id="test-results-container" className="test-results">
            <div className="results-header">
              <h3>Test Results</h3>
              <button 
                className="btn btn-outline-secondary btn-sm"
                onClick={() => clearAllTests()}
              >
                Clear All
              </button>
            </div>
            <div id="test-output" className="test-output">
              <p className="text-muted">Click on any component card above to start testing...</p>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

// Helper functions for component testing
function testDashboard() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Dashboard component...</div>';
    // This would call the actual test function from the playground
    if (typeof window.testDashboard === 'function') {
      window.testDashboard();
    }
  }
}

function testDashboardCard() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Dashboard Card component...</div>';
    if (typeof window.testDashboardCard === 'function') {
      window.testDashboardCard();
    }
  }
}

function testChart() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Investment Chart component...</div>';
    if (typeof window.testChart === 'function') {
      window.testChart();
    }
  }
}

function testStockQuotes() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Stock Quotes...</div>';
    if (typeof window.testStockQuotes === 'function') {
      window.testStockQuotes();
    }
  }
}

function testRealTimeStockTicker() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Real-Time Stock Ticker...</div>';
    if (typeof window.testRealTimeStockTicker === 'function') {
      window.testRealTimeStockTicker();
    }
  }
}

function testMarketNews() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Market News...</div>';
    if (typeof window.testMarketNews === 'function') {
      window.testMarketNews();
    }
  }
}

function testCongressionalTrading() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Congressional Trading Dashboard...</div>';
    if (typeof window.testCongressionalTrading === 'function') {
      window.testCongressionalTrading();
    }
  }
}

function testPlaidIntegration() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Plaid API Integration...</div>';
    if (typeof window.testPlaidIntegration === 'function') {
      window.testPlaidIntegration();
    }
  }
}

function testCryptoData() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<div class="alert alert-info">Testing Crypto Data...</div>';
    if (typeof window.testCryptoData === 'function') {
      window.testCryptoData();
    }
  }
}

function clearAllTests() {
  const container = document.getElementById('test-output');
  if (container) {
    container.innerHTML = '<p class="text-muted">Click on any component card above to start testing...</p>';
  }
  
  // Clear all test containers
  const testContainers = [
    'dashboard-test-container',
    'card-test-container', 
    'chart-test-container',
    'market-api-test-container',
    'congressional-trading-test-container',
    'home-landing-test-container',
    'plaid-test-container'
  ];
  
  testContainers.forEach(id => {
    const element = document.getElementById(id);
    if (element) {
      element.innerHTML = '';
    }
  });
}

export default WebApp;
