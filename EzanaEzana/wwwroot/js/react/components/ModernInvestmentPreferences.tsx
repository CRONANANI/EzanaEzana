import React, { useState, useEffect } from 'react';
import { 
  Target, 
  Shield, 
  TrendingUp, 
  Clock, 
  Save, 
  RefreshCw,
  CheckCircle,
  AlertCircle,
  Info,
  DollarSign,
  PieChart,
  BarChart3,
  Globe
} from 'lucide-react';
import { toast } from 'react-hot-toast';

interface PreferenceForm {
  riskTolerance: number;
  investmentHorizon: string;
  investmentAmount: number;
  preferredSectors: string[];
  ethicalInvesting: boolean;
  dividendPreference: boolean;
  growthPreference: boolean;
  internationalExposure: number;
  bondAllocation: number;
  stockAllocation: number;
  alternativeAllocation: number;
}

const ModernInvestmentPreferences: React.FC = () => {
  const [formData, setFormData] = useState<PreferenceForm>({
    riskTolerance: 5,
    investmentHorizon: '5-10',
    investmentAmount: 10000,
    preferredSectors: [],
    ethicalInvesting: false,
    dividendPreference: false,
    growthPreference: true,
    internationalExposure: 20,
    bondAllocation: 30,
    stockAllocation: 60,
    alternativeAllocation: 10
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const sectors = [
    'Technology', 'Healthcare', 'Financial Services', 'Consumer Goods',
    'Energy', 'Real Estate', 'Industrials', 'Materials', 'Utilities',
    'Communication Services', 'Consumer Discretionary', 'Consumer Staples'
  ];

  const investmentHorizons = [
    { value: '1-3', label: '1-3 years', description: 'Short-term goals' },
    { value: '3-5', label: '3-5 years', description: 'Medium-term goals' },
    { value: '5-10', label: '5-10 years', description: 'Long-term goals' },
    { value: '10+', label: '10+ years', description: 'Retirement planning' }
  ];

  const handleInputChange = (field: keyof PreferenceForm, value: any) => {
    setFormData(prev => ({ ...prev, [field]: value }));
  };

  const handleSectorToggle = (sector: string) => {
    setFormData(prev => ({
      ...prev,
      preferredSectors: prev.preferredSectors.includes(sector)
        ? prev.preferredSectors.filter(s => s !== sector)
        : [...prev.preferredSectors, sector]
    }));
  };

  const validateAllocation = () => {
    const total = formData.bondAllocation + formData.stockAllocation + formData.alternativeAllocation;
    return Math.abs(total - 100) < 1; // Allow for small rounding differences
  };

  const getRiskProfile = () => {
    if (formData.riskTolerance <= 3) return { level: 'Conservative', color: 'var(--ezana-success)', icon: <Shield size={20} /> };
    if (formData.riskTolerance <= 6) return { level: 'Moderate', color: 'var(--ezana-warning)', icon: <Target size={20} /> };
    return { level: 'Aggressive', color: 'var(--ezana-danger)', icon: <TrendingUp size={20} /> };
  };

  const getRecommendations = () => {
    const riskProfile = getRiskProfile();
    const recommendations = [];

    if (formData.riskTolerance <= 3) {
      recommendations.push(
        'Focus on high-quality bonds and dividend-paying stocks',
        'Consider index funds for broad market exposure',
        'Maintain higher cash reserves for stability'
      );
    } else if (formData.riskTolerance <= 6) {
      recommendations.push(
        'Balanced portfolio with growth and value stocks',
        'Moderate bond allocation for income generation',
        'Consider sector-specific ETFs for diversification'
      );
    } else {
      recommendations.push(
        'Higher allocation to growth stocks and emerging markets',
        'Consider alternative investments for diversification',
        'Regular rebalancing to manage volatility'
      );
    }

    return recommendations;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateAllocation()) {
      toast.error('Asset allocation must equal 100%');
      return;
    }

    setIsSubmitting(true);
    
    try {
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      toast.success('Investment preferences saved successfully!');
      
      // Here you would typically save to your backend
      console.log('Saving preferences:', formData);
      
    } catch (error) {
      toast.error('Failed to save preferences. Please try again.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleReset = () => {
    setFormData({
      riskTolerance: 5,
      investmentHorizon: '5-10',
      investmentAmount: 10000,
      preferredSectors: [],
      ethicalInvesting: false,
      dividendPreference: false,
      growthPreference: true,
      internationalExposure: 20,
      bondAllocation: 30,
      stockAllocation: 60,
      alternativeAllocation: 10
    });
    toast.success('Preferences reset to defaults');
  };

  const riskProfile = getRiskProfile();
  const recommendations = getRecommendations();

  return (
    <div className="ezana-animate-fade-in">
      <div className="row">
        <div className="col-lg-8">
          {/* Main Form */}
          <div className="ezana-card mb-4">
            <div className="ezana-card-header">
              <h3 className="ezana-text-heading-2 mb-0">
                <Target size={24} className="me-2" />
                Investment Preferences
              </h3>
            </div>
            <div className="ezana-card-body">
              <form onSubmit={handleSubmit}>
                {/* Risk Tolerance */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <Shield size={18} className="me-2" />
                    Risk Tolerance Level
                  </label>
                  <div className="d-flex align-items-center gap-3 mb-2">
                    <span className="ezana-text-small" style={{ color: 'var(--ezana-gray-600)' }}>Conservative</span>
                    <input
                      type="range"
                      className="form-range flex-grow-1"
                      min="1"
                      max="10"
                      value={formData.riskTolerance}
                      onChange={(e) => handleInputChange('riskTolerance', parseInt(e.target.value))}
                      style={{ accentColor: 'var(--ezana-primary)' }}
                    />
                    <span className="ezana-text-small" style={{ color: 'var(--ezana-gray-600)' }}>Aggressive</span>
                  </div>
                  <div className="d-flex justify-content-between">
                    {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(num => (
                      <span key={num} className="ezana-text-small" style={{ color: 'var(--ezana-gray-500)' }}>
                        {num}
                      </span>
                    ))}
                  </div>
                </div>

                {/* Investment Horizon */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <Clock size={18} className="me-2" />
                    Investment Horizon
                  </label>
                  <div className="row">
                    {investmentHorizons.map((horizon) => (
                      <div key={horizon.value} className="col-md-6 mb-3">
                        <div className="form-check">
                          <input
                            className="form-check-input"
                            type="radio"
                            name="investmentHorizon"
                            id={horizon.value}
                            value={horizon.value}
                            checked={formData.investmentHorizon === horizon.value}
                            onChange={(e) => handleInputChange('investmentHorizon', e.target.value)}
                          />
                          <label className="form-check-label" htmlFor={horizon.value}>
                            <div className="fw-semibold">{horizon.label}</div>
                            <small className="text-muted">{horizon.description}</small>
                          </label>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>

                {/* Investment Amount */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <DollarSign size={18} className="me-2" />
                    Initial Investment Amount
                  </label>
                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input
                      type="number"
                      className="ezana-form-input"
                      value={formData.investmentAmount}
                      onChange={(e) => handleInputChange('investmentAmount', parseInt(e.target.value))}
                      min="1000"
                      step="1000"
                    />
                  </div>
                </div>

                {/* Preferred Sectors */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <PieChart size={18} className="me-2" />
                    Preferred Investment Sectors
                  </label>
                  <div className="row">
                    {sectors.map((sector) => (
                      <div key={sector} className="col-md-4 mb-2">
                        <div className="form-check">
                          <input
                            className="form-check-input"
                            type="checkbox"
                            id={sector}
                            checked={formData.preferredSectors.includes(sector)}
                            onChange={() => handleSectorToggle(sector)}
                          />
                          <label className="form-check-label" htmlFor={sector}>
                            {sector}
                          </label>
                        </div>
                      </div>
                    ))}
                  </div>
                </div>

                {/* Investment Preferences */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <BarChart3 size={18} className="me-2" />
                    Investment Preferences
                  </label>
                  <div className="row">
                    <div className="col-md-4 mb-3">
                      <div className="form-check">
                        <input
                          className="form-check-input"
                          type="checkbox"
                          id="ethicalInvesting"
                          checked={formData.ethicalInvesting}
                          onChange={(e) => handleInputChange('ethicalInvesting', e.target.checked)}
                        />
                        <label className="form-check-label" htmlFor="ethicalInvesting">
                          Ethical/ESG Investing
                        </label>
                      </div>
                    </div>
                    <div className="col-md-4 mb-3">
                      <div className="form-check">
                        <input
                          className="form-check-input"
                          type="checkbox"
                          id="dividendPreference"
                          checked={formData.dividendPreference}
                          onChange={(e) => handleInputChange('dividendPreference', e.target.checked)}
                        />
                        <label className="form-check-label" htmlFor="dividendPreference">
                          Dividend Preference
                        </label>
                      </div>
                    </div>
                    <div className="col-md-4 mb-3">
                      <div className="form-check">
                        <input
                          className="form-check-input"
                          type="checkbox"
                          id="growthPreference"
                          checked={formData.growthPreference}
                          onChange={(e) => handleInputChange('growthPreference', e.target.checked)}
                        />
                        <label className="form-check-label" htmlFor="growthPreference">
                          Growth Preference
                        </label>
                      </div>
                    </div>
                  </div>
                </div>

                {/* Asset Allocation */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <PieChart size={18} className="me-2" />
                    Asset Allocation (must equal 100%)
                  </label>
                  
                  <div className="row">
                    <div className="col-md-4 mb-3">
                      <label className="ezana-form-label">Bonds (%)</label>
                      <input
                        type="number"
                        className="ezana-form-input"
                        value={formData.bondAllocation}
                        onChange={(e) => handleInputChange('bondAllocation', parseInt(e.target.value))}
                        min="0"
                        max="100"
                      />
                    </div>
                    <div className="col-md-4 mb-3">
                      <label className="ezana-form-label">Stocks (%)</label>
                      <input
                        type="number"
                        className="ezana-form-input"
                        value={formData.stockAllocation}
                        onChange={(e) => handleInputChange('stockAllocation', parseInt(e.target.value))}
                        min="0"
                        max="100"
                      />
                    </div>
                    <div className="col-md-4 mb-3">
                      <label className="ezana-form-label">Alternatives (%)</label>
                      <input
                        type="number"
                        className="ezana-form-input"
                        value={formData.alternativeAllocation}
                        onChange={(e) => handleInputChange('alternativeAllocation', parseInt(e.target.value))}
                        min="0"
                        max="100"
                      />
                    </div>
                  </div>
                  
                  <div className="ezana-progress mb-2">
                    <div 
                      className="ezana-progress-bar" 
                      style={{ width: `${formData.bondAllocation}%` }}
                    ></div>
                  </div>
                  
                  <div className="d-flex justify-content-between mb-3">
                    <span className="ezana-text-small">Bonds: {formData.bondAllocation}%</span>
                    <span className="ezana-text-small">Stocks: {formData.stockAllocation}%</span>
                    <span className="ezana-text-small">Alternatives: {formData.alternativeAllocation}%</span>
                  </div>
                  
                  <div className="d-flex justify-content-between">
                    <span className="ezana-text-small">Total: {formData.bondAllocation + formData.stockAllocation + formData.alternativeAllocation}%</span>
                    {!validateAllocation() && (
                      <span className="ezana-text-small" style={{ color: 'var(--ezana-danger)' }}>
                        ⚠️ Allocation must equal 100%
                      </span>
                    )}
                  </div>
                </div>

                {/* International Exposure */}
                <div className="ezana-form-group">
                  <label className="ezana-form-label">
                    <Globe size={18} className="me-2" />
                    International Market Exposure
                  </label>
                  <div className="d-flex align-items-center gap-3 mb-2">
                    <span className="ezana-text-small" style={{ color: 'var(--ezana-gray-600)' }}>0%</span>
                    <input
                      type="range"
                      className="form-range flex-grow-1"
                      min="0"
                      max="50"
                      value={formData.internationalExposure}
                      onChange={(e) => handleInputChange('internationalExposure', parseInt(e.target.value))}
                      style={{ accentColor: 'var(--ezana-primary)' }}
                    />
                    <span className="ezana-text-small" style={{ color: 'var(--ezana-gray-600)' }}>50%</span>
                  </div>
                  <div className="text-center">
                    <span className="ezana-text-heading-3" style={{ color: 'var(--ezana-primary)' }}>
                      {formData.internationalExposure}%
                    </span>
                  </div>
                </div>

                {/* Form Actions */}
                <div className="d-flex gap-3 pt-3">
                  <button
                    type="submit"
                    className="ezana-btn ezana-btn-primary"
                    disabled={isSubmitting || !validateAllocation()}
                  >
                    {isSubmitting ? (
                      <>
                        <RefreshCw size={16} className="me-2 animate-spin" />
                        Saving...
                      </>
                    ) : (
                      <>
                        <Save size={16} className="me-2" />
                        Save Preferences
                      </>
                    )}
                  </button>
                  
                  <button
                    type="button"
                    className="ezana-btn ezana-btn-secondary"
                    onClick={handleReset}
                  >
                    <RefreshCw size={16} className="me-2" />
                    Reset to Defaults
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>

        {/* Sidebar */}
        <div className="col-lg-4">
          {/* Risk Profile Summary */}
          <div className="ezana-card mb-4">
            <div className="ezana-card-header">
              <h5 className="ezana-text-heading-3 mb-0">Risk Profile</h5>
            </div>
            <div className="ezana-card-body text-center">
              <div className="rounded-circle d-flex align-items-center justify-content-center mx-auto mb-3" 
                   style={{ width: '80px', height: '80px', backgroundColor: `${riskProfile.color}20`, color: riskProfile.color }}>
                {riskProfile.icon}
              </div>
              <h4 className="ezana-text-heading-3 mb-2">{riskProfile.level}</h4>
              <p className="ezana-text-body" style={{ color: 'var(--ezana-gray-600)' }}>
                Risk Level: {formData.riskTolerance}/10
              </p>
            </div>
          </div>

          {/* Recommendations */}
          <div className="ezana-card mb-4">
            <div className="ezana-card-header">
              <h5 className="ezana-text-heading-3 mb-0">
                <Info size={18} className="me-2" />
                Recommendations
              </h5>
            </div>
            <div className="ezana-card-body">
              <ul className="list-unstyled">
                {recommendations.map((recommendation, index) => (
                  <li key={index} className="d-flex align-items-start gap-2 mb-3">
                    <CheckCircle size={16} className="mt-1" style={{ color: 'var(--ezana-success)', flexShrink: 0 }} />
                    <span className="ezana-text-small">{recommendation}</span>
                  </li>
                ))}
              </ul>
            </div>
          </div>

          {/* Quick Stats */}
          <div className="ezana-card">
            <div className="ezana-card-header">
              <h5 className="ezana-text-heading-3 mb-0">Quick Stats</h5>
            </div>
            <div className="ezana-card-body">
              <div className="row text-center">
                <div className="col-6 mb-3">
                  <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>
                    ${formData.investmentAmount.toLocaleString()}
                  </div>
                  <div className="ezana-text-caption">Investment</div>
                </div>
                <div className="col-6 mb-3">
                  <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>
                    {formData.preferredSectors.length}
                  </div>
                  <div className="ezana-text-caption">Sectors</div>
                </div>
                <div className="col-6">
                  <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>
                    {formData.internationalExposure}%
                  </div>
                  <div className="ezana-text-caption">International</div>
                </div>
                <div className="col-6">
                  <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>
                    {formData.bondAllocation}%
                  </div>
                  <div className="ezana-text-caption">Bonds</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ModernInvestmentPreferences;
