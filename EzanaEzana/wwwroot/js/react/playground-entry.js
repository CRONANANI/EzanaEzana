import React from 'react';
import { createRoot } from 'react-dom/client';
import ComponentPlayground from './components/ComponentPlayground';
import './index.css';

// Create root and render the playground
const container = document.getElementById('react-playground-root');
if (container) {
  const root = createRoot(container);
  root.render(<ComponentPlayground />);
} else {
  console.error('React playground root element not found');
}
