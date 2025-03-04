import React from 'react';

/**
 * Loading Component
 * 
 * A reusable loading spinner component with customizable size and message
 * 
 * @param {Object} props
 * @param {string} props.size - Size of the spinner (sm, md, lg)
 * @param {string} props.message - Optional message to display
 * @param {boolean} props.fullPage - Whether to display as a full page overlay
 */
function Loading({ size = 'md', message = 'Loading...', fullPage = false }) {
  const spinnerSize = {
    sm: '',
    md: 'spinner-border-sm',
    lg: ''
  }[size];
  
  const spinnerClasses = `spinner-border ${spinnerSize} text-primary`;
  
  if (fullPage) {
    return (
      <div className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center bg-white bg-opacity-75" style={{ zIndex: 1050 }}>
        <div className="text-center">
          <div className={spinnerClasses} role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          {message && <p className="mt-3">{message}</p>}
        </div>
      </div>
    );
  }
  
  return (
    <div className="d-flex justify-content-center align-items-center py-4">
      <div className="text-center">
        <div className={spinnerClasses} role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
        {message && <p className="mt-2 text-muted">{message}</p>}
      </div>
    </div>
  );
}

export default Loading; 