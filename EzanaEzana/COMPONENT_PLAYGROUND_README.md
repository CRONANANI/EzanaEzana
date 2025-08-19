# Component Playground

A comprehensive testing environment for all React components in the Ezana Investment Analytics application.

## 🚀 Quick Start

### Option 1: Through the Web Application
1. Navigate to `/Home/Playground` in your browser
2. The playground will load with all available components
3. Select components from the sidebar to preview and test them

### Option 2: Direct HTML File
1. Open `wwwroot/playground-test.html` directly in your browser
2. This standalone file includes all necessary dependencies
3. Perfect for offline testing and development

## 🎯 Features

- **Component Library**: Browse all React components organized by category
- **Live Preview**: See components render in real-time with sample data
- **Code Display**: View component usage examples and import statements
- **Category Filtering**: Filter components by Dashboard, Marketing, Forms, etc.
- **Responsive Design**: Works on desktop and mobile devices
- **Dark Mode Toggle**: Switch between light and dark themes

## 📁 Component Categories

### Dashboard
- `Dashboard` - Main dashboard with investment analytics
- `ModernDashboard` - Enhanced modern dashboard with animations

### Marketing
- `LandingPage` - Landing page component
- `ModernLandingPage` - Enhanced landing page with modern design

### Forms
- `InvestmentPreferences` - Investment preferences form
- `ModernInvestmentPreferences` - Enhanced investment preferences with charts

### Integration
- `PlaidDemo` - Plaid integration demo
- `PlaidTester` - Plaid API testing interface

### Data
- `RealTimeStockTicker` - Real-time stock price ticker
- `CongressionalTrading` - Congressional trading data visualization
- `StockDetails` - Detailed stock information display

### Portfolio
- `PortfolioSummary` - Portfolio summary component

### Charts
- `InvestmentChart` - Investment performance charts

### UI
- `DashboardCard` - Individual dashboard card component
- `ShadcnDemo` - Shadcn UI components demo

### App
- `WebApp` - Web application wrapper

## 🛠️ Development

### Adding New Components
1. Create your component in `wwwroot/js/react/components/`
2. Add it to the `components` array in `ComponentPlayground.tsx`
3. Provide appropriate props and metadata
4. Rebuild with `npm run build`

### Component Configuration
Each component in the playground includes:
- **name**: Component display name
- **component**: React component reference
- **icon**: Lucide React icon
- **description**: Component description
- **category**: Component category for filtering
- **props**: Sample props for testing

### Building
```bash
# Development build with watch
npm run dev

# Production build
npm run build

# Full development (CSS + JS)
npm run dev:full
```

## 🔧 Technical Details

### Dependencies
- React 18
- TypeScript
- Tailwind CSS
- Framer Motion
- Lucide React Icons

### File Structure
```
wwwroot/js/react/
├── components/
│   ├── ComponentPlayground.tsx    # Main playground component
│   ├── Dashboard.tsx              # Dashboard components
│   ├── Marketing.tsx              # Marketing components
│   └── ...                        # Other components
├── playground-entry.js            # Playground entry point
└── index.css                      # Global styles

Views/Home/
└── Playground.cshtml              # Playground Razor view

Controllers/
└── HomeController.cs               # Playground route
```

### Webpack Configuration
The playground is configured as a separate entry point in `webpack.config.js`:
```javascript
entry: {
    // ... other entries
    playground: './wwwroot/js/react/playground-entry.js'
}
```

## 🎨 Customization

### Styling
- Components use Tailwind CSS classes
- Custom CSS variables for theming
- Responsive design with Bootstrap grid system

### Props
- Components receive `isPlayground: true` prop when rendered in playground
- Sample data is provided through the component configuration
- Real API calls are disabled in playground mode

### Navigation
- Sidebar can be collapsed for more screen space
- Category filtering for better organization
- Search functionality can be added easily

## 🐛 Troubleshooting

### Build Errors
- Ensure all Tailwind CSS plugins are installed
- Check for syntax errors in component files
- Verify all imports are correct

### Runtime Errors
- Check browser console for JavaScript errors
- Ensure all dependencies are loaded
- Verify component props are valid

### Styling Issues
- Check if CSS files are properly loaded
- Verify Tailwind CSS classes are correct
- Ensure custom CSS variables are defined

## 📱 Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## 🤝 Contributing

1. Add new components to the playground
2. Improve component documentation
3. Add new playground features
4. Fix bugs and improve performance

## 📄 License

This component playground is part of the Ezana Investment Analytics application.
