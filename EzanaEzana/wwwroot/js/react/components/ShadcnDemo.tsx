import React, { useState } from 'react';

const ShadcnDemo: React.FC = () => {
  const [activeTab, setActiveTab] = useState('components');
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    message: ''
  });

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  return (
    <div className="min-h-screen bg-background text-foreground p-6">
      <div className="max-w-6xl mx-auto">
        {/* Header */}
        <div className="text-center mb-12">
          <h1 className="text-4xl font-bold text-primary mb-4">
            Shadcn Theme Demo
          </h1>
          <p className="text-lg text-muted-foreground max-w-2xl mx-auto">
            Experience the modern design system with CSS custom properties, 
            featuring a beautiful amber/orange color palette and comprehensive theming support.
          </p>
        </div>

        {/* Navigation Tabs */}
        <div className="flex justify-center mb-8">
          <div className="flex bg-muted rounded-lg p-1">
            {['components', 'colors', 'typography', 'forms'].map((tab) => (
              <button
                key={tab}
                onClick={() => setActiveTab(tab)}
                className={`px-4 py-2 rounded-md font-medium transition-all ${
                  activeTab === tab
                    ? 'bg-background text-foreground shadow-sm'
                    : 'text-muted-foreground hover:text-foreground'
                }`}
              >
                {tab.charAt(0).toUpperCase() + tab.slice(1)}
              </button>
            ))}
          </div>
        </div>

        {/* Components Tab */}
        {activeTab === 'components' && (
          <div className="space-y-8">
            {/* Buttons */}
            <section>
              <h2 className="text-2xl font-semibold mb-4">Buttons</h2>
              <div className="flex flex-wrap gap-4">
                <button className="shadcn-btn shadcn-btn-primary">
                  Primary Button
                </button>
                <button className="shadcn-btn shadcn-btn-secondary">
                  Secondary Button
                </button>
                <button className="shadcn-btn shadcn-btn-outline">
                  Outline Button
                </button>
                <button className="shadcn-btn bg-destructive text-destructive-foreground">
                  Destructive Button
                </button>
              </div>
            </section>

            {/* Cards */}
            <section>
              <h2 className="text-2xl font-semibold mb-4">Cards</h2>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                <div className="shadcn-card p-6">
                  <h3 className="text-lg font-semibold mb-2">Feature Card</h3>
                  <p className="text-muted-foreground mb-4">
                    This card demonstrates the new shadcn theme with hover effects and shadows.
                  </p>
                  <button className="shadcn-btn shadcn-btn-primary text-sm">
                    Learn More
                  </button>
                </div>
                <div className="shadcn-card p-6">
                  <h3 className="text-lg font-semibold mb-2">Stats Card</h3>
                  <div className="text-3xl font-bold text-primary mb-2">2,847</div>
                  <p className="text-muted-foreground">Total Investments</p>
                </div>
                <div className="shadcn-card p-6">
                  <h3 className="text-lg font-semibold mb-2">Info Card</h3>
                  <p className="text-muted-foreground mb-4">
                    Beautiful, accessible components built with modern CSS.
                  </p>
                  <div className="flex gap-2">
                    <span className="px-2 py-1 bg-accent text-accent-foreground rounded text-sm">
                      New
                    </span>
                    <span className="px-2 py-1 bg-secondary text-secondary-foreground rounded text-sm">
                      Updated
                    </span>
                  </div>
                </div>
              </div>
            </section>

            {/* Alerts */}
            <section>
              <h2 className="text-2xl font-semibold mb-4">Alerts & Notifications</h2>
              <div className="space-y-4">
                <div className="p-4 bg-primary text-primary-foreground rounded-lg">
                  <strong>Primary Alert:</strong> This is an important message using the primary color.
                </div>
                <div className="p-4 bg-accent text-accent-foreground rounded-lg">
                  <strong>Accent Alert:</strong> This is an informational message using the accent color.
                </div>
                <div className="p-4 bg-destructive text-destructive-foreground rounded-lg">
                  <strong>Destructive Alert:</strong> This is an error message using the destructive color.
                </div>
              </div>
            </section>
          </div>
        )}

        {/* Colors Tab */}
        {activeTab === 'colors' && (
          <div className="space-y-8">
            <h2 className="text-2xl font-semibold mb-6">Color Palette</h2>
            
            {/* Primary Colors */}
            <section>
              <h3 className="text-xl font-semibold mb-4">Primary Colors</h3>
              <div className="grid grid-cols-2 md:grid-cols-5 gap-4">
                {[50, 100, 200, 300, 400, 500, 600, 700, 800, 900].map((shade) => (
                  <div key={shade} className="text-center">
                    <div 
                      className="w-16 h-16 rounded-lg mb-2 mx-auto shadow-sm"
                      style={{ backgroundColor: `var(--primary-${shade})` }}
                    ></div>
                    <div className="text-sm font-mono text-muted-foreground">
                      {shade}
                    </div>
                  </div>
                ))}
              </div>
            </section>

            {/* Semantic Colors */}
            <section>
              <h3 className="text-xl font-semibold mb-4">Semantic Colors</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
                <div className="p-4 bg-background border border-border rounded-lg">
                  <div className="text-sm font-medium mb-2">Background</div>
                  <div className="text-xs text-muted-foreground">var(--background)</div>
                </div>
                <div className="p-4 bg-card border border-border rounded-lg">
                  <div className="text-sm font-medium mb-2">Card</div>
                  <div className="text-xs text-muted-foreground">var(--card)</div>
                </div>
                <div className="p-4 bg-muted border border-border rounded-lg">
                  <div className="text-sm font-medium mb-2">Muted</div>
                  <div className="text-xs text-muted-foreground">var(--muted)</div>
                </div>
                <div className="p-4 bg-accent border border-border rounded-lg">
                  <div className="text-sm font-medium mb-2">Accent</div>
                  <div className="text-xs text-muted-foreground">var(--accent)</div>
                </div>
              </div>
            </section>

            {/* Chart Colors */}
            <section>
              <h3 className="text-xl font-semibold mb-4">Chart Colors</h3>
              <div className="grid grid-cols-5 gap-4">
                {[1, 2, 3, 4, 5].map((num) => (
                  <div key={num} className="text-center">
                    <div 
                      className="w-16 h-16 rounded-lg mb-2 mx-auto shadow-sm"
                      style={{ backgroundColor: `var(--chart-${num})` }}
                    ></div>
                    <div className="text-sm font-mono text-muted-foreground">
                      Chart {num}
                    </div>
                  </div>
                ))}
              </div>
            </section>
          </div>
        )}

        {/* Typography Tab */}
        {activeTab === 'typography' && (
          <div className="space-y-8">
            <h2 className="text-2xl font-semibold mb-6">Typography System</h2>
            
            <section>
              <h3 className="text-xl font-semibold mb-4">Font Families</h3>
              <div className="space-y-4">
                <div>
                  <div className="text-sm font-medium text-muted-foreground mb-2">Inter (Sans)</div>
                  <div className="text-2xl font-sans">The quick brown fox jumps over the lazy dog</div>
                </div>
                <div>
                  <div className="text-sm font-medium text-muted-foreground mb-2">Source Serif 4 (Serif)</div>
                  <div className="text-2xl font-serif">The quick brown fox jumps over the lazy dog</div>
                </div>
                <div>
                  <div className="text-sm font-medium text-muted-foreground mb-2">JetBrains Mono (Monospace)</div>
                  <div className="text-2xl font-mono">The quick brown fox jumps over the lazy dog</div>
                </div>
              </div>
            </section>

            <section>
              <h3 className="text-xl font-semibold mb-4">Text Sizes & Weights</h3>
              <div className="space-y-4">
                <div className="text-6xl font-bold">Heading 1 (6xl, Bold)</div>
                <div className="text-5xl font-semibold">Heading 2 (5xl, Semibold)</div>
                <div className="text-4xl font-medium">Heading 3 (4xl, Medium)</div>
                <div className="text-3xl font-normal">Heading 4 (3xl, Normal)</div>
                <div className="text-2xl font-light">Heading 5 (2xl, Light)</div>
                <div className="text-xl font-light">Heading 6 (xl, Light)</div>
                <div className="text-lg">Body Large (lg)</div>
                <div className="text-base">Body (base)</div>
                <div className="text-sm">Body Small (sm)</div>
                <div className="text-xs">Caption (xs)</div>
              </div>
            </section>

            <section>
              <h3 className="text-xl font-semibold mb-4">Text Colors</h3>
              <div className="space-y-2">
                <div className="text-foreground">Foreground text (default)</div>
                <div className="text-muted-foreground">Muted foreground text</div>
                <div className="text-primary">Primary text</div>
                <div className="text-secondary-foreground">Secondary text</div>
                <div className="text-accent-foreground">Accent text</div>
                <div className="text-destructive">Destructive text</div>
              </div>
            </section>
          </div>
        )}

        {/* Forms Tab */}
        {activeTab === 'forms' && (
          <div className="space-y-8">
            <h2 className="text-2xl font-semibold mb-6">Form Components</h2>
            
            <section>
              <h3 className="text-xl font-semibold mb-4">Input Fields</h3>
              <div className="max-w-md space-y-4">
                <div>
                  <label className="block text-sm font-medium mb-2">Name</label>
                  <input
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                    className="shadcn-input"
                    placeholder="Enter your name"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium mb-2">Email</label>
                  <input
                    type="email"
                    name="email"
                    value={formData.email}
                    onChange={handleInputChange}
                    className="shadcn-input"
                    placeholder="Enter your email"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium mb-2">Message</label>
                  <textarea
                    name="message"
                    value={formData.message}
                    onChange={handleInputChange}
                    className="shadcn-input min-h-[100px] resize-none"
                    placeholder="Enter your message"
                  />
                </div>
                <button className="shadcn-btn shadcn-btn-primary w-full">
                  Submit Form
                </button>
              </div>
            </section>

            <section>
              <h3 className="text-xl font-semibold mb-4">Form Validation States</h3>
              <div className="max-w-md space-y-4">
                <div>
                  <label className="block text-sm font-medium mb-2">Valid Input</label>
                  <input
                    type="text"
                    className="shadcn-input border-green-500"
                    value="Valid input"
                    readOnly
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium mb-2">Invalid Input</label>
                  <input
                    type="text"
                    className="shadcn-input border-destructive"
                    value="Invalid input"
                    readOnly
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium mb-2">Disabled Input</label>
                  <input
                    type="text"
                    className="shadcn-input opacity-50 cursor-not-allowed"
                    value="Disabled input"
                    disabled
                  />
                </div>
              </div>
            </section>
          </div>
        )}
      </div>
    </div>
  );
};

export default ShadcnDemo;
