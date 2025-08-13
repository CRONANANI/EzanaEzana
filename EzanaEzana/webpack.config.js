const path = require('path');

module.exports = {
    mode: process.env.NODE_ENV === 'production' ? 'production' : 'development',
    entry: {
        main: './wwwroot/js/react/index.js',
        dashboard: './wwwroot/js/react/dashboard-entry.js',
        home: './wwwroot/js/react/home-entry.js'
    },
    output: {
        path: path.resolve(__dirname, 'wwwroot/js/dist'),
        filename: '[name].bundle.js',
        publicPath: '/js/dist/',
        clean: true
    },
    module: {
        rules: [
            {
                test: /\.(ts|tsx|js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: [
                            '@babel/preset-env',
                            '@babel/preset-typescript',
                            ['@babel/preset-react', { runtime: 'automatic' }]
                        ]
                    }
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader',
                    'postcss-loader'
                ]
            },
            {
                test: /\.(png|svg|jpg|jpeg|gif)$/i,
                type: 'asset/resource'
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/i,
                type: 'asset/resource'
            }
        ]
    },
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.jsx'],
        alias: {
            '@': path.resolve(__dirname, 'wwwroot/js'),
            '@/components': path.resolve(__dirname, 'wwwroot/js/react/components'),
            '@/services': path.resolve(__dirname, 'wwwroot/js/react/services'),
            '@/context': path.resolve(__dirname, 'wwwroot/js/react/context'),
            '@/types': path.resolve(__dirname, 'wwwroot/js/react/types'),
            '@/utils': path.resolve(__dirname, 'wwwroot/js/react/utils')
        }
    },
    devtool: process.env.NODE_ENV === 'production' ? 'source-map' : 'eval-source-map',
    optimization: {
        splitChunks: {
            chunks: 'all',
            cacheGroups: {
                vendor: {
                    test: /[\\/]node_modules[\\/]/,
                    name: 'vendors',
                    chunks: 'all'
                }
            }
        }
    },
    performance: {
        hints: process.env.NODE_ENV === 'production' ? 'warning' : false
    }
}; 