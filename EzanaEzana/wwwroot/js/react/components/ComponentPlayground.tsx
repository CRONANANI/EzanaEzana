import React, { useState } from 'react';
import { motion } from 'framer-motion';
import { 
  ChevronDown, 
  ChevronRight, 
  Play, 
  Code, 
  Eye,
  Settings,
  BarChart3,
  TrendingUp,
  Users,
  MessageSquare,
  CreditCard,
  PieChart,
  Activity,
  DollarSign,
  Shield,
  Zap
} from 'lucide-react';

// Import all your components
import Dashboard from './Dashboard';
import ModernDashboard from './ModernDashboard';
import LandingPage from './LandingPage';
import ModernLandingPage from './ModernLandingPage';
import InvestmentPreferences from './InvestmentPreferences';
import ModernInvestmentPreferences from './ModernInvestmentPreferences';
import PlaidDemo from './PlaidDemo';
import PlaidTester from './PlaidTester';
import RealTimeStockTicker from './RealTimeStockTicker';
import CongressionalTrading from './CongressionalTrading';
import PortfolioSummary from './PortfolioSummary';
import InvestmentChart from './InvestmentChart';
import DashboardCard from './DashboardCard';
import StockDetails from './StockDetails';
import WebApp from './WebApp';
import ShadcnDemo from './ShadcnDemo';

interface ComponentConfig {
  name: string;
  component: React.ComponentType<any>;
  icon: React.ReactNode;
  description: string;
  category: string;
  props?: Record<string, any>;
}

const components: ComponentConfig[] = [
  {
    name: 'Dashboard',
    component: Dashboard,
    icon: <BarChart3 className="w-4 h-4" />,
    description: 'Main dashboard with investment analytics',
    category: 'Dashboard',
    props: { isPlayground: true }
  },
  {
    name: 'ModernDashboard',
    component: ModernDashboard,
    icon: <TrendingUp className="w-4 h-4" />,
    description: 'Enhanced modern dashboard with animations',
    category: 'Dashboard',
    props: { isPlayground: true }
  },
  {
    name: 'LandingPage',
    component: LandingPage,
    icon: <Eye className="w-4 h-4" />,
    description: 'Landing page component',
    category: 'Marketing',
    props: { isPlayground: true }
  },
  {
    name: 'ModernLandingPage',
    component: ModernLandingPage,
    icon: <Zap className="w-4 h-4" />,
    description: 'Enhanced landing page with modern design',
    category: 'Marketing',
    props: { isPlayground: true }
  },
  {
    name: 'InvestmentPreferences',
    component: InvestmentPreferences,
    icon: <Settings className="w-4 h-4" />,
    description: 'Investment preferences form',
    category: 'Forms',
    props: { isPlayground: true }
  },
  {
    name: 'ModernInvestmentPreferences',
    component: ModernInvestmentPreferences,
    icon: <PieChart className="w-4 h-4" />,
    description: 'Enhanced investment preferences with charts',
    category: 'Forms',
    props: { isPlayground: true }
  },
  {
    name: 'PlaidDemo',
    component: PlaidDemo,
    icon: <CreditCard className="w-4 h-4" />,
    description: 'Plaid integration demo',
    category: 'Integration',
    props: { isPlayground: true }
  },
  {
    name: 'PlaidTester',
    component: PlaidTester,
    icon: <Activity className="w-4 h-4" />,
    description: 'Plaid API testing interface',
    category: 'Integration',
    props: { isPlayground: true }
  },
  {
    name: 'RealTimeStockTicker',
    component: RealTimeStockTicker,
    icon: <TrendingUp className="w-4 h-4" />,
    description: 'Real-time stock price ticker',
    category: 'Data',
    props: { isPlayground: true }
  },
  {
    name: 'CongressionalTrading',
    component: CongressionalTrading,
    icon: <Users className="w-4 h-4" />,
    description: 'Congressional trading data visualization',
    category: 'Data',
    props: { isPlayground: true }
  },
  {
    name: 'PortfolioSummary',
    component: PortfolioSummary,
    icon: <DollarSign className="w-4 h-4" />,
    description: 'Portfolio summary component',
    category: 'Portfolio',
    props: { isPlayground: true }
  },
  {
    name: 'InvestmentChart',
    component: InvestmentChart,
    icon: <BarChart3 className="w-4 h-4" />,
    description: 'Investment performance charts',
    category: 'Charts',
    props: { isPlayground: true }
  },
  {
    name: 'DashboardCard',
    component: DashboardCard,
    icon: <Activity className="w-4 h-4" />,
    description: 'Individual dashboard card component',
    category: 'UI',
    props: { 
      isPlayground: true,
      title: "Sample Card",
      value: "$10,000",
      change: "+5.2%",
      isPositive: true
    }
  },
  {
    name: 'StockDetails',
    component: StockDetails,
    icon: <TrendingUp className="w-4 h-4" />,
    description: 'Detailed stock information display',
    category: 'Data',
    props: { isPlayground: true }
  },
  {
    name: 'WebApp',
    component: WebApp,
    icon: <Code className="w-4 h-4" />,
    description: 'Web application wrapper',
    category: 'App',
    props: { isPlayground: true }
  },
  {
    name: 'ShadcnDemo',
    component: ShadcnDemo,
    icon: <Shield className="w-4 h-4" />,
    description: 'Shadcn UI components demo',
    category: 'UI',
    props: { isPlayground: true }
  }
];

const categories = ['All', 'Dashboard', 'Marketing', 'Forms', 'Integration', 'Data', 'Portfolio', 'Charts', 'UI', 'App'];

const ComponentPlayground: React.FC = () => {
  const [selectedComponent, setSelectedComponent] = useState<ComponentConfig | null>(null);
  const [selectedCategory, setSelectedCategory] = useState('All');
  const [showCode, setShowCode] = useState(false);
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);

  const filteredComponents = selectedCategory === 'All' 
    ? components 
    : components.filter(comp => comp.category === selectedCategory);

  const handleComponentSelect = (component: ComponentConfig) => {
    setSelectedComponent(component);
    setShowCode(false);
  };

  const toggleSidebar = () => {
    setSidebarCollapsed(!sidebarCollapsed);
  };

  return (
    <div className="min-h-screen bg-gray-50 flex">
      {/* Sidebar */}
      <motion.div 
        className={`bg-white border-r border-gray-200 shadow-sm ${sidebarCollapsed ? 'w-16' : 'w-80'}`}
        initial={{ width: 320 }}
        animate={{ width: sidebarCollapsed ? 64 : 320 }}
        transition={{ duration: 0.3 }}
      >
        <div className="p-4 border-b border-gray-200">
          <div className="flex items-center justify-between">
            {!sidebarCollapsed && (
              <h1 className="text-xl font-bold text-gray-900">Component Playground</h1>
            )}
            <button
              onClick={toggleSidebar}
              className="p-2 rounded-lg hover:bg-gray-100 transition-colors"
            >
              {sidebarCollapsed ? <ChevronRight className="w-4 h-4" /> : <ChevronDown className="w-4 h-4" />}
            </button>
          </div>
          {!sidebarCollapsed && (
            <p className="text-sm text-gray-600 mt-1">Test and preview your React components</p>
          )}
        </div>

        {/* Category Filter */}
        {!sidebarCollapsed && (
          <div className="p-4 border-b border-gray-200">
            <label className="block text-sm font-medium text-gray-700 mb-2">Category</label>
            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            >
              {categories.map(category => (
                <option key={category} value={category}>{category}</option>
              ))}
            </select>
          </div>
        )}

        {/* Component List */}
        <div className="flex-1 overflow-y-auto">
          {filteredComponents.map((component) => (
            <motion.div
              key={component.name}
              className={`p-4 cursor-pointer transition-colors hover:bg-gray-50 ${
                selectedComponent?.name === component.name ? 'bg-blue-50 border-r-2 border-blue-500' : ''
              }`}
              onClick={() => handleComponentSelect(component)}
              whileHover={{ backgroundColor: '#f9fafb' }}
              whileTap={{ scale: 0.98 }}
            >
              <div className="flex items-center space-x-3">
                <div className="text-blue-600">
                  {component.icon}
                </div>
                {!sidebarCollapsed && (
                  <div className="flex-1 min-w-0">
                    <h3 className="text-sm font-medium text-gray-900 truncate">
                      {component.name}
                    </h3>
                    <p className="text-xs text-gray-500 truncate">
                      {component.description}
                    </p>
                  </div>
                )}
              </div>
            </motion.div>
          ))}
        </div>
      </motion.div>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Header */}
        <div className="bg-white border-b border-gray-200 px-6 py-4">
          <div className="flex items-center justify-between">
            <div>
              <h2 className="text-2xl font-bold text-gray-900">
                {selectedComponent ? selectedComponent.name : 'Select a Component'}
              </h2>
              {selectedComponent && (
                <p className="text-gray-600 mt-1">{selectedComponent.description}</p>
              )}
            </div>
            {selectedComponent && (
              <div className="flex space-x-2">
                <button
                  onClick={() => setShowCode(!showCode)}
                  className={`px-4 py-2 rounded-lg flex items-center space-x-2 transition-colors ${
                    showCode 
                      ? 'bg-gray-800 text-white' 
                      : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                  }`}
                >
                  <Code className="w-4 h-4" />
                  <span>{showCode ? 'Hide Code' : 'Show Code'}</span>
                </button>
                <button
                  onClick={() => window.open(`/Home/React`, '_blank')}
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors flex items-center space-x-2"
                >
                  <Play className="w-4 h-4" />
                  <span>Open in New Tab</span>
                </button>
              </div>
            )}
          </div>
        </div>

        {/* Component Preview */}
        <div className="flex-1 p-6 overflow-auto">
          {selectedComponent ? (
            <div className="space-y-6">
              {/* Component Render */}
              <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
                <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center">
                  <Eye className="w-5 h-5 mr-2 text-blue-600" />
                  Component Preview
                </h3>
                <div className="border border-gray-200 rounded-lg p-4 bg-gray-50">
                  <selectedComponent.component {...selectedComponent.props} />
                </div>
              </div>

              {/* Code Display */}
              {showCode && (
                <motion.div 
                  className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm"
                  initial={{ opacity: 0, y: 20 }}
                  animate={{ opacity: 1, y: 0 }}
                  transition={{ duration: 0.3 }}
                >
                  <h3 className="text-lg font-medium text-gray-900 mb-4 flex items-center">
                    <Code className="w-5 h-5 mr-2 text-green-600" />
                    Component Code
                  </h3>
                  <div className="bg-gray-900 rounded-lg p-4 overflow-x-auto">
                    <pre className="text-green-400 text-sm">
                      <code>{`import { ${selectedComponent.name} } from './components/${selectedComponent.name}';

// Usage
<${selectedComponent.name} 
  ${Object.entries(selectedComponent.props || {})
    .map(([key, value]) => `${key}={${JSON.stringify(value)}}`)
    .join('\n  ')}
/>`}</code>
                    </pre>
                  </div>
                </motion.div>
              )}
            </div>
          ) : (
            <div className="flex items-center justify-center h-full">
              <div className="text-center">
                <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
                  <Code className="w-8 h-8 text-blue-600" />
                </div>
                <h3 className="text-lg font-medium text-gray-900 mb-2">Welcome to Component Playground</h3>
                <p className="text-gray-600">Select a component from the sidebar to preview and test it</p>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default ComponentPlayground;
