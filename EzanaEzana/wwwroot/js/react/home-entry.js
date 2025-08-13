import React from 'react';
import { createRoot } from 'react-dom/client';
import { Toaster } from 'react-hot-toast';
import Dashboard from './components/Dashboard';
import './index.css';

// Initialize the Home component
const initHome = () => {
  const container = document.getElementById('react-home-root');
  
  if (!container) {
    console.error('Home root element not found');
    return;
  }

  const root = createRoot(container);

  root.render(
    <React.StrictMode>
      <div className="home-app">
        <Dashboard />
        <Toaster
          position="top-right"
          toastOptions={{
            duration: 4000,
            style: {
              background: '#363636',
              color: '#fff',
            },
            success: {
              duration: 3000,
              iconTheme: {
                primary: '#10b981',
                secondary: '#fff',
              },
            },
            error: {
              duration: 5000,
              iconTheme: {
                primary: '#ef4444',
                secondary: '#fff',
              },
            },
          }}
        />
      </div>
    </React.StrictMode>
  );
};

// Wait for DOM to be ready
if (document.readyState === 'loading') {
  document.addEventListener('DOMContentLoaded', initHome);
} else {
  initHome();
}
