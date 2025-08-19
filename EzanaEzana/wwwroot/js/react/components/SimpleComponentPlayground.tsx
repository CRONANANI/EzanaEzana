import React, { useState } from 'react';

// Simple test components
const SimpleCard: React.FC<{ title: string; content: string }> = ({ title, content }) => (
  <div className="bg-white p-4 rounded-lg shadow border">
    <h3 className="text-lg font-semibold mb-2">{title}</h3>
    <p className="text-gray-600">{content}</p>
  </div>
);

const SimpleButton: React.FC<{ onClick: () => void; children: React.ReactNode }> = ({ onClick, children }) => (
  <button 
    onClick={onClick}
    className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded transition-colors"
  >
    {children}
  </button>
);

const SimpleCounter: React.FC = () => {
  const [count, setCount] = useState(0);
  return (
    <div className="text-center">
      <p className="text-2xl font-bold mb-4">Count: {count}</p>
      <SimpleButton onClick={() => setCount(count + 1)}>Increment</SimpleButton>
    </div>
  );
};

const SimpleForm: React.FC = () => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    alert(`Form submitted: ${name} - ${email}`);
  };
  
  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div>
        <label className="block text-sm font-medium mb-1">Name:</label>
        <input
          type="text"
          value={name}
          onChange={(e) => setName(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>
      <div>
        <label className="block text-sm font-medium mb-1">Email:</label>
        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </div>
      <SimpleButton onClick={() => {}} type="submit">Submit</SimpleButton>
    </form>
  );
};

interface ComponentConfig {
  name: string;
  component: React.ComponentType<any>;
  description: string;
  category: string;
}

const components: ComponentConfig[] = [
  {
    name: 'SimpleCard',
    component: SimpleCard,
    description: 'A basic card component with title and content',
    category: 'UI'
  },
  {
    name: 'SimpleButton',
    component: SimpleButton,
    description: 'A styled button component',
    category: 'UI'
  },
  {
    name: 'SimpleCounter',
    component: SimpleCounter,
    description: 'A counter component with increment functionality',
    category: 'Interactive'
  },
  {
    name: 'SimpleForm',
    component: SimpleForm,
    description: 'A basic form component with name and email fields',
    category: 'Forms'
  }
];

const categories = ['All', 'UI', 'Interactive', 'Forms'];

const SimpleComponentPlayground: React.FC = () => {
  const [selectedComponent, setSelectedComponent] = useState<ComponentConfig | null>(null);
  const [selectedCategory, setSelectedCategory] = useState('All');

  const filteredComponents = selectedCategory === 'All' 
    ? components 
    : components.filter(comp => comp.category === selectedCategory);

  const handleComponentSelect = (component: ComponentConfig) => {
    setSelectedComponent(component);
  };

  return (
    <div className="min-h-screen bg-gray-50 flex">
      {/* Sidebar */}
      <div className="w-80 bg-white border-r border-gray-200 shadow-sm">
        <div className="p-4 border-b border-gray-200">
          <h1 className="text-xl font-bold text-gray-900">Simple Component Playground</h1>
          <p className="text-sm text-gray-600 mt-1">Testing basic React components</p>
        </div>

        {/* Category Filter */}
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

        {/* Component List */}
        <div className="flex-1 overflow-y-auto">
          {filteredComponents.map((component) => (
            <div
              key={component.name}
              className={`p-4 cursor-pointer transition-colors hover:bg-gray-50 ${
                selectedComponent?.name === component.name ? 'bg-blue-50 border-r-2 border-blue-500' : ''
              }`}
              onClick={() => handleComponentSelect(component)}
            >
              <h3 className="text-sm font-medium text-gray-900">{component.name}</h3>
              <p className="text-xs text-gray-500 mt-1">{component.description}</p>
            </div>
          ))}
        </div>
      </div>

      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Header */}
        <div className="bg-white border-b border-gray-200 px-6 py-4">
          <h2 className="text-2xl font-bold text-gray-900">
            {selectedComponent ? selectedComponent.name : 'Select a Component'}
          </h2>
          {selectedComponent && (
            <p className="text-gray-600 mt-1">{selectedComponent.description}</p>
          )}
        </div>

        {/* Component Preview */}
        <div className="flex-1 p-6 overflow-auto">
          {selectedComponent ? (
            <div className="bg-white rounded-lg border border-gray-200 p-6 shadow-sm">
              <h3 className="text-lg font-medium text-gray-900 mb-4">Component Preview</h3>
              <div className="border border-gray-200 rounded-lg p-4 bg-gray-50">
                <selectedComponent.component />
              </div>
            </div>
          ) : (
            <div className="flex items-center justify-center h-full">
              <div className="text-center">
                <div className="w-16 h-16 bg-blue-100 rounded-full flex items-center justify-center mx-auto mb-4">
                  <span className="text-2xl">🎯</span>
                </div>
                <h3 className="text-lg font-medium text-gray-900 mb-2">Welcome to Simple Component Playground</h3>
                <p className="text-gray-600">Select a component from the sidebar to preview it</p>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default SimpleComponentPlayground;
