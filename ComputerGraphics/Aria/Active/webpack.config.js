const { resolve } = require('path');
const path=require('path');
const WebpackObfuscator = require('webpack-obfuscator');
const webpack = require("webpack")
const UglifyJsPlugin  = require("uglifyjs-webpack-plugin");

module.exports={
    entry:'./src/index.ts',
    output:{
        path:path.resolve(__dirname,'./dist/'),
        filename:'bundle.js'
    },
    resolve:{
        extensions:['.ts','.js']
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/,    
                use: "ts-loader",   
                exclude: "/node-modules/", 
                resolve:{
                    extensions:['.ts','.js']
                }
                
            }
        ]
    },
    plugins: [
        new WebpackObfuscator ({
            rotateStringArray: true,
            compact: true,
            deadCodeInjection: true,
            deadCodeInjectionThreshold: 0.4,
            stringArray: true,
            stringArrayEncoding: ['rc4','base64'],
            stringArrayThreshold: 1,
        }, ['excluded_bundle_name.js']),
        new UglifyJsPlugin({
            extractComments: {
                condition:false,
            }
        })

    ],
    mode: 'development',
    devServer:{
        static: './dist',
        compress: true,
        port:1551,
        hot:true,
    },
    cache:false,
    
}