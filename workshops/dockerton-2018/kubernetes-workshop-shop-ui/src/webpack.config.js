const webpack = require("webpack");
const path = require("path");

module.exports = {
    entry: {
        frontpage: "./UI/Features/Frontpage/main.js"
    },
    output: {
        path: path.resolve(__dirname, "UI", "wwwroot"),
        filename: "[name].bundle.js"
    },
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        },
        extensions: [".js", ".scss"]
    },
    devtool: 'source-map',
    module: {
        rules: [{
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            },
            {
                test: /\.(sa|sc|c)ss$/,
                use: [
                    "style-loader",
                    "css-loader",
                    "sass-loader",
                ]
            },
            {
                test: /\.(jpe?g|png|gif|svg|eot|woff|ttf|svg|woff2)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: 'fonts/'
                    }
                }]                
            }
        ]
    }
}