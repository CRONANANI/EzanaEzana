import React from 'react';
import { createRoot } from 'react-dom/client';
import Dashboard from './components/Dashboard';
import CongressionalTrading from './components/CongressionalTrading';
import './index.css';

// Create a combined dashboard component
const CombinedDashboard = () => {
  return (
    <div className="space-y-8">
      <Dashboard />
      <CongressionalTrading />
    </div>
  );
};

// Render the combined dashboard
const container = document.getElementById('react-dashboard-root');
if (container) {
  const root = createRoot(container);
  root.render(<CombinedDashboard />);
} else {
  console.error('Dashboard root element not found');
}
