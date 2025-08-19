import React from 'react';
import { createRoot } from 'react-dom/client';
import SimpleComponentPlayground from './components/SimpleComponentPlayground';

// Create root and render the simple playground
const container = document.getElementById('react-playground-root');
if (container) {
  const root = createRoot(container);
  root.render(<SimpleComponentPlayground />);
} else {
  console.error('React playground root element not found');
}
