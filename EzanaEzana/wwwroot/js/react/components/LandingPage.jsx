import React, { useState } from 'react';

const LandingPage = ({ onLogin, onRegister, onDemo }) => {
  const [showLoginModal, setShowLoginModal] = useState(false);
  const [showRegisterModal, setShowRegisterModal] = useState(false);
  const [loginForm, setLoginForm] = useState({ email: '', password: '' });
  const [registerForm, setRegisterForm] = useState({
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const handleLoginSubmit = (e) => {
    e.preventDefault();
    if (loginForm.email && loginForm.password) {
      onLogin({ email: loginForm.email, name: loginForm.email.split('@')[0] });
      setShowLoginModal(false);
      setLoginForm({ email: '', password: '' });
    }
  };

  const handleRegisterSubmit = (e) => {
    e.preventDefault();
    if (registerForm.password === registerForm.confirmPassword) {
      onRegister({
        email: registerForm.email,
        name: `${registerForm.firstName} ${registerForm.lastName}`
      });
      setShowRegisterModal(false);
      setRegisterForm({ firstName: '', lastName: '', email: '', password: '', confirmPassword: '' });
    }
  };

  return (
    <div className="landing-page">
      {/* Header */}
      <header className="landing-header">
        <div className="header-content">
          <div className="logo">
            <span className="logo-text">@ezana</span>
          </div>
          <nav className="header-nav">
            <a href="#features" className="nav-link">Features</a>
            <button className="btn btn-primary btn-sm" onClick={() => setShowRegisterModal(true)}>
              Join the waitlist
            </button>
          </nav>
        </div>
      </header>

      {/* Hero Section */}
      <section className="hero-section">
        <div className="hero-content">
          <h1 className="hero-title">Unleash the power of intuitive finance</h1>
          <p className="hero-subtitle">
            Say goodbye to the outdated financial tools. Every small business owner, regardless of background,
            can now manage their business like a pro. Simple. Intuitive. And never boring.
          </p>
          <div className="hero-buttons">
            <button className="btn btn-primary btn-lg" onClick={() => setShowRegisterModal(true)}>
              Join the waitlist
            </button>
            <button className="btn btn-text btn-lg">
              Learn more <i className="bi bi-arrow-down"></i>
            </button>
          </div>
        </div>
        <div className="hero-visual">
          <div className="dashboard-preview">
            <div className="dashboard-header">
              <div className="dashboard-nav">
                <span className="nav-item active">Insights</span>
                <span className="nav-item">Company</span>
                <span className="nav-item">Transactions</span>
                <span className="nav-item">Cards</span>
                <span className="nav-item">Accounting</span>
              </div>
            </div>
            <div className="dashboard-content">
              <div className="metrics-grid">
                <div className="metric-card">
                  <div className="metric-value">$9,540.17</div>
                  <div className="metric-label">Cash Flow</div>
                  <div className="metric-period">28 days</div>
                </div>
                <div className="metric-card">
                  <div className="metric-value">$12,007.45</div>
                  <div className="metric-label">Revenue</div>
                  <div className="metric-period">28 days</div>
                </div>
                <div className="metric-card">
                  <div className="metric-value">$8,391.87</div>
                  <div className="metric-label">Expenses</div>
                  <div className="metric-period">28 days</div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="features-section" id="features">
        <div className="container">
          <h2 className="section-title">Everything you need. Nothing you don't</h2>
          <p className="section-subtitle">
            Financial management and visibility in one place. Experience a flexible toolkit that makes every task feel like a breeze.
          </p>

          <div className="features-grid">
            <div className="feature-card">
              <div className="feature-visual">
                <div className="mockup-dashboard">
                  <div className="mockup-header">
                    <div className="mockup-title">Today</div>
                    <div className="mockup-actions">
                      <span className="action-btn">Configure</span>
                    </div>
                  </div>
                  <div className="mockup-chart">
                    <div className="chart-line income"></div>
                    <div className="chart-line expenses"></div>
                  </div>
                </div>
              </div>
              <h3 className="feature-title">Insights at your fingertips</h3>
              <p className="feature-description">
                All your data and finances in one place to provide quick answers and make decisions instantly.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-visual">
                <div className="mockup-mobile">
                  <div className="mobile-screen">
                    <div className="mobile-header">Cards</div>
                    <div className="mobile-content">
                      <div className="mobile-item">Overview</div>
                      <div className="mobile-item">Other suppliers</div>
                      <div className="mobile-item">Settings</div>
                    </div>
                  </div>
                </div>
              </div>
              <h3 className="feature-title">Manage in real time</h3>
              <p className="feature-description">
                Have full control of your business finances on the go using our iOS/Android mobile apps. Because, you know, it's 2024.
              </p>
            </div>

            <div className="feature-card">
              <div className="feature-visual">
                <div className="mockup-alerts">
                  <div className="alert-item">
                    <div className="alert-content">
                      <div className="alert-title">Outstanding Invoice</div>
                      <div className="alert-actions">
                        <button className="alert-btn approve">Approve</button>
                        <button className="alert-btn decline">Decline</button>
                        <button className="alert-btn edit">Edit</button>
                      </div>
                    </div>
                  </div>
                  <div className="alert-item success">
                    <div className="alert-content">
                      <div className="alert-title">Revenue Increase Alert!</div>
                      <div className="alert-message">27% increase in the last 7 days</div>
                    </div>
                  </div>
                </div>
              </div>
              <h3 className="feature-title">Important business alerts</h3>
              <p className="feature-description">
                Choose the alerts you need and receive them via email, mobile or Slack. Review and take action in one click.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* Integration Section */}
      <section className="integration-section">
        <div className="container">
          <div className="integration-grid">
            <div className="integration-card">
              <div className="integration-visual">
                <div className="app-grid">
                  <div className="app-icon amazon">A</div>
                  <div className="app-icon quickbooks">QB</div>
                  <div className="app-icon stripe">S</div>
                  <div className="app-icon bank">🏦</div>
                  <div className="app-icon sage">Sage</div>
                  <div className="app-icon capital">C</div>
                </div>
              </div>
              <h3 className="integration-title">Connect all your apps</h3>
              <p className="integration-description">
                Bring your data with our built-in integrations for accounting, revenue tools and banking.
              </p>
            </div>

            <div className="integration-card">
              <div className="integration-visual">
                <div className="command-palette">
                  <div className="palette-header">What would you like to do?</div>
                  <div className="palette-items">
                    <div className="palette-item">
                      <span>View active cards</span>
                      <span className="shortcut">K</span>
                    </div>
                    <div className="palette-item">
                      <span>View all summary reports</span>
                    </div>
                    <div className="palette-item">
                      <span>Manage expenses</span>
                    </div>
                    <div className="palette-item">
                      <span>Manage settings</span>
                      <span className="shortcut">⌘K</span>
                    </div>
                  </div>
                </div>
              </div>
              <h3 className="integration-title">You're in control</h3>
              <p className="integration-description">
                Lightning fast. Shortcuts for everything. Command+K on Mac, Ctrl+K on Windows. Dark mode.
              </p>
            </div>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-section">
        <div className="container">
          <h2 className="cta-title">See where financial automation can take your business</h2>
          <p className="cta-subtitle">The first financial tool you'll love. And the last one you'll ever need.</p>
          <div className="cta-buttons">
            <button className="btn btn-primary btn-lg" onClick={() => setShowRegisterModal(true)}>
              Join the waitlist
            </button>
            <button className="btn btn-outline btn-lg" onClick={onDemo}>
              Try Demo
            </button>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="landing-footer">
        <div className="container">
          <div className="footer-content">
            <div className="footer-left">
              <span className="copyright">© 2024 Ezana Investment Analytics Inc.</span>
              <div className="footer-links">
                <a href="#" className="footer-link">Privacy Policy</a>
                <span className="separator">•</span>
                <a href="#" className="footer-link">Terms of Use</a>
              </div>
            </div>
            <div className="footer-right">
              <div className="social-icons">
                <a href="#" className="social-icon"><i className="bi bi-twitter-x"></i></a>
                <a href="#" className="social-icon"><i className="bi bi-linkedin"></i></a>
                <a href="#" className="social-icon"><i className="bi bi-instagram"></i></a>
              </div>
            </div>
          </div>
        </div>
      </footer>

      {/* Login Modal */}
      {showLoginModal && (
        <div className="modal-overlay" onClick={() => setShowLoginModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Welcome Back</h3>
              <button className="modal-close" onClick={() => setShowLoginModal(false)}>
                <i className="bi bi-x"></i>
              </button>
            </div>
            <form onSubmit={handleLoginSubmit} className="modal-form">
              <div className="form-group">
                <label htmlFor="login-email">Email</label>
                <input
                  type="email"
                  id="login-email"
                  value={loginForm.email}
                  onChange={(e) => setLoginForm({ ...loginForm, email: e.target.value })}
                  placeholder="Enter your email"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="login-password">Password</label>
                <input
                  type="password"
                  id="login-password"
                  value={loginForm.password}
                  onChange={(e) => setLoginForm({ ...loginForm, password: e.target.value })}
                  placeholder="Enter your password"
                  required
                />
              </div>
              <button type="submit" className="btn btn-primary btn-full">
                Sign In
              </button>
            </form>
            <div className="modal-footer">
              <p>Don't have an account? <button className="btn btn-text" onClick={() => { setShowLoginModal(false); setShowRegisterModal(true); }}>Sign up</button></p>
            </div>
          </div>
        </div>
      )}

      {/* Register Modal */}
      {showRegisterModal && (
        <div className="modal-overlay" onClick={() => setShowRegisterModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h3>Create Your Account</h3>
              <button className="modal-close" onClick={() => setShowRegisterModal(false)}>
                <i className="bi bi-x"></i>
              </button>
            </div>
            <form onSubmit={handleRegisterSubmit} className="modal-form">
              <div className="form-row">
                <div className="form-group">
                  <label htmlFor="register-firstName">First Name</label>
                  <input
                    type="text"
                    id="register-firstName"
                    value={registerForm.firstName}
                    onChange={(e) => setRegisterForm({ ...registerForm, firstName: e.target.value })}
                    placeholder="First name"
                    required
                  />
                </div>
                <div className="form-group">
                  <label htmlFor="register-lastName">Last Name</label>
                  <input
                    type="text"
                    id="register-lastName"
                    value={registerForm.lastName}
                    onChange={(e) => setRegisterForm({ ...registerForm, lastName: e.target.value })}
                    placeholder="Last name"
                    required
                  />
                </div>
              </div>
              <div className="form-group">
                <label htmlFor="register-email">Email</label>
                <input
                  type="email"
                  id="register-email"
                  value={registerForm.email}
                  onChange={(e) => setRegisterForm({ ...registerForm, email: e.target.value })}
                  placeholder="Enter your email"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="register-password">Password</label>
                <input
                  type="password"
                  id="register-password"
                  value={registerForm.password}
                  onChange={(e) => setRegisterForm({ ...registerForm, password: e.target.value })}
                  placeholder="Create a password"
                  required
                />
              </div>
              <div className="form-group">
                <label htmlFor="register-confirmPassword">Confirm Password</label>
                <input
                  type="password"
                  id="register-confirmPassword"
                  value={registerForm.confirmPassword}
                  onChange={(e) => setRegisterForm({ ...registerForm, confirmPassword: e.target.value })}
                  placeholder="Confirm your password"
                  required
                />
              </div>
              <button type="submit" className="btn btn-primary btn-full">
                Create Account
              </button>
            </form>
            <div className="modal-footer">
              <p>Already have an account? <button className="btn btn-text" onClick={() => { setShowRegisterModal(false); setShowLoginModal(true); }}>Sign in</button></p>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default LandingPage;
