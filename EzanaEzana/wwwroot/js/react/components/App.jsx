import React, { useState, useEffect } from 'react';
import Dashboard from './Dashboard';
import Loading from './common/Loading';
import ErrorMessage from './common/ErrorMessage';
import { useAppContext } from '../context/AppContext';

function App() {
  const { isLoading, error, user } = useAppContext();
  const [count, setCount] = useState(0);
  const [showDashboard, setShowDashboard] = useState(false);

  // If the app is in the initial loading state, show a loading spinner
  if (isLoading && !showDashboard) {
    return <Loading fullPage={true} message="Loading application..." />;
  }

  return (
    <div className="react-app">
      {error && (
        <div className="mb-4">
          <ErrorMessage message={error} />
        </div>
      )}
      
      {!showDashboard ? (
        <div className="card p-4 shadow-sm mb-4">
          <h2 className="mb-3">React Integration Demo</h2>
          {user && (
            <div className="alert alert-success mb-3">
              <i className="bi bi-check-circle me-2"></i>
              Welcome, {user.name}! You are successfully logged in.
            </div>
          )}
          <p className="mb-3">
            This component demonstrates React integration with your ASP.NET Core application.
          </p>
          <div className="d-flex align-items-center mb-3">
            <button 
              className="btn btn-primary me-2" 
              onClick={() => setCount(count + 1)}
            >
              Count: {count}
            </button>
            <button 
              className="btn btn-outline-secondary" 
              onClick={() => setCount(0)}
            >
              Reset
            </button>
          </div>
          <div className="alert alert-info mb-4">
            <i className="bi bi-info-circle me-2"></i>
            You can now build more complex React components for your investment analytics app!
          </div>
          <button 
            className="btn btn-success" 
            onClick={() => setShowDashboard(true)}
          >
            <i className="bi bi-graph-up me-2"></i>
            Show Investment Dashboard Demo
          </button>
        </div>
      ) : (
        <div>
          <div className="mb-4">
            <button 
              className="btn btn-outline-secondary" 
              onClick={() => setShowDashboard(false)}
            >
              <i className="bi bi-arrow-left me-2"></i>
              Back to Simple Demo
            </button>
          </div>
          {isLoading ? (
            <Loading message="Loading dashboard data..." />
          ) : (
            <Dashboard />
          )}
        </div>
      )}
    </div>
  );
}

export default App; 