import React from 'react';
import { createRoot } from 'react-dom/client';
import App from './components/App';
import { AppProvider } from './context/AppContext';

// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', () => {
  // Find the container element where React will be mounted
  const container = document.getElementById('react-app');
  
  // Only mount if the container exists
  if (container) {
    const root = createRoot(container);
    root.render(
      <AppProvider>
        <App />
      </AppProvider>
    );
  }
}); 