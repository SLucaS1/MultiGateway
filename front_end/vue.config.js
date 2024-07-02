module.exports = {
    devServer: {
      // proxy: {
      //     '/WeatherForecast': {
      //     target: 'http://localhost:5167',
      //     changeOrigin: true, 
      //   },
      // }
// serverModuleFormat: 'cjs',
//       proxy: {
//         '/WeatherForecast': {
//           target: 'http://localhost:5167',
//           ws: true,
//           changeOrigin: true
//         }
//       }
  

      //   proxy:{
      //     '^/WeatherForecast': {
      //       target: 'http://localhost:5167',
      //       changeOrigin: true,
      //       logLevel: 'debug' 
      //   },            '/WeatherForecast': {
      //     target: 'http://localhost:5167',
      //     changeOrigin: true,
      //     logLevel: 'debug' 
      // }
      //   }
    
         
      proxy: 'http://localhost:5050'

      // proxy: {
      //   '/WeatherForecast': {
      //     target: 'http://localhost:5050/WeatherForecast',
      //     changeOrigin: true,
      //     logLevel: 'debug'
      //   }
      // }
    }
  }


