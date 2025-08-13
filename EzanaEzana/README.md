# Ezana Investment Analytics - Frontend

A modern, responsive investment analytics platform built with ASP.NET Core and React.

## 🚀 Features

- **Modern React 19** with TypeScript support
- **Tailwind CSS** for utility-first styling
- **Zustand** for state management
- **Recharts** for data visualization
- **Webpack 5** with hot reloading
- **ESLint** and **Jest** for code quality
- **Responsive design** with mobile-first approach
- **Dark/Light theme** support
- **Real-time data** updates
- **Interactive charts** and dashboards

## 🛠️ Tech Stack

### Frontend
- **React 19** - Latest React with concurrent features
- **TypeScript** - Type-safe JavaScript development
- **Tailwind CSS** - Utility-first CSS framework
- **Zustand** - Lightweight state management
- **Recharts** - Composable charting library
- **Lucide React** - Beautiful icon library
- **React Hook Form** - Performant forms
- **React Hot Toast** - Elegant notifications

### Build Tools
- **Webpack 5** - Modern module bundler
- **Babel** - JavaScript compiler
- **PostCSS** - CSS processing
- **ESLint** - Code linting
- **Jest** - Testing framework

### Backend
- **ASP.NET Core 8** - Modern web framework
- **Entity Framework** - Data access
- **SignalR** - Real-time communication

## 📦 Installation

### Prerequisites
- Node.js 18+ and npm
- .NET 8 SDK
- Visual Studio 2022 or VS Code

### 1. Clone the Repository
```bash
git clone <repository-url>
cd EzanaEzana
```

### 2. Install Frontend Dependencies
```bash
npm install
```

### 3. Build Frontend Assets
```bash
# Development mode with watch
npm run dev

# Production build
npm run build

# Full development (CSS + JS)
npm run dev:full
```

### 4. Run the Application
```bash
dotnet run
```

## 🎯 Available Scripts

| Script | Description |
|--------|-------------|
| `npm run dev` | Start Webpack in development mode with watch |
| `npm run build` | Build for production |
| `npm run build:css` | Build Tailwind CSS with watch |
| `npm run build:css:prod` | Build optimized CSS |
| `npm run dev:full` | Run both JS and CSS builds simultaneously |
| `npm test` | Run Jest tests |
| `npm run lint` | Run ESLint |
| `npm run lint:fix` | Fix ESLint issues automatically |
| `npm run type-check` | Run TypeScript type checking |

## 🏗️ Project Structure

```
wwwroot/js/react/
├── components/          # React components
│   ├── Dashboard.tsx   # Main dashboard
│   ├── DashboardCard.tsx
│   ├── InvestmentChart.tsx
│   └── PortfolioSummary.tsx
├── store/              # Zustand stores
│   └── useStore.ts
├── services/           # API services
│   └── api.ts
├── context/            # React context
├── types/              # TypeScript types
├── utils/              # Utility functions
└── index.js            # Main entry point
```

## 🎨 Design System

### Colors
- **Primary**: Blue (#3b82f6)
- **Success**: Green (#22c55e)
- **Warning**: Yellow (#f59e0b)
- **Danger**: Red (#ef4444)

### Components
- **Buttons**: Multiple variants (primary, secondary, outline, ghost)
- **Cards**: Consistent card design with hover effects
- **Forms**: Accessible form components
- **Tables**: Responsive data tables
- **Charts**: Interactive data visualizations

### Typography
- **Font**: Inter (Google Fonts)
- **Monospace**: JetBrains Mono
- **Scale**: Consistent heading hierarchy

## 🔧 Configuration Files

### Webpack (`webpack.config.js`)
- TypeScript and JSX support
- CSS and asset processing
- Development and production modes
- Path aliases for clean imports

### Tailwind (`tailwind.config.js`)
- Custom color palette
- Extended spacing and animations
- Component variants
- Responsive breakpoints

### TypeScript (`tsconfig.json`)
- Modern ES2020 target
- Strict type checking
- Path mapping for clean imports
- React JSX support

### ESLint (`.eslintrc.js`)
- React and TypeScript rules
- Consistent code style
- Best practices enforcement

## 📱 Responsive Design

The application is built with a mobile-first approach:

- **Mobile**: < 640px
- **Tablet**: 640px - 1024px
- **Desktop**: > 1024px

### Breakpoints
```css
sm: 640px   /* Small devices */
md: 768px   /* Medium devices */
lg: 1024px  /* Large devices */
xl: 1280px  /* Extra large devices */
2xl: 1536px /* 2X large devices */
```

## 🧪 Testing

### Jest Configuration
- React Testing Library
- DOM environment simulation
- Component testing utilities
- Mock implementations for browser APIs

### Running Tests
```bash
npm test              # Run all tests
npm test -- --watch  # Watch mode
npm test -- --coverage # Coverage report
```

## 🚀 Performance

### Optimizations
- **Code splitting** with Webpack
- **Tree shaking** for unused code removal
- **Lazy loading** for components
- **Image optimization** with Webpack
- **CSS purging** with Tailwind

### Bundle Analysis
```bash
npm run build
# Check wwwroot/js/dist/ for bundle files
```

## 🔒 Security

- **CSRF protection** with ASP.NET Core
- **XSS prevention** with React
- **Secure headers** configuration
- **Input validation** and sanitization

## 🌐 Browser Support

- **Chrome**: 90+
- **Firefox**: 88+
- **Safari**: 14+
- **Edge**: 90+

## 📚 Development Guidelines

### Code Style
- Use TypeScript for all new code
- Follow ESLint rules
- Use functional components with hooks
- Implement proper error boundaries
- Write meaningful component names

### Component Structure
```tsx
import React from 'react';

interface ComponentProps {
  // Define props interface
}

const Component: React.FC<ComponentProps> = ({ prop1, prop2 }) => {
  // Component logic
  
  return (
    // JSX
  );
};

export default Component;
```

### State Management
- Use Zustand for global state
- Local state with useState for component-specific data
- Custom hooks for reusable logic
- Avoid prop drilling

## 🐛 Troubleshooting

### Common Issues

1. **Build Errors**
   ```bash
   npm run build
   # Check for TypeScript errors
   npm run type-check
   ```

2. **CSS Not Loading**
   ```bash
   npm run build:css
   # Ensure Tailwind is processing correctly
   ```

3. **Hot Reload Not Working**
   ```bash
   npm run dev
   # Check Webpack configuration
   ```

4. **TypeScript Errors**
   ```bash
   npm run type-check
   # Fix type issues before building
   ```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Check the documentation
- Review the troubleshooting section

---

**Happy coding! 🎉**
