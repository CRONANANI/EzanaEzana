import React, { useState } from 'react';
import { 
  ArrowRight, 
  CheckCircle, 
  TrendingUp, 
  Shield, 
  Users, 
  BarChart3,
  Play,
  Star,
  Download,
  Zap,
  Target,
  Globe,
  Lock,
  Smartphone
} from 'lucide-react';

const ModernLandingPage: React.FC = () => {
  const [activeFeature, setActiveFeature] = useState(0);

  const features = [
    {
      icon: <TrendingUp size={32} />,
      title: 'Smart Analytics',
      description: 'AI-powered investment insights and portfolio optimization recommendations.',
      color: 'var(--ezana-primary)'
    },
    {
      icon: <Shield size={32} />,
      title: 'Risk Management',
      description: 'Advanced risk assessment and portfolio diversification strategies.',
      color: 'var(--ezana-success)'
    },
    {
      icon: <Users size={32} />,
      title: 'Social Investing',
      description: 'Connect with other investors and share insights and strategies.',
      color: 'var(--ezana-warning)'
    },
    {
      icon: <BarChart3 size={32} />,
      title: 'Real-time Data',
      description: 'Live market data and portfolio performance tracking.',
      color: 'var(--ezana-info)'
    }
  ];

  const testimonials = [
    {
      name: 'Sarah Johnson',
      role: 'Portfolio Manager',
      company: 'Tech Investments Inc.',
      content: 'Ezana has transformed how we manage our investment portfolios. The analytics are incredibly insightful.',
      rating: 5,
      avatar: 'SJ'
    },
    {
      name: 'Michael Chen',
      role: 'Individual Investor',
      company: 'Self-employed',
      content: 'The social features help me learn from other investors. Great platform for both beginners and experts.',
      rating: 5,
      avatar: 'MC'
    },
    {
      name: 'Emily Rodriguez',
      role: 'Financial Advisor',
      company: 'Wealth Management Group',
      content: 'My clients love the intuitive interface and comprehensive reporting features.',
      rating: 5,
      avatar: 'ER'
    }
  ];

  const stats = [
    { value: '50K+', label: 'Active Users' },
    { value: '$2.5B+', label: 'Portfolio Value' },
    { value: '98%', label: 'Satisfaction Rate' },
    { value: '24/7', label: 'Support Available' }
  ];

  return (
    <div className="ezana-animate-fade-in">
      {/* Hero Section */}
      <section className="py-5" style={{ background: 'linear-gradient(135deg, var(--ezana-gray-50) 0%, var(--ezana-gray-100) 100%)' }}>
        <div className="container">
          <div className="row align-items-center">
            <div className="col-lg-6 mb-5 mb-lg-0">
              <div className="ezana-animate-slide-in-left">
                <h1 className="ezana-text-display mb-4" style={{ 
                  background: 'linear-gradient(135deg, var(--ezana-primary) 0%, var(--ezana-primary-dark) 100%)', 
                  WebkitBackgroundClip: 'text', 
                  WebkitTextFillColor: 'transparent', 
                  backgroundClip: 'text' 
                }}>
                  Smart Investment Analytics for Modern Investors
                </h1>
                <p className="ezana-text-body-large mb-4" style={{ color: 'var(--ezana-gray-600)', lineHeight: '1.7' }}>
                  Transform your investment strategy with AI-powered insights, real-time analytics, and social investing features. 
                  Make informed decisions backed by data and community wisdom.
                </p>
                <div className="d-flex flex-wrap gap-3 mb-4">
                  <button className="ezana-btn ezana-btn-primary ezana-btn-lg">
                    Get Started Free
                    <ArrowRight size={20} className="ms-2" />
                  </button>
                  <button className="ezana-btn ezana-btn-secondary ezana-btn-lg">
                    <Play size={20} className="me-2" />
                    Watch Demo
                  </button>
                </div>
                <div className="d-flex align-items-center gap-4">
                  <div className="d-flex align-items-center gap-2">
                    <CheckCircle size={20} style={{ color: 'var(--ezana-success)' }} />
                    <span className="ezana-text-small">No credit card required</span>
                  </div>
                  <div className="d-flex align-items-center gap-2">
                    <CheckCircle size={20} style={{ color: 'var(--ezana-success)' }} />
                    <span className="ezana-text-small">14-day free trial</span>
                  </div>
                </div>
              </div>
            </div>
            <div className="col-lg-6">
              <div className="ezana-animate-slide-in-right">
                <div className="ezana-card" style={{ padding: '2rem', textAlign: 'center' }}>
                  <div className="position-relative">
                    <div className="rounded-circle d-flex align-items-center justify-content-center mx-auto mb-4" 
                         style={{ width: '120px', height: '120px', background: 'linear-gradient(135deg, var(--ezana-primary) 0%, var(--ezana-primary-light) 100%)' }}>
                      <TrendingUp size={48} style={{ color: 'white' }} />
                    </div>
                    <div className="position-absolute" style={{ top: '20px', right: '20px' }}>
                      <div className="ezana-status ezana-status-success">Live</div>
                    </div>
                  </div>
                  <h3 className="ezana-text-heading-3 mb-3">Portfolio Dashboard</h3>
                  <p className="ezana-text-body mb-4" style={{ color: 'var(--ezana-gray-600)' }}>
                    Real-time portfolio tracking with advanced analytics and insights
                  </p>
                  <div className="d-flex justify-content-center gap-3">
                    <div className="text-center">
                      <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem' }}>$125,430</div>
                      <div className="ezana-text-caption">Total Value</div>
                    </div>
                    <div className="text-center">
                      <div className="ezana-metric-value" style={{ fontSize: '1.5rem', marginBottom: '0.25rem', color: 'var(--ezana-profit)' }}>+12.4%</div>
                      <div className="ezana-text-caption">This Month</div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="py-5">
        <div className="container">
          <div className="row">
            {stats.map((stat, index) => (
              <div key={stat.label} className="col-lg-3 col-md-6 mb-4">
                <div className="text-center ezana-animate-scale-in" style={{ animationDelay: `${index * 0.1}s` }}>
                  <div className="ezana-metric-value mb-2" style={{ 
                    fontSize: '2.5rem', 
                    background: 'linear-gradient(135deg, var(--ezana-primary) 0%, var(--ezana-primary-dark) 100%)', 
                    WebkitBackgroundClip: 'text', 
                    WebkitTextFillColor: 'transparent', 
                    backgroundClip: 'text' 
                  }}>
                    {stat.value}
                  </div>
                  <div className="ezana-text-caption">{stat.label}</div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-5" style={{ background: 'linear-gradient(135deg, var(--ezana-gray-100) 0%, var(--ezana-gray-50) 100%)' }}>
        <div className="container">
          <div className="row mb-5">
            <div className="col-lg-8 mx-auto text-center">
              <h2 className="ezana-text-heading-1 mb-3">Why Choose Ezana?</h2>
              <p className="ezana-text-body-large" style={{ color: 'var(--ezana-gray-600)' }}>
                Built for investors who demand more than just basic portfolio tracking
              </p>
            </div>
          </div>
          
          <div className="row">
            {features.map((feature, index) => (
              <div key={feature.title} className="col-lg-6 mb-4">
                <div className="ezana-card ezana-card-interactive h-100" 
                     onMouseEnter={() => setActiveFeature(index)}
                     style={{ borderLeft: `4px solid ${feature.color}` }}>
                  <div className="ezana-card-body">
                    <div className="d-flex align-items-start gap-3">
                      <div className="rounded-circle d-flex align-items-center justify-content-center" 
                           style={{ width: '64px', height: '64px', backgroundColor: `${feature.color}20`, color: feature.color, flexShrink: 0 }}>
                        {feature.icon}
                      </div>
                      <div>
                        <h4 className="ezana-text-heading-3 mb-2">{feature.title}</h4>
                        <p className="ezana-text-body" style={{ color: 'var(--ezana-gray-600)' }}>
                          {feature.description}
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* Testimonials Section */}
      <section className="py-5">
        <div className="container">
          <div className="row mb-5">
            <div className="col-lg-8 mx-auto text-center">
              <h2 className="ezana-text-heading-1 mb-3">Trusted by Investors Worldwide</h2>
              <p className="ezana-text-body-large" style={{ color: 'var(--ezana-gray-600)' }}>
                See what our users have to say about their experience with Ezana
              </p>
            </div>
          </div>
          
          <div className="row">
            {testimonials.map((testimonial, index) => (
              <div key={testimonial.name} className="col-lg-4 mb-4">
                <div className="ezana-card h-100 ezana-animate-scale-in" style={{ animationDelay: `${index * 0.1}s` }}>
                  <div className="ezana-card-body">
                    <div className="d-flex align-items-center gap-2 mb-3">
                      {[...Array(testimonial.rating)].map((_, i) => (
                        <Star key={i} size={16} style={{ color: '#fbbf24', fill: '#fbbf24' }} />
                      ))}
                    </div>
                    <p className="ezana-text-body mb-4" style={{ color: 'var(--ezana-gray-700)', fontStyle: 'italic' }}>
                      "{testimonial.content}"
                    </p>
                    <div className="d-flex align-items-center gap-3">
                      <div className="rounded-circle d-flex align-items-center justify-content-center" 
                           style={{ width: '48px', height: '48px', backgroundColor: 'var(--ezana-primary)', color: 'white', fontWeight: '600' }}>
                        {testimonial.avatar}
                      </div>
                      <div>
                        <div className="fw-semibold">{testimonial.name}</div>
                        <div className="ezana-text-small" style={{ color: 'var(--ezana-gray-600)' }}>
                          {testimonial.role} at {testimonial.company}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="py-5" style={{ background: 'linear-gradient(135deg, var(--ezana-primary) 0%, var(--ezana-primary-dark) 100%)' }}>
        <div className="container">
          <div className="row">
            <div className="col-lg-8 mx-auto text-center text-white">
              <h2 className="ezana-text-heading-1 mb-3">Ready to Transform Your Investment Strategy?</h2>
              <p className="ezana-text-body-large mb-4" style={{ opacity: 0.9 }}>
                Join thousands of investors who are already using Ezana to make smarter investment decisions
              </p>
              <div className="d-flex flex-wrap gap-3 justify-content-center">
                <button className="ezana-btn ezana-btn-lg" style={{ backgroundColor: 'white', color: 'var(--ezana-primary)', borderColor: 'white' }}>
                  <Zap size={20} className="me-2" />
                  Start Free Trial
                </button>
                <button className="ezana-btn ezana-btn-lg" style={{ backgroundColor: 'transparent', color: 'white', borderColor: 'white' }}>
                  <Download size={20} className="me-2" />
                  Download App
                </button>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="py-5" style={{ background: 'var(--ezana-gray-900)', color: 'white' }}>
        <div className="container">
          <div className="row">
            <div className="col-lg-4 mb-4">
              <div className="d-flex align-items-center gap-2 mb-3">
                <div className="rounded-circle d-flex align-items-center justify-content-center" 
                     style={{ width: '40px', height: '40px', backgroundColor: 'var(--ezana-primary)', color: 'white' }}>
                  <TrendingUp size={20} />
                </div>
                <span className="ezana-text-heading-3" style={{ color: 'white' }}>Ezana</span>
              </div>
              <p className="ezana-text-body" style={{ color: 'var(--ezana-gray-400)' }}>
                Empowering investors with intelligent analytics and social insights for better financial decisions.
              </p>
            </div>
            <div className="col-lg-2 col-md-6 mb-4">
              <h5 className="ezana-text-heading-3 mb-3" style={{ color: 'white' }}>Product</h5>
              <ul className="list-unstyled">
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Features</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Pricing</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>API</a></li>
              </ul>
            </div>
            <div className="col-lg-2 col-md-6 mb-4">
              <h5 className="ezana-text-heading-3 mb-3" style={{ color: 'white' }}>Company</h5>
              <ul className="list-unstyled">
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>About</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Blog</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Careers</a></li>
              </ul>
            </div>
            <div className="col-lg-2 col-md-6 mb-4">
              <h5 className="ezana-text-heading-3 mb-3" style={{ color: 'white' }}>Support</h5>
              <ul className="list-unstyled">
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Help Center</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Contact</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Status</a></li>
              </ul>
            </div>
            <div className="col-lg-2 col-md-6 mb-4">
              <h5 className="ezana-text-heading-3 mb-3" style={{ color: 'white' }}>Legal</h5>
              <ul className="list-unstyled">
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Privacy</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Terms</a></li>
                <li className="mb-2"><a href="#" className="text-decoration-none" style={{ color: 'var(--ezana-gray-400)' }}>Security</a></li>
              </ul>
            </div>
          </div>
          <hr style={{ borderColor: 'var(--ezana-gray-700)' }} />
          <div className="row align-items-center">
            <div className="col-md-6">
              <p className="mb-0" style={{ color: 'var(--ezana-gray-400)' }}>
                &copy; 2024 Ezana Investment Analytics. All rights reserved.
              </p>
            </div>
            <div className="col-md-6 text-md-end">
              <div className="d-flex gap-3 justify-content-md-end">
                <a href="#" style={{ color: 'var(--ezana-gray-400)' }}><Globe size={20} /></a>
                <a href="#" style={{ color: 'var(--ezana-gray-400)' }}><Smartphone size={20} /></a>
                <a href="#" style={{ color: 'var(--ezana-gray-400)' }}><Lock size={20} /></a>
              </div>
            </div>
          </div>
        </div>
      </footer>
    </div>
  );
};

export default ModernLandingPage;
