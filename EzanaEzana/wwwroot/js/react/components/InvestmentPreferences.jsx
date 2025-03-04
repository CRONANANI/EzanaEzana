import React, { useState, useEffect } from 'react';
import { useAppContext } from '../context/AppContext';
import Loading from './common/Loading';
import ErrorMessage from './common/ErrorMessage';

/**
 * Investment Preferences Component
 * 
 * Allows users to set their investment preferences and risk tolerance
 */
function InvestmentPreferences() {
  const { preferences, savePreferences, fetchPreferences, isLoading, error } = useAppContext();
  const [formValues, setFormValues] = useState({
    riskTolerance: 'moderate',
    investmentGoals: ['retirement', 'growth'],
    investmentHorizon: '5-10',
    monthlyContribution: 500,
    autoRebalance: true,
    preferredSectors: ['technology', 'healthcare', 'finance']
  });
  const [formSubmitted, setFormSubmitted] = useState(false);

  // Available options for form selects
  const riskOptions = ['conservative', 'moderate', 'aggressive'];
  const goalOptions = ['retirement', 'growth', 'income', 'preservation', 'education'];
  const horizonOptions = ['0-2', '2-5', '5-10', '10+'];
  const sectorOptions = [
    'technology', 'healthcare', 'finance', 'consumer', 'industrial', 
    'energy', 'utilities', 'materials', 'real-estate', 'communication'
  ];

  // Load preferences from context when component mounts
  useEffect(() => {
    fetchPreferences();
  }, []);

  // Update form values when preferences are loaded
  useEffect(() => {
    if (preferences) {
      setFormValues(preferences);
    }
  }, [preferences]);

  // Handle form input changes
  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    
    if (type === 'checkbox') {
      setFormValues({
        ...formValues,
        [name]: checked
      });
    } else {
      setFormValues({
        ...formValues,
        [name]: value
      });
    }
  };

  // Handle multi-select changes (for checkboxes)
  const handleMultiSelectChange = (e) => {
    const { value, checked } = e.target;
    const { investmentGoals } = formValues;
    
    if (checked) {
      setFormValues({
        ...formValues,
        investmentGoals: [...investmentGoals, value]
      });
    } else {
      setFormValues({
        ...formValues,
        investmentGoals: investmentGoals.filter(goal => goal !== value)
      });
    }
  };

  // Handle sector preference changes
  const handleSectorChange = (e) => {
    const { value, checked } = e.target;
    const { preferredSectors } = formValues;
    
    if (checked) {
      setFormValues({
        ...formValues,
        preferredSectors: [...preferredSectors, value]
      });
    } else {
      setFormValues({
        ...formValues,
        preferredSectors: preferredSectors.filter(sector => sector !== value)
      });
    }
  };

  // Handle form submission
  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Save preferences using context method
    const result = await savePreferences(formValues);
    
    if (result.success) {
      setFormSubmitted(true);
      
      // Reset form submitted state after 3 seconds
      setTimeout(() => {
        setFormSubmitted(false);
      }, 3000);
    }
  };

  // Reset to default values
  const handleReset = () => {
    setFormValues({
      riskTolerance: 'moderate',
      investmentGoals: ['retirement', 'growth'],
      investmentHorizon: '5-10',
      monthlyContribution: 500,
      autoRebalance: true,
      preferredSectors: ['technology', 'healthcare', 'finance']
    });
  };

  if (isLoading && !preferences) {
    return <Loading message="Loading investment preferences..." />;
  }

  return (
    <div className="card shadow-sm">
      <div className="card-header bg-white">
        <h5 className="card-title mb-0">Investment Preferences</h5>
      </div>
      <div className="card-body">
        {error && <ErrorMessage message={error} onRetry={fetchPreferences} />}
        
        {formSubmitted && (
          <div className="alert alert-success mb-4">
            <i className="bi bi-check-circle me-2"></i>
            Your investment preferences have been saved successfully!
          </div>
        )}
        
        <form onSubmit={handleSubmit}>
          {/* Risk Tolerance */}
          <div className="mb-4">
            <label className="form-label fw-bold">Risk Tolerance</label>
            <div className="d-flex flex-column gap-2">
              {riskOptions.map(option => (
                <div className="form-check" key={option}>
                  <input
                    className="form-check-input"
                    type="radio"
                    name="riskTolerance"
                    id={`risk-${option}`}
                    value={option}
                    checked={formValues.riskTolerance === option}
                    onChange={handleInputChange}
                  />
                  <label className="form-check-label text-capitalize" htmlFor={`risk-${option}`}>
                    {option}
                    {option === 'conservative' && ' - Lower risk, lower potential returns'}
                    {option === 'moderate' && ' - Balanced risk and potential returns'}
                    {option === 'aggressive' && ' - Higher risk, higher potential returns'}
                  </label>
                </div>
              ))}
            </div>
          </div>

          {/* Investment Goals */}
          <div className="mb-4">
            <label className="form-label fw-bold">Investment Goals</label>
            <div className="row">
              {goalOptions.map(goal => (
                <div className="col-md-6" key={goal}>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id={`goal-${goal}`}
                      value={goal}
                      checked={formValues.investmentGoals.includes(goal)}
                      onChange={handleMultiSelectChange}
                    />
                    <label className="form-check-label text-capitalize" htmlFor={`goal-${goal}`}>
                      {goal}
                    </label>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Investment Horizon */}
          <div className="mb-4">
            <label htmlFor="investmentHorizon" className="form-label fw-bold">Investment Horizon</label>
            <select
              className="form-select"
              id="investmentHorizon"
              name="investmentHorizon"
              value={formValues.investmentHorizon}
              onChange={handleInputChange}
            >
              <option value="0-2">Less than 2 years</option>
              <option value="2-5">2-5 years</option>
              <option value="5-10">5-10 years</option>
              <option value="10+">More than 10 years</option>
            </select>
          </div>

          {/* Monthly Contribution */}
          <div className="mb-4">
            <label htmlFor="monthlyContribution" className="form-label fw-bold">Monthly Contribution ($)</label>
            <input
              type="number"
              className="form-control"
              id="monthlyContribution"
              name="monthlyContribution"
              value={formValues.monthlyContribution}
              onChange={handleInputChange}
              min="0"
              step="50"
            />
          </div>

          {/* Auto Rebalance */}
          <div className="mb-4">
            <div className="form-check form-switch">
              <input
                className="form-check-input"
                type="checkbox"
                id="autoRebalance"
                name="autoRebalance"
                checked={formValues.autoRebalance}
                onChange={handleInputChange}
              />
              <label className="form-check-label fw-bold" htmlFor="autoRebalance">
                Auto-rebalance portfolio quarterly
              </label>
            </div>
            <div className="form-text">
              Automatically adjust your portfolio to maintain your target asset allocation.
            </div>
          </div>

          {/* Preferred Sectors */}
          <div className="mb-4">
            <label className="form-label fw-bold">Preferred Sectors</label>
            <div className="row">
              {sectorOptions.map(sector => (
                <div className="col-md-4" key={sector}>
                  <div className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id={`sector-${sector}`}
                      value={sector}
                      checked={formValues.preferredSectors.includes(sector)}
                      onChange={handleSectorChange}
                    />
                    <label className="form-check-label text-capitalize" htmlFor={`sector-${sector}`}>
                      {sector}
                    </label>
                  </div>
                </div>
              ))}
            </div>
          </div>

          <div className="d-grid gap-2 d-md-flex justify-content-md-end">
            <button 
              type="button" 
              className="btn btn-outline-secondary"
              onClick={handleReset}
            >
              Reset to Defaults
            </button>
            <button 
              type="submit" 
              className="btn btn-primary"
              disabled={isLoading}
            >
              {isLoading ? (
                <>
                  <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                  Saving...
                </>
              ) : 'Save Preferences'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default InvestmentPreferences; 