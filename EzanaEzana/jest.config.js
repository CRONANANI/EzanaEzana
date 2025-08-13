module.exports = {
  testEnvironment: 'jsdom',
  setupFilesAfterEnv: ['<rootDir>/wwwroot/js/react/setupTests.js'],
  moduleNameMapping: {
    '^@/(.*)$': '<rootDir>/wwwroot/js/$1',
    '^@/components/(.*)$': '<rootDir>/wwwroot/js/react/components/$1',
    '^@/services/(.*)$': '<rootDir>/wwwroot/js/react/services/$1',
    '^@/context/(.*)$': '<rootDir>/wwwroot/js/react/context/$1',
    '^@/types/(.*)$': '<rootDir>/wwwroot/js/react/types/$1',
    '^@/utils/(.*)$': '<rootDir>/wwwroot/js/react/utils/$1',
  },
  transform: {
    '^.+\\.(ts|tsx|js|jsx)$': 'babel-jest',
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'jsx', 'json'],
  collectCoverageFrom: [
    'wwwroot/js/react/**/*.{ts,tsx,js,jsx}',
    '!wwwroot/js/react/**/*.d.ts',
    '!wwwroot/js/react/index.js',
  ],
  coverageDirectory: 'coverage',
  coverageReporters: ['text', 'lcov', 'html'],
};
