import React from 'react';
import { useAppContext } from '../../context/AppContext';

/**
 * Error Message Component
 * 
 * A reusable error message component with optional retry functionality
 * 
 * @param {Object} props
 * @param {string} props.message - Error message to display
 * @param {Function} props.onRetry - Optional retry function
 * @param {string} props.type - Error type (danger, warning, info)
 */
function ErrorMessage({ message, onRetry, type = 'danger' }) {
  const { clearError } = useAppContext();
  
  const handleRetry = () => {
    clearError();
    if (onRetry) {
      onRetry();
    }
  };
  
  return (
    <div className={`alert alert-${type} d-flex align-items-center`} role="alert">
      <div className="me-3">
        {type === 'danger' && <i className="bi bi-exclamation-triangle-fill fs-4"></i>}
        {type === 'warning' && <i className="bi bi-exclamation-circle-fill fs-4"></i>}
        {type === 'info' && <i className="bi bi-info-circle-fill fs-4"></i>}
      </div>
      <div className="flex-grow-1">
        {message}
      </div>
      {onRetry && (
        <button 
          className="btn btn-sm btn-outline-secondary ms-3" 
          onClick={handleRetry}
        >
          Retry
        </button>
      )}
      <button 
        className="btn-close ms-2" 
        onClick={clearError}
        aria-label="Close"
      ></button>
    </div>
  );
}

export default ErrorMessage; 