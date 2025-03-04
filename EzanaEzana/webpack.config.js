const path = require('path');

module.exports = {
  mode: process.env.NODE_ENV === 'production' ? 'production' : 'development',
  entry: {
    // Define entry points for different React components/pages
    main: './wwwroot/js/react/index.js',
    // Add more entry points as needed for different pages
  },
  output: {
    path: path.resolve(__dirname, 'wwwroot/js/dist'),
    filename: '[name].bundle.js',
    publicPath: '/js/dist/'
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader'
        }
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.jsx']
  },
  devtool: process.env.NODE_ENV === 'production' ? 'source-map' : 'eval-source-map'
}; 